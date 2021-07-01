using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmScanner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            int size = 1024 * 5;
            Smdkd.Attach();
            var process_data = Smdkd.GetProcessData("ac_client.exe"); // ac_client.exe
            byte[] bytes = new byte[size];
            Smdkd.ReadMemBlock(process_data.Id, process_data.Address, (IntPtr)System.Diagnostics.Process.GetCurrentProcess().Id, bytes, bytes.Length);


            var hexDumps = Smdkd.GetHexDumps(process_data.Address, bytes);
            var disassembles = Smdkd.GetDisassembles(process_data.Address, bytes, process_data.IsWow64);
        }

    }
}
