using System;
using System.Runtime.InteropServices;
using static WrapperDisassemble.Smdkd;

namespace WrapperDisassemble
{
    [Guid("7F10C7D5-6BC1-4ED1-BF50-6D37864404D6")]
    public interface iInterface
    {
        bool IsWow64();
        string GetS();
        bool DisassembleCode(bool isWow64, IntPtr address, int length, IntPtr virtualAddress, bool determineStaticInstructionBytes, EnumerateInstructionCallback callback);
    }
    [Guid("5B1D6A02-48C9-497C-997D-0F8A8796D2A1")]
    public class Wrapper : iInterface
    {
        public bool DisassembleCode(bool isWow64, IntPtr address, int length, IntPtr virtualAddress, bool determineStaticInstructionBytes, EnumerateInstructionCallback callback)
        {
            return Smdkd.DisassembleCode(isWow64, address, (IntPtr)length, virtualAddress, determineStaticInstructionBytes, callback);
        }

        public string GetS()
        {
            return "S";
        }
        public bool IsWow64()
        {
            return !Environment.Is64BitProcess;
        }
    }
}
