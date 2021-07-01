using SmScanner.Core;
using SmScanner.Core.Modules;
using SmScanner.Forms;
using SmScanner.Util;
using SmScanner.Wrappers;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SmScanner
{
    static class Program
    {
        //public static ILogger Logger { get; private set; }
        public static Process ProcessWrapper { get; private set; }
        public static IDisassemblerWrapper Disassembler32 { get; private set; }
        public static Settings Settings { get; private set; }
        public static Random GlobalRandom { get; } = new Random();
        public static RemoteProcess RemoteProcess { get; private set; } = new RemoteProcess();
        public static string AddressHexFormat { get => RemoteProcess.IsWow64 ? "X08" : "X10"; }
        public static bool DesignMode { get; private set; } = true;
        public static FontEx MonoSpaceFont { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DesignMode = false; // The designer doesn't call Main()

            try
            {
                DpiUtil.ConfigureProcess();
                DpiUtil.TrySetDpiFromCurrentDesktop();
            }
            catch { /* ignored */ }

            MonoSpaceFont = new FontEx
            {
                Font = new Font("Courier New", DpiUtil.ScaleIntX(13), GraphicsUnit.Pixel),
                Width = DpiUtil.ScaleIntX(8),
                Height = DpiUtil.ScaleIntY(16)
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            Settings = SettingsSerializer.Load();

            if (Settings.RunAsAdmin && !WinUtil.IsAdministrator)
            {
                WinUtil.RunElevated(Process.GetCurrentProcess().MainModule?.FileName, null);
                return;
            }

            var process = Process.GetProcessesByName("Disassembler32.Wrapper");
            if( process.Count() <= 0)
            {
                ProcessWrapper = Process.Start(
                    $"{System.IO.Directory.GetCurrentDirectory()}\\Disassembler32.Wrapper.exe",
                    "SmEnv Loop");
            }
            else ProcessWrapper = process.First();

            Disassembler32 = new DisassemblerWrapper(
                ProcessWrapper,
                "SmDisassembler32Pipe",
                -1
                );

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.Run(new ScannerForm());// Form1 ScannerForm MemoryViewForm

            ProcessWrapper?.Kill();
            RemoteProcess.Dispose();

            SettingsSerializer.Save(Settings);
        }

        public static Process RunProcessWrapper(string path, string args)
        {
           return Process.Start(path, args);
        }

        public static bool ProcessAlredyRunning(string process_name)
        {
            var process = Process.GetProcessesByName(process_name);
           return process != null;
        }

        /// <summary>Shows the exception in a special form.</summary>
        /// <param name="ex">The exception.</param>
        public static void ShowException(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        /// <summary>Shows the message in a special form.</summary>
        /// <param name="msg">The message.</param>
        public static void ShowMessage(string msg)
        {
            MessageBox.Show(msg,"SmScanner");
        }

        public static void SetControlDoubleBuffered(Control control)
        {
            Type dgvType = control.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
              BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(control, true, null);
        }
    }
}
