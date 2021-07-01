using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;



namespace PipeClient
{
    public class PipeClient
    {
        static int? maxAwaitForConnection = null;
        static string PipelineName = "SmDisassembler32Pipe";
        static string ArgsToDissasemble32Wrapper = "SmEnv Loop";
        static string PathToDissasemble32Wrapper = "C:\\Users\\doronPrograming\\source\\repos\\PipeServer\\Disassembler32.Wrapper\\bin\\Debug\\netcoreapp3.1\\Disassembler32.Wrapper.exe";
        public static NamedPipeClientStream _PipeClient;

        public static void ClientThread()
        {
            var pipeClient =
                      new NamedPipeClientStream(".", "testpipe",
                          PipeDirection.InOut, PipeOptions.None,
                          TokenImpersonationLevel.Impersonation);

            Console.WriteLine("Connecting to server...\n");

            pipeClient.Connect();
            try
            {
                var ss = new StreamString(pipeClient);
                while (true)
                {
                    string msg = ss.ReadString();
                    /*switch (msg)
                    {

                    }*/
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
            }

            pipeClient.Close();
        }

        public static void Main(string[] args)
        {
            if(ConnectAndRead(new byte[] { 0xd, 0xff, 0x6e }, (IntPtr)0x00400000, -1, out var list)){
                Console.WriteLine(list.Count);
            }

            Console.ReadKey();
            // Give the client process some time to display results before exiting.
            Thread.Sleep(4000);
        }

        static bool ConnectAndRead(byte[] data, IntPtr virtualAddress, int maxInstructions, out IReadOnlyList<DisassembledInstruction> list)
        {
            list = new List<DisassembledInstruction>();
            if (ConnectToPipeLine(PipelineName, maxAwaitForConnection))
            {
                list = DisassembleCode(data, virtualAddress, maxInstructions);
                return true;
            }
            else return false;
        }
        static IReadOnlyList<DisassembledInstruction> DisassembleCode(byte[] data, IntPtr virtualAddress, int maxInstructions)
        {
            IReadOnlyList<DisassembledInstruction> disassembleds = null;
            var streamString = new StreamString(_PipeClient);
            var parameters = new Parameters()
            {
                Data = data.Clone() as byte[],
                VirtualAddress = virtualAddress,
                MaxInstructions = maxInstructions
            };

            try
            {
                var jsonFormat = JsonConvert.SerializeObject(parameters);
                streamString.WriteString(jsonFormat);
                jsonFormat = streamString.ReadString();
                var ins = JsonConvert.DeserializeObject<List<InstructionData>>(jsonFormat);
                disassembleds = ins.Select(i => new DisassembledInstruction(ref i)).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

            return disassembleds;
        }
        static bool ConnectToPipeLine(string pipeName,int? maxAwaitTimeForConnection = null)
        {
            System.Diagnostics.Process.Start(PathToDissasemble32Wrapper, ArgsToDissasemble32Wrapper);

            _PipeClient =
                    new NamedPipeClientStream(".", pipeName,
                        PipeDirection.InOut, PipeOptions.None,
                        TokenImpersonationLevel.Impersonation);

            if(maxAwaitTimeForConnection != null && maxAwaitTimeForConnection.Value > 0)
                _PipeClient.Connect(maxAwaitTimeForConnection.Value);
            else
                _PipeClient.Connect();

            return _PipeClient.IsConnected;
        }
    }

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
    public class DisassembledInstruction
    {
        public IntPtr Address { get; set; }
        public int Length { get; set; }
        public byte[] Data { get; set; }
        public string Instruction { get; set; }

        public bool IsValid => Length > 0;

        public DisassembledInstruction(ref InstructionData data)
        {
            Address = data.Address;
            Length = data.Length;
            Data = data.Data;
            Instruction = data.Instruction;
        }

        public override string ToString() => $"{Address.ToString("X08")} - {Instruction}";
    }


    public class Parameters
    {
        public byte[] Data { get; set; }
        public IntPtr VirtualAddress { get; set; }
        public int MaxInstructions { get; set; }
    }

    // Defines the data protocol for reading and writing strings on our stream.
    public class StreamString
    {
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;

        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UnicodeEncoding();
        }

        public string ReadString()
        {
            int len;
            len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();

            if (len < 1) return string.Empty;

            var inBuffer = new byte[len];
            ioStream.Read(inBuffer, 0, len);

            return streamEncoding.GetString(inBuffer);
        }

        public int WriteString(string outString)
        {
            byte[] outBuffer = streamEncoding.GetBytes(outString);
            int len = outBuffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = (int)UInt16.MaxValue;
            }
            ioStream.WriteByte((byte)(len / 256));
            ioStream.WriteByte((byte)(len & 255));
            ioStream.Write(outBuffer, 0, len);
            ioStream.Flush();

            return outBuffer.Length + 2;
        }
    }
}
