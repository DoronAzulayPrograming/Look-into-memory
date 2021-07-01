using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PipeServer
{
    public delegate string GetParameters(Parameters parameters);

    public class PipeServer
    {
        private static StreamString ss;
        private static event GetParameters OnGetParameters;

        public static void Main()
        {
            OnGetParameters += new GetParameters(GetInstractionList);
            Console.WriteLine("\n*** Named pipe server stream with impersonation example ***\n");
            Console.WriteLine("Waiting for client connect...\n");

            Thread server = new Thread(ServerThread);
            server.Start();

            Thread.Sleep(250);

            server.Join();

            Console.WriteLine("\nServer threads exhausted, exiting.");
        }

        private static string GetInstractionList(Parameters parameters)
        {
            var list = new List<InstructionData>();
            list.Add(new InstructionData()
            {
                Address = (IntPtr)0x00400000,
                Data = new byte[] { 0x4d },
                Length = 1,
                Instruction = "dec ebp"
            });
            list.Add(new InstructionData()
            {
                Address = (IntPtr)0x00400001,
                Data = new byte[] { 0xbe, 0xF0 },
                Length = 2,
                Instruction = "ah,dl"
            });
            list.Add(new InstructionData()
            {
                Address = (IntPtr)0x00400001,
                Data = new byte[] { 0xbe, 0x4d, 0xF0 },
                Length = 3,
                Instruction = "add eax,dl"
            });

            return JsonConvert.SerializeObject(list);
        }
        private static void ServerThread(object data)
        {
            NamedPipeServerStream pipeServer =
                new NamedPipeServerStream("testpipe", PipeDirection.InOut, 1);

            int threadId = Thread.CurrentThread.ManagedThreadId;

            // Wait for a client to connect
            pipeServer.WaitForConnection();

            Console.WriteLine("Client connected on thread[{0}].", threadId);
            ss = new StreamString(pipeServer);
            try
            {
                while (true)
                {
                    string parametersJsonFormat = ss.ReadString();
                    var parameters = JsonConvert.DeserializeObject<Parameters>(parametersJsonFormat);
                    ss.WriteString(OnGetParameters.Invoke(parameters));
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }
            pipeServer.Close();
        }
    }

    // Defines the data protocol for reading and writing strings on our stream
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
            int len = 0;

            len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();
            byte[] inBuffer = new byte[len];
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


    public class Parameters
    {
        public bool IsWow64 { get; set; }
        public byte[] Data { get; set; }
        public IntPtr VirtualAddress { get; set; }
        public int MaxInstructions { get; set; }
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
}
