// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"

#include <codecvt>
#include "./include/distorm.h"
#include "./include/mnemonics.h"
extern "C"
{
#include "instructions.h"
}


#if _WIN64 
// 64 bit build
#define RECLASSNET64 
#else
// 32 bit build
#define RECLASSNET32 
#endif

#ifdef SMLIBRARY_EXPORTS
#define SMLIBRARY_API __declspec(dllexport)
#else
#define SMLIBRARY_API __declspec(dllimport)
#endif

//types

using SM_Pointer = void*;
using SM_Size = size_t;
using SM_Char = char16_t;

typedef SM_Pointer SM_Pointer, * PSM_Pointer;

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

// Helpers

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

inline char16_t* str16cpy(char16_t* destination, const char16_t* source, size_t n)
{
	char16_t* temp = destination;
	while (n > 0 && *source != 0)
	{
		*temp++ = *source++;
		--n;
	}
	while (n > 0)
	{
		*temp++ = 0;
		--n;
	}
	return destination;
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}


bool AreOperandsStatic(const _DInst& instruction, const int prefixLength)
{
	const auto fc = META_GET_FC(instruction.meta);
	if (fc == FC_UNC_BRANCH || fc == FC_CND_BRANCH)
	{
		if (instruction.size - prefixLength < 5)
		{
			return true;
		}
	}

	const auto ops = instruction.ops;
	for (auto i = 0; i < OPERANDS_NO; i++)
	{
		switch (ops[i].type)
		{
		case O_NONE:
		case O_REG:
		case O_IMM1:
		case O_IMM2:
			continue;
		case O_IMM:
			if (ops[i].size < 32)
			{
				continue;
			}
			return false;
		case O_DISP:
		case O_SMEM:
		case O_MEM:
			if (instruction.dispSize < 32)
			{
				continue;
			}

#ifdef RECLASSNET64
			if (ops[i].index == R_RIP)
			{
				continue;
			}
#endif
			return false;
		case O_PC:
		case O_PTR:
			return false;
		}
	}

	return true;
}

_CodeInfo CreateCodeInfo(const uint8_t* address, int length, const _OffsetType virtualAddress)
{
	_CodeInfo info = {};
	info.codeOffset = virtualAddress;
	info.code = address;
	info.codeLen = length;
	info.features = DF_NONE;

#ifdef RECLASSNET32
	info.dt = Decode32Bits;
#else
	info.dt = Decode64Bits;
#endif

	return info;
}


int GetStaticInstructionBytes(const _DInst& instruction, const uint8_t* data)
{
	auto info = CreateCodeInfo(data, instruction.size, reinterpret_cast<_OffsetType>(data));

	_PrefixState ps = {};
	int isPrefixed;
	inst_lookup(&info, &ps, &isPrefixed);

	if (AreOperandsStatic(instruction, ps.count))
	{
		return instruction.size;
	}

	return instruction.size - info.codeLen - ps.count;
}

void FillInstructionData(const _CodeInfo& info, const SM_Pointer address, const _DInst& instruction, const bool determineStaticInstructionBytes, InstructionData* data)
{
	data->Address = reinterpret_cast<SM_Pointer>(instruction.addr);
	data->Length = instruction.size;
	std::memcpy(data->Data, address, instruction.size);
	data->StaticInstructionBytes = -1;

	if (instruction.flags == FLAG_NOT_DECODABLE)
	{
		std::memcpy(data->Instruction, L"???", sizeof(SM_Char) * 3);
	}
	else
	{
		_DecodedInst instructionInfo = {};
		distorm_format(&info, &instruction, &instructionInfo);

		MultiByteToUnicode(
			reinterpret_cast<const char*>(instructionInfo.mnemonic.p),
			data->Instruction,
			instructionInfo.mnemonic.length
		);
		if (instructionInfo.operands.length != 0)
		{
			data->Instruction[instructionInfo.mnemonic.length] = ' ';

			MultiByteToUnicode(
				reinterpret_cast<const char*>(instructionInfo.operands.p),
				0,
				data->Instruction,
				instructionInfo.mnemonic.length + 1,
				std::min<int>(64 - 1 - instructionInfo.mnemonic.length, instructionInfo.operands.length)
			);
		}

		if (determineStaticInstructionBytes)
		{
			data->StaticInstructionBytes = GetStaticInstructionBytes(
				instruction,
				reinterpret_cast<const uint8_t*>(address)
			);
		}
	}
}

bool DisassembleInstructionsImpl(const SM_Pointer address, const SM_Size length, const SM_Pointer virtualAddress, const bool determineStaticInstructionBytes, EnumerateInstructionCallback callback)
{
	auto info = CreateCodeInfo(static_cast<const uint8_t*>(address), static_cast<int>(length), reinterpret_cast<_OffsetType>(virtualAddress));

	const unsigned MaxInstructions = 50;

	_DInst decodedInstructions[MaxInstructions] = {};
	unsigned count = 0;

	auto instructionAddress = static_cast<uint8_t*>(address);

	while (true)
	{
		const auto res = distorm_decompose(&info, decodedInstructions, MaxInstructions, &count);
		if (res == DECRES_INPUTERR)
		{
			return false;
		}

		for (auto i = 0u; i < count; ++i)
		{
			const auto& instruction = decodedInstructions[i];

			InstructionData data = {};
			FillInstructionData(info, instructionAddress, instruction, determineStaticInstructionBytes, &data);

			if (callback(&data) == false)
			{
				return true;
			}

			instructionAddress += instruction.size;
		}

		if (res == DECRES_SUCCESS || count == 0)
		{
			return true;
		}

		const auto offset = static_cast<unsigned>(decodedInstructions[count - 1].addr + decodedInstructions[count - 1].size - info.codeOffset);

		info.codeOffset += offset;
		info.code += offset;
		info.codeLen -= offset;
	}
}




extern "C" SMLIBRARY_API bool __stdcall DisassembleCode(SM_Pointer address, SM_Size length, SM_Pointer virtualAddress, bool determineStaticInstructionBytes, EnumerateInstructionCallback callback);
bool __stdcall DisassembleCode(SM_Pointer address, SM_Size length, SM_Pointer virtualAddress, bool determineStaticInstructionBytes, EnumerateInstructionCallback callback)
{
	return DisassembleInstructionsImpl(address, length, virtualAddress, determineStaticInstructionBytes, callback);
}


