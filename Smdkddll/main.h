#pragma once

#include "defs.h"
#include "smStr.h"
#include <strsafe.h>
#include <iostream>
#include <comdef.h>  // you will need this


#ifdef SMLIBRARY_EXPORTS
#define SMLIBRARY_API __declspec(dllexport)
#else
#define SMLIBRARY_API __declspec(dllimport)
#endif

extern "C" {

	SMLIBRARY_API bool __stdcall DisassembleCode(SM_Pointer address, SM_Size length, SM_Pointer virtualAddress, bool determineStaticInstructionBytes, EnumerateInstructionCallback callback);

	SMLIBRARY_API bool Attach();
	SMLIBRARY_API void Detach();

	SMLIBRARY_API bool IsProcessWow64(IN SM_Pointer process_id, OUT BOOL* isWow64);
	SMLIBRARY_API bool IsProcessValid(IN SM_Pointer process_handle);

	SMLIBRARY_API bool __stdcall EnumerateProcesses(OUT EnumerateProcessCallback callbackProcess);
	SMLIBRARY_API bool GetProccessData(IN const char* module_name, OUT PProcessData process_data);

	SMLIBRARY_API bool OpenRemoteProcess(IN SM_Pointer process_id, OUT PSM_Pointer process_handle);
	SMLIBRARY_API bool ResumeRemoteProcess(IN SM_Pointer process_handle);
	SMLIBRARY_API bool SuspendRemoteProcess(IN SM_Pointer process_handle);
	SMLIBRARY_API bool TerminateRemoteProcess(IN SM_Pointer process_handle, int method);

	SMLIBRARY_API bool OpenRemoteThread(IN SM_Pointer thread_id, OUT PSM_Pointer thread_handle);
	SMLIBRARY_API bool ResumeRemoteThread(IN SM_Pointer thread_handle, OUT PULONG suspend_count);
	SMLIBRARY_API bool SuspendRemoteThread(IN SM_Pointer thread_handle, IN OUT PULONG previous_suspend_count);

	SMLIBRARY_API bool EnumerateRemoteModules(IN SM_Pointer process_id, OUT EnumerateRemoteModulesCallback callbackModule);
	SMLIBRARY_API bool __stdcall EnumerateRemoteSectionsAndModules(IN SM_Pointer process, OUT EnumerateRemoteSectionsCallback callbackSection, OUT EnumerateRemoteModulesCallback callbackModule);
	SMLIBRARY_API bool MmCpy(SM_Pointer source_process_id, PVOID read_address, SM_Pointer target_process_id, PVOID target_address, ULONG size);

	SMLIBRARY_API bool __stdcall HexDumpBytes(PVOID start_address, BYTE* bytesBuffer, int len, OUT EnumerateDumpCallback callbackDump);
	SMLIBRARY_API bool __stdcall DisassembleBytes(PVOID start_address, BYTE* data, int len, bool isWow64, OUT EnumerateDisassembleCallback callbackDisassemble);
}

using InternalEnumerateRemoteModulesCallback = std::function<void(EnumerateRemoteModuleData&)>;

// interface for our driver
class SmKdInterface
{
public:
	HANDLE hDriver; // Handle to driver

	/*** Initializer ***/
	SmKdInterface()
	{
		hDriver = INVALID_HANDLE_VALUE;
	}
	SmKdInterface(LPCSTR RegistryPath)
	{
		hDriver = CreateFileA(RegistryPath, GENERIC_READ | GENERIC_WRITE,
			FILE_SHARE_READ | FILE_SHARE_WRITE, 0, OPEN_EXISTING, 0, 0);
	}

	bool EnumerateProcesses(EnumerateProcessCallback callbackProcess)
	{
		if (callbackProcess == nullptr) return false;

		if (hDriver == INVALID_HANDLE_VALUE)
			return false;

		ULONG bytes;
		bool durum = false;
		ULONG qmemsize = 0x1024;
		PVOID qmemptr = 0;
		PSYSTEM_PROCESS_INFO spi;

		do
		{
			qmemptr = malloc(qmemsize); // alloc memory for spi
			if (qmemptr == NULL) // check memory is allocated or not.
			{
				return FALSE;
			}
			durum = QuerySystemInformation(SystemProcessInformation, qmemptr, qmemsize, &bytes);
			if (!durum)
			{
				qmemsize = qmemsize * 2; // increase qmemsize for next memory alloc
				free(qmemptr); // free memory
			}
		} while (!durum); // resize memory
		spi = (PSYSTEM_PROCESS_INFO)qmemptr;

		// runing on the list and compare pid to find the match pid
		while (true)
		{
			if (spi->ImageName.Length > 0 && spi->ImageName.Buffer > 0) {
				if (wcsstr(spi->ImageName.Buffer, L".exe"))
				{
					EnumerateProcessData data = { };
					data.Id = spi->UniqueProcessId;
					data.VirtualSize = spi->VirtualSize;

					PVOID address;
					IsProcessWow64(data.Id, &data.IsWow64, data.Path, &address);
					/*if (GetProcessDeviceImageFilePath(data.Id, imagepath))
						GetWindowsFileName(data.Path, data.Path);*/

					callbackProcess(&data);
				}
			}

			if (spi->NextEntryOffset == 0)
				break;

			spi = (PSYSTEM_PROCESS_INFO)((unsigned char*)spi + spi->NextEntryOffset); // next info 
		}

		free(qmemptr); // free memory
		return true;
	}
	bool EnumerateRemoteModulesNative(SM_Pointer process_id, const InternalEnumerateRemoteModulesCallback& callback) {
		PModuleData pMsi = NULL;
		ULONG modulesCount = 0;
		if (pMsi = GetProcessModules(process_id, &modulesCount)) {
			if (pMsi == nullptr) return 0;
			for (int i = 0; i < modulesCount; i++)
			{
				EnumerateRemoteModuleData data = {};
				data.Size = pMsi[i].Size;
				data.BaseAddress = pMsi[i].BaseAddress;
				wcscpy_s(data.Path, pMsi[i].Path);
				callback(data);
			}
			free(pMsi);

			return true;
		}
		return false;
	}


	/*** Extentions ***/
	PVmSectionData VmSectionList = NULL;
	ULONG VmSectionListCount = 0;
	BOOL ProcessVirtualMemorySnapshot(IN HANDLE process_handle, OUT PULONG sections_count) {
		if (VmSectionList != NULL) // check memory is allocated or not.
			free(VmSectionList); // free memory

		VmSectionListCount = 0;
		VmSectionData vm_data = {};

		MEMORY_BASIC_INFORMATION mbi = { };
		mbi.RegionSize = 0x1000;
		size_t address = 0;
		BOOL isWow64 = IsProcessWow64(DecodeHandle(process_handle));

		SIZE_T endAddress = 0x7FFFFFFF;
		if (!isWow64)
			endAddress = 0x7FFFFFFFFFFF;

		int index = 0;
		for (address = 0; address < endAddress && address + mbi.RegionSize > address; address += mbi.RegionSize)
		{
			if (QueryVirtualMemory(process_handle, (PVOID)address, &mbi)) {
				if (mbi.State == MEM_COMMIT)
					index++;
			}
		}

		if (index == 0) return FALSE;

		VmSectionList = (PVmSectionData)malloc(index * sizeof(VmSectionData)); // alloc memory for spi
		if (VmSectionList == NULL) // check memory is allocated or not.
			return FALSE;

		int oldIndex = index;
		index = 0;
		for (address = 0; (address < endAddress) && (address + mbi.RegionSize > address) && index <= oldIndex; address += mbi.RegionSize)
		{
			if (QueryVirtualMemory(process_handle, (PVOID)address, &mbi)) {

				if (mbi.State == MEM_COMMIT) {
					vm_data = {};
					vm_data.Size = mbi.RegionSize;
					vm_data.BaseAddress = mbi.BaseAddress;

					vm_data.Protection = SectionProtection::NoAccess;
					if ((mbi.Protect & PAGE_EXECUTE) == PAGE_EXECUTE) vm_data.Protection |= SectionProtection::Execute;
					if ((mbi.Protect & PAGE_EXECUTE_READ) == PAGE_EXECUTE_READ) vm_data.Protection |= SectionProtection::Execute | SectionProtection::Read;
					if ((mbi.Protect & PAGE_EXECUTE_READWRITE) == PAGE_EXECUTE_READWRITE) vm_data.Protection |= SectionProtection::Execute | SectionProtection::Read | SectionProtection::Write;
					if ((mbi.Protect & PAGE_EXECUTE_WRITECOPY) == PAGE_EXECUTE_WRITECOPY) vm_data.Protection |= SectionProtection::Execute | SectionProtection::Read | SectionProtection::CopyOnWrite;
					if ((mbi.Protect & PAGE_READONLY) == PAGE_READONLY) vm_data.Protection |= SectionProtection::Read;
					if ((mbi.Protect & PAGE_READWRITE) == PAGE_READWRITE) vm_data.Protection |= SectionProtection::Read | SectionProtection::Write;
					if ((mbi.Protect & PAGE_WRITECOPY) == PAGE_WRITECOPY) vm_data.Protection |= SectionProtection::Read | SectionProtection::CopyOnWrite;
					if ((mbi.Protect & PAGE_GUARD) == PAGE_GUARD) vm_data.Protection |= SectionProtection::Guard;

					switch (mbi.Type)
					{
					case MEM_IMAGE:
						vm_data.Type = SectionType::Image;
						break;
					case MEM_MAPPED:
						vm_data.Type = SectionType::Mapped;
						break;
					case MEM_PRIVATE:
						vm_data.Type = SectionType::Private;
						break;
					}

					vm_data.Category = vm_data.Type == SectionType::Private ? SectionCategory::HEAP : SectionCategory::Unknown;

					memcpy(&VmSectionList[index++], &vm_data, sizeof(VmSectionData));
				}
			}
		}

		if (index == 0) return FALSE;

		*sections_count = VmSectionListCount = index - 1;
		return TRUE;
	}
	BOOL GetProcessVirtualMemoryList(OUT PVmSectionData section_data) {
		if (VmSectionList == 0)
			return FALSE;

		memcpy(section_data, VmSectionList, sizeof(VmSectionData) * VmSectionListCount);

		free(VmSectionList);
		VmSectionList = NULL;
		VmSectionListCount = 0;

		return TRUE;
	}

	PStackAreaData StackAreaList = NULL;
	ULONG StackAreaListCount = 0;
	BOOL ProcessStackSnapshot(IN HANDLE process_id, OUT PULONG stackAreaCount) {
		if (StackAreaList != NULL) // check memory is allocated or not.
		{
			free(StackAreaList); // free memory
			StackAreaList = NULL;
		}
		StackAreaListCount = 0;
		StackAreaData tempStackArea;
		NT_TIB tib;
		BOOL status = FALSE;

		SystemProcessInfo spi;
		if (!GetSystemProcessInformation(process_id, &spi)) return FALSE;
		SYSTEM_THREAD_INFORMATION* st = spi.Threads;

		StackAreaList = (PStackAreaData)malloc(spi.NumberOfThreads * sizeof(StackAreaData)); // alloc memory for spi
		if (StackAreaList == NULL) // check memory is allocated or not.
			return status;

		HANDLE hThread;
		for (int i = 0; i < spi.NumberOfThreads; i++)
		{
			status = OpenThread(st->ClientId, &hThread);

			if (!status) {
				free(StackAreaList); // free memory
				return FALSE;
			}

			THREAD_BASIC_INFORMATION info;
			status = QueryInformationThread(hThread, THREAD_INFO_CLASS::ThreadBasicInformation, &info, sizeof(THREAD_BASIC_INFORMATION), NULL);

			if (!status) {
				free(StackAreaList); // free memory
				return FALSE;
			}

			if (MemCopy(spi.UniqueProcessId, info.TebBaseAddress, (HANDLE)GetCurrentProcessId(), &tib, sizeof(NT_TIB), NULL)) {
				tempStackArea = { 0 };
				tempStackArea.ThreadId = st->ClientId.UniqueThread;
				tempStackArea.Base = tib.StackBase;
				tempStackArea.Limit = tib.StackLimit;
				memcpy(&StackAreaList[i], &tempStackArea, sizeof(StackAreaData));
			}
			st += 1;
		}

		if (!status)
		{
			free(StackAreaList); // free memory
			return FALSE;
		}

		StackAreaListCount = spi.NumberOfThreads;
		if (StackAreaListCount > 0)
			*stackAreaCount = StackAreaListCount;

		free(spi.Threads);
		return TRUE;
	}
	BOOL GetProcessStackList(OUT PStackAreaData stackArea) {
		if (StackAreaListCount == 0)
			return FALSE;

		memcpy(stackArea, StackAreaList, sizeof(StackAreaData) * StackAreaListCount);

		free(StackAreaList);
		StackAreaList = NULL;
		StackAreaListCount = 0;

		return TRUE;
	}

	LONG IsProcessWow64(IN HANDLE process_id) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return -1;

		BOOL IsWow64;

		if (IsProcessWow64(process_id, &IsWow64)) {
			return IsWow64;
		}
		else
			return -1;
	}
	PPEB GetPeb(IN HANDLE process_handle) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return nullptr;

		PEB peb;
		ULONG bytes;
		PROCESS_BASIC_INFORMATION pbi;

		// send code to our driver with the arguments
		if (QueryInformationProcess(process_handle, ProcessBasicInformation, &pbi, sizeof(PROCESS_BASIC_INFORMATION), &bytes)) {
			if (ReadVirtualMemory(process_handle, pbi.PebBaseAddress, &peb, sizeof(PEB), NULL)) {
				return &peb;
			}

			return nullptr;
		}
		else
			return nullptr;
	}
	PModuleData GetProcessModules(IN HANDLE process_id, PULONG modulesCount) {
		PModuleData msiArr;
		if (ProcessModulesSnapshot(process_id, modulesCount)) {
			msiArr = (PModuleData)malloc(*modulesCount * sizeof(ModuleData));
			if (GetProcessModules(*modulesCount, msiArr))
				return msiArr;
			else
				return nullptr;
		}
		else
			return nullptr;
	}

	//sm process data 2 overloads
	BOOL GetProcessData(IN HANDLE process_id, OUT PProcessData process_data) {
		SystemProcessInfo spi;
		if (GetSystemProcessInformation(process_id, &spi)) {
			process_data->Id = process_id;
			process_data->VirtualSize = spi.VirtualSize;

			IsProcessWow64(process_id, &process_data->IsWow64, process_data->Path, &process_data->Address);
			process_data->NameOffset = GetNameOffsetFromPath(process_data->Path);
			free(spi.Threads);
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL GetProcessData(IN CONST PCCH process_name, OUT PProcessData process_data) {
		SystemProcessInfo spi;
		if (GetSystemProcessInformation(process_name, &spi)) {
			process_data->Id = spi.UniqueProcessId;
			process_data->VirtualSize = spi.VirtualSize;

			IsProcessWow64(spi.UniqueProcessId, &process_data->IsWow64, process_data->Path, &process_data->Address);
			process_data->NameOffset = GetNameOffsetFromPath(process_data->Path);

			free(spi.Threads);
			return TRUE;
		}
		else
			return FALSE;
	}
	//end

	//system process data 2 overloads
	BOOL GetSystemProcessInformation(IN HANDLE process_id, OUT PSystemProcessInfo pSpi) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		BOOL process_found = FALSE;
		ULONG bytes;
		BOOL durum = FALSE;
		ULONG qmemsize = 0x1024;
		PVOID qmemptr = 0;
		PSYSTEM_PROCESS_INFO spi;

		do
		{
			qmemptr = malloc(qmemsize); // alloc memory for spi
			if (qmemptr == NULL) // check memory is allocated or not.
			{
				return FALSE;
			}
			durum = QuerySystemInformation(SystemProcessInformation, qmemptr, qmemsize, &bytes);
			if (!durum)
			{
				qmemsize = qmemsize * 2; // increase qmemsize for next memory alloc
				free(qmemptr); // free memory
			}
		} while (!durum); // resize memory

		spi = (PSYSTEM_PROCESS_INFO)qmemptr;
		// runing on the list and compare pid to find the match pid
		while (spi->NextEntryOffset && !process_found)
		{
			if (spi->UniqueProcessId == process_id) {
				process_found = TRUE;
				break;
			}

			spi = (PSYSTEM_PROCESS_INFO)((unsigned char*)spi + spi->NextEntryOffset); // next info 
		}

		if (!process_found) {
			free(qmemptr); // free memory
			return process_found;
		}

		pSpi->Threads = NULL;
		pSpi->Threads = (SYSTEM_THREAD_INFORMATION*)malloc(sizeof(SYSTEM_THREAD_INFORMATION) * spi->NumberOfThreads);
		if (pSpi->Threads == NULL) {
			free(qmemptr); // free memory
			return FALSE;
		}

		CopySpiToPi(pSpi, spi);
		free(qmemptr); // free memory

		return process_found;
	}
	BOOL GetSystemProcessInformation(IN CONST PCCH process_name, OUT PSystemProcessInfo pSpi) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		BOOL process_found = FALSE;
		ULONG bytes;
		BOOL durum = FALSE;
		ULONG qmemsize = 0x1024;
		PVOID qmemptr = 0;
		PSYSTEM_PROCESS_INFO spi;

		do
		{
			qmemptr = malloc(qmemsize); // alloc memory for spi
			if (qmemptr == NULL) // check memory is allocated or not.
			{
				return FALSE;
			}
			durum = QuerySystemInformation(SystemProcessInformation, qmemptr, qmemsize, &bytes);
			if (!durum)
			{
				qmemsize = qmemsize * 2; // increase qmemsize for next memory alloc
				free(qmemptr); // free memory
			}
		} while (!durum); // resize memory
		spi = (PSYSTEM_PROCESS_INFO)qmemptr;
		// runing on the list and compare pid to find the match pid
		while (spi->NextEntryOffset && !process_found)
		{
			wchar_t* wc = (wchar_t*)getWChar(process_name);
			if (!_strcmpi_w((wchar_t*)spi->ImageName.Buffer, wc)) {
				process_found = TRUE;
				break;
			}

			free(wc);

			spi = (PSYSTEM_PROCESS_INFO)((unsigned char*)spi + spi->NextEntryOffset); // next info 
		}

		if (!process_found) {
			free(qmemptr); // free memory
			return process_found;
		}

		pSpi->Threads = NULL;
		pSpi->Threads = (SYSTEM_THREAD_INFORMATION*)malloc(sizeof(SYSTEM_THREAD_INFORMATION) * spi->NumberOfThreads);
		if (pSpi->Threads == NULL) {
			free(qmemptr); // free memory
			return FALSE;
		}

		CopySpiToPi(pSpi, spi);
		free(qmemptr); // free memory

		return process_found;
	}
	//end

	ULONG GetNameOffsetFromPath(IN PWCHAR path) {
		if (path <= 0) return 0;

		int length = wcslen(path);

		if (length <= 0) return 0;

		wchar_t separator = L'\\';
		int index;
		for (index = length - 1; index >= 0; index--)
			if (path[index] == separator)break;

		return index + 1;
	}
	BOOL GetWindowsFileName(IN PWCHAR device_file_name, IN OUT PWCHAR dos_file_name)
	{
		BOOL bFound = FALSE;

		// Translate path with device name to drive letters.
		wchar_t szTemp[MAX_PATH];
		szTemp[0] = '\0';

		if (GetLogicalDriveStrings(MAX_PATH - 1, szTemp))
		{
			wchar_t szName[MAX_PATH];//32
			wchar_t szDrive[3] = TEXT(" :");
			wchar_t* p = szTemp;

			do
			{
				// Copy the drive letter to the template string
				*szDrive = *p;

				// Look up each device name
				if (QueryDosDevice(szDrive, szName, MAX_PATH))
				{
					size_t uNameLen = wcslen(szName);

					if (uNameLen < MAX_PATH)
					{
						bFound = _wcsnicmp(device_file_name, szName, uNameLen) == 0
							&& *(device_file_name + uNameLen) == L'\\';

						if (bFound)
						{
							// Replace device path with DOS path
							StringCchPrintf(dos_file_name,
								MAX_PATH,
								TEXT("%s%s"),
								szDrive,
								device_file_name + uNameLen);
						}
					}
				}
				// Go to the next NULL character.
				while (*p++);
			} while (!bFound && *p);
		}

		return(bFound);
	}
	BOOL GetProcessDeviceImageFilePath(IN HANDLE process_handle, PWCHAR process_path) {
		if (process_path <= 0) return FALSE;

		WCHAR strBuffer[(sizeof(UNICODE_STRING) / sizeof(WCHAR)) + MAX_PATH_LENGTH];
		PUNICODE_STRING str;
		str = (UNICODE_STRING*)&strBuffer;

		//initialize
		(*str).Buffer = &strBuffer[sizeof(UNICODE_STRING) / sizeof(WCHAR)];
		(*str).Length = 0x0;
		(*str).MaximumLength = MAX_PATH_LENGTH * sizeof(WCHAR);

		if (!QueryInformationProcess(process_handle, ProcessImageFileName, &strBuffer, sizeof(strBuffer), NULL))
			return FALSE;

		wcscpy_s(process_path, MAX_PATH_LENGTH, (*str).Buffer);
		return TRUE;
	}
	BOOL GetProcessDeviceImageFilePathNew(IN HANDLE process_handle, PWCHAR process_path) {
		if (process_path <= 0) return FALSE;

		ULONG retBytes;
		PUNICODE_STRING unicodeString = NULL;
		PUNICODE_STRING* punicodeString = &unicodeString;
		QueryInformationProcess(process_handle, ProcessImageFileName, NULL, 0, &retBytes);

		*punicodeString = (PUNICODE_STRING)malloc(retBytes);
		QueryInformationProcess(process_handle, ProcessImageFileName, *punicodeString, retBytes, &retBytes);

		wcscpy_s(process_path, MAX_PATH_LENGTH, (*punicodeString)->Buffer);
		return TRUE;
	}


	/*** Process ***/
	BOOL GetProcessModules(IN ULONG modulesCount, IN OUT PModuleData moduleData_arry) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		SMNTDEFS instructions = { 0 };
		instructions.get_process_modules = { 0 };
		instructions.get_process_modules.modules = moduleData_arry;

		if (instructions.get_process_modules.modules == NULL)return FALSE;

		if (DeviceIoControl(hDriver, SM_CTL_GET_PROCESS_MODULES, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL ProcessModulesSnapshot(IN HANDLE process_id, OUT PULONG modulesCount) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		ULONG len;
		SMNTDEFS instructions = { 0 };
		instructions.snap_shot_process_modules.process_id = process_id;
		instructions.snap_shot_process_modules.modulesCount = &len;

		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_SNAP_SHOT_PROCESS_MODULES, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			*modulesCount = *instructions.snap_shot_process_modules.modulesCount;
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL OpenProcess(IN HANDLE process_id, OUT PHANDLE pHandle) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		CLIENT_ID clientId = { 0 };
		clientId.UniqueProcess = process_id;
		HANDLE handle;
		OBJECT_ATTRIBUTES   ObjectAttributes;
		InitializeObjectAttributes(&ObjectAttributes, NULL, 0, NULL, NULL);
		SMNTDEFS instructions = { 0 };
		instructions.open_process_args = { 0 };

		instructions.open_process_args.ClientId = &clientId;
		instructions.open_process_args.DesiredAccess = MAXIMUM_ALLOWED;
		instructions.open_process_args.ProcessHandle = NULL;
		instructions.open_process_args.ObjectAttributes = &ObjectAttributes;
		instructions.open_process_args.ProcessHandle = &handle;


		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_OPEN_PROCESS, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			*pHandle = *instructions.open_process_args.ProcessHandle;
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL SuspendProcess(IN HANDLE handle) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		SMNTDEFS instructions = { 0 };
		instructions.suspend_process_args = { 0 };
		instructions.suspend_process_args.ProcessHandle = handle;


		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_SUSPENDP_PROCESS, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL ResumeProcess(IN HANDLE handle) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		SMNTDEFS instructions = { 0 };
		instructions.resume_process_args = { 0 };
		instructions.resume_process_args.ProcessHandle = handle;

		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_RESUME_PROCESS, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL TerminateProcess(IN HANDLE handle, NTSTATUS exit_status, TERMINATION_METHOD method) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		SMNTDEFS instructions = { 0 };
		instructions.terminate_process_args = { 0 };
		instructions.terminate_process_args.Method = method;
		instructions.terminate_process_args.ProcessHandle = handle;
		instructions.terminate_process_args.ExitStatus = exit_status;


		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_TERMINATE_PROCESS, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL IsProcessWow64(IN HANDLE process_id, OUT PBOOL isWow64Process) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		PVOID baseAddress = 0;
		wchar_t path[MAX_PATH_LENGTH];
		SMNTDEFS instructions = { 0 };
		instructions.is_wow64_process.ProcessId = process_id;
		instructions.is_wow64_process.IsWow64 = isWow64Process;
		instructions.is_wow64_process.Path = path;
		instructions.is_wow64_process.BaseAddress = &baseAddress;

		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_IS_WOW64_PROCESS, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL IsProcessWow64(IN HANDLE process_id, OUT PBOOL isWow64Process, OUT wchar_t* Path, OUT PVOID baseAddress) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		SMNTDEFS instructions = { 0 };
		instructions.is_wow64_process.ProcessId = process_id;
		instructions.is_wow64_process.IsWow64 = isWow64Process;
		instructions.is_wow64_process.Path = Path;
		instructions.is_wow64_process.BaseAddress = baseAddress;

		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_IS_WOW64_PROCESS, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL QueryInformationProcess(IN HANDLE process_handle, PROCESSINFOCLASS processInformationClass, PVOID processInformationBuffer, ULONG processInformationLength, PULONG returnLength) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		ULONG bytes;
		PROCESS_BASIC_INFORMATION pbi;
		SMNTDEFS instructions = { 0 };
		instructions.queryInformationProcess_args.ProcessHandle = process_handle;
		instructions.queryInformationProcess_args.ProcessInformationClass = processInformationClass;
		instructions.queryInformationProcess_args.ProcessInformation = processInformationBuffer;
		instructions.queryInformationProcess_args.ProcessInformationLength = processInformationLength;
		instructions.queryInformationProcess_args.ReturnLength = returnLength;

		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_QUERY_INFORMATION_PROCESS, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			return TRUE;
		}
		else
			return FALSE;
	}


	/*** Memory ***/
	BOOL MemCopy(IN HANDLE source_process_id, IN OUT PVOID source_address, IN HANDLE target_process_id, IN OUT PVOID target_address, SIZE_T numberOfBytesToRead, OPTIONAL IN OUT PSIZE_T numberOfBytesCopyed) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		SIZE_T bytes;
		SMNTDEFS instructions = { 0 };
		instructions.mm_copy_vm_args = { 0 };

		instructions.mm_copy_vm_args.SourceProcessId = source_process_id;
		instructions.mm_copy_vm_args.SourceAddress = source_address;
		instructions.mm_copy_vm_args.TargetProcessId = target_process_id;
		instructions.mm_copy_vm_args.TargetAddress = target_address;
		instructions.mm_copy_vm_args.Size = numberOfBytesToRead;
		if (numberOfBytesCopyed != NULL)
			instructions.mm_copy_vm_args.NumberOfBytesCopyed = numberOfBytesCopyed;
		else
			instructions.mm_copy_vm_args.NumberOfBytesCopyed = &bytes;

		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_MMCOPY_VM, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL ReadVirtualMemory(IN HANDLE process_handle, IN PVOID address, PVOID buffer, SIZE_T numberOfBytesToRead, OPTIONAL PSIZE_T numberOfBytesRead) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		SIZE_T bytes;
		SMNTDEFS instructions = { 0 };
		instructions.readVirtualMemory_args = { 0 };

		instructions.readVirtualMemory_args.ProcessHandle = process_handle;
		instructions.readVirtualMemory_args.BaseAddress = address;
		instructions.readVirtualMemory_args.Buffer = buffer;
		instructions.readVirtualMemory_args.NumberOfBytesToRead = numberOfBytesToRead;
		if (numberOfBytesRead != NULL)
			instructions.readVirtualMemory_args.NumberOfBytesRead = numberOfBytesRead;
		else
			instructions.readVirtualMemory_args.NumberOfBytesRead = &bytes;

		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_READ_VIRTUAL_MEMORY, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL WriteVirtualMemory(IN HANDLE process_handle, IN PVOID address, PVOID buffer, SIZE_T numberOfBytesToWrite, OPTIONAL PSIZE_T numberOfBytesWritten) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		SIZE_T bytes;
		SMNTDEFS instructions = { 0 };
		instructions.writeVirtualMemory_args = { 0 };

		instructions.writeVirtualMemory_args.ProcessHandle = process_handle;
		instructions.writeVirtualMemory_args.BaseAddress = address;
		instructions.writeVirtualMemory_args.Buffer = buffer;
		instructions.writeVirtualMemory_args.NumberOfBytesToWrite = numberOfBytesToWrite;
		if (numberOfBytesWritten != NULL)
			instructions.writeVirtualMemory_args.NumberOfBytesWritten = numberOfBytesWritten;
		else
			instructions.writeVirtualMemory_args.NumberOfBytesWritten = &bytes;

		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_WRITE_VIRTUAL_MEMORY, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL QueryVirtualMemory(IN HANDLE process_handle, IN PVOID address, MEMORY_BASIC_INFORMATION* pMbi) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		SIZE_T bytes;
		SMNTDEFS instructions = { 0 };
		instructions.queryVirtualMemory_args = { 0 };

		instructions.queryVirtualMemory_args.ProcessHandle = process_handle;
		instructions.queryVirtualMemory_args.BaseAddress = address;
		instructions.queryVirtualMemory_args.MemoryInformationClass = MemoryBasicInformation;
		instructions.queryVirtualMemory_args.MemoryInformation = pMbi;
		instructions.queryVirtualMemory_args.MemoryInformationLength = sizeof(MEMORY_BASIC_INFORMATION);
		instructions.queryVirtualMemory_args.ReturnLength = &bytes;

		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_QUERY_VIRTUAL_MEMORY, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL QuerySystemInformation(IN SYSTEM_INFORMATION_CLASS systemInformationClass, IN OUT PVOID systemInformationBuffer, IN  ULONG systemInformationLength, IN OUT PULONG returnLength) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		ULONG bytes;
		SMNTDEFS instructions = { 0 };

		instructions.querySystemInformation_args.SystemInformationClass = systemInformationClass;
		instructions.querySystemInformation_args.Buffer = systemInformationBuffer;
		instructions.querySystemInformation_args.Length = systemInformationLength;
		instructions.querySystemInformation_args.ReturnLength = returnLength;

		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_QUERY_SYSTEM_INFORMATION, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			return TRUE;
		}
		else {
			return FALSE;
		}
	}
	BOOL QuerySystemInformationEx(IN HANDLE process_handle, IN SYSTEM_INFORMATION_CLASS systemInformationClass, IN OUT PVOID systemInformationBuffer, IN OUT PVOID inputBuffer, IN ULONG inputBufferLength, IN ULONG systemInformationLength, OPTIONAL IN OUT PULONG returnLength) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		SMNTDEFS instructions = { 0 };

		instructions.querySystemInformationEx_args.SystemInformationClass = systemInformationClass;
		instructions.querySystemInformationEx_args.SystemInformation = systemInformationBuffer;
		/*	instructions.querySystemInformationEx_args.InputBuffer = &process_handle;
			instructions.querySystemInformationEx_args.InputBufferLength = sizeof(process_handle);*/
		instructions.querySystemInformationEx_args.InputBuffer = inputBuffer;
		instructions.querySystemInformationEx_args.InputBufferLength = inputBufferLength;
		instructions.querySystemInformationEx_args.SystemInformationLength = systemInformationLength;
		instructions.querySystemInformationEx_args.ReturnLength = returnLength;

		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_QUERY_SYSTEM_INFORMATION_EX, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			return TRUE;
		}
		else
			return FALSE;
	}


	/*** Thread ***/
	BOOL OpenThread(IN CLIENT_ID client_id, OUT PHANDLE tHandle) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		HANDLE handle;
		OBJECT_ATTRIBUTES   ObjectAttributes;
		InitializeObjectAttributes(&ObjectAttributes, NULL, 0, NULL, NULL);

		SMNTDEFS instructions = { 0 };
		instructions.open_thread_args = { 0 };

		instructions.open_thread_args.ClientId = &client_id;
		instructions.open_thread_args.ThreadHandle = &handle;
		instructions.open_thread_args.AccessMask = MAXIMUM_ALLOWED;
		instructions.open_thread_args.ObjectAttributes = &ObjectAttributes;


		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_OPEN_THREAD, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			*tHandle = *instructions.open_thread_args.ThreadHandle;
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL OpenThread(IN HANDLE thread_id, OUT PHANDLE thread_handle) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		HANDLE handle;
		CLIENT_ID client_id;
		client_id.UniqueThread = thread_id;
		OBJECT_ATTRIBUTES   ObjectAttributes;
		InitializeObjectAttributes(&ObjectAttributes, NULL, 0, NULL, NULL);

		SMNTDEFS instructions = { 0 };
		instructions.open_thread_args = { 0 };

		instructions.open_thread_args.ClientId = &client_id;
		instructions.open_thread_args.ThreadHandle = &handle;
		instructions.open_thread_args.AccessMask = MAXIMUM_ALLOWED;
		instructions.open_thread_args.ObjectAttributes = &ObjectAttributes;


		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_OPEN_THREAD, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			*thread_handle = *instructions.open_thread_args.ThreadHandle;
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL ResumeThread(IN HANDLE thread_handle, OUT PULONG suspendCount) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		SMNTDEFS instructions = { 0 };
		instructions.resume_thread_args = { 0 };

		instructions.resume_thread_args.ThreadHandle = thread_handle;
		instructions.resume_thread_args.SuspendCount = suspendCount;


		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_OPEN_THREAD, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			*suspendCount = *instructions.resume_thread_args.SuspendCount;
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL SuspendThread(IN HANDLE thread_handle, IN OUT PULONG previousSuspendCount) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		SMNTDEFS instructions = { 0 };
		instructions.suspend_thread_args = { 0 };

		instructions.suspend_thread_args.ThreadHandle = thread_handle;
		instructions.suspend_thread_args.PreviousSuspendCount = previousSuspendCount;


		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_OPEN_THREAD, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			*previousSuspendCount = *instructions.suspend_thread_args.PreviousSuspendCount;
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL QueryInformationThread(IN HANDLE thread_handle, IN THREAD_INFO_CLASS threadInformationClass, IN OUT PVOID threadInformationBuffer, IN ULONG threadInformationBufferLength, OPTIONAL IN OUT PULONG returnLength) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		ULONG bytes;
		SMNTDEFS instructions = { 0 };
		instructions.queryInformationThread_args.ThreadHandle = thread_handle;
		instructions.queryInformationThread_args.ThreadInformationClass = threadInformationClass;
		instructions.queryInformationThread_args.ThreadInformation = threadInformationBuffer;
		instructions.queryInformationThread_args.ThreadInformationLength = threadInformationBufferLength;
		if (returnLength != NULL)
			instructions.queryInformationThread_args.ReturnLength = returnLength;
		else
			instructions.queryInformationThread_args.ReturnLength = &bytes;

		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_QUERY_INFORMATION_THREAD, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			return TRUE;
		}
		else
			return FALSE;
	}

	/*** Sync ***/
	BOOL WaitForSingleObject(IN HANDLE process_handle, IN ULONG timeoutMilliseconds, IN BOOLEAN alertable, OUT PULONG status) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		NTSTATUS _status = 0;
		LARGE_INTEGER timeout;
		timeout.QuadPart = timeoutMilliseconds;

		NTWAITFORSINGLEOBJECT_ARGS instructions = { 0 };
		instructions.Handle = process_handle;
		instructions.Alertable = alertable;
		instructions.Timeout = &timeout;
		instructions.Status = &_status;

		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_WAIT_FOR_SINGLE_OBJECT, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			*status = _status;
			return TRUE;
		}
		else
			return FALSE;
	}
	
	/*** Sm Ex ***/
	BOOL CloseObjectByPointer(IN HANDLE handle) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		ObCloseObjectByPointer_ARGS instructions = { 0 };
		instructions.Handle = handle;

		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_CLOSE_OBJECT, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			return TRUE;
		}
		else
			return FALSE;
	}
	BOOL OpenObjectByPointer(IN HANDLE id, IN ACCESS_MASK access_mask, OUT PHANDLE handle) {
		if (hDriver == INVALID_HANDLE_VALUE)
			return FALSE;

		ObOpenObjectByPointer_ARGS instructions = { 0 };
		instructions.Id = id;
		instructions.Access = access_mask;
		instructions.Handle = handle;

		// send code to our driver with the arguments
		if (DeviceIoControl(hDriver, SM_CTL_OPEN_OBJECT, &instructions,
			sizeof(instructions), &instructions, sizeof(instructions), 0, 0)) {
			return TRUE;
		}
		else
			return FALSE;
	}

private:
	/*** Helpers ***/
	void CopySpiToPi(IN OUT PSystemProcessInfo pi, IN PSYSTEM_PROCESS_INFO spi) {
		pi->NextEntryOffset = spi->NextEntryOffset;
		pi->NumberOfThreads = spi->NumberOfThreads;
		pi->WorkingSetPrivateSize = spi->WorkingSetPrivateSize;
		pi->HardFaultCount = spi->HardFaultCount;
		pi->NumberOfThreadsHighWatermark = spi->NumberOfThreadsHighWatermark;
		pi->CycleTime = spi->CycleTime;
		pi->CreateTime = spi->CreateTime;
		pi->UserTime = spi->UserTime;
		pi->KernelTime = spi->KernelTime;
		wcscpy_s(pi->ImageName, MAX_NAME_LENGTH, spi->ImageName.Buffer);
		pi->BasePriority = spi->BasePriority;
		pi->UniqueProcessId = spi->UniqueProcessId;
		pi->InheritedFromUniqueProcessId = spi->InheritedFromUniqueProcessId;
		pi->HandleCount = spi->HandleCount;
		pi->SessionId = spi->SessionId;
		pi->UniqueProcessKey = spi->UniqueProcessKey;
		pi->PeakVirtualSize = spi->PeakVirtualSize;
		pi->VirtualSize = spi->VirtualSize;
		pi->PageFaultCount = spi->PageFaultCount;
		pi->PeakWorkingSetSize = spi->PeakWorkingSetSize;
		pi->WorkingSetSize = spi->WorkingSetSize;
		pi->QuotaPeakPagedPoolUsage = spi->QuotaPeakPagedPoolUsage;
		pi->QuotaPagedPoolUsage = spi->QuotaPagedPoolUsage;
		pi->QuotaPeakNonPagedPoolUsage = spi->QuotaPeakNonPagedPoolUsage;
		pi->QuotaNonPagedPoolUsage = spi->QuotaNonPagedPoolUsage;
		pi->PagefileUsage = spi->PagefileUsage;
		pi->PeakPagefileUsage = spi->PeakPagefileUsage;
		pi->PrivatePageCount = spi->PrivatePageCount;
		pi->ReadOperationCount = spi->ReadOperationCount;
		pi->WriteOperationCount = spi->WriteOperationCount;
		pi->OtherOperationCount = spi->OtherOperationCount;
		pi->ReadTransferCount = spi->ReadTransferCount;
		pi->WriteTransferCount = spi->WriteTransferCount;
		pi->OtherTransferCount = spi->OtherTransferCount;
		memcpy(pi->Threads, spi->Threads, sizeof(SYSTEM_THREAD_INFORMATION) * spi->NumberOfThreads);
	}
};
