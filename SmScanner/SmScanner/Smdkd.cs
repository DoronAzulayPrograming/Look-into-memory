using SmScanner.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace SmScanner
{
    public static class Smdkd
    {
        #region Enums

        public enum TerminationMethod
        {
            TerminateEx,
            UnmapImage
        }

        public enum SmSectionCategory
        {
            Unknown,
            CODE,
            DATA,
            HEAP
        }

        [Flags]
        public enum SmSectionProtection
        {
            NoAccess = 0,

            Read = 1,
            Write = 2,
            CopyOnWrite = 4,
            Execute = 8,

            Guard = 16
        }

        public enum SmSectionType
        {
            Unknown,

            Private,
            Mapped,
            Image
        }

        #endregion

        #region Structs

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct ProcessData
        {
            public bool IsWow64;
            public IntPtr Id;
            public IntPtr Address;
            public IntPtr VirtualSize;
            public uint NameOffset;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string Path;

            public SmProcessData ToProcessData()
            {
                return new SmProcessData
                {
                    IsWow64 = IsWow64,
                    Id = Id,
                    Address = Address,
                    VirtualSize = VirtualSize,
                    NameOffset = NameOffset,
                    Path = Path
                };
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct EnumerateDumpData
        {
            public IntPtr Address;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public int[] HexBytes;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] TextBytes;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct EnumerateDisassembleData
        {
            public IntPtr Address;
            public long InstructionLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] Instruction;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Opcodes;
        };

        //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        //public struct EnumerateProcessData
        //{
        //    public IntPtr Id;
        //    public bool IsWow64;
        //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        //    public string Path;
        //};

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct EnumerateProcessData
        {
            public IntPtr Id;
            public bool IsWow64;
            public IntPtr Address;
            public IntPtr VirtualSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string Path;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public struct EnumerateRemoteModuleData
        {
            public IntPtr BaseAddress;

            public IntPtr Size;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string Path;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public struct EnumerateRemoteSectionData
        {
            public IntPtr BaseAddress;

            public IntPtr Size;

            public SmSectionType Type;

            public SmSectionCategory Category;

            public SmSectionProtection Protection;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string Name;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string ModulePath;
        }
        #endregion

        #region Classes

        public class SmProcessData
        {
            public bool IsWow64;
            public IntPtr Id;
            public IntPtr Address;
            public IntPtr VirtualSize;
            public uint NameOffset;
            public string Path;
        };

        public class SmHexDumpData
        {
            public IntPtr Address { get; }
            public int[] HexBytes { get; }
            public char[] TextBytes { get; }

            public SmHexDumpData(IntPtr address, int[] hexbytes, char[] textBytes)
            {
                Address = address;
                HexBytes = hexbytes;
                TextBytes = textBytes;
            }
        };
        public class SmDisassembleData
        {
            public IntPtr Address { get; }
            public long InstructionLength { get; }
            public byte[] Instruction { get; }
            public string Opcodes { get; }

            public SmDisassembleData(IntPtr address, byte[] instruction, long instructionLength, string opcodes)
            {
                Address = address;
                InstructionLength = instructionLength;
                Instruction = instruction;
                Opcodes = opcodes;
            }
        };

        public class SmProcessBasicData
        {
            public IntPtr Id;
            public string Path;
        };

        public class SmProcessInfo
        {
            public IntPtr Id { get; }
            public string Name { get; }
            public string Path { get; }
            public bool IsWow64 { get; }
            public IntPtr Address { get; }
            public IntPtr VirtualSize { get; }
            public IntPtr EndAddress { get => IntPtrExtension.From(Address.ToInt64()).Add(VirtualSize); }
            public System.Drawing.Image Icon => icon.Value;

            private readonly Lazy<System.Drawing.Image> icon;
            //public System.Drawing.Image Icon { get; }

            public SmProcessInfo(EnumerateProcessData data)
            {

                try
                {
                    System.Diagnostics.Contracts.Contract.Requires(data.Path != null);

                    Id = data.Id;
                    Name = System.IO.Path.GetFileName(data.Path);
                    Path = data.Path;
                    IsWow64 = data.IsWow64;
                    Address = data.Address;
                    VirtualSize = data.VirtualSize;
                    icon = new Lazy<System.Drawing.Image>(() =>
                    {
                        using var i = WinApi.GetIconForFile(Path);
                        return i?.ToBitmap();
                    });
                    //Icon = new System.Drawing.Bitmap(System.Drawing.Icon.ExtractAssociatedIcon(Path).ToBitmap(), 20, 20);
                }
                catch (Exception ex)  {  Program.ShowException(ex); }
            }
            public SmProcessInfo(IntPtr id, IntPtr address, IntPtr virtualSize, bool isWow64, string path)
            {

                try
                {
                    System.Diagnostics.Contracts.Contract.Requires(path != null);

                    Id = id;
                    Name = System.IO.Path.GetFileName(path);
                    Path = path;
                    IsWow64 = isWow64;
                    Address = address;
                    VirtualSize = virtualSize;
                    icon = new Lazy<System.Drawing.Image>(() =>
                    {
                        using var i = WinApi.GetIconForFile(Path);
                        return i?.ToBitmap();
                    });
                    //Icon = new System.Drawing.Bitmap(System.Drawing.Icon.ExtractAssociatedIcon(Path).ToBitmap(), 20, 20);
                }
                catch (Exception ex) { Program.ShowException(ex); }
            }
        }
        public class SmModule
        {
            public IntPtr Start { get; set; }
            public IntPtr End { get; set; }
            public IntPtr Size { get; set; }
            public string Name { get; set; }
            public string Path { get; set; }
        }
        public class SmSection
        {
            public IntPtr Start { get; set; }
            public IntPtr End { get; set; }
            public IntPtr Size { get; set; }
            public string Name { get; set; }
            public SmSectionCategory Category { get; set; }
            public SmSectionProtection Protection { get; set; }
            public SmSectionType Type { get; set; }
            public string ModuleName { get; set; }
            public string ModulePath { get; set; }
        }


        public class DistinctProcessInfoComparer : IEqualityComparer<SmProcessInfo>
        {
            public bool Equals(SmProcessInfo x, SmProcessInfo y)
            {
                return x.Name.Equals(y.Name);
            }

            public int GetHashCode(SmProcessInfo obj)
            {
                return obj.Name.GetHashCode();
            }
        }
        #endregion

        public delegate void EnumerateDumpCallback(ref EnumerateDumpData data);
        public delegate void EnumerateDisassembleCallback(ref EnumerateDisassembleData data);

        public delegate void EnumerateProcessCallback(ref EnumerateProcessData data);
        public delegate void EnumerateRemoteModuleCallback(ref EnumerateRemoteModuleData data);
        public delegate void EnumerateRemoteSectionCallback(ref EnumerateRemoteSectionData data);

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

        [DllImport("Disassembler64.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool DisassembleCode(IntPtr address, IntPtr length, IntPtr virtualAddress, bool determineStaticInstructionBytes, EnumerateInstructionCallback callback);
        public static bool DisassembleCode(IntPtr address, int length, IntPtr virtualAddress, bool determineStaticInstructionBytes, EnumerateInstructionCallback callback)
        {
            return DisassembleCode(address, (IntPtr)length, virtualAddress, determineStaticInstructionBytes, callback);
        }


        [DllImport("Smdkddll.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool HexDumpBytes(IntPtr start_address, byte[] bytesBuffer, int len, EnumerateDumpCallback callbackDump);
        
        [DllImport("Smdkddll.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool DisassembleBytes(IntPtr start_address, byte[] bytesBuffer, int len, bool isWow64, EnumerateDisassembleCallback callbackDisassemble);


        [DllImport("Smdkddll.dll")] // PUBLIC
        public static extern bool Attach();

        [DllImport("Smdkddll.dll")] // PUBLIC
        public static extern void Detach();


        [DllImport("Smdkddll.dll")] // PUBLIC
        public static extern bool IsProcessValid(IntPtr process_handle);

        [DllImport("Smdkddll.dll")] // PUBLIC
        public static extern bool IsProcessWow64(IntPtr process_id, out bool isWow64);


        [DllImport("Smdkddll.dll")]
        private static extern bool GetProccessData(string process_name, out ProcessData process_data);


        [DllImport("Smdkddll.dll")] // PUBLIC
        public static extern bool OpenRemoteProcess(IntPtr process_id, out IntPtr process_handle);

        [DllImport("Smdkddll.dll")] // PUBLIC
        public static extern bool ResumeRemoteProcess(IntPtr process_handle);

        [DllImport("Smdkddll.dll")] // PUBLIC
        public static extern bool SuspendRemoteProcess(IntPtr process_handle);

        [DllImport("Smdkddll.dll")] // PUBLIC
        public static extern bool TerminateRemoteProcess(IntPtr process_handle, TerminationMethod method);


        [DllImport("Smdkddll.dll")] // PUBLIC
        public static extern bool OpenRemoteThread(IntPtr thread_id, out IntPtr thread_handle);

        [DllImport("Smdkddll.dll")] // PUBLIC
        public static extern bool ResumeRemoteThread(IntPtr thread_handle);

        [DllImport("Smdkddll.dll")] // PUBLIC
        public static extern bool SuspendRemoteThread(IntPtr thread_handle);



        [DllImport("Smdkddll.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool EnumerateProcesses(EnumerateProcessCallback callbackProcess);

        [DllImport("Smdkddll.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool EnumerateModules(IntPtr process_id, EnumerateRemoteModuleCallback callbackModule);

        [DllImport("Smdkddll.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool EnumerateRemoteSectionsAndModules(IntPtr process_id, EnumerateRemoteSectionCallback callbackSection, EnumerateRemoteModuleCallback callbackModule);



        [DllImport("Smdkddll.dll", EntryPoint = "MmCpy")]
        public static extern bool ReadMem(IntPtr s_pid, IntPtr s_address, IntPtr t_pid, ref IntPtr t_address, int size);

        [DllImport("Smdkddll.dll", EntryPoint = "MmCpy")]
        public static extern bool ReadMemBlock(IntPtr s_pid, IntPtr s_address, IntPtr t_pid, byte[] buffer, int size);

        [DllImport("Smdkddll.dll", EntryPoint = "MmCpy")]
        public static extern bool WriteMem(IntPtr s_pid, ref IntPtr s_address, IntPtr t_pid, IntPtr t_address, int size);

        [DllImport("Smdkddll.dll", EntryPoint = "MmCpy")]
        public static extern bool WriteMemBlock(IntPtr s_pid, byte[] buffer, IntPtr t_pid, IntPtr t_address, int size);

        [DllImport("Smdkddll.dll", EntryPoint = "MmCpy")]
        public static extern bool WriteMemBlock(IntPtr s_pid, char[] buffer, IntPtr t_pid, IntPtr t_address, int size);

        [DllImport("Smdkddll.dll", EntryPoint = "MmCpy")]
        public static extern bool MmCpy(IntPtr s_pid, byte[] s_buffer, IntPtr t_pid, byte[] t_buffer, int size);


        #endregion

        public static IList<SmHexDumpData> GetHexDumps(IntPtr start_address, byte[] bytes)
        {
            List<SmHexDumpData> list = new List<SmHexDumpData>();
            EnumerateHexDumps(start_address, bytes, list.Add);

            return list;
        }
        public static bool EnumerateHexDumps(IntPtr start_address, byte[] bytes, Action<SmHexDumpData> callbackHexDump)
        {
            var c = (EnumerateDumpCallback)delegate (ref EnumerateDumpData data)
            {
                callbackHexDump(new SmHexDumpData(data.Address, data.HexBytes, data.TextBytes));
            };

            return HexDumpBytes(start_address, bytes, bytes.Length, c);
        }

        public static IList<SmDisassembleData> GetDisassembles(IntPtr start_address, byte[] bytes, bool isWow64)
        {
            List<Smdkd.SmDisassembleData> list = new List<Smdkd.SmDisassembleData>();
            EnumerateDisassembles(start_address, bytes, isWow64,list.Add);

            return list;
        }
        public static bool EnumerateDisassembles(IntPtr start_address, byte[] bytes, bool isWow64, Action<SmDisassembleData> callbackHexDump)
        {
            var c = (EnumerateDisassembleCallback)delegate (ref EnumerateDisassembleData data)
            {
                callbackHexDump(new SmDisassembleData(data.Address, data.Instruction, data.InstructionLength, data.Opcodes));
            };

            return DisassembleBytes(start_address, bytes, bytes.Length, isWow64, c);
        }

        public static IList<SmProcessInfo> EnumerateProcesses()
        {
            var processes = new List<SmProcessInfo>();
            EnumerateProcesses(processes.Add);
            return processes;
        }
        public static bool EnumerateProcesses(Action<SmProcessInfo> callbackProcess)
        {
            var c = callbackProcess == null ? null : (EnumerateProcessCallback)delegate (ref EnumerateProcessData data)
            {
                if(data.Path.Split('\\').First().Length != 0)
                    callbackProcess(new SmProcessInfo(data));
            };

            return EnumerateProcesses(c);
        }

        public static SmProcessData GetProcessData(string process_name)
        {
            if (!GetProccessData(process_name, out var process_data))
                return null;

            return process_data.ToProcessData();
        }

        public static bool EnumerateRemoteModules(IntPtr process_id, out List<SmModule> _modules)
        {
            _modules = new List<SmModule>();

            EnumerateRemotModules(process_id, _modules.Add);

            return true;
        }
        public static void EnumerateRemotModules(IntPtr process_id, Action<SmModule> callbackModule)
        {
            var c2 = callbackModule == null ? null : (EnumerateRemoteModuleCallback)delegate (ref EnumerateRemoteModuleData data)
            {
                callbackModule(new SmModule
                {
                    Start = data.BaseAddress,
                    End = data.BaseAddress.Add(data.Size),
                    Size = data.Size,
                    Path = data.Path,
                    Name = System.IO.Path.GetFileName(data.Path)
                });
            };

            EnumerateModules(process_id, c2);
        }
        public static void EnumerateRemoteSectionsAndModules(IntPtr process, out List<SmSection> sections, out List<SmModule> modules)
        {
            sections = new List<SmSection>();
            modules = new List<SmModule>();

            EnumerateRemoteSectionsAndModules(process, sections.Add, modules.Add);
        }
        public static void EnumerateRemoteSectionsAndModules(IntPtr process, Action<SmSection> callbackSection, Action<SmModule> callbackModule)
        {
            var c1 = callbackSection == null ? null : (EnumerateRemoteSectionCallback)delegate (ref EnumerateRemoteSectionData data)
            {
                callbackSection(new SmSection
                {
                    Start = data.BaseAddress,
                    End = data.BaseAddress.Add(data.Size),
                    Size = data.Size,
                    Name = data.Name,
                    Protection = data.Protection,
                    Type = data.Type,
                    ModulePath = data.ModulePath,
                    ModuleName = System.IO.Path.GetFileName(data.ModulePath),
                    Category = data.Category
                });
            };

            var c2 = callbackModule == null ? null : (EnumerateRemoteModuleCallback)delegate (ref EnumerateRemoteModuleData data)
            {
                callbackModule(new SmModule
                {
                    Start = data.BaseAddress,
                    End = data.BaseAddress.Add(data.Size),
                    Size = data.Size,
                    Path = data.Path,
                    Name = System.IO.Path.GetFileName(data.Path)
                });
            };

            EnumerateRemoteSectionsAndModules(process, c1, c2);
        }

        public static T Rlmfp<T>(IntPtr pid, Int64 address) where T : struct
        {
            int byteSize = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[byteSize];

            ReadMemBlock(pid, (IntPtr)address, (IntPtr)System.Diagnostics.Process.GetCurrentProcess().Id, buffer, buffer.Length);

            return ByteArrayToStruct<T>(buffer);
        }
        public static void Wlmfp<T>(IntPtr pid, Int64 address, T args) where T : struct
        {
            byte[] buffer = StructToByteArray(args);
            WriteMemBlock((IntPtr)System.Diagnostics.Process.GetCurrentProcess().Id, buffer, pid, (IntPtr)address, buffer.Length);
        }

        #region Helpers
        public static byte[] StructToByteArray(object obj)
        {
            int size = Marshal.SizeOf(obj);//16*9
            byte[] sizeArray = new byte[size];

            IntPtr cPtrSize = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, cPtrSize, true);
            Marshal.Copy(cPtrSize, sizeArray, 0, size);
            Marshal.FreeHGlobal(cPtrSize);

            return sizeArray;
        }
        public static T ByteArrayToStruct<T>(byte[] currentBytes) where T : struct
        {
            var gcHandle = GCHandle.Alloc(currentBytes, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                gcHandle.Free();
            }
        }
        #endregion
    }
}
