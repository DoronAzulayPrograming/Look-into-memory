using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;

namespace Disassembler32.Wrapper
{
    public delegate string GetParameters(Parameters parameters);

    class Program
    {
        internal enum WrapperMode
        {
            Loop,
            SingleJob
        }
        const int ServersCount = 10;
        static Thread[] Servers;
        static Thread Server;
        static WrapperMode ServerMode = WrapperMode.Loop;
        private static event GetParameters OnGetParameters;

        static void Main(string[] args)
        {
            if (args.Length < 2) return; // Path.exe smenv l

            switch (args[1].ToLower())
            {
                case "l": ServerMode = WrapperMode.Loop; break;
                case "loop": ServerMode = WrapperMode.Loop; break;
                case "s": ServerMode = WrapperMode.SingleJob; break;
                case "single": ServerMode = WrapperMode.SingleJob; break;
            }

            if (args[0].ToLower().Equals("smenv"))
            {
                OnGetParameters += new GetParameters(Disassemble);
                InitServersThread();
            }
        }

        private static void InitServersThread()
        {
            Servers = new Thread[ServersCount];
            for (int i = 0; i < ServersCount; i++)
            {
                Servers[i] = new Thread(ServerThread);
                Servers[i].IsBackground = true;
                Servers[i].Start();
            }

            Server = new Thread(ServerThread);
            Server.IsBackground = true;
            Server.Start();

            Server.Join();
        }
        private static void InitServerThread()
        {
            Server = new Thread(ServerThread);
            Server.IsBackground = true;
            Server.Start();

            Server.Join();
        }
        private static void ServerThread(object data)
        {
            NamedPipeServerStream pipeServer =
                new NamedPipeServerStream("SmDisassembler32Pipe", PipeDirection.InOut, ServersCount+1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);

            int threadId = Thread.CurrentThread.ManagedThreadId;

            // Wait for a client to connect
            pipeServer.WaitForConnection();

            Console.WriteLine("Client connected on thread[{0}].", threadId);

            try
            {
                do
                {
                    if (!pipeServer.IsConnected)
                    {
                        pipeServer.Close();
                        Console.WriteLine("Client disconnected from thread[{0}].", threadId);

                        pipeServer = new NamedPipeServerStream(
                            "SmDisassembler32Pipe",
                            PipeDirection.InOut, 
                            ServersCount+1,
                            PipeTransmissionMode.Message,
                            PipeOptions.Asynchronous
                            );

                        // Wait for a client to connect
                        pipeServer.WaitForConnection();

                        Console.WriteLine("Client connected on thread[{0}].", threadId);
                    }
                    var parametersJsonFormat = pipeServer?.ReadBigString();
                    if (string.IsNullOrEmpty(parametersJsonFormat)) continue;
                    var parameters = JsonConvert.DeserializeObject<Parameters>(parametersJsonFormat);
                    pipeServer.WriteString(OnGetParameters.Invoke(parameters));
                }
                while (ServerMode == WrapperMode.Loop);
            }
            catch (IOException e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }
            pipeServer.Close();
        }
        private static string GetCurrentProcessName()
        {
            string currentProcessName = Environment.CommandLine;

            // Remove extra characters when launched from Visual Studio
            currentProcessName = currentProcessName.Trim('"', ' ');

            currentProcessName = System.IO.Path.ChangeExtension(currentProcessName, ".exe");

            if (currentProcessName.Contains(Environment.CurrentDirectory))
            {
                currentProcessName = currentProcessName.Replace(Environment.CurrentDirectory, string.Empty);
            }

            // Remove extra characters when launched from Visual Studio
            currentProcessName = currentProcessName.Replace("\\", string.Empty);
            currentProcessName = currentProcessName.Replace("\"", string.Empty);

            return currentProcessName;
        }
        private static string Disassemble(Parameters parameters)
        {
            IReadOnlyList<InstructionData> instructions;
            if(parameters.MaxInstructions == -2)
            {
                instructions = Disassembler.DisassembleFunction(parameters.Data, parameters.VirtualAddress);
            }
            else if (parameters.MaxInstructions == -3)
            {
                var ins = Disassembler.RemoteGetPreviousInstruction(parameters.Data, parameters.VirtualAddress);
                var list = new List<InstructionData>();
                if(ins.Address != IntPtr.Zero)
                    list.Add(ins);
                instructions = list;
            }
            else
            {
                instructions = Disassembler.DisassembleCode(parameters.Data, parameters.VirtualAddress, parameters.MaxInstructions);
            }

            return JsonConvert.SerializeObject(instructions);
        }
    }
    public static class PipeStreamEx
    {
        public static void WriteString(this PipeStream pipe, string str)
        {
            // On the server side, we need to send it all as one byte[]
            var buffer = Encoding.Unicode.GetBytes(str);
            pipe.Write(buffer, 0, buffer.Length);
        }
        public static string ReadBigString(this PipeStream pipe, int buffer_size = 1024)
        {
            var bufferRead = new byte[buffer_size];
            var sb = new StringBuilder();

            int read = pipe.Read(bufferRead, 0, bufferRead.Length);
            // Reading the stream as usual, but only the first message
            if (read > 0)
            {
                sb.Append(Encoding.Unicode.GetString(bufferRead, 0, read));

                if (!pipe.IsMessageComplete)
                {
                    while ((read = pipe.Read(bufferRead, 0, bufferRead.Length)) > 0 && !pipe.IsMessageComplete)
                    {
                        sb.Append(Encoding.Unicode.GetString(bufferRead, 0, read));
                    }
                    if (read > 0)
                    {
                        sb.Append(Encoding.Unicode.GetString(bufferRead, 0, read));
                    }
                }
            }

            return sb.ToString();
        }
    }
}
