using Newtonsoft.Json;
using SmScanner.Controls;
using SmScanner.Core.Extensions;
using SmScanner.Core.Interfaces;
using SmScanner.Core.Memory;
using SmScanner.Core.Modules;
using SmScanner.Util;
using SmScanner.Wrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmScanner.Forms
{
    public partial class MemoryViewForm : Form
    {
        private string PathToSaveTheTable = string.Empty;


        IntPtr initial_address;
        Timer RefreshRecordsTimer;
        private readonly RemoteProcess process;
        private readonly StructsViewForm structsForm;
        private readonly MemoryViewSettingsForm settingsForm;
        private readonly Disassembler disassembler;
        private readonly Disassembler tiks_disassembler;
        private readonly IDisassemblerWrapper wrapper;
        private readonly IDisassemblerWrapper tiks_wrapper;
        public MemoryViewForm(RemoteProcess process)
        {
            Contract.Requires(process != null);
            this.process = process;

            InitializeComponent();
            structsForm = new StructsViewForm();
            settingsForm = new MemoryViewSettingsForm();

            RefreshRecordsTimer = new Timer();
            RefreshRecordsTimer.Interval = 550;
            RefreshRecordsTimer.Tick += new EventHandler(RefreshRecordsTimer_Ticks);

            wrapper = Program.Disassembler32.Clone();
            wrapper.Connect();
            disassembler = new Disassembler(wrapper);

            tiks_wrapper = wrapper.Clone();
            tiks_wrapper.Connect();
            tiks_disassembler = new Disassembler(tiks_wrapper);

            hexDumpRecordList.Init(process);
            disassembleRecordList.Init(process, wrapper);

            RefreshRecordsTimer.Start();
        }
        public void InitFromRandomAddress()
        {
            var firstModule = process.GetModuleToPointer(process.UnderlayingProcess.Address);

            long start = firstModule.Start.ToInt64() + (firstModule.Size.ToInt64() / 3);
            long end = firstModule.End.ToInt64() - (firstModule.Size.ToInt64() / 3);

            initial_address = (IntPtr)(new Random().Long(start, end));
        }

        #region Events


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CenterToParent();

            hexDumpRecordList.GoToAddress(initial_address);
            disassembleRecordList.GoToAddress(initial_address);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            wrapper.Close();
            tiks_wrapper.Close();
            RefreshRecordsTimer?.Dispose();
        }

        private async void RefreshRecordsTimer_Ticks(object sender, EventArgs e)
        {
            await hexDumpRecordList.RefreshVisibleRecordsAsync();
            await disassembleRecordList.RefreshVisibleRecordsAsync(tiks_disassembler);
        }

        private void copyOpcodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string textToClipboard = string.Empty;
            var records = disassembleRecordList.SelectedRecords.Reverse();
            if (records != null)
            {
                int len = records.Count();
                string enter = string.Empty;
                if (len > 1) enter += '\n';
                foreach (var record in records) textToClipboard += $"{record.opcode}{enter}";
                Clipboard.SetText(textToClipboard);
            }
        }
        private void copyBytesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cms = (ContextMenuStrip)((ToolStripDropDown)((ToolStripMenuItem)sender).Owner).OwnerItem.Owner;
            try
            {
                var recordList = (DisassembleRecordList)cms.SourceControl.Parent;

                string textToClipboard = string.Empty;
                var records = recordList.SelectedRecords.Reverse();
                if (records != null)
                {
                    int len = records.Count();
                    string enter = string.Empty;
                    if (len > 1) enter += '\n';
                    foreach (var record in records) textToClipboard += $"{record.bytes}{enter}";
                    Clipboard.SetText(textToClipboard);
                }
            }
            catch (Exception)
            {
                var recordList = (HexDumpRecordList)cms.SourceControl.Parent;

                string textToClipboard = string.Empty;
                var records = recordList.SelectedRecords.Reverse();
                if (records != null)
                {
                    int len = records.Count();
                    string enter = string.Empty;
                    if (len > 1) enter += '\n';
                    foreach (var record in records) textToClipboard += $"{string.Join(" ", record.Bytes.Select(b => $"{b:X2}"))}{enter}";
                    Clipboard.SetText(textToClipboard);
                }
            }
        }
        private void copyAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cms = (ContextMenuStrip)((ToolStripDropDown)((ToolStripMenuItem)sender).Owner).OwnerItem.Owner;
            try
            {
                var recordList = (DisassembleRecordList)cms.SourceControl.Parent;

                string textToClipboard = string.Empty;
                var records = recordList.SelectedRecords.Reverse();
                if (records != null)
                {
                    int len = records.Count();
                    string enter = string.Empty;
                    if (len > 1) enter += '\n';
                    foreach (var record in records) textToClipboard += $"{record.address}{enter}";
                    Clipboard.SetText(textToClipboard);
                }
            }
            catch (Exception)
            {
                var recordList = (HexDumpRecordList)cms.SourceControl.Parent;

                string textToClipboard = string.Empty;
                var records = recordList.SelectedRecords.Reverse();
                if (records != null)
                {
                    int len = records.Count();
                    string enter = string.Empty;
                    if (len > 1) enter += '\n';
                    foreach (var record in records) textToClipboard += $"{record.address}{enter}";
                    Clipboard.SetText(textToClipboard);
                }
            }
        }
        private void copyRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cms = (ContextMenuStrip)((ToolStripDropDown)((ToolStripMenuItem)sender).Owner).OwnerItem.Owner;
            try
            {
                var recordList = (DisassembleRecordList)cms.SourceControl.Parent;

                string textToClipboard = string.Empty;
                var records = recordList.SelectedRecords.Reverse();
                if (records != null)
                {
                    int len = records.Count();
                    string enter = string.Empty;
                    if (len > 1) enter += '\n';
                    foreach (var record in records) textToClipboard += $"{record.address} {record.bytes} {record.opcode}{enter}";
                    Clipboard.SetText(textToClipboard);
                }
            }
            catch (Exception)
            {
                var recordList = (HexDumpRecordList)cms.SourceControl.Parent;

                string textToClipboard = string.Empty;
                var records = recordList.SelectedRecords;
                if (records != null)
                {
                    int len = records.Count();
                    string enter = string.Empty;
                    if (len > 1) enter += '\n';
                    foreach (var record in records) textToClipboard += $"{record.address} {string.Join(" ", record.Bytes.Select(b => $"{b:X2}"))} {string.Join(" ", record.Chars.Select(c => $"{c}"))}{enter}";
                    Clipboard.SetText(textToClipboard);
                }
            }
        }
        private void copySelectedsValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string textToClipboard = string.Empty;
            var cells = hexDumpRecordList.GetSelectedCells();
            if (cells != null)
            {
                int len = cells.Count();
                string enter = string.Empty;
                if (len > 1) enter += '\n';

                var rowsIndexes = cells.Select(c => c.RowIndex).Distinct().OrderBy(v => v);
                foreach (var index in rowsIndexes)
                    textToClipboard += string.Join(" ", cells.Where(c => c.RowIndex == index).OrderBy(c=>c.ColumnIndex).Select(c => $"{c.Value}")) + enter;
                Clipboard.SetText(textToClipboard);
            }
        }
        private void goToAddressDisassembleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cms = (ContextMenuStrip)((ToolStripMenuItem)sender).Owner;

            if (typeof(DisassembleRecordList).IsInstanceOfType(cms.SourceControl.Parent))
            {
                var recordList = (DisassembleRecordList)cms.SourceControl.Parent;
                var record = recordList.SelectedRecord;
                var vf = new ChangeValueForm(record.Address.ToString(Program.AddressHexFormat), "Enter Hex Address.", "Go to Address");
                if (vf.ShowDialog(this) == DialogResult.OK)
                {
                    if (long.TryParse(vf.ValueText, System.Globalization.NumberStyles.HexNumber, null, out var address))
                    {
                        disassembleRecordList.GoToAddress((IntPtr)(address));
                    }
                    else
                    {
                        disassembleRecordList.GoToAddress(vf.ValueText);
                    }
                }
            }
            else
            {
                var recordList = (HexDumpRecordList)cms.SourceControl.Parent;

                var record = recordList.SelectedRecord;
                var vf = new ChangeValueForm(record.Address.ToString(Program.AddressHexFormat), "Enter Hex Address.", "Go to Address");
                if (vf.ShowDialog(this) == DialogResult.OK)
                {
                    if (long.TryParse(vf.ValueText, System.Globalization.NumberStyles.HexNumber, null, out var address))
                    {
                        hexDumpRecordList.GoToAddress((IntPtr)address);
                    }
                }
            }
        }
        private void selectCurrentFunctionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var record = disassembleRecordList.SelectedRecord;
            var list = disassembleRecordList.Records
                .Where(r => r.Address.CompareTo(record.Address) <= 0)
                .Select(r=>r.ToInstraction());

            var start = disassembler.RemoteGetFirstFunctionStartAddress(list);

            list = disassembleRecordList.Records
                .Where(r => r.Address.CompareTo(record.Address) >= 0)
                .Select(r => r.ToInstraction());

            var end = disassembler.RemoteGetFirstFunctionEndAddress(list);

            if (end == IntPtr.Zero && start != IntPtr.Zero)
            {
                Program.ShowMessage("Cant find function end address.\n try to load from:\n 10 instractions after current address.");
                return;
            }

            else if (start == IntPtr.Zero && end != IntPtr.Zero)
            {
                Program.ShowMessage("Cant find function start address.\n try to load from:\n 10 instractions brfore current address.");
                return;
            }
            
            else if (start == IntPtr.Zero && end == IntPtr.Zero)
            {
                Program.ShowMessage("Cant find function start and end address.\n try to set bigger buffer size from the settings.");
                return;
            }


            disassembleRecordList.SelectRecords(start,end);
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            var cms = (ContextMenuStrip)sender;

            var isHexList = cms.SourceControl.Parent == hexDumpRecordList;

            copyOpcodetoolStripMenuItem.Visible = !isHexList;
            copySelectedsValuesToolStripMenuItem.Visible = isHexList;
            selectCurrentFunctionToolStripMenuItem.Visible = !isHexList;

            // Hide all other items if multiple records are selected.
            var multipleRecordsSelected = (isHexList ? hexDumpRecordList.SelectedRecords.Count : disassembleRecordList.SelectedRecords.Count) > 1;
            for (var i = 0; i < cms.Items.Count - 1; ++i)
                cms.Items[i].Visible = !multipleRecordsSelected;

            if (isHexList)
            {
                selectCurrentFunctionToolStripMenuItem.Visible = !isHexList;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Json files (*.json)|*.json";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader reader = new StreamReader(ofd.FileName);
                    var strFromFile = reader.ReadToEnd();
                    if(!strFromFile.Contains("~"))
                    {
                        Program.ShowException(new Exception("Error the file table are not in the currect layout."));
                        return;
                    }

                    var strSplit = strFromFile.Split('~');
                    var hexJsonFormat = strSplit[0];
                    var disassemblerJsonFormat = strSplit[1];

                    var hexPtrs = JsonConvert.DeserializeObject<List<IntPtr>>(hexJsonFormat);
                    var disassemblerPtrs = JsonConvert.DeserializeObject<List<IntPtr>>(disassemblerJsonFormat);

                    var hexRecords = hexPtrs.Select(p=> new HexDumpRecord(p));
                    var disassemblerRecords = disassemblerPtrs.Select(p=> {
                        var ins = new DisassembledInstruction(p);
                        return new DisassembledRecord(ref ins);
                    });


                    if(hexRecords == null || hexRecords.Count() < 0)
                    {
                        Program.ShowException(new Exception("Error the hex records list was null or empty."));
                        return;
                    }

                    if(disassemblerRecords == null || disassemblerRecords.Count() < 0)
                    {
                        Program.ShowException(new Exception("Error the disassemble Records list was null or empty."));
                        return;
                    }

                    if(hexRecords.Count() > 0)
                    {
                        hexDumpRecordList.GoToAddress(hexRecords.First().Address);
                        hexDumpRecordList.SelectRecords(hexRecords);
                    }

                    if (disassemblerRecords.Count() > 0)
                    {
                        disassembleRecordList.GoToAddress(disassemblerRecords.First().Address);
                        disassembleRecordList.SelectRecords(disassemblerRecords);
                    }


                    PathToSaveTheTable = ofd.FileName;
                }
                catch (Exception ex)
                {
                    PathToSaveTheTable = string.Empty;
                    Program.ShowException(ex);
                }
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var hexRecords = hexDumpRecordList.SelectedRecords.Select(r => r.Address);
                var disassembleRecords = disassembleRecordList.SelectedRecords.OrderBy(r => r.Address, IntPtrComparer.Instance).Select(r => r.Address);

                var hexJsonFormat = JsonConvert.SerializeObject(hexRecords);
                var disassembleJsonFormat = JsonConvert.SerializeObject(disassembleRecords);

                var strTofile = $"{hexJsonFormat}~{disassembleJsonFormat}";

                StreamWriter writer = new StreamWriter(PathToSaveTheTable);
                writer.Write(strTofile);
                writer.Close();
            }
            catch (Exception ex)
            {
                Program.ShowException(ex);
            }
        }
        private void saveAstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.DefaultExt = "json";
            sfd.FileName = "memoryView_table";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var hexRecords = hexDumpRecordList.SelectedRecords.Select(r => r.Address);
                    var disassembleRecords = disassembleRecordList.SelectedRecords.OrderBy(r => r.Address, IntPtrComparer.Instance).Select(r => r.Address);

                    var hexJsonFormat = JsonConvert.SerializeObject(hexRecords);
                    var disassembleJsonFormat = JsonConvert.SerializeObject(disassembleRecords);

                    var strTofile = $"{hexJsonFormat}~{disassembleJsonFormat}";

                    StreamWriter writer = new StreamWriter(sfd.FileName);
                    writer.Write(strTofile);
                    writer.Close();
                }
                catch (Exception ex)
                {
                    Program.ShowException(ex);
                }
            }
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingsForm.Show();
            settingsForm.Focus();
        }

        private void aboutHexViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new HexViewAbout().Show(this);
        }
        private void aboutDisassemblerViewtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DisassemblerViewAboutForm().Show(this);
        }

        private void structsViewtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            structsForm.Show();
            structsForm.Focus();
        }
        #endregion


        #region Ex
        // This function checks the room size and your text and appropriate font
        //  for your text to fit in room
        // PreferedFont is the Font that you wish to apply
        // Room is your space in which your text should be in.
        // LongString is the string which it's bounds is more than room bounds.
        private Font FindFont(
           Graphics g,
           string longString,
           Size Room,
           Font PreferedFont
        )
        {
            // you should perform some scale functions!!!
            SizeF RealSize = g.MeasureString(longString, PreferedFont);
            float HeightScaleRatio = Room.Height / RealSize.Height;
            float WidthScaleRatio = Room.Width / RealSize.Width;

            float ScaleRatio = (HeightScaleRatio < WidthScaleRatio)
               ? HeightScaleRatio
               : WidthScaleRatio;

            float ScaleFontSize = PreferedFont.Size * ScaleRatio;

            return new Font(PreferedFont.FontFamily, ScaleFontSize);
        }
        #endregion

    }
}
