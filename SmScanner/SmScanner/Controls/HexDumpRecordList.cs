using SmScanner.Core.Extensions;
using SmScanner.Core.Interfaces;
using SmScanner.Core.Memory;
using SmScanner.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmScanner.Controls
{
    public delegate void HexDumpRecordListRecordDoubleClickEventHandler(object sender, HexDumpRecord record);
    public partial class HexDumpRecordList : UserControl
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<HexDumpRecord> Records => bindings;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HexDumpRecord SelectedRecord => GetSelectedRecords().FirstOrDefault();

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<HexDumpRecord> SelectedRecords => GetSelectedRecords().Distinct(new DistinctHexDumpRecordComparer()).OrderBy(r => r.Address, IntPtrComparer.Instance).ToList();
        public void ScrollTo(HexDumpRecord record) => hexDataGridView.ScrollTo(record);
        public void ScrollTo(IntPtr address) => hexDataGridView.ScrollToDumpAddress(address);
        public IEnumerable<DataGridViewCell> GetSelectedCells() => hexDataGridView.SelectedCells.Cast<DataGridViewCell>();

        public event HexDumpRecordListRecordDoubleClickEventHandler RecordDoubleClick;
        public override ContextMenuStrip ContextMenuStrip
        {
            get;
            set;
        }

        //private int READ_SIZE = 1024*4;
        private int READ_SIZE { get => Program.Settings.HexBufferSize; }

        private IRemoteProcess process;
        private DataGridViewCell lastCell;
        private readonly BindingList<HexDumpRecord> bindings;

        private IEnumerable<HexDumpRecord> GetSelectedRecords() => GetSelectedCells().Select(c => (HexDumpRecord)hexDataGridView.Rows[c.RowIndex].DataBoundItem);

        public HexDumpRecordList()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            if (!SystemInformation.TerminalServerSession)
                Program.SetControlDoubleBuffered(hexDataGridView);

            if (Program.DesignMode) return;

            hexDataGridView.AutoGenerateColumns = false;
            hexDataGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            for (int i = 16; i > 0; i--)
                hexDataGridView.Columns[i].Width = 25;

            for (int i = 18; i < 33; i++)
                hexDataGridView.Columns[i].Width = 18;

            bindings = new BindingList<HexDumpRecord>
            {
                AllowNew = true,
                AllowEdit = true,
                RaiseListChangedEvents = true
            };
            hexDataGridView.DataSource = bindings;

            hexDataGridView.Scroll += new ScrollEventHandler(hexDataGrid_Scroll);
            hexDataGridView.CellClick += new DataGridViewCellEventHandler(hexDataGridCell_Click);
            hexDataGridView.CellPainting += new DataGridViewCellPaintingEventHandler(hexDataGridCell_Painting);
            hexDataGridView.CellDoubleClick += new DataGridViewCellEventHandler(hexDataGridView_CellDoubleClick);
        }

        public void Init(IRemoteProcess process)
        {
            Contract.Requires(process != null);

            this.process = process;
        }


        #region Helpers

        public void GoToAddress(IntPtr address)
        {
            IntPtr _address = address.Sub(READ_SIZE / 2).Div(16);
            _address = IntPtrExtension.From(long.Parse($"{_address.ToString("X")}0", System.Globalization.NumberStyles.HexNumber));
            ReadAndLoadHexDump(_address, READ_SIZE);
            hexDataGridView.ScrollToDumpAddress(address);
        }
        private void OnRecordDoubleClick(HexDumpRecord record)
        {
            var evt = RecordDoubleClick;
            evt?.Invoke(this, record);
        }
        public void ReadAndLoadHexDump(IntPtr address, int size)
        {
            byte[] bytes = new byte[size];
            process.ReadRemoteMemoryIntoBuffer(address, ref bytes);

            var hexDumps = HexDump(address, bytes);

            SetHexRecords(hexDumps.Select(d => new HexDumpRecord(ref d)));

            Task.Run(() => RefreshRecords());
        }
        public void SetHexRecords(IEnumerable<HexDumpRecord> records)
        {
            bindings.Clear();

            bindings.RaiseListChangedEvents = false;

            foreach (var record in records) bindings.Add(record);

            bindings.RaiseListChangedEvents = true;
            bindings.ResetBindings();
        }
        public void SelectRecords(IEnumerable<HexDumpRecord> records)
        {
            Contract.Requires(records != null);

            hexDataGridView.ClearSelection();

            foreach (var record in records)
            {
                int index = Records.GetClosestIndex(record.Address);
                if (index < 0) continue;

                foreach (DataGridViewCell cell in hexDataGridView.Rows[index].Cells) cell.Selected = true;
            }

        }

        public void RefreshRecords()
        {
            lock (Records)
            {
                foreach (var record in Records)
                {
                    record.Refresh(process);
                }
            }
            hexDataGridView.Invalidate();
        }
        public Task RefreshRecordsAsync()
        {
            return Task.Run(() =>
            {
                lock (Records)
                {
                    foreach (var record in Records)
                    {
                        record.Refresh(process);
                    }
                }
                hexDataGridView.Invalidate();
            });
        }
        public void RefreshVisibleRecords()
        {
            var visibleRecords = hexDataGridView.GetVisibleRows().Cast<DataGridViewRow>().Select(r => (HexDumpRecord)r.DataBoundItem);

            lock (visibleRecords)
            {
                foreach (var record in visibleRecords)
                {
                    record.Refresh(process);
                }
            }

            hexDataGridView.Invalidate();
        }
        public async Task RefreshVisibleRecordsAsync()
        {
            var visibleRecords = hexDataGridView.GetVisibleRows().Cast<DataGridViewRow>().Select(r => (HexDumpRecord)r.DataBoundItem);

            foreach (var record in visibleRecords)
            {
               await record.RefreshAsync(process);
            }

            hexDataGridView.Invalidate();
        }
        public IList<Smdkd.SmHexDumpData> HexDump(IntPtr start_address, byte[] buffer)
        {
            var list = new List<Smdkd.SmHexDumpData>();
            var data = new Smdkd.EnumerateDumpData();

            data.HexBytes = new int[16];
            data.TextBytes = new char[16];
            int len = buffer.Length;
            //if (start_address.Equals(IntPtr.Zero)) return list;

            long address = start_address.ToInt64();

            for (int j = 0; j < len; j += 16)
            {
                data = new Smdkd.EnumerateDumpData();

                data.HexBytes = new int[16];
                data.TextBytes = new char[16];
                int index = 0;
                data.Address = (IntPtr)address;
                for (int i = j; i < j + 16; i++)
                {
                    if (i < len) data.HexBytes[index++] = buffer[i];
                    else data.HexBytes[index++] = 0;
                }

                index = 0;
                for (int i = j; i < j + 16; i++)
                {
                    if ((char)buffer[i] < 32) data.TextBytes[index++] = '.';
                    else if ((char)buffer[i] > 126) data.TextBytes[index++] = ' ';
                    else data.TextBytes[index++] = (char)buffer[i];
                }

                address += 16;
                list.Add(new Smdkd.SmHexDumpData(data.Address, data.HexBytes, data.TextBytes));
            }

            return list;
        }
        #endregion

        #region Events

        private void hexDataGrid_Scroll(object sender, ScrollEventArgs e)
        {
            var visibleRecords = hexDataGridView.GetVisibleRows().Select(r => (HexDumpRecord)r.DataBoundItem);
            int visibleCount = visibleRecords.Count();

            var rfRecord = bindings.First().Clone();
            var rlRecord = bindings.Last().Clone();

            var fVRecord = visibleRecords.First().Clone();
            var lVRecord = visibleRecords.Last().Clone();


            if (fVRecord.Address.Equals(rfRecord.Address))
            {
                var offset = int.Parse(READ_SIZE.ToString("X"), System.Globalization.NumberStyles.HexNumber);
                var address = lVRecord.Address.Sub(offset);
                address = address.Add(0x100);
                ReadAndLoadHexDump(address, READ_SIZE);
                hexDataGridView.ScrollToDumpAddress(lVRecord.Address);
            }
            else if (lVRecord.Address.Equals(rlRecord.Address))
            {
                ReadAndLoadHexDump(fVRecord.Address.Sub(0x100), READ_SIZE);
                hexDataGridView.ScrollToDumpAddress(fVRecord.Address);
            }
        }
        private void hexDataGridCell_Click(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == 0 || e.ColumnIndex == 17)
            {
                if (lastCell != null)
                    lastCell.Style.BackColor = Color.White;
                return;
            }
            if (lastCell != null)
                lastCell.Style.BackColor = Color.White;

            if (e.ColumnIndex < 18)
                lastCell = hexDataGridView[e.ColumnIndex + 18 - 1, e.RowIndex];
            else
                lastCell = hexDataGridView[e.ColumnIndex - 18 + 1, e.RowIndex];


            lastCell.Style.BackColor = Color.Yellow;
        }
        private void hexDataGridCell_Painting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            /*if (e.RowIndex < 0) return;
            if (e.ColumnIndex < 18) return;

            float fontSize = e.CellStyle.Font.Size;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            DataGridViewCell cell = hexDataGridView[e.ColumnIndex, e.RowIndex];
            SolidBrush seaGreen = new SolidBrush(e.CellStyle.ForeColor);
            if (cell.Selected)
                seaGreen = new SolidBrush(e.CellStyle.SelectionForeColor);

            string text = cell.Value.ToString();
            //Font font = FindFont(e.Graphics, text, e.CellBounds.Size, e.CellStyle.Font);
            if (text.Equals(".")) fontSize = 19f;
            if (text.Equals(",")) fontSize = 17f;
            if (text.Equals(";")) fontSize = 16f;
            if (text.Equals(":")) fontSize = 16f;

            Font font = new Font(e.CellStyle.Font.FontFamily, fontSize, FontStyle.Regular);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

            RectangleF textRec;
            textRec = new RectangleF(e.CellBounds.Location, new Size(0, 0));
            textRec = new RectangleF(new PointF(textRec.Location.X, textRec.Location.Y), new SizeF(textSize.Width, e.CellBounds.Height));

            e.Graphics.DrawString(text, font, seaGreen, textRec, sf);

            e.Handled = true;*/
        }

        private void hexDataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = hexDataGridView.HitTest(e.X, e.Y);
                if (hti.RowIndex < 0) return;
                var cell = hexDataGridView[hti.ColumnIndex,hti.RowIndex];
                if (!cell.Selected && !(ModifierKeys == Keys.Shift || ModifierKeys == Keys.Control))
                    hexDataGridView.ClearSelection();
                cell.Selected = true;
            }
        }
        private void hexDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            OnRecordDoubleClick((HexDumpRecord)hexDataGridView.Rows[e.RowIndex].DataBoundItem);
        }
        private void hexDataGridView_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            e.ContextMenuStrip = ContextMenuStrip;
        }

        #endregion
    }
    public class HexDumpRecord
    {
        public string address { get => Address.ToString(Program.AddressHexFormat); }
        public bool IsValid { get; set; } = false;
        public IntPtr Address { get; set; }

        public int[] Bytes { get; set; }
        public char[] Chars { get; set; }

        private string GetChar(int index) => IsValid ? Chars[index].ToString() : "?";
        private string GetByte(int index) => IsValid ? Bytes[index].ToString("X02") : "??";

        public string hex_00 { get => GetByte(0); }
        public string hex_01 { get => GetByte(1); }
        public string hex_02 { get => GetByte(2); }
        public string hex_03 { get => GetByte(3); }
        public string hex_04 { get => GetByte(4); }
        public string hex_05 { get => GetByte(5); }
        public string hex_06 { get => GetByte(6); }
        public string hex_07 { get => GetByte(7); }
        public string hex_08 { get => GetByte(8); }
        public string hex_09 { get => GetByte(9); }
        public string hex_0A { get => GetByte(10); }
        public string hex_0B { get => GetByte(11); }
        public string hex_0C { get => GetByte(12); }
        public string hex_0D { get => GetByte(13); }
        public string hex_0E { get => GetByte(14); }
        public string hex_0F { get => GetByte(15); }

        public string char_00 { get => GetChar(0); }
        public string char_01 { get => GetChar(1); }
        public string char_02 { get => GetChar(2); }
        public string char_03 { get => GetChar(3); }
        public string char_04 { get => GetChar(4); }
        public string char_05 { get => GetChar(5); }
        public string char_06 { get => GetChar(6); }
        public string char_07 { get => GetChar(7); }
        public string char_08 { get => GetChar(8); }
        public string char_09 { get => GetChar(9); }
        public string char_0A { get => GetChar(10); }
        public string char_0B { get => GetChar(11); }
        public string char_0C { get => GetChar(12); }
        public string char_0D { get => GetChar(13); }
        public string char_0E { get => GetChar(14); }
        public string char_0F { get => GetChar(15); }

        public HexDumpRecord()
        {

        }
        public HexDumpRecord(IntPtr address)
        {
            Address = address;
            Bytes = new int[16];
            Chars = new char[16] { '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?' };
        }
        public HexDumpRecord(ref HexDumpRecord data)
        {
            Address = data.Address;
            Bytes = data.Bytes;
            Chars = data.Chars;
        }
        public HexDumpRecord(ref Smdkd.SmHexDumpData data)
        {
            Address = data.Address;
            Bytes = data.HexBytes;
            Chars = data.TextBytes;
        }

        public bool Refresh(IProcessReader reader)
        {
            return RefreshAsync(reader).Result;
        }
        public Task<bool> RefreshAsync(IProcessReader reader)
        {
            return Task.Run(() =>
            {
                byte[] data = new byte[Bytes.Length];
                bool status = true;
                if (reader.ReadRemoteMemoryIntoBuffer(Address, ref data))
                {
                    var hexDump = HexDump(Address, data);

                    Address = hexDump.Address;
                    Array.Copy(hexDump.HexBytes, Bytes, Bytes.Length);
                    Array.Copy(hexDump.TextBytes, Chars, Chars.Length);
                    IsValid = true;
                }
                else
                {
                    status = false;
                    IsValid = false;
                    Bytes = new int[16];
                    Chars = new char[16] { '?', '?', '?', '?','?', '?', '?', '?','?', '?', '?', '?','?', '?', '?', '?' };
                }
                return status;
            });
        }
        public HexDumpRecord Clone() => new HexDumpRecord()
        {
            Address = Address,
            Bytes = Bytes.Clone() as int[],
            Chars = Chars.Clone() as char[],
        };


        public Smdkd.SmHexDumpData HexDump(IntPtr start_address, byte[] buffer)
        {
            var data = new Smdkd.EnumerateDumpData();

            data.HexBytes = new int[16];
            data.TextBytes = new char[16];
            int len = buffer.Length;
            //if (start_address.Equals(IntPtr.Zero)) return list;

            long address = start_address.ToInt64();

            data = new Smdkd.EnumerateDumpData();

            data.HexBytes = new int[16];
            data.TextBytes = new char[16];
            int index = 0;
            data.Address = (IntPtr)address;
            for (int i = 0; i < 16; i++)
            {
                if (i < len) data.HexBytes[index++] = buffer[i];
                else data.HexBytes[index++] = 0;
            }

            index = 0;
            for (int i = 0; i < 16; i++)
            {
                if ((char)buffer[i] < 32) data.TextBytes[index++] = '.';
                else if ((char)buffer[i] > 126) data.TextBytes[index++] = ' ';
                else data.TextBytes[index++] = (char)buffer[i];
            }

            return new Smdkd.SmHexDumpData(data.Address, data.HexBytes, data.TextBytes);
        }
    }

    public class DistinctHexDumpRecordComparer : IEqualityComparer<HexDumpRecord>
    {
        public bool Equals(HexDumpRecord x, HexDumpRecord y)
        {
            return x.Address.Equals(y.Address);
        }

        public int GetHashCode(HexDumpRecord obj)
        {
            return obj.Address.GetHashCode();
        }
    }
}
