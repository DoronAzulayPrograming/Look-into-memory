using System;
using System.Runtime.InteropServices;

namespace WrapperDisassemble
{
    public static class Smdkd
    {

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct InstructionData
        {
            public IntPtr Address;

            public int Length;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public byte[] Data;

            public int StaticInstructionBytes;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string Instruction;
        };
        public delegate bool EnumerateInstructionCallback(ref InstructionData data);

        #region Smdkd dll, Dll imports and functions definitions

        [DllImport("Disassembler32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool DisassembleCode(bool isWow64, IntPtr address, IntPtr length, IntPtr virtualAddress, bool determineStaticInstructionBytes, EnumerateInstructionCallback callback);
        #endregion
        public static bool DisassembleCode(bool isWow64, IntPtr address, int length, IntPtr virtualAddress, bool determineStaticInstructionBytes, EnumerateInstructionCallback callback)
        {
            return DisassembleCode(isWow64, address, (IntPtr)length, virtualAddress, determineStaticInstructionBytes, callback);
        }
    }
}
