using System;
using System.Collections.Generic;
using System.Text;

namespace Disassembler32.Wrapper
{
    public class Parameters
    {
        public byte[] Data { get; set; }
        public IntPtr VirtualAddress { get; set; }
        public int MaxInstructions { get; set; }
    }
}
