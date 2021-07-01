using Newtonsoft.Json;
using SmScanner.Core.Memory;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SmScanner.Wrappers
{
    public class Parameters
    {
        public byte[] Data { get; set; }
        public IntPtr VirtualAddress { get; set; }
        public int MaxInstructions { get; set; }
    }
    public interface IWrapper
    {
        public void Close();
        public bool Connect();
        public bool ReConncet();

        public object IClone();
        public bool IsProcessRunning();
    }
    public interface IDisassemblerWrapper : IWrapper
    {
        public DisassemblerWrapper Clone();
        public DisassembledInstruction RemoteGetPreviousInstruction(byte[] data, IntPtr virtualAddress);
        public IReadOnlyList<DisassembledInstruction> DisassembleFunction(byte[] data, IntPtr virtualAddress);
        public IReadOnlyList<DisassembledInstruction> DisassembleCode(byte[] data, IntPtr virtualAddress, int maxInstructions);
    }
    public class DisassemblerWrapper : IDisassemblerWrapper
    {
        public System.Diagnostics.Process Process { get; private set; }
        public string Args { get; }
        public string PipelineName { get; }
        public string PathToWrapper { get; }
        public int? MaxAwaitForConnection { get; }
        public NamedPipeClientStream PipeClient { get; private set; }
        public bool IsConnected { get => PipeClient.IsConnected; }

        public DisassemblerWrapper(System.Diagnostics.Process process, string pipe_Name, int? maxAwaitForConnection = null)
        {
            PipelineName = pipe_Name;
            MaxAwaitForConnection = maxAwaitForConnection;

            Process = process;
        }

        public bool IsProcessRunning() => !Process.HasExited;
        public object IClone() => Clone();
        public DisassemblerWrapper Clone() => new DisassemblerWrapper(Process, PipelineName, MaxAwaitForConnection);

        public bool Connect()
        {
            PipeClient = new NamedPipeClientStream(".", PipelineName, PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);

            if (MaxAwaitForConnection != null && MaxAwaitForConnection.Value > 0)
                PipeClient.Connect(MaxAwaitForConnection.Value);
            else
                PipeClient.Connect();

            if (PipeClient.IsConnected) PipeClient.ReadMode = PipeTransmissionMode.Message;

            return PipeClient.IsConnected;
        }
        public bool ReConncet()
        {
            Close();
            return Connect();
        }
        public void Close() => PipeClient?.Close();

        public DisassembledInstruction RemoteGetPreviousInstruction(byte[] data, IntPtr virtualAddress)
        {
            if (!PipeClient.IsConnected) return null;
            DisassembledInstruction disassembled = null;
            var streamString = new StreamString(PipeClient);
            var parameters = new Parameters()
            {
                Data = data.Clone() as byte[],
                VirtualAddress = virtualAddress,
                MaxInstructions = -3
            };

            try
            {
                var jsonFormat = JsonConvert.SerializeObject(parameters);
                PipeClient.WriteString(jsonFormat);
                string sb = PipeClient.ReadBigString();
                var ins = JsonConvert.DeserializeObject<List<Smdkd.InstructionData>>(sb);
                disassembled = ins.Select(i => new DisassembledInstruction(ref i)).First();
            }
            catch (Exception ex)
            {
                Program.ShowException(ex);
            }

            return disassembled;
        }
        public IReadOnlyList<DisassembledInstruction> DisassembleFunction(byte[] data, IntPtr virtualAddress)
        {
            if (!PipeClient.IsConnected) return null;
            IReadOnlyList<DisassembledInstruction> disassembleds = null;
            var streamString = new StreamString(PipeClient);
            var parameters = new Parameters()
            {
                Data = data.Clone() as byte[],
                VirtualAddress = virtualAddress,
                MaxInstructions = -2
            };

            try
            {
                var jsonFormat = JsonConvert.SerializeObject(parameters);
                PipeClient.WriteString(jsonFormat);
                string sb = PipeClient.ReadBigString();
                var ins = JsonConvert.DeserializeObject<List<Smdkd.InstructionData>>(sb);
                disassembleds = ins.Select(i => new DisassembledInstruction(ref i)).ToList();
            }
            catch (Exception ex)
            {
                Program.ShowException(ex);
            }

            return disassembleds;
        }
        public IReadOnlyList<DisassembledInstruction> DisassembleCode(byte[] data, IntPtr virtualAddress, int maxInstructions)
        {
            if (!PipeClient.IsConnected) return null;
            IReadOnlyList<DisassembledInstruction> disassembleds = null;
            var streamString = new StreamString(PipeClient);
            var parameters = new Parameters()
            {
                Data = data.Clone() as byte[],
                VirtualAddress = virtualAddress,
                MaxInstructions = maxInstructions
            };

            try
            {
                var jsonFormat = JsonConvert.SerializeObject(parameters);
                PipeClient.WriteString(jsonFormat);
                string sb = PipeClient.ReadBigString();
                var ins = JsonConvert.DeserializeObject<List<Smdkd.InstructionData>>(sb);
                disassembleds = ins.Select(i => new DisassembledInstruction(ref i)).ToList();
            }
            catch (Exception ex)
            {
                Program.ShowException(ex);
            }

            return disassembleds;
        }
    }
    public static class PipeStreamEx {
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
