using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Disassembler32.Wrapper
{
    public delegate bool EnumerateInstructionCallback(ref InstructionData data);

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
    public static class Dll
    {
        [DllImport("Disassembler32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool DisassembleCode(IntPtr address, IntPtr length, IntPtr virtualAddress, bool determineStaticInstructionBytes, EnumerateInstructionCallback callback);
        public static bool DisassembleCode(IntPtr address, int length, IntPtr virtualAddress, bool determineStaticInstructionBytes, EnumerateInstructionCallback callback)
        {
            return DisassembleCode(address, (IntPtr)length, virtualAddress, determineStaticInstructionBytes, callback);
        }
    }
}
