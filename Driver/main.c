#include "main.h"

NTSTATUS SmGetProcessInformation(IN UNICODE_STRING process_name, OUT SYSTEM_PROCESS_INFORMATION* system_process_information);

NTSTATUS ProccessModulesSnapshot32(IN PPEB32 pPEB32, OUT ULONG* modules_count);
NTSTATUS ProccessModulesSnapshot64(IN PPEB pPeb, OUT ULONG* modules_count);
NTSTATUS ProccessModulesSnapshot(IN OUT PSNAPSHOTPROCESSMODULES_ARGS args);
NTSTATUS GetProccessModules(IN OUT PGETPROCESSMODULES_ARGS args);

NTSTATUS MmCpyVm(IN OUT PNTMMCOPYVM_ARGS args);
NTSTATUS IsWow64Process(IN OUT PISWOW64PROCESS_ARGS args);
NTSTATUS QuerySystemInformation(IN OUT PNTQUERYSYSTEMINFORMATION_ARGS args);

ULONG PreviousModeOffset = 0;
KPROCESSOR_MODE KeSetPreviousMode(KPROCESSOR_MODE mode) {
    KPROCESSOR_MODE old = ExGetPreviousMode();
    *(KPROCESSOR_MODE*)((PBYTE)KeGetCurrentThread() + PreviousModeOffset) = mode;
    return old;
}

/*
* PrintIrql
*
* Purpose:
*
* Debug print current irql.
*
*/
VOID PrintIrql()
{
    KIRQL Irql;
    PSTR sIrql;

    Irql = KeGetCurrentIrql();

    switch (Irql) {

    case PASSIVE_LEVEL:
        sIrql = "PASSIVE_LEVEL";
        break;
    case APC_LEVEL:
        sIrql = "APC_LEVEL";
        break;
    case DISPATCH_LEVEL:
        sIrql = "DISPATCH_LEVEL";
        break;
    case CMCI_LEVEL:
        sIrql = "CMCI_LEVEL";
        break;
    case CLOCK_LEVEL:
        sIrql = "CLOCK_LEVEL";
        break;
    case IPI_LEVEL:
        sIrql = "IPI_LEVEL";
        break;
    case HIGH_LEVEL:
        sIrql = "HIGH_LEVEL";
        break;
    default:
        sIrql = "Unknown Value";
        break;
    }

    DbgPrintEx(DPFLTR_DEFAULT_ID,
        DPFLTR_INFO_LEVEL,
        "KeGetCurrentIrql=%u(%s)\r\n",
        Irql,
        sIrql);
}

/*
* DevioctlDispatch
*
* Purpose:
*
* IRP_MJ_DEVICE_CONTROL dispatch.
*
*/
NTSTATUS DevioctlDispatch(
    _In_ struct _DEVICE_OBJECT* DeviceObject,
    _Inout_ struct _IRP* Irp
)
{
    NTSTATUS				status = STATUS_SUCCESS;
    ULONG					bytesIO = 0;
    PIO_STACK_LOCATION		stack;
    BOOLEAN					condition = FALSE;
    PSMNTDEFS               rp, wp;

    UNREFERENCED_PARAMETER(DeviceObject);

    stack = IoGetCurrentIrpStackLocation(Irp);

    do {

        if (stack == NULL) {
            status = STATUS_INTERNAL_ERROR;
            break;
        }

        rp = (PSMNTDEFS)Irp->AssociatedIrp.SystemBuffer;
        wp = (PSMNTDEFS)Irp->AssociatedIrp.SystemBuffer;
        if (rp == NULL) {
            status = STATUS_INVALID_PARAMETER;
            break;
        }

        switch (stack->Parameters.DeviceIoControl.IoControlCode) {
        case SM_CTL_OPEN_OBJECT:
            status = CoreObOpenObjectByPointer((PObOpenObjectByPointer_ARGS)Irp->AssociatedIrp.SystemBuffer);
        break;
        case SM_CTL_CLOSE_OBJECT:
            status = NtClose(((PObCloseObjectByPointer_ARGS)Irp->AssociatedIrp.SystemBuffer)->Handle);
        break;
        case SM_CTL_TERMINATE_PROCESS:
            status = CoreNtTerminateProcess(&wp->terminate_process_args);
        break;
        case SM_CTL_MMCOPY_VM:
            status = MmCpyVm(&wp->mm_copy_vm_args);
        break;
        case SM_CTL_GET_PROCESS_MODULES:
            status = GetProccessModules(&wp->get_process_modules);
        break;
        case SM_CTL_SNAP_SHOT_PROCESS_MODULES:
            status = ProccessModulesSnapshot(&wp->snap_shot_process_modules);
        break;
        case SM_CTL_IS_WOW64_PROCESS:
            status = IsWow64Process(&wp->is_wow64_process);
        break;
        case SM_CTL_OPEN_PROCESS:
            status = CoreNtOpenProcess(&wp->open_process_args);
        break;
        case SM_CTL_SUSPENDP_PROCESS:
            status = CoreNtSuspendProcess(&wp->suspend_process_args);
        break;
        case SM_CTL_RESUME_PROCESS:
            status = CoreNtResumeProcess(&wp->resume_process_args);
        break;
        case SM_CTL_QUERY_SYSTEM_INFORMATION:
            status = QuerySystemInformation(&wp->querySystemInformation_args);
        break;
        case SM_CTL_QUERY_SYSTEM_INFORMATION_EX:
            status = CoreNtQuerySystemInformationEx(&wp->querySystemInformationEx_args);
        break;
        case SM_CTL_QUERY_INFORMATION_PROCESS:
            status = CoreNtQueryInformationProcess(&wp->queryInformationProcess_args);
        break;
        case SM_CTL_SET_INFORMATION_PROCESS:
            status = CoreNtSetInformationProcess(&wp->setInformationProcess_args);
        break;
        case SM_CTL_FLUSH_INSTRUCTION_CACHE:
            status = CoreNtFlushInstructionCache(&wp->flushInstructioncache_args);
        break;
        case SM_CTL_ALLOCATE_VIRTUAL_MEMORY:
            status = CoreNtAllocateVirtualMemory(&wp->allocateVirtualMemory_args);
        break;
        case SM_CTL_FLUSH_VIRTUAL_MEMORY:
            status = CoreNtFlushVirtualMemory(&wp->flushVirtualMemory_args);
        break;
        case SM_CTL_FREE_VIRTUAL_MEMORY:
            status = CoreNtFreeVirtualMemory(&wp->freeVirtualMemory_args);
        break;
        case SM_CTL_LOCK_VIRTUAL_MEMORY:
            status = CoreNtLockVirtualMemory(&wp->lockVirtualMemory_args);
        break;
        case SM_CTL_UNLOCK_VIRTUAL_MEMORY:
            status = CoreNtUnlockVirtualMemory(&wp->unlockVirtualMemory_args);
        break;
        case SM_CTL_PROTECT_VIRTUAL_MEMORY:
            status = CoreNtProtectVirtualMemory(&wp->protectVirtualMemory_args);
        break;
        case SM_CTL_READ_VIRTUAL_MEMORY:
            status = CoreNtReadVirtualMemory(&wp->readVirtualMemory_args);
        break;
        case SM_CTL_WRITE_VIRTUAL_MEMORY:
            status = CoreNtWriteVirtualMemory(&wp->writeVirtualMemory_args);
        break;
        case SM_CTL_QUERY_VIRTUAL_MEMORY:
            status = CoreNtQueryVirtualMemory(&wp->queryVirtualMemory_args);
        break;
        case SM_CTL_OPEN_THREAD:
            status = CoreNtOpenThread(&wp->open_thread_args);
        break;
        case SM_CTL_QUERY_INFORMATION_THREAD:
            status = CoreNtQueryInformationThread(&wp->queryInformationThread_args);
        break;
        case SM_CTL_SET_INFORMATION_THREAD:
            status = CoreNtSetInformationThread(&wp->setInformationThread_args);
        break;
        case SM_CTL_GET_CONTEXT_THREAD:
            status = CoreNtGetContextThread(&wp->getContextThread_args);
        break;
        case SM_CTL_SET_CONTEXT_THREAD:
            status = CoreNtSetContextThread(&wp->setContextThread_args);
        break;
        case SM_CTL_RESUME_THREAD:
            status = CoreNtResumeThread(&wp->resume_thread_args);
        break;
        case SM_CTL_SUSPENDP_THREAD:
            status = CoreNtSuspendThread(&wp->suspend_thread_args);
        break;
        case SM_CTL_WAIT_FOR_SINGLE_OBJECT:
            status = CoreNtWaitForSingleObject((PNTWAITFORSINGLEOBJECT_ARGS)Irp->AssociatedIrp.SystemBuffer);
        break;

        default:
            status = STATUS_INVALID_PARAMETER;
            break;
        };

    } while (condition);

    Irp->IoStatus.Status = status;
    Irp->IoStatus.Information = sizeof(PSMNTDEFS);
    IoCompleteRequest(Irp, IO_NO_INCREMENT);
    return status;
}

/*
* UnsupportedDispatch
*
* Purpose:
*
* Unused IRP_MJ_* dispatch.
*
*/
NTSTATUS UnsupportedDispatch(
    _In_ struct _DEVICE_OBJECT* DeviceObject,
    _Inout_ struct _IRP* Irp
)
{
    UNREFERENCED_PARAMETER(DeviceObject);

    Irp->IoStatus.Status = STATUS_NOT_SUPPORTED;
    IoCompleteRequest(Irp, IO_NO_INCREMENT);
    return STATUS_NOT_SUPPORTED;
}

/*
* CreateDispatch
*
* Purpose:
*
* IRP_MJ_CREATE dispatch.
*
*/
NTSTATUS CreateDispatch(
    _In_ struct _DEVICE_OBJECT* DeviceObject,
    _Inout_ struct _IRP* Irp
)
{
    NTSTATUS status = Irp->IoStatus.Status;
    UNREFERENCED_PARAMETER(DeviceObject);

    IoCompleteRequest(Irp, IO_NO_INCREMENT);
    return status;
}

/*
* CloseDispatch
*
* Purpose:
*
* IRP_MJ_CLOSE dispatch.
*
*/
NTSTATUS CloseDispatch(
    _In_ struct _DEVICE_OBJECT* DeviceObject,
    _Inout_ struct _IRP* Irp
)
{
    NTSTATUS status = Irp->IoStatus.Status;
    UNREFERENCED_PARAMETER(DeviceObject);

    IoCompleteRequest(Irp, IO_NO_INCREMENT);
    return status;
}

/*
* DriverInitialize
*
* Purpose:
*
* Driver main.
*
*/
NTSTATUS DriverInitialize(
    _In_  struct _DRIVER_OBJECT* DriverObject,
    _In_  PUNICODE_STRING RegistryPath
)
{
    NTSTATUS        status;
    UNICODE_STRING  SymLink, DevName;
    PDEVICE_OBJECT  devobj;
    ULONG           t;
    DbgPrintEx(0, 0, "Smdkd Initialize\n");
    //RegistryPath is NULL
    UNREFERENCED_PARAMETER(RegistryPath);

    RtlInitUnicodeString(&DevName, L"\\Device\\SmkdNt");
    status = IoCreateDevice(DriverObject, 0, &DevName, FILE_DEVICE_UNKNOWN, FILE_DEVICE_SECURE_OPEN, TRUE, &devobj);

   
    if (!NT_SUCCESS(status)) {
        return status;
    }

    RtlInitUnicodeString(&SymLink, L"\\DosDevices\\SmkdNt");
    status = IoCreateSymbolicLink(&SymLink, &DevName);

    devobj->Flags |= DO_BUFFERED_IO;

    for (t = 0; t <= IRP_MJ_MAXIMUM_FUNCTION; t++)
        DriverObject->MajorFunction[t] = &UnsupportedDispatch;

    DriverObject->MajorFunction[IRP_MJ_DEVICE_CONTROL] = &DevioctlDispatch;
    DriverObject->MajorFunction[IRP_MJ_CREATE] = &CreateDispatch;
    DriverObject->MajorFunction[IRP_MJ_CLOSE] = &CloseDispatch;
    DriverObject->DriverUnload = NULL; //nonstandard way of driver loading, no unload

    devobj->Flags &= ~DO_DEVICE_INITIALIZING;
    return status;
}

/*
* DriverEntry
*
* Purpose:
*
* Driver base entry point.
*
*/
NTSTATUS DriverEntry(
    _In_  struct _DRIVER_OBJECT* DriverObject,
    _In_  PUNICODE_STRING RegistryPath
)
{
    NTSTATUS        status;
    UNICODE_STRING  drvName;
    DbgPrintEx(0, 0, "Smdkd Entry\n");

    /* This parameters are invalid due to nonstandard way of loading and should not be used. */
    UNREFERENCED_PARAMETER(DriverObject);
    UNREFERENCED_PARAMETER(RegistryPath);

    RtlInitUnicodeString(&drvName, L"\\Driver\\SmkdNt");
    status = IoCreateDriver(&drvName, &DriverInitialize);

    //if (!NT_SUCCESS(status)) {
    //    ANSI_STRING AS;
    //    UNICODE_STRING ModuleName = { 0 };

    //    RtlInitAnsiString(&AS, "ac_client.exe");//notepad //ac_client
    //    RtlAnsiStringToUnicodeString(&ModuleName, &AS, TRUE);

    //    SYSTEM_PROCESS_INFORMATION spi;
    //    if (NT_SUCCESS(SmGetProcessInformation(ModuleName, &spi))) {


    //    }
    //}

    return status;
}

NTSTATUS SmGetProcessInformation(IN UNICODE_STRING process_name, OUT SYSTEM_PROCESS_INFORMATION* system_process_information) {
    NTSTATUS durum = STATUS_UNSUCCESSFUL;
    ULONG qmemsize = 0x1024;
    PVOID qmemptr = 0;
    PSYSTEM_PROCESS_INFORMATION spi;

    do
    {
        qmemptr = ExAllocatePool(PagedPool, qmemsize); // alloc memory for spi
        if (qmemptr == NULL) // check memory is allocated or not.
        {
            return STATUS_UNSUCCESSFUL;
        }
        durum = ZwQuerySystemInformation(SystemProcessInformation, qmemptr, qmemsize, NULL);
        if (durum == STATUS_INFO_LENGTH_MISMATCH)
        {
            qmemsize = qmemsize * 2; // increase qmemsize for next memory alloc
            ExFreePool(qmemptr); // free memory
        }
    } while (durum == STATUS_INFO_LENGTH_MISMATCH); // resize memory
    spi = (PSYSTEM_PROCESS_INFORMATION)qmemptr;

    // runing on the list and compare pid to find the match pid
    while (1)
    {
        if (!RtlCompareUnicodeString(&process_name, &spi->ImageName, TRUE))
            break;

        if (spi->NextEntryOffset == 0)
            break;

        spi = (PSYSTEM_PROCESS_INFORMATION)((unsigned char*)spi + spi->NextEntryOffset); // next info 
    }

    memcpy(system_process_information, spi, sizeof(SYSTEM_PROCESS_INFORMATION));

    ExFreePool(qmemptr); // free memory
    return STATUS_SUCCESS;
}

/// <summary>
/// copy memory from source address to target address by size
/// </summary>
/// <param name="SourceProcess"></param>
/// <param name="SourceAddress"></param>
/// <param name="TargetProcess"></param>
/// <param name="TargetAddress"></param>
/// <param name="Size"></param>
/// <returns></returns>
NTSTATUS MmCpyVm(IN OUT PNTMMCOPYVM_ARGS args)
{
    if (((PBYTE)args->SourceAddress + args->Size < (PBYTE)args->SourceAddress) ||
        ((PBYTE)args->TargetAddress + args->Size < (PBYTE)args->TargetAddress) ||
        ((PVOID)((PBYTE)args->SourceAddress + args->Size) > MM_HIGHEST_USER_ADDRESS) ||
        ((PVOID)((PBYTE)args->TargetAddress + args->Size) > MM_HIGHEST_USER_ADDRESS)) {

        return STATUS_ACCESS_VIOLATION;
    }

    if (args->NumberOfBytesCopyed) {
        if (!ProbeUserAddress(args->NumberOfBytesCopyed, sizeof(SIZE_T), sizeof(LONG))) {
            return STATUS_ACCESS_VIOLATION;
        }
    }

    SIZE_T     bytes = 0;
    PEPROCESS  sourceProcess = 0;
    PEPROCESS  targetProcess = 0;
    NTSTATUS   status = STATUS_UNSUCCESSFUL;

    status = PsLookupProcessByProcessId(args->SourceProcessId, &sourceProcess);
    if (!NT_SUCCESS(status)) return status;

    status = PsLookupProcessByProcessId(args->TargetProcessId, &targetProcess);
    if (!NT_SUCCESS(status)) return status;

    status = MmCopyVirtualMemory(sourceProcess, args->SourceAddress, targetProcess, args->TargetAddress, args->Size, ExGetPreviousMode(), &bytes);

    if (args->NumberOfBytesCopyed) {
        if (!SafeCopy(args->NumberOfBytesCopyed, &bytes, sizeof(bytes))) {
            status = STATUS_ACCESS_VIOLATION;
        }
    }

    ObDereferenceObject(sourceProcess);
    ObDereferenceObject(targetProcess);

    return status;
}
NTSTATUS IsWow64Process(IN OUT PISWOW64PROCESS_ARGS args) {
    PEPROCESS process = 0;
    NTSTATUS status = PsLookupProcessByProcessId(args->ProcessId, &process);
    if (NT_SUCCESS(status)) {
        PVOID baseAddress;
        wchar_t path[MAX_PATH_LENGTH];
        BOOL isWow64 = FALSE;
        PPEB32 pPEB32 = (PPEB32)PsGetProcessWow64Process(process);

        KAPC_STATE ApcState;
        KeStackAttachProcess(process, &ApcState);
        if (pPEB32) {
            isWow64 = TRUE;
            baseAddress = pPEB32->ImageBaseAddress;
            wcscpy_s(path, MAX_PATH_LENGTH, (wchar_t*)((PRTL_USER_PROCESS_PARAMETERS32)pPEB32->ProcessParameters)->ImagePathName.Buffer);
        }
        else {
            PPEB pPeb = PsGetProcessPeb(process);
            if (pPeb) {
                isWow64 = FALSE;
                baseAddress = pPeb->ImageBaseAddress;
                wcscpy_s(path, MAX_PATH_LENGTH,pPeb->ProcessParameters->ImagePathName.Buffer);
            }
        }
        KeUnstackDetachProcess(&ApcState);

        SafeCopy(args->IsWow64, &isWow64, sizeof(isWow64));
        SafeCopy(args->Path, path, sizeof(wchar_t) * MAX_PATH_LENGTH);
        SafeCopy(args->BaseAddress, &baseAddress, sizeof(PVOID));
       

        ObDereferenceObject(process);
    }
    return status;
}
NTSTATUS QuerySystemInformation(IN OUT PNTQUERYSYSTEMINFORMATION_ARGS args) {
   
    if (args->Buffer <= 0)
        return STATUS_INVALID_ADDRESS;
    else if (args->Length <= 0)
        return STATUS_INVALID_BUFFER_SIZE;
    else if ((PBYTE)args->Buffer + args->Length < (PBYTE)args->Buffer)
        return STATUS_ACCESS_VIOLATION;

    return ZwQuerySystemInformation(args->SystemInformationClass, args->Buffer, args->Length, args->ReturnLength);
}


ULONG GetNameOffsetFromPath(IN wchar_t* path) {
    if (path <= 0) return 0;

    int length = wcslen(path);

    if (length <= 0) return 0;

    wchar_t separator = L'\\';
    int index;
    for (index = length - 1; index >= 0; index--)
        if (path[index] == separator)break;

    return index + 1;
}
PModuleData ModulesList;
ULONG ModulesListCount;
NTSTATUS ProccessModulesSnapshot32(IN PPEB32 pPEB32, OUT ULONG* modules_count)
{
    if (ModulesList != NULL) // check memory is allocated or not.
    {
        ExFreePool(ModulesList); // free memory 
        ModulesList = NULL;
    }

    ModulesListCount = 0;
    UNICODE_STRING dllName;
    UNICODE_STRING dllPath;
    *modules_count = 0;
    if (pPEB32)
    {
        if (!pPEB32->Ldr)
            DbgPrintEx(0, 0, "Is x86 process, not init Ldr \n");
        else
        {
            int index = 0;
            for (PLIST_ENTRY32 pListEntry = (PLIST_ENTRY32)((PPEB_LDR_DATA32)pPEB32->Ldr)->InLoadOrderModuleList.Flink;
                pListEntry != &((PPEB_LDR_DATA32)pPEB32->Ldr)->InLoadOrderModuleList; pListEntry = (PLIST_ENTRY32)pListEntry->Flink)
            {
                PLDR_DATA_TABLE_ENTRY32 pEntry = CONTAINING_RECORD(pListEntry, LDR_DATA_TABLE_ENTRY32, InLoadOrderLinks);

                if (((wchar_t*)pEntry->BaseDllName.Buffer > 0 && (pEntry->BaseDllName.Length > 0 && pEntry->BaseDllName.Length < MAX_NAME_LENGTH)) && ((wchar_t*)pEntry->FullDllName.Buffer > 0 && (pEntry->FullDllName.Length > 0 && pEntry->FullDllName.Length < MAX_PATH_LENGTH))) {
                    index++;
                }
            }

            if (index == 0)
            {
                return STATUS_UNSUCCESSFUL;
            }

            ModulesList = (PModuleData)ExAllocatePool(PagedPool, sizeof(ModuleData) * index);
            if (ModulesList == NULL) // check memory is allocated or not.
            {
                return STATUS_UNSUCCESSFUL;
            }
            RtlSecureZeroMemory(ModulesList, sizeof(ModuleData) * index);

            for (PLIST_ENTRY32 pListEntry = (PLIST_ENTRY32)((PPEB_LDR_DATA32)pPEB32->Ldr)->InLoadOrderModuleList.Flink;
                pListEntry != &((PPEB_LDR_DATA32)pPEB32->Ldr)->InLoadOrderModuleList && ModulesListCount < index; pListEntry = (PLIST_ENTRY32)pListEntry->Flink)
            {
                PLDR_DATA_TABLE_ENTRY32 pEntry = CONTAINING_RECORD(pListEntry, LDR_DATA_TABLE_ENTRY32, InLoadOrderLinks);

                RtlInitUnicodeString(&dllName, (wchar_t*)pEntry->BaseDllName.Buffer);
                RtlInitUnicodeString(&dllPath, (wchar_t*)pEntry->FullDllName.Buffer);

                if ((dllName.Buffer > 0 && (dllName.Length > 0 && dllName.Length < MAX_NAME_LENGTH)) && (dllPath.Buffer > 0 && (dllPath.Length > 0 && dllPath.Length < MAX_PATH_LENGTH))) {

                    wcscpy_s(ModulesList[ModulesListCount].Path, MAX_PATH_LENGTH, dllPath.Buffer);
                    ModulesList[ModulesListCount].NameOffset = GetNameOffsetFromPath(ModulesList[ModulesListCount].Path);

                    ModulesList[ModulesListCount].Size = pEntry->SizeOfImage;
                    ModulesList[ModulesListCount].BaseAddress = pEntry->DllBase;

                    ModulesListCount++;
                }
            }
        }
    }

    *modules_count = ModulesListCount;

    return STATUS_SUCCESS;
}
NTSTATUS ProccessModulesSnapshot64(IN PPEB pPeb, OUT ULONG* modules_count)
{
    if (ModulesList != NULL) // check memory is allocated or not.
    {
        ExFreePool(ModulesList); // free memory 
        ModulesList = NULL;
    }

    ModulesListCount = 0;
    *modules_count = 0;
    if (pPeb)
    {
        if (!pPeb->Ldr)
            DbgPrintEx(0, 0, "Is x64 process, not init Ldr \n");
        else
        {
            int index = 0;
            for (PLIST_ENTRY pListEntry = (PLIST_ENTRY)((PPEB_LDR_DATA)pPeb->Ldr)->InLoadOrderModuleList.Flink;
                pListEntry != &((PPEB_LDR_DATA)pPeb->Ldr)->InLoadOrderModuleList; pListEntry = (PLIST_ENTRY)pListEntry->Flink)
            {
                PLDR_DATA_TABLE_ENTRY pEntry = CONTAINING_RECORD(pListEntry, LDR_DATA_TABLE_ENTRY, InLoadOrderLinks);
                
                if ((pEntry->BaseDllName.Buffer > 0 && (pEntry->BaseDllName.Length > 0 && pEntry->BaseDllName.Length < MAX_NAME_LENGTH)) && pEntry->FullDllName.Buffer > 0 && (pEntry->FullDllName.Length > 0 && pEntry->FullDllName.Length < MAX_PATH_LENGTH)) {
                    index++;
                }
            }

            if (index == 0)
            {
                return STATUS_UNSUCCESSFUL;
            }

            ModulesList = (PModuleData)ExAllocatePool(PagedPool, sizeof(ModuleData) * index);
            if (ModulesList == NULL) // check memory is allocated or not.
            {
                return STATUS_UNSUCCESSFUL;
            }
            RtlSecureZeroMemory(ModulesList, sizeof(ModuleData) * index);


            for (PLIST_ENTRY pListEntry = (PLIST_ENTRY)((PPEB_LDR_DATA)pPeb->Ldr)->InLoadOrderModuleList.Flink;
                pListEntry != &((PPEB_LDR_DATA)pPeb->Ldr)->InLoadOrderModuleList && ModulesListCount < index; pListEntry = (PLIST_ENTRY)pListEntry->Flink)
            {
                PLDR_DATA_TABLE_ENTRY pEntry = CONTAINING_RECORD(pListEntry, LDR_DATA_TABLE_ENTRY, InLoadOrderLinks);

                if ((pEntry->BaseDllName.Buffer > 0 && (pEntry->BaseDllName.Length > 0 && pEntry->BaseDllName.Length < MAX_NAME_LENGTH)) && pEntry->FullDllName.Buffer > 0 && (pEntry->FullDllName.Length > 0 && pEntry->FullDllName.Length < MAX_PATH_LENGTH)) {
                   
                    wcscpy_s(ModulesList[ModulesListCount].Path, MAX_PATH_LENGTH, pEntry->FullDllName.Buffer);
                    ModulesList[ModulesListCount].NameOffset = GetNameOffsetFromPath(ModulesList[ModulesListCount].Path);

                    ModulesList[ModulesListCount].Size = pEntry->SizeOfImage;
                    ModulesList[ModulesListCount].BaseAddress = pEntry->DllBase;

                    ModulesListCount++;
                }
            }
        }
    }

    *modules_count = ModulesListCount;

    return STATUS_SUCCESS;
}
NTSTATUS ProccessModulesSnapshot(IN OUT PSNAPSHOTPROCESSMODULES_ARGS args)
{
    __try
    {
        ULONG modules_count = 0;
        NTSTATUS status = STATUS_UNSUCCESSFUL;
        PEPROCESS process;
        status = PsLookupProcessByProcessId(args->process_id, &process);
        if (!NT_SUCCESS(status)) {
            return status;
        }

        PPEB32 pPEB32 = (PPEB32)PsGetProcessWow64Process(process);
        KAPC_STATE ApcState;
        KeStackAttachProcess(process, &ApcState);
        if (pPEB32)
        {
            ProccessModulesSnapshot32(pPEB32, &modules_count);
        }
        else {
            PPEB pPeb = PsGetProcessPeb(process);
            if (pPeb) {
                ProccessModulesSnapshot64(pPeb, &modules_count);
            }
        }
        KeUnstackDetachProcess(&ApcState);

        SafeCopy(args->modulesCount, &modules_count, sizeof(modules_count));

        ObDereferenceObject(process);
    }
    __except (EXCEPTION_EXECUTE_HANDLER)
    {
        DbgPrintEx(0, 0, "SmkdNt: %s: Exception, Code: 0x%X\n", __FUNCTION__, GetExceptionCode());
        return STATUS_UNSUCCESSFUL;
    }
    return STATUS_SUCCESS;
}
NTSTATUS GetProccessModules(IN OUT PGETPROCESSMODULES_ARGS args) {
    if (ModulesListCount == 0)
        return STATUS_ACCESS_DENIED;

    for (int i = 0; i < ModulesListCount; i++)
    {
        args->modules[i].Size = ModulesList[i].Size;
        args->modules[i].NameOffset = ModulesList[i].NameOffset;
        args->modules[i].BaseAddress = ModulesList[i].BaseAddress;
        if (args->modules->Path != NULL)
            wcscpy_s(args->modules[i].Path, MAX_PATH_LENGTH, ModulesList[i].Path);
    }

    ExFreePool(ModulesList); // free memory 
    ModulesList = NULL;
    ModulesListCount = 0;

    return STATUS_SUCCESS;
}

//
//#include "stdafx.h"
//
//INT64(NTAPI* EnumerateDebuggingDevicesOriginal)(PVOID, PVOID);
//
//NTSTATUS(NTAPI* PsResumeThread)(PETHREAD Thread, PULONG PreviousCount);
//NTSTATUS(NTAPI* PsSuspendThread)(PETHREAD Thread, PULONG PreviousSuspendCount);
//
//INT64 NTAPI EnumerateDebuggingDevicesHook(PSYSCALL_DATA data, PINT64 status) {
//    if (ExGetPreviousMode() != UserMode) {
//        return EnumerateDebuggingDevicesOriginal(data, status);
//    }
//
//    SYSCALL_DATA safeData = { 0 };
//    if (!ProbeUserAddress(data, sizeof(safeData), sizeof(ULONG)) || !SafeCopy(&safeData, data, sizeof(safeData)) || safeData.Unique != SYSCALL_UNIQUE) {
//        return EnumerateDebuggingDevicesOriginal(data, status);
//    }
//
//    switch (safeData.Syscall) {
//        /*** Process ***/
//        HANDLE_SYSCALL(NtOpenProcess, NTOPENPROCESS_ARGS)
//            HANDLE_SYSCALL(NtSuspendProcess, NTSUSPENDPROCESS_ARGS)
//            HANDLE_SYSCALL(NtResumeProcess, NTRESUMEPROCESS_ARGS)
//            HANDLE_SYSCALL(NtQuerySystemInformationEx, NTQUERYSYSTEMINFORMATIONEX_ARGS)
//            HANDLE_SYSCALL(NtQueryInformationProcess, NTQUERYINFORMATIONPROCESS_ARGS)
//            HANDLE_SYSCALL(NtSetInformationProcess, NTSETINFORMATIONPROCESS_ARGS)
//            HANDLE_SYSCALL(NtFlushInstructionCache, NTFLUSHINSTRUCTIONCACHE_ARGS)
//
//            /*** Memory ***/
//            HANDLE_SYSCALL(NtAllocateVirtualMemory, NTALLOCATEVIRTUALMEMORY_ARGS)
//            HANDLE_SYSCALL(NtFlushVirtualMemory, NTFLUSHVIRTUALMEMORY_ARGS)
//            HANDLE_SYSCALL(NtFreeVirtualMemory, NTFREEVIRTUALMEMORY_ARGS)
//            HANDLE_SYSCALL(NtLockVirtualMemory, NTLOCKVIRTUALMEMORY_ARGS)
//            HANDLE_SYSCALL(NtUnlockVirtualMemory, NTUNLOCKVIRTUALMEMORY_ARGS)
//            HANDLE_SYSCALL(NtProtectVirtualMemory, NTPROTECTVIRTUALMEMORY_ARGS)
//            HANDLE_SYSCALL(NtReadVirtualMemory, NTREADVIRTUALMEMORY_ARGS)
//            HANDLE_SYSCALL(NtWriteVirtualMemory, NTWRITEVIRTUALMEMORY_ARGS)
//            HANDLE_SYSCALL(NtQueryVirtualMemory, NTQUERYVIRTUALMEMORY_ARGS)
//
//            /*** Threads ***/
//            HANDLE_SYSCALL(NtOpenThread, NTOPENTHREAD_ARGS)
//            HANDLE_SYSCALL(NtQueryInformationThread, NTQUERYINFORMATIONTHREAD_ARGS)
//            HANDLE_SYSCALL(NtSetInformationThread, NTSETINFORMATIONTHREAD_ARGS)
//            HANDLE_SYSCALL(NtGetContextThread, NTGETCONTEXTTHREAD_ARGS)
//            HANDLE_SYSCALL(NtSetContextThread, NTSETCONTEXTTHREAD_ARGS)
//            HANDLE_SYSCALL(NtResumeThread, NTRESUMETHREAD_ARGS)
//            HANDLE_SYSCALL(NtSuspendThread, NTSUSPENDTHREAD_ARGS)
//
//            /*** Sync ***/
//            HANDLE_SYSCALL(NtWaitForSingleObject, NTWAITFORSINGLEOBJECT_ARGS)
//    }
//
//    *status = STATUS_NOT_IMPLEMENTED;
//    return 0;
//}
//
//ULONG PreviousModeOffset = 0;
//KPROCESSOR_MODE KeSetPreviousMode(KPROCESSOR_MODE mode) {
//    KPROCESSOR_MODE old = ExGetPreviousMode();
//    *(KPROCESSOR_MODE*)((PBYTE)KeGetCurrentThread() + PreviousModeOffset) = mode;
//    return old;
//}
//
//NTSTATUS Main() {
//    // Get KTHREAD.PreviousMode offset
//    PreviousModeOffset = *(PULONG)((PBYTE)ExGetPreviousMode + 0xC);
//    if (PreviousModeOffset > 0x400) {
//        printf("! invalid PreviousModeOffset (%x) !\n", PreviousModeOffset);
//        return STATUS_FAILED_DRIVER_ENTRY;
//    }
//
//    // NtSuspend/ResumeThread not exported
//    PVOID func = FindPattern((PCHAR)PsRegisterPicoProvider, 0x100, "\x48\x8D\x0D\x00\x00\x00\x00\x48\x89\x4A\x40", "xxx????xxxx");
//    if (!func) {
//        printf("! failed to find \"PsResumeThread\" !\n");
//        return STATUS_FAILED_DRIVER_ENTRY;
//    }
//
//    *(PVOID*)&PsResumeThread = (PBYTE)func + *(PINT)((PBYTE)func + 3) + 7;
//
//    func = FindPattern(func, 0x40, "\x48\x8D\x0D\x00\x00\x00\x00\x48\x89\x4A\x50", "xxx????xxxx");
//    if (!func) {
//        printf("! failed to find \"PsSuspendThead\" !\n");
//        return STATUS_FAILED_DRIVER_ENTRY;
//    }
//
//    *(PVOID*)&PsSuspendThread = (PBYTE)func + *(PINT)((PBYTE)func + 3) + 7;
//
//    // Hook ntoskrnl syscall
//    PVOID base = GetBaseAddress("ntoskrnl.exe", 0);
//    if (!base) {
//        printf("! failed to get \"ntoskrnl.exe\" base !\n");
//        return STATUS_FAILED_DRIVER_ENTRY;
//    }
//
//    func = FindPatternImage(base, "\x48\x8B\x05\x00\x00\x00\x00\x75\x07\x48\x8B\x05\x00\x00\x00\x00\xE8\x00\x00\x00\x00", "xxx????xxxxx????x????");
//    if (!func) {
//        printf("! failed to find xKdEnumerateDebuggingDevices !\n");
//        return STATUS_FAILED_DRIVER_ENTRY;
//    }
//
//    func = (PBYTE)func + *(PINT)((PBYTE)func + 3) + 7;
//    *(PVOID*)&EnumerateDebuggingDevicesOriginal = InterlockedExchangePointer(func, (PVOID)EnumerateDebuggingDevicesHook);
//
//    printf("success\n");
//    return STATUS_SUCCESS;
//}
//
//NTSTATUS DriverEntry(PDRIVER_OBJECT driver, PUNICODE_STRING registryPath) {
//    UNREFERENCED_PARAMETER(driver);
//    UNREFERENCED_PARAMETER(registryPath);
//
//    return Main();
//}
