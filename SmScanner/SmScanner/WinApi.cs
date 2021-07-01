using SmScanner.Core.Extensions;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace SmScanner
{

    public static class WinApi
    {
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0;
        private const uint SHGFI_SMALLICON = 0x1;
        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };
        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbSizeFileInfo, uint uFlags);

        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern int DestroyIcon(IntPtr hIcon);

        public static Icon GetIconForFile(string path)
        {
            var shinfo = new SHFILEINFO();
            if (!SHGetFileInfo(path, 0, ref shinfo, Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON).IsNull())
            {
                var icon = Icon.FromHandle(shinfo.hIcon).Clone() as Icon;
                DestroyIcon(shinfo.hIcon);
                return icon;
            }

            return null;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetProcessDPIAware();

        private enum ProcessDpiAwareness : uint
        {
            Unaware = 0,
            SystemAware = 1,
            PerMonitorAware = 2
        }
        [DllImport("shcore.dll")]
        private static extern int SetProcessDpiAwareness([MarshalAs(UnmanagedType.U4)] ProcessDpiAwareness a);
        public static void SetProcessDpiAwareness()
        {
            SetProcessDpiAwareness(ProcessDpiAwareness.SystemAware);
        }


        [DllImport("dbghelp.dll", CharSet = CharSet.Unicode)]
        private static extern int UnDecorateSymbolName(string DecoratedName, StringBuilder UnDecoratedName, int UndecoratedLength, int Flags);
        public static string UndecorateSymbolName(string name)
        {
            var sb = new StringBuilder(255);
            if (UnDecorateSymbolName(name, sb, sb.Capacity, /*UNDNAME_NAME_ONLY*/0x1000) != 0)
            {
                return sb.ToString();
            }
            return name;
        }
    }
}
