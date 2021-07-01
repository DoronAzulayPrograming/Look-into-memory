#pragma once
#include <winioctl.h>
#include <winternl.h>

#include <functional>
#include <vector>
#include <xlocbuf>
#include <codecvt>
#include <type_traits>

//types

using SM_Pointer = void*;
using SM_Size = size_t;
using SM_Sizes = SIZE_T;
using SM_Char = wchar_t;

typedef SM_Pointer SM_Pointer, * PSM_Pointer;
// defines

#define HANDLE_SIGNATURE (1 << 31 | 1 << 29)
#define IsValidHandle(handle) (((SIZE_T)handle & HANDLE_SIGNATURE) && ((SIZE_T)handle % 4 == 0))
#define EncodeHandle(id) (HANDLE)((SIZE_T)id | HANDLE_SIGNATURE)
#define DecodeHandle(handle) (HANDLE)((SIZE_T)handle & ~HANDLE_SIGNATURE)

#define MAX_NAME_LENGTH         256
#define MAX_PATH_LENGTH         260

// Enums
enum class SectionType
{
	Unknown,

	Private,
	Mapped,
	Image
};
enum class SectionCategory
{
	Unknown,
	CODE,
	DATA,
	HEAP
};
enum class SectionProtection
{
	NoAccess = 0,

	Read = 1,
	Write = 2,
	CopyOnWrite = 4,
	Execute = 8,

	Guard = 16
};

inline SectionProtection operator|(SectionProtection lhs, SectionProtection rhs)
{
	using T = std::underlying_type_t<SectionProtection>;

	return static_cast<SectionProtection>(static_cast<T>(lhs) | static_cast<T>(rhs));
}
inline SectionProtection& operator|=(SectionProtection& lhs, SectionProtection rhs)
{
	using T = std::underlying_type_t<SectionProtection>;

	lhs = static_cast<SectionProtection>(static_cast<T>(lhs) | static_cast<T>(rhs));

	return lhs;
}

// Structs
typedef struct ProcessBasicData {
	SM_Pointer Id;
	ULONG NameOffset;
	SM_Char Path[MAX_PATH_LENGTH];
}*PProcessBasicData;

typedef struct ProcessData {
	BOOL IsWow64;
	SM_Pointer Id;
	SM_Pointer Address;
	SM_Size VirtualSize;
	ULONG NameOffset;
	SM_Char Path[MAX_PATH_LENGTH];
}*PProcessData;

typedef struct ModuleData {
	SM_Pointer BaseAddress;
	SM_Size Size;
	ULONG NameOffset;
	SM_Char Path[MAX_PATH_LENGTH];
}*PModuleData;

typedef struct StackAreaData {
	SM_Pointer ThreadId;
	SM_Pointer Base;
	SM_Pointer Limit;
}*PStackAreaData;

typedef struct VmSectionData {
	SM_Pointer BaseAddress;
	SM_Size Size;
	SectionType Type;
	SectionCategory Category;
	SectionProtection Protection;
}*PVmSectionData;

struct EnumerateProcessData
{
	SM_Pointer Id;
	BOOL IsWow64;
	SM_Pointer Address;
	SM_Size VirtualSize;
	SM_Char Path[MAX_PATH_LENGTH];
};

struct EnumerateRemoteModuleData
{
	SM_Pointer BaseAddress;
	SM_Size Size;
	SM_Char Path[MAX_PATH_LENGTH];
};

struct EnumerateRemoteSectionData
{
	SM_Pointer BaseAddress;
	SM_Size Size;
	SectionType Type;
	SectionCategory Category;
	SectionProtection Protection;
	SM_Char Name[16];
	SM_Char ModulePath[MAX_PATH_LENGTH];
};

struct EnumerateDumpData
{
	SM_Pointer Address;
	int HexBytes[16];
	char TextBytes[16];
};

struct EnumerateDisassembleData
{
	SM_Pointer Address;
	SM_Size InstructionLength;
	BYTE Instruction[64];
	char Opcodes[256];
};


/* Disassembler */
struct InstructionData
{
	SM_Pointer Address;
	int Length;
	uint8_t Data[15];
	int StaticInstructionBytes;
	SM_Char Instruction[64];
};
typedef bool(__stdcall EnumerateInstructionCallback)(InstructionData* data);
/* End */

typedef void(__stdcall EnumerateDumpCallback)(EnumerateDumpData* data);
typedef void(__stdcall EnumerateDisassembleCallback)(EnumerateDisassembleData* data);

typedef void(__stdcall EnumerateProcessCallback)(EnumerateProcessData* data);
typedef void(__stdcall EnumerateRemoteModulesCallback)(EnumerateRemoteModuleData* data);
typedef void(__stdcall EnumerateRemoteSectionsCallback)(EnumerateRemoteSectionData* data);

#pragma region Driver System calls

typedef enum _MEMORY_INFORMATION_CLASS {
	MemoryBasicInformation
} MEMORY_INFORMATION_CLASS;

typedef enum class _THREAD_INFO_CLASS {
	ThreadBasicInformation = 0,
	ThreadTimes = 1,
	ThreadPriority = 2,
	ThreadBasePriority = 3,
	ThreadAffinityMask = 4,
	ThreadImpersonationToken = 5,
	ThreadDescriptorTableEntry = 6,
	ThreadEnableAlignmentFaultFixup = 7,
	ThreadEventPair_Reusable = 8,
	ThreadQuerySetWin32StartAddress = 9,
	ThreadZeroTlsCell = 10,
	ThreadPerformanceCount = 11,
	ThreadAmILastThread = 12,
	ThreadIdealProcessor = 13,
	ThreadPriorityBoost = 14,
	ThreadSetTlsArrayAddress = 15,   // Obsolete
	ThreadIsIoPending1 = 16,
	ThreadHideFromDebugger = 17,
	ThreadBreakOnTermination = 18,
	ThreadSwitchLegacyState = 19,
	ThreadIsTerminated = 20,
	ThreadLastSystemCall = 21,
	ThreadIoPriority = 22,
	ThreadCycleTime = 23,
	ThreadPagePriority = 24,
	ThreadActualBasePriority = 25,
	ThreadTebInformation = 26,
	ThreadCSwitchMon = 27,   // Obsolete
	ThreadCSwitchPmu = 28,
	ThreadWow64Context = 29,
	ThreadGroupInformation = 30,
	ThreadUmsInformation = 31,   // UMS
	ThreadCounterProfiling = 32,
	ThreadIdealProcessorEx = 33,
	ThreadCpuAccountingInformation = 34,
	ThreadSuspendCount = 35,
	ThreadActualGroupAffinity = 41,
	ThreadDynamicCodePolicyInfo = 42,
	ThreadSubsystemInformation = 45,
	MaxThreadInfoClass = 51,
} THREAD_INFO_CLASS;

typedef struct _SYSCALL_DATA {
	DWORD Unique;
	DWORD Syscall;
	PVOID Arguments;
} SYSCALL_DATA, * PSYSCALL_DATA;

typedef enum _SYSCALL {
	/*** Process ***/
	SyscallNtOpenProcess,
	SyscallNtSuspendProcess,
	SyscallNtResumeProcess,
	SyscallNtQuerySystemInformationEx,
	SyscallNtQueryInformationProcess,
	SyscallNtSetInformationProcess,
	SyscallNtFlushInstructionCache,

	/*** Memory ***/
	SyscallNtAllocateVirtualMemory,
	SyscallNtFlushVirtualMemory,
	SyscallNtFreeVirtualMemory,
	SyscallNtLockVirtualMemory,
	SyscallNtUnlockVirtualMemory,
	SyscallNtProtectVirtualMemory,
	SyscallNtReadVirtualMemory,
	SyscallNtWriteVirtualMemory,
	SyscallNtQueryVirtualMemory,

	/*** Thread ***/
	SyscallNtOpenThread,
	SyscallNtQueryInformationThread,
	SyscallNtSetInformationThread,
	SyscallNtGetContextThread,
	SyscallNtSetContextThread,
	SyscallNtResumeThread,
	SyscallNtSuspendThread,

	/*** Sync ***/
	SyscallNtWaitForSingleObject
} SYSCALL;

typedef enum  _TERMINATION_METHOD
{
	TerminateEx,
	UnmapImage
}TERMINATION_METHOD, * PTERMINATION_METHOD;

/*** Process ***/
typedef struct _NTTERMINATEPROCESS_ARGS {
	HANDLE ProcessHandle;
	NTSTATUS ExitStatus; // use only for method enums [ AttachTerminate, TerminateEx ]
	TERMINATION_METHOD Method;
} NTTERMINATEPROCESS_ARGS, * PNTTERMINATEPROCESS_ARGS;

typedef struct _NTOPENPROCESS_ARGS {
	PHANDLE ProcessHandle;
	ACCESS_MASK DesiredAccess;
	POBJECT_ATTRIBUTES ObjectAttributes;
	CLIENT_ID* ClientId;
} NTOPENPROCESS_ARGS, * PNTOPENPROCESS_ARGS;

typedef struct _NTSUSPENDPROCESS_ARGS {
	HANDLE ProcessHandle;
} NTSUSPENDPROCESS_ARGS, * PNTSUSPENDPROCESS_ARGS;

typedef struct _NTRESUMEPROCESS_ARGS {
	HANDLE ProcessHandle;
} NTRESUMEPROCESS_ARGS, * PNTRESUMEPROCESS_ARGS;

typedef struct _NTQUERYSYSTEMINFORMATION_ARGS {
	SYSTEM_INFORMATION_CLASS SystemInformationClass;
	PVOID Buffer;
	ULONG Length;
	PULONG ReturnLength;
} NTQUERYSYSTEMINFORMATION_ARGS, * PNTQUERYSYSTEMINFORMATION_ARGS;

typedef struct _NTQUERYSYSTEMINFORMATIONEX_ARGS {
	SYSTEM_INFORMATION_CLASS SystemInformationClass;
	PVOID InputBuffer;
	ULONG InputBufferLength;
	PVOID SystemInformation;
	ULONG SystemInformationLength;
	PULONG ReturnLength;
} NTQUERYSYSTEMINFORMATIONEX_ARGS, * PNTQUERYSYSTEMINFORMATIONEX_ARGS;

typedef struct _NTQUERYINFORMATIONPROCESS_ARGS {
	HANDLE ProcessHandle;
	PROCESSINFOCLASS ProcessInformationClass;
	PVOID ProcessInformation;
	ULONG ProcessInformationLength;
	PULONG ReturnLength;
} NTQUERYINFORMATIONPROCESS_ARGS, * PNTQUERYINFORMATIONPROCESS_ARGS;

typedef struct _NTSETINFORMATIONPROCESS_ARGS {
	HANDLE ProcessHandle;
	PROCESSINFOCLASS ProcessInformationClass;
	PVOID ProcessInformation;
	ULONG ProcessInformationLength;
} NTSETINFORMATIONPROCESS_ARGS, * PNTSETINFORMATIONPROCESS_ARGS;

typedef struct _NTFLUSHINSTRUCTIONCACHE_ARGS {
	HANDLE ProcessHandle;
	PVOID BaseAddress;
	ULONG NumberOfBytesToFlush;
} NTFLUSHINSTRUCTIONCACHE_ARGS, * PNTFLUSHINSTRUCTIONCACHE_ARGS;

/*** Memory ***/
typedef struct _NTALLOCATEVIRTUALMEMORY_ARGS {
	HANDLE ProcessHandle;
	PVOID* BaseAddress;
	SIZE_T ZeroBits;
	PSIZE_T RegionSize;
	ULONG AllocationType;
	ULONG Protect;
} NTALLOCATEVIRTUALMEMORY_ARGS, * PNTALLOCATEVIRTUALMEMORY_ARGS;

typedef struct _NTFLUSHVIRTUALMEMORY_ARGS {
	HANDLE ProcessHandle;
	PVOID* BaseAddress;
	PSIZE_T RegionSize;
	PIO_STATUS_BLOCK IoStatus;
} NTFLUSHVIRTUALMEMORY_ARGS, * PNTFLUSHVIRTUALMEMORY_ARGS;

typedef struct _NTFREEVIRTUALMEMORY_ARGS {
	HANDLE ProcessHandle;
	PVOID* BaseAddress;
	PSIZE_T RegionSize;
	ULONG FreeType;
} NTFREEVIRTUALMEMORY_ARGS, * PNTFREEVIRTUALMEMORY_ARGS;

typedef struct _NTLOCKVIRTUALMEMORY_ARGS {
	HANDLE ProcessHandle;
	PVOID* BaseAddress;
	PSIZE_T RegionSize;
	ULONG LockOption;
} NTLOCKVIRTUALMEMORY_ARGS, * PNTLOCKVIRTUALMEMORY_ARGS;

typedef struct _NTUNLOCKVIRTUALMEMORY_ARGS {
	HANDLE ProcessHandle;
	PVOID* BaseAddress;
	PSIZE_T RegionSize;
	ULONG LockOption;
} NTUNLOCKVIRTUALMEMORY_ARGS, * PNTUNLOCKVIRTUALMEMORY_ARGS;

typedef struct _NTPROTECTVIRTUALMEMORY_ARGS {
	HANDLE ProcessHandle;
	PVOID* BaseAddress;
	PSIZE_T RegionSize;
	ULONG NewAccessProtection;
	PULONG OldAccessProtection;
} NTPROTECTVIRTUALMEMORY_ARGS, * PNTPROTECTVIRTUALMEMORY_ARGS;

typedef struct _NTREADVIRTUALMEMORY_ARGS {
	HANDLE ProcessHandle;
	PVOID BaseAddress;
	PVOID Buffer;
	SIZE_T NumberOfBytesToRead;
	PSIZE_T NumberOfBytesRead;
} NTREADVIRTUALMEMORY_ARGS, * PNTREADVIRTUALMEMORY_ARGS;

typedef struct _NTWRITEVIRTUALMEMORY_ARGS {
	HANDLE ProcessHandle;
	PVOID BaseAddress;
	PVOID Buffer;
	SIZE_T NumberOfBytesToWrite;
	PSIZE_T NumberOfBytesWritten;
} NTWRITEVIRTUALMEMORY_ARGS, * PNTWRITEVIRTUALMEMORY_ARGS;

typedef struct _NTQUERYVIRTUALMEMORY_ARGS {
	HANDLE ProcessHandle;
	PVOID BaseAddress;
	MEMORY_INFORMATION_CLASS MemoryInformationClass;
	PVOID MemoryInformation;
	SIZE_T MemoryInformationLength;
	PSIZE_T ReturnLength;
} NTQUERYVIRTUALMEMORY_ARGS, * PNTQUERYVIRTUALMEMORY_ARGS;

/*** Thread ***/
typedef struct _NTOPENTHREAD_ARGS {
	PHANDLE ThreadHandle;
	ACCESS_MASK AccessMask;
	POBJECT_ATTRIBUTES ObjectAttributes;
	CLIENT_ID* ClientId;
} NTOPENTHREAD_ARGS, * PNTOPENTHREAD_ARGS;

typedef struct _NTQUERYINFORMATIONTHREAD_ARGS {
	HANDLE ThreadHandle;
	THREAD_INFO_CLASS ThreadInformationClass;
	PVOID ThreadInformation;
	ULONG ThreadInformationLength;
	PULONG ReturnLength;
} NTQUERYINFORMATIONTHREAD_ARGS, * PNTQUERYINFORMATIONTHREAD_ARGS;

typedef struct _NTSETINFORMATIONTHREAD_ARGS {
	HANDLE ThreadHandle;
	THREADINFOCLASS ThreadInformationClass;
	PVOID ThreadInformation;
	ULONG ThreadInformationLength;
} NTSETINFORMATIONTHREAD_ARGS, * PNTSETINFORMATIONTHREAD_ARGS;

typedef struct _NTGETCONTEXTTHREAD_ARGS {
	HANDLE ThreadHandle;
	PCONTEXT Context;
} NTGETCONTEXTTHREAD_ARGS, * PNTGETCONTEXTTHREAD_ARGS;

typedef struct _NTSETCONTEXTTHREAD_ARGS {
	HANDLE ThreadHandle;
	PCONTEXT Context;
} NTSETCONTEXTTHREAD_ARGS, * PNTSETCONTEXTTHREAD_ARGS;

typedef struct _NTRESUMETHREAD_ARGS {
	HANDLE ThreadHandle;
	PULONG SuspendCount;
} NTRESUMETHREAD_ARGS, * PNTRESUMETHREAD_ARGS;

typedef struct _NTSUSPENDTHREAD_ARGS {
	HANDLE ThreadHandle;
	PULONG PreviousSuspendCount;
} NTSUSPENDTHREAD_ARGS, * PNTSUSPENDTHREAD_ARGS;

/*** Sync ***/
typedef struct _NTWAITFORSINGLEOBJECT_ARGS {
	HANDLE Handle;
	BOOLEAN Alertable;
	PLARGE_INTEGER Timeout;
	NTSTATUS* Status;
} NTWAITFORSINGLEOBJECT_ARGS, * PNTWAITFORSINGLEOBJECT_ARGS;


/*** Ex ***/

typedef struct _GETPROCESSMODULES_ARGS {
	PModuleData modules;
} GETPROCESSMODULES_ARGS, * PGETPROCESSMODULES_ARGS;

typedef struct _SNAPSHOTPROCESSMODULES_ARGS {
	HANDLE process_id;
	PULONG modulesCount;
} SNAPSHOTPROCESSMODULES_ARGS, * PSNAPSHOTPROCESSMODULES_ARGS;

typedef struct _ISWOW64PROCESS_ARGS {
	HANDLE ProcessId;
	PVOID BaseAddress;
	BOOL* IsWow64;
	wchar_t* Path;
} ISWOW64PROCESS_ARGS, * PISWOW64PROCESS_ARGS;

typedef struct _NTMMCOPYVM_ARGS {
	HANDLE SourceProcessId;
	PVOID SourceAddress;
	HANDLE TargetProcessId;
	PVOID TargetAddress;
	SIZE_T Size;
	PSIZE_T NumberOfBytesCopyed;
} NTMMCOPYVM_ARGS, * PNTMMCOPYVM_ARGS;

#pragma endregion

/*** Sm Ex args ***/
typedef struct _ObCloseObjectByPointer_ARGS {
	HANDLE Handle;
} ObCloseObjectByPointer_ARGS, * PObCloseObjectByPointer_ARGS;

typedef struct _ObOpenObjectByPointer_ARGS {
	HANDLE Id;
	PHANDLE Handle;
	ACCESS_MASK Access;
} ObOpenObjectByPointer_ARGS, * PObOpenObjectByPointer_ARGS;

/*** Ex ***/
typedef struct _THREAD_BASIC_INFORMATION {
	NTSTATUS                ExitStatus;
	PVOID                   TebBaseAddress;
	CLIENT_ID               ClientId;
	KAFFINITY               AffinityMask;
	KPRIORITY               Priority;
	KPRIORITY               BasePriority;
} THREAD_BASIC_INFORMATION, * PTHREAD_BASIC_INFORMATION;

typedef struct _SYSTEM_PROCESS_INFO
{
	ULONG NextEntryOffset;
	ULONG NumberOfThreads;
	LARGE_INTEGER WorkingSetPrivateSize; // Since Vista
	ULONG HardFaultCount; // Since Windows 7
	ULONG NumberOfThreadsHighWatermark; // Since Windows 7
	ULONGLONG CycleTime; // Since Windows 7
	LARGE_INTEGER CreateTime;
	LARGE_INTEGER UserTime;
	LARGE_INTEGER KernelTime;
	UNICODE_STRING ImageName;
	KPRIORITY BasePriority;
	HANDLE UniqueProcessId;
	HANDLE InheritedFromUniqueProcessId;
	ULONG HandleCount;
	ULONG SessionId;
	ULONG_PTR UniqueProcessKey; // Since Vista (requires SystemExtendedProcessInformation)
	SIZE_T PeakVirtualSize;
	SIZE_T VirtualSize;
	ULONG PageFaultCount;
	SIZE_T PeakWorkingSetSize;
	SIZE_T WorkingSetSize;
	SIZE_T QuotaPeakPagedPoolUsage;
	SIZE_T QuotaPagedPoolUsage;
	SIZE_T QuotaPeakNonPagedPoolUsage;
	SIZE_T QuotaNonPagedPoolUsage;
	SIZE_T PagefileUsage;
	SIZE_T PeakPagefileUsage;
	SIZE_T PrivatePageCount;
	LARGE_INTEGER ReadOperationCount;
	LARGE_INTEGER WriteOperationCount;
	LARGE_INTEGER OtherOperationCount;
	LARGE_INTEGER ReadTransferCount;
	LARGE_INTEGER WriteTransferCount;
	LARGE_INTEGER OtherTransferCount;
	SYSTEM_THREAD_INFORMATION Threads[1];
} SYSTEM_PROCESS_INFO, * PSYSTEM_PROCESS_INFO;

typedef struct SystemProcessInfo
{
	ULONG NextEntryOffset;
	ULONG NumberOfThreads;
	LARGE_INTEGER WorkingSetPrivateSize; // Since Vista
	ULONG HardFaultCount; // Since Windows 7
	ULONG NumberOfThreadsHighWatermark; // Since Windows 7
	ULONGLONG CycleTime; // Since Windows 7
	LARGE_INTEGER CreateTime;
	LARGE_INTEGER UserTime;
	LARGE_INTEGER KernelTime;
	wchar_t ImageName[MAX_NAME_LENGTH];
	KPRIORITY BasePriority;
	HANDLE UniqueProcessId;
	HANDLE InheritedFromUniqueProcessId;
	ULONG HandleCount;
	ULONG SessionId;
	ULONG_PTR UniqueProcessKey; // Since Vista (requires SystemExtendedProcessInformation)
	SIZE_T PeakVirtualSize;
	SIZE_T VirtualSize;
	ULONG PageFaultCount;
	SIZE_T PeakWorkingSetSize;
	SIZE_T WorkingSetSize;
	SIZE_T QuotaPeakPagedPoolUsage;
	SIZE_T QuotaPagedPoolUsage;
	SIZE_T QuotaPeakNonPagedPoolUsage;
	SIZE_T QuotaNonPagedPoolUsage;
	SIZE_T PagefileUsage;
	SIZE_T PeakPagefileUsage;
	SIZE_T PrivatePageCount;
	LARGE_INTEGER ReadOperationCount;
	LARGE_INTEGER WriteOperationCount;
	LARGE_INTEGER OtherOperationCount;
	LARGE_INTEGER ReadTransferCount;
	LARGE_INTEGER WriteTransferCount;
	LARGE_INTEGER OtherTransferCount;
	SYSTEM_THREAD_INFORMATION* Threads;
}*PSystemProcessInfo;


typedef struct _STRING32 {
	USHORT   Length;
	USHORT   MaximumLength;
	ULONG  Buffer;
} STRING32;
typedef STRING32* PSTRING32;

typedef STRING32 UNICODE_STRING32;
typedef UNICODE_STRING32* PUNICODE_STRING32;

#pragma region Helpres methods

/*** Helpers ***/
inline void MultiByteToUnicode(const char* src, const int srcOffset, SM_Char* dst, const int dstOffset, const int size)
{
#if _MSC_VER >= 1900
	// VS Bug: https://connect.microsoft.com/VisualStudio/feedback/details/1348277/link-error-when-using-std-codecvt-utf8-utf16-char16-t

	using converter = std::wstring_convert<std::codecvt_utf8_utf16<int16_t>, int16_t>;
#else
	using converter = std::wstring_convert<std::codecvt_utf8_utf16<RC_UnicodeChar>, RC_UnicodeChar>;
#endif

	const auto temp = converter{}.from_bytes(src + srcOffset);

	std::memcpy(dst + dstOffset, temp.c_str(), std::min<int>(static_cast<int>(temp.length()), size) * sizeof(SM_Char));
}
inline void MultiByteToUnicode(const char* src, SM_Char* dst, const int size)
{
	MultiByteToUnicode(src, 0, dst, 0, size);
}

#pragma endregion


#define SM_CTL_OPEN_PROCESS						 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0701, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_SUSPENDP_PROCESS					 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0702, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_RESUME_PROCESS					 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0703, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_QUERY_SYSTEM_INFORMATION_EX	     CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0704, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_QUERY_INFORMATION_PROCESS		 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0705, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_SET_INFORMATION_PROCESS			 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0706, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_FLUSH_INSTRUCTION_CACHE			 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0707, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_ALLOCATE_VIRTUAL_MEMORY			 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0708, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_FLUSH_VIRTUAL_MEMORY				 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0709, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_FREE_VIRTUAL_MEMORY				 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0710, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_LOCK_VIRTUAL_MEMORY				 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0711, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_UNLOCK_VIRTUAL_MEMORY			 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0712, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_PROTECT_VIRTUAL_MEMORY			 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0713, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_READ_VIRTUAL_MEMORY				 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0714, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_WRITE_VIRTUAL_MEMORY				 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0715, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_QUERY_VIRTUAL_MEMORY				 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0716, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_OPEN_THREAD						 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0717, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_QUERY_INFORMATION_THREAD			 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0718, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_SET_INFORMATION_THREAD			 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0719, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_GET_CONTEXT_THREAD				 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0720, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_SET_CONTEXT_THREAD				 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0721, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_RESUME_THREAD					 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0722, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_SUSPENDP_THREAD					 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0723, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_WAIT_FOR_SINGLE_OBJECT			 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0724, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_QUERY_VIRTUAL_MEMORY1			 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0725, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_SYSTEM_PROCESS_INFORMATION		 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0726, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_IS_WOW64_PROCESS					 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0727, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_QUERY_SYSTEM_INFORMATION			 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0728, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_GET_PROCESS_PEB					 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0729, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_GET_WOW64_PROCESS_PEB			 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0730, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_SNAP_SHOT_PROCESS_MODULES		 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0731, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_GET_PROCESS_MODULES				 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0732, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_MMCOPY_VM						 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0733, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_TERMINATE_PROCESS				 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0734, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)

#define SM_CTL_OPEN_OBJECT						 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0735, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define SM_CTL_CLOSE_OBJECT						 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x0736, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)

typedef struct _SMNTDEFS {
	ObCloseObjectByPointer_ARGS obCloseObjectByPointer_args;
	ObOpenObjectByPointer_ARGS obOpenObjectByPointer_args;
	NTTERMINATEPROCESS_ARGS terminate_process_args;
	NTMMCOPYVM_ARGS mm_copy_vm_args;
	ISWOW64PROCESS_ARGS is_wow64_process;
	GETPROCESSMODULES_ARGS get_process_modules;
	SNAPSHOTPROCESSMODULES_ARGS snap_shot_process_modules;
	SYSTEM_PROCESS_INFORMATION* system_information_process;
	NTOPENPROCESS_ARGS open_process_args;
	NTSUSPENDPROCESS_ARGS suspend_process_args;
	NTRESUMEPROCESS_ARGS resume_process_args;
	NTQUERYSYSTEMINFORMATION_ARGS querySystemInformation_args;
	NTQUERYSYSTEMINFORMATIONEX_ARGS querySystemInformationEx_args;
	NTQUERYINFORMATIONPROCESS_ARGS queryInformationProcess_args;
	NTSETINFORMATIONPROCESS_ARGS setInformationProcess_args;
	NTFLUSHINSTRUCTIONCACHE_ARGS flushInstructioncache_args;
	NTALLOCATEVIRTUALMEMORY_ARGS allocateVirtualMemory_args;
	NTFLUSHVIRTUALMEMORY_ARGS flushVirtualMemory_args;
	NTFREEVIRTUALMEMORY_ARGS freeVirtualMemory_args;
	NTLOCKVIRTUALMEMORY_ARGS lockVirtualMemory_args;
	NTUNLOCKVIRTUALMEMORY_ARGS unlockVirtualMemory_args;
	NTPROTECTVIRTUALMEMORY_ARGS protectVirtualMemory_args;
	NTREADVIRTUALMEMORY_ARGS readVirtualMemory_args;
	NTWRITEVIRTUALMEMORY_ARGS writeVirtualMemory_args;
	NTQUERYVIRTUALMEMORY_ARGS queryVirtualMemory_args;
	NTOPENTHREAD_ARGS open_thread_args;
	NTQUERYINFORMATIONTHREAD_ARGS queryInformationThread_args;
	NTSETINFORMATIONTHREAD_ARGS setInformationThread_args;
	NTGETCONTEXTTHREAD_ARGS getContextThread_args;
	NTSETCONTEXTTHREAD_ARGS setContextThread_args;
	NTRESUMETHREAD_ARGS resume_thread_args;
	NTSUSPENDTHREAD_ARGS suspend_thread_args;
	NTWAITFORSINGLEOBJECT_ARGS waitForSingleObject_thread_args;
} SMNTDEFS, * PSMNTDEFS;