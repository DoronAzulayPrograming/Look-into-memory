// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"

SmKdInterface Driver;

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        break;
    case DLL_THREAD_ATTACH:
        break;
    case DLL_THREAD_DETACH:
        break;
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

bool Attach()
{
	AllocConsole();
	FILE* _ssttddoouutt;
	freopen_s(&_ssttddoouutt, "CONOUT$", "w", stdout);

	printf("[ ]: Link to driver...\n");
	Driver = SmKdInterface("\\\\.\\SmkdNt");
	if (Driver.hDriver != INVALID_HANDLE_VALUE)
		printf("[+]: driver linked successfully.\n");
	else
		printf("[-]: ERROR.!!! driver NOT linked.\n");

	return Driver.hDriver != INVALID_HANDLE_VALUE;
}
void Detach() {
	printf("[ ]: Release linked driver...\n");
	Driver = SmKdInterface();
	printf("[+]: driver release successfully.\n");
}

bool IsProcessValid(IN SM_Pointer process_handle)
{
	printf("[ ]: IsProcessValid...\n");
	if (process_handle == nullptr) return FALSE;

	ULONG status;
	if (Driver.WaitForSingleObject(process_handle, 0, FALSE, &status)) {
		if (status == WAIT_FAILED) return FALSE;
		return status == WAIT_TIMEOUT;
	} else return FALSE;
}
bool IsProcessWow64(IN SM_Pointer process_id, OUT BOOL* isWow64) {
	printf("[ ]: IsProcessWow64...\n");
	if (Driver.IsProcessWow64(process_id, isWow64)) return isWow64;
	else return false;
}

bool __stdcall EnumerateProcesses(OUT EnumerateProcessCallback callbackProcess) {
	if (callbackProcess == nullptr) return false;

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
		durum = Driver.QuerySystemInformation(SystemProcessInformation, qmemptr, qmemsize, &bytes);
		if (!durum)
		{
			qmemsize = qmemsize * 2; // increase qmemsize for next memory alloc
			free(qmemptr); // free memory
		}
	} while (!durum); // resize memory
	spi = (PSYSTEM_PROCESS_INFO)qmemptr;

	int index = 0;
	// runing on the list and compare pid to find the match pid
	while (index++ < 1000)
	{
		if (spi->ImageName.Length > 0 && spi->ImageName.Buffer > 0) {
			if (wcsstr(spi->ImageName.Buffer, L".exe"))
			{
				EnumerateProcessData data = { };

				data.Id = spi->UniqueProcessId;
				data.VirtualSize = spi->VirtualSize;
				PVOID address;
				Driver.IsProcessWow64(data.Id, &data.IsWow64, data.Path, &data.Address);
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
bool GetProccessData(IN const char* module_name, OUT PProcessData process_data) {
	printf("[ ]: GetProcessData of: %s ...\n", module_name);
	return Driver.GetProcessData(module_name, process_data);
}

bool OpenRemoteProcess(IN SM_Pointer process_id, OUT PSM_Pointer process_handle) {
	printf("[ ]: OpenRemoteProcess of pid: %d ...\n", (ULONG)process_id);
	return Driver.OpenProcess(process_id, process_handle);
}
bool ResumeRemoteProcess(IN SM_Pointer process_handle) {
	printf("[ ]: ResumeRemoteProcess of ph: %#016x ...\n", process_handle);
	return Driver.ResumeProcess(process_handle);
}
bool SuspendRemoteProcess(IN SM_Pointer process_handle) {
	printf("[ ]: SuspendRemoteProcess of ph: %#016x ...\n", process_handle);
	return Driver.SuspendProcess(process_handle);
}
bool TerminateRemoteProcess(IN SM_Pointer process_handle, int method) {
	printf("[ ]: TerminateRemoteProcess of ph: %#016x ...\n", process_handle);
	return Driver.TerminateProcess(process_handle, 0, (TERMINATION_METHOD)method);
}

bool OpenRemoteThread(IN SM_Pointer thread_id, OUT PSM_Pointer thread_handle) {
	return Driver.OpenThread(thread_id, thread_handle);
}
bool ResumeRemoteThread(IN SM_Pointer thread_handle, OUT PULONG suspend_count) {
	return Driver.ResumeThread(thread_handle, suspend_count);
}
bool SuspendRemoteThread(IN SM_Pointer thread_handle, IN OUT PULONG previous_suspend_count) {
	return Driver.SuspendThread(thread_handle, previous_suspend_count);
}

bool EnumerateRemoteModules(IN SM_Pointer process_id, OUT EnumerateRemoteModulesCallback callbackModule) {
    printf("[ ]: EnumerateRemoteModules...\n");
    const auto moduleEnumerator = [&](EnumerateRemoteModuleData& data)
    {
        if (callbackModule != nullptr)
        {
            callbackModule(&data);
        }
    };
    return Driver.EnumerateRemoteModulesNative(process_id, moduleEnumerator);
}
bool __stdcall EnumerateRemoteSectionsAndModules1(IN SM_Pointer process, OUT EnumerateRemoteSectionsCallback callbackSection, OUT EnumerateRemoteModulesCallback callbackModule)
{
	printf("[ ]: EnumerateRemoteSectionsAndModules...\n");
	if (callbackSection == nullptr && callbackModule == nullptr)
	{
		return false;
	}
	BOOL isWow64;
	if(!Driver.IsProcessWow64(DecodeHandle(process), &isWow64)) return false;

	std::vector<EnumerateRemoteSectionData> sections;

	MEMORY_BASIC_INFORMATION memory = { };
	memory.RegionSize = 0x0000;
	size_t address = 0;

	while (Driver.QueryVirtualMemory(process, (PVOID)address, &memory) != 0 && address + memory.RegionSize > address)
	{
		if (memory.State == MEM_COMMIT)
		{
			EnumerateRemoteSectionData section = {};
			section.BaseAddress = memory.BaseAddress;
			section.Size = memory.RegionSize;

			section.Protection = SectionProtection::NoAccess;
			if ((memory.Protect & PAGE_EXECUTE) == PAGE_EXECUTE) section.Protection |= SectionProtection::Execute;
			if ((memory.Protect & PAGE_EXECUTE_READ) == PAGE_EXECUTE_READ) section.Protection |= SectionProtection::Execute | SectionProtection::Read;
			if ((memory.Protect & PAGE_EXECUTE_READWRITE) == PAGE_EXECUTE_READWRITE) section.Protection |= SectionProtection::Execute | SectionProtection::Read | SectionProtection::Write;
			if ((memory.Protect & PAGE_EXECUTE_WRITECOPY) == PAGE_EXECUTE_WRITECOPY) section.Protection |= SectionProtection::Execute | SectionProtection::Read | SectionProtection::CopyOnWrite;
			if ((memory.Protect & PAGE_READONLY) == PAGE_READONLY) section.Protection |= SectionProtection::Read;
			if ((memory.Protect & PAGE_READWRITE) == PAGE_READWRITE) section.Protection |= SectionProtection::Read | SectionProtection::Write;
			if ((memory.Protect & PAGE_WRITECOPY) == PAGE_WRITECOPY) section.Protection |= SectionProtection::Read | SectionProtection::CopyOnWrite;
			if ((memory.Protect & PAGE_GUARD) == PAGE_GUARD) section.Protection |= SectionProtection::Guard;

			switch (memory.Type)
			{
			case MEM_IMAGE:
				section.Type = SectionType::Image;
				break;
			case MEM_MAPPED:
				section.Type = SectionType::Mapped;
				break;
			case MEM_PRIVATE:
				section.Type = SectionType::Private;
				break;
			}

			section.Category = section.Type == SectionType::Private ? SectionCategory::HEAP : SectionCategory::Unknown;

			sections.push_back(section);
		}
		address = reinterpret_cast<size_t>(memory.BaseAddress) + memory.RegionSize;
	}

	const auto moduleEnumerator = [&](EnumerateRemoteModuleData& data)
	{
		if (callbackModule != nullptr)
		{
			callbackModule(&data);
		}

		if (callbackSection != nullptr)
		{
			IMAGE_DOS_HEADER imageDosHeader = {};
			auto it = std::lower_bound(std::begin(sections), std::end(sections), static_cast<LPVOID>(data.BaseAddress), [&sections](const auto& lhs, const LPVOID& rhs)
			{
				return lhs.BaseAddress < rhs;
			});
			if (isWow64) {

				IMAGE_NT_HEADERS32 imageNtHeaders = {};

				if (!Driver.ReadVirtualMemory(process, data.BaseAddress, &imageDosHeader, sizeof(IMAGE_DOS_HEADER), NULL)
					|| !Driver.ReadVirtualMemory(process, PUCHAR(data.BaseAddress) + imageDosHeader.e_lfanew, &imageNtHeaders, sizeof(IMAGE_NT_HEADERS32), NULL))
				{
					return;
				}

				std::vector<IMAGE_SECTION_HEADER> sectionHeaders(imageNtHeaders.FileHeader.NumberOfSections);
				Driver.ReadVirtualMemory(process, PUCHAR(data.BaseAddress) + imageDosHeader.e_lfanew + sizeof(IMAGE_NT_HEADERS32), sectionHeaders.data(), imageNtHeaders.FileHeader.NumberOfSections * sizeof(IMAGE_SECTION_HEADER), NULL);
				for (auto&& sectionHeader : sectionHeaders)
				{
					const auto sectionAddress = reinterpret_cast<size_t>(data.BaseAddress) + sectionHeader.VirtualAddress;

					for (; it != std::end(sections); ++it)
					{
						auto&& section = *it;

						if (sectionAddress >= reinterpret_cast<size_t>(section.BaseAddress)
							&& sectionAddress < reinterpret_cast<size_t>(section.BaseAddress) + static_cast<size_t>(section.Size)
							&& sectionHeader.VirtualAddress + sectionHeader.Misc.VirtualSize <= data.Size)
						{
							if ((sectionHeader.Characteristics & IMAGE_SCN_CNT_CODE) == IMAGE_SCN_CNT_CODE)
							{
								section.Category = SectionCategory::CODE;
							}
							else if (sectionHeader.Characteristics & (IMAGE_SCN_CNT_INITIALIZED_DATA | IMAGE_SCN_CNT_UNINITIALIZED_DATA))
							{
								section.Category = SectionCategory::DATA;
							}

							try
							{
								/*IMAGE_OPTIONAL_HEADER imageOptionalHeader;
								if (Driver.ReadVirtualMemory(process, (PVOID)sectionHeader.PointerToRawData, &imageOptionalHeader, sizeof(IMAGE_OPTIONAL_HEADER), NULL)) {
									printf("%#010x\n", imageOptionalHeader.ImageBase);
								}*/
								// Copy the name because it is not null padded.
								printf("name: %s\n", (char*)sectionHeader.Name);
								char buffer[IMAGE_SIZEOF_SHORT_NAME + 1] = { 0 };
								std::memcpy(buffer, sectionHeader.Name, IMAGE_SIZEOF_SHORT_NAME);
								MultiByteToUnicode(buffer, section.Name, IMAGE_SIZEOF_SHORT_NAME);
							}
							catch (std::range_error&)
							{
								std::memset(section.Name, 0, sizeof(section.Name));
							}
							std::memcpy(section.ModulePath, data.Path, MAX_PATH_LENGTH);

							break;
						}
					}
				}

			}
			else {
				IMAGE_NT_HEADERS imageNtHeaders = {};

				if (!Driver.ReadVirtualMemory(process, data.BaseAddress, &imageDosHeader, sizeof(IMAGE_DOS_HEADER), NULL)
					|| !Driver.ReadVirtualMemory(process, PUCHAR(data.BaseAddress) + imageDosHeader.e_lfanew, &imageNtHeaders, sizeof(IMAGE_NT_HEADERS), NULL))
				{
					return;
				}

				std::vector<IMAGE_SECTION_HEADER> sectionHeaders(imageNtHeaders.FileHeader.NumberOfSections);
				Driver.ReadVirtualMemory(process, PUCHAR(data.BaseAddress) + imageDosHeader.e_lfanew + sizeof(IMAGE_NT_HEADERS), sectionHeaders.data(), imageNtHeaders.FileHeader.NumberOfSections * sizeof(IMAGE_SECTION_HEADER), NULL);
				for (auto&& sectionHeader : sectionHeaders)
				{
					const auto sectionAddress = reinterpret_cast<size_t>(data.BaseAddress) + sectionHeader.VirtualAddress;

					for (; it != std::end(sections); ++it)
					{
						auto&& section = *it;

						if (sectionAddress >= reinterpret_cast<size_t>(section.BaseAddress)
							&& sectionAddress < reinterpret_cast<size_t>(section.BaseAddress) + static_cast<size_t>(section.Size)
							&& sectionHeader.VirtualAddress + sectionHeader.Misc.VirtualSize <= data.Size)
						{
							if ((sectionHeader.Characteristics & IMAGE_SCN_CNT_CODE) == IMAGE_SCN_CNT_CODE)
							{
								section.Category = SectionCategory::CODE;
							}
							else if (sectionHeader.Characteristics & (IMAGE_SCN_CNT_INITIALIZED_DATA | IMAGE_SCN_CNT_UNINITIALIZED_DATA))
							{
								section.Category = SectionCategory::DATA;
							}

							try
							{
								char buffer[IMAGE_SIZEOF_SHORT_NAME + 1] = { 0 };
								std::memcpy(buffer, sectionHeader.Name, IMAGE_SIZEOF_SHORT_NAME);
								MultiByteToUnicode(buffer, section.Name, IMAGE_SIZEOF_SHORT_NAME);
							}
							catch (std::range_error&)
							{
								std::memset(section.Name, 0, sizeof(section.Name));
							}
							std::memcpy(section.ModulePath, data.Path, MAX_PATH_LENGTH);

							break;
						}
					}
				}
			}
		}
	};

	bool status = Driver.EnumerateRemoteModulesNative(DecodeHandle(process), moduleEnumerator);

	if (callbackSection != nullptr)
	{
		for (auto&& section : sections)
		{
			callbackSection(&section);
		}
	}

	return status;
}
bool MmCpy(SM_Pointer source_process_id, PVOID read_address, SM_Pointer target_process_id, PVOID target_address, ULONG size) {
	return Driver.MemCopy(source_process_id, read_address, target_process_id, target_address, size, NULL);
}

bool EnumerateRemoteSectionsAndModules32(IN SM_Pointer process, OUT EnumerateRemoteSectionsCallback callbackSection, OUT EnumerateRemoteModulesCallback callbackModule) {
	std::vector<EnumerateRemoteSectionData> sections;

	MEMORY_BASIC_INFORMATION memory = { };
	memory.RegionSize = 0x0000;
	size_t address = 0;

	while (Driver.QueryVirtualMemory(process, (PVOID)address, &memory) != 0 && address + memory.RegionSize > address)
	{
		if (memory.State == MEM_COMMIT)
		{
			EnumerateRemoteSectionData section = {};
			section.BaseAddress = memory.BaseAddress;
			section.Size = memory.RegionSize;

			section.Protection = SectionProtection::NoAccess;
			if ((memory.Protect & PAGE_EXECUTE) == PAGE_EXECUTE) section.Protection |= SectionProtection::Execute;
			if ((memory.Protect & PAGE_EXECUTE_READ) == PAGE_EXECUTE_READ) section.Protection |= SectionProtection::Execute | SectionProtection::Read;
			if ((memory.Protect & PAGE_EXECUTE_READWRITE) == PAGE_EXECUTE_READWRITE) section.Protection |= SectionProtection::Execute | SectionProtection::Read | SectionProtection::Write;
			if ((memory.Protect & PAGE_EXECUTE_WRITECOPY) == PAGE_EXECUTE_WRITECOPY) section.Protection |= SectionProtection::Execute | SectionProtection::Read | SectionProtection::CopyOnWrite;
			if ((memory.Protect & PAGE_READONLY) == PAGE_READONLY) section.Protection |= SectionProtection::Read;
			if ((memory.Protect & PAGE_READWRITE) == PAGE_READWRITE) section.Protection |= SectionProtection::Read | SectionProtection::Write;
			if ((memory.Protect & PAGE_WRITECOPY) == PAGE_WRITECOPY) section.Protection |= SectionProtection::Read | SectionProtection::CopyOnWrite;
			if ((memory.Protect & PAGE_GUARD) == PAGE_GUARD) section.Protection |= SectionProtection::Guard;

			switch (memory.Type)
			{
			case MEM_IMAGE:
				section.Type = SectionType::Image;
				break;
			case MEM_MAPPED:
				section.Type = SectionType::Mapped;
				break;
			case MEM_PRIVATE:
				section.Type = SectionType::Private;
				break;
			}

			section.Category = section.Type == SectionType::Private ? SectionCategory::HEAP : SectionCategory::Unknown;

			sections.push_back(section);
		}
		address = reinterpret_cast<size_t>(memory.BaseAddress) + memory.RegionSize;
	}

	const auto moduleEnumerator = [&](EnumerateRemoteModuleData& data)
	{
		if (callbackModule != nullptr)
		{
			callbackModule(&data);
		}

		if (callbackSection != nullptr)
		{
			IMAGE_DOS_HEADER imageDosHeader = {};
			auto it = std::lower_bound(std::begin(sections), std::end(sections), static_cast<LPVOID>(data.BaseAddress), [&sections](const auto& lhs, const LPVOID& rhs)
				{
					return lhs.BaseAddress < rhs;
				});
			IMAGE_NT_HEADERS32 imageNtHeaders = {};

			if (!Driver.ReadVirtualMemory(process, data.BaseAddress, &imageDosHeader, sizeof(IMAGE_DOS_HEADER), NULL)
				|| !Driver.ReadVirtualMemory(process, PUCHAR(data.BaseAddress) + imageDosHeader.e_lfanew, &imageNtHeaders, sizeof(IMAGE_NT_HEADERS32), NULL))
			{
				return;
			}

			std::vector<IMAGE_SECTION_HEADER> sectionHeaders(imageNtHeaders.FileHeader.NumberOfSections);
			Driver.ReadVirtualMemory(process, PUCHAR(data.BaseAddress) + imageDosHeader.e_lfanew + sizeof(IMAGE_NT_HEADERS32), sectionHeaders.data(), imageNtHeaders.FileHeader.NumberOfSections * sizeof(IMAGE_SECTION_HEADER), NULL);
			for (auto&& sectionHeader : sectionHeaders)
			{
				const auto sectionAddress = reinterpret_cast<size_t>(data.BaseAddress) + sectionHeader.VirtualAddress;

				for (; it != std::end(sections); ++it)
				{
					auto&& section = *it;

					if (sectionAddress >= reinterpret_cast<size_t>(section.BaseAddress)
						&& sectionAddress < reinterpret_cast<size_t>(section.BaseAddress) + static_cast<size_t>(section.Size)
						&& sectionHeader.VirtualAddress + sectionHeader.Misc.VirtualSize <= data.Size)
					{
						if ((sectionHeader.Characteristics & IMAGE_SCN_CNT_CODE) == IMAGE_SCN_CNT_CODE)
						{
							section.Category = SectionCategory::CODE;
						}
						else if (sectionHeader.Characteristics & (IMAGE_SCN_CNT_INITIALIZED_DATA | IMAGE_SCN_CNT_UNINITIALIZED_DATA))
						{
							section.Category = SectionCategory::DATA;
						}

						try
						{
							char buffer[IMAGE_SIZEOF_SHORT_NAME + 1] = { 0 };
							std::memcpy(buffer, sectionHeader.Name, IMAGE_SIZEOF_SHORT_NAME);
							MultiByteToUnicode(buffer, section.Name, IMAGE_SIZEOF_SHORT_NAME);
						}
						catch (std::range_error&)
						{
							std::memset(section.Name, 0, sizeof(section.Name));
						}
						std::memcpy(section.ModulePath, data.Path, MAX_PATH_LENGTH);

						break;
					}
				}
			}
		}
	};

	bool status = Driver.EnumerateRemoteModulesNative(DecodeHandle(process), moduleEnumerator);

	if (callbackSection != nullptr)
	{
		for (auto&& section : sections)
		{
			callbackSection(&section);
		}
	}
	return status;
}
bool EnumerateRemoteSectionsAndModules64(IN SM_Pointer process, OUT EnumerateRemoteSectionsCallback callbackSection, OUT EnumerateRemoteModulesCallback callbackModule) {
	std::vector<EnumerateRemoteSectionData> sections;

	MEMORY_BASIC_INFORMATION memory = { };
	memory.RegionSize = 0x0000;
	size_t address = 0;

	while (Driver.QueryVirtualMemory(process, (PVOID)address, &memory) != 0 && address + memory.RegionSize > address)
	{
		if (memory.State == MEM_COMMIT)
		{
			EnumerateRemoteSectionData section = {};
			section.BaseAddress = memory.BaseAddress;
			section.Size = memory.RegionSize;

			section.Protection = SectionProtection::NoAccess;
			if ((memory.Protect & PAGE_EXECUTE) == PAGE_EXECUTE) section.Protection |= SectionProtection::Execute;
			if ((memory.Protect & PAGE_EXECUTE_READ) == PAGE_EXECUTE_READ) section.Protection |= SectionProtection::Execute | SectionProtection::Read;
			if ((memory.Protect & PAGE_EXECUTE_READWRITE) == PAGE_EXECUTE_READWRITE) section.Protection |= SectionProtection::Execute | SectionProtection::Read | SectionProtection::Write;
			if ((memory.Protect & PAGE_EXECUTE_WRITECOPY) == PAGE_EXECUTE_WRITECOPY) section.Protection |= SectionProtection::Execute | SectionProtection::Read | SectionProtection::CopyOnWrite;
			if ((memory.Protect & PAGE_READONLY) == PAGE_READONLY) section.Protection |= SectionProtection::Read;
			if ((memory.Protect & PAGE_READWRITE) == PAGE_READWRITE) section.Protection |= SectionProtection::Read | SectionProtection::Write;
			if ((memory.Protect & PAGE_WRITECOPY) == PAGE_WRITECOPY) section.Protection |= SectionProtection::Read | SectionProtection::CopyOnWrite;
			if ((memory.Protect & PAGE_GUARD) == PAGE_GUARD) section.Protection |= SectionProtection::Guard;

			switch (memory.Type)
			{
			case MEM_IMAGE:
				section.Type = SectionType::Image;
				break;
			case MEM_MAPPED:
				section.Type = SectionType::Mapped;
				break;
			case MEM_PRIVATE:
				section.Type = SectionType::Private;
				break;
			}

			section.Category = section.Type == SectionType::Private ? SectionCategory::HEAP : SectionCategory::Unknown;

			sections.push_back(section);
		}
		address = reinterpret_cast<size_t>(memory.BaseAddress) + memory.RegionSize;
	}

	const auto moduleEnumerator = [&](EnumerateRemoteModuleData& data)
	{
		if (callbackModule != nullptr)
		{
			callbackModule(&data);
		}

		if (callbackSection != nullptr)
		{
			auto it = std::lower_bound(std::begin(sections), std::end(sections), static_cast<LPVOID>(data.BaseAddress), [&sections](const auto& lhs, const LPVOID& rhs)
			{
					return lhs.BaseAddress < rhs;
			});

			IMAGE_DOS_HEADER imageDosHeader = {};
			IMAGE_NT_HEADERS imageNtHeaders = {};

			if (!Driver.ReadVirtualMemory(process, data.BaseAddress, &imageDosHeader, sizeof(IMAGE_DOS_HEADER), NULL)
				|| !Driver.ReadVirtualMemory(process, PUCHAR(data.BaseAddress) + imageDosHeader.e_lfanew, &imageNtHeaders, sizeof(IMAGE_NT_HEADERS), NULL))
			{
				return;
			}

			std::vector<IMAGE_SECTION_HEADER> sectionHeaders(imageNtHeaders.FileHeader.NumberOfSections);
			Driver.ReadVirtualMemory(process, PUCHAR(data.BaseAddress) + imageDosHeader.e_lfanew + sizeof(IMAGE_NT_HEADERS), sectionHeaders.data(), imageNtHeaders.FileHeader.NumberOfSections * sizeof(IMAGE_SECTION_HEADER), NULL);
			for (auto&& sectionHeader : sectionHeaders)
			{
				const auto sectionAddress = reinterpret_cast<size_t>(data.BaseAddress) + sectionHeader.VirtualAddress;

				for (; it != std::end(sections); ++it)
				{
					auto&& section = *it;

					if (sectionAddress >= reinterpret_cast<size_t>(section.BaseAddress)
						&& sectionAddress < reinterpret_cast<size_t>(section.BaseAddress) + static_cast<size_t>(section.Size)
						&& sectionHeader.VirtualAddress + sectionHeader.Misc.VirtualSize <= data.Size)
					{
						if ((sectionHeader.Characteristics & IMAGE_SCN_CNT_CODE) == IMAGE_SCN_CNT_CODE)
						{
							section.Category = SectionCategory::CODE;
						}
						else if (sectionHeader.Characteristics & (IMAGE_SCN_CNT_INITIALIZED_DATA | IMAGE_SCN_CNT_UNINITIALIZED_DATA))
						{
							section.Category = SectionCategory::DATA;
						}

						try
						{
							char buffer[IMAGE_SIZEOF_SHORT_NAME + 1] = { 0 };
							std::memcpy(buffer, sectionHeader.Name, IMAGE_SIZEOF_SHORT_NAME);
							MultiByteToUnicode(buffer, section.Name, IMAGE_SIZEOF_SHORT_NAME);
						}
						catch (std::range_error&)
						{
							std::memset(section.Name, 0, sizeof(section.Name));
						}
						std::memcpy(section.ModulePath, data.Path, MAX_PATH_LENGTH);

						break;
					}
				}
			}
		}
	};

	bool status = Driver.EnumerateRemoteModulesNative(DecodeHandle(process), moduleEnumerator);

	if (callbackSection != nullptr)
	{
		for (auto&& section : sections)
		{
			callbackSection(&section);
		}
	}
	return status;
}
bool __stdcall EnumerateRemoteSectionsAndModules(IN SM_Pointer process, OUT EnumerateRemoteSectionsCallback callbackSection, OUT EnumerateRemoteModulesCallback callbackModule)
{
	printf("[ ]: EnumerateRemoteSectionsAndModules...\n");
	if (callbackSection == nullptr && callbackModule == nullptr)
	{
		return false;
	}

	BOOL isWow64;
	if (!Driver.IsProcessWow64(DecodeHandle(process), &isWow64)) return false;

	if (isWow64)
		return EnumerateRemoteSectionsAndModules32(process, callbackSection, callbackModule);

	return EnumerateRemoteSectionsAndModules64(process, callbackSection, callbackModule);
}

bool __stdcall HexDumpBytes(PVOID start_address, BYTE* bytesBuffer, int len, OUT EnumerateDumpCallback callbackDump) {
	printf("[ ]: EnumerateDump...\n");
	if (start_address <= 0 || bytesBuffer <= 0 || len <= 0) return false;
	if (callbackDump == nullptr) return false;

	char* buffer = (char*)bytesBuffer;
	unsigned long address = (unsigned long)start_address;
	char c;
	for (int j = 0; j < len; j += 16)
	{
		EnumerateDumpData data = { 0 };
		data.Address = (SM_Pointer)address;
		int index = 0;
		for (int i = j; i < j + 16; i++)
		{
			if (i < len) data.HexBytes[index++] = (unsigned int)(unsigned char)buffer[i];
			else data.HexBytes[index++] = 0;
		}

		index = 0;
		for (int i = j; i < j + 16; i++)
		{
			if (buffer[i] < 32) data.TextBytes[index++] = '.';
			else data.TextBytes[index++] = buffer[i];
		}

		callbackDump(&data);
		address += 16;
	}

	return true;
}


