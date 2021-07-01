using SmScanner.Core.Enums;
using SmScanner.Core.Extensions;
using SmScanner.Core.Fotmattors;
using SmScanner.Core.Interfaces;
using SmScanner.Core.Modules;
using SmScanner.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SmScanner.Controls
{
    public partial class StructControl : UserControl
    {
        private EventHandler selectionChanged { get; set; }
        public EventHandler SelectionChanged { get => selectionChanged; set => selectionChanged = value; }
        public Struct Details { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<StructRecord> Records => bindings;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StructRecord SelectedRecord => GetSelectedRecords().FirstOrDefault();

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<StructRecord> SelectedRecords => GetSelectedRecords().ToList();

        public override ContextMenuStrip ContextMenuStrip
        {
            get;
            set;
        }

        public event MemorySearchResultControlResultDoubleClickEventHandler RecordDoubleClick;

        private IEnumerable<StructRecord> GetSelectedRecords() => recordsDataGridView.SelectedRows.Cast<DataGridViewRow>().Select(r => (StructRecord)r.DataBoundItem);

        private readonly IRemoteProcess process;
        private readonly BindingList<StructRecord> bindings;

        public StructControl(IRemoteProcess process, ref Struct @struct)
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            if (!SystemInformation.TerminalServerSession)
            {
                Program.SetControlDoubleBuffered(recordsDataGridView);
            }

            if (Program.DesignMode)
            {
                return;
            }

            Details = @struct;

            this.process = process;

            recordsDataGridView.SelectionChanged += new EventHandler(recordsDataGridView_SelectionChanged);
            recordsDataGridView.AutoGenerateColumns = false;

            bindings = new BindingList<StructRecord>
            {
                AllowNew = true,
                AllowEdit = true,
                RaiseListChangedEvents = true
            };
            recordsDataGridView.DataSource = bindings;

            Details.PropertyChanged += new PropertyChangedEventHandler(structPropertyChange);
        }

        #region Helpers

        public void RefreshVisibleRecords()
        {
            var records = recordsDataGridView.GetVisibleRows().Cast<DataGridViewRow>().Select(r => (StructRecord)r.DataBoundItem);

            foreach (var record in records)
            {
                record.RefreshValue(process);
            }
            recordsDataGridView.Invalidate();
        }
        public void FixOffsets(int start_index = 1)
        {
            lock (Records)
            {
                int len = Records.Count;
                Records[start_index - 1].RefreshValue(process);
                for (int i = start_index; i < len; i++)
                {
                    var current = Records[i];
                    var prev = Records[i - 1];

                    current.Offset = (IntPtr)(prev.Offset.ToInt64() + prev.Size);
                    current.RefreshValue(process);
                }
            }
        }
        public void SetRecords(IEnumerable<StructRecord> records)
        {
            bindings.Clear();

            bindings.RaiseListChangedEvents = false;

            foreach (var record in records) bindings.Add(record);

            bindings.RaiseListChangedEvents = true;
            bindings.ResetBindings();
        }

        #endregion

        #region Events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var encoding = Details.Encoding;
            var records = new List<StructRecord>();
            int len = Details.Size / Details.OffsetJump;
            for (int i = 0; i < len; i += Details.OffsetJump)
            {
                var record = new StructRecord(Details.Address, (IntPtr)i, ValueType.FloatInt32UInt32, encoding);
                record.PropertyChanged += new PropertyChangedEventHandler((s, e) =>
                {
                    if (e.PropertyName.Equals("Size"))
                    {
                        if (record.ValueType != ValueType.String && record.ValueType != ValueType.BytesArray) return;
                        record.RefreshValue(process);
                        FixOffsets();
                    }
                });
                record.RefreshValue(process);
                records.Add(record);
            }
            SetRecords(records);
        }
        private void structPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            var @struct = sender as Struct;

            if (e.PropertyName.Equals("Encoding"))
            {
                foreach (var record in Records)
                {
                    record.Encoding = @struct.Encoding;
                }
            }
        }
        private void recordsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            selectionChanged?.Invoke(sender, e);
        }

        private void recordsDataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = recordsDataGridView.HitTest(e.X, e.Y);
                if (hti.RowIndex < 0) return;
                var row = recordsDataGridView.Rows[hti.RowIndex];
                if (!row.Selected && !(ModifierKeys == Keys.Shift || ModifierKeys == Keys.Control))
                    recordsDataGridView.ClearSelection();
                row.Selected = true;
            }
        }
        private void recordsDataGridView_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            e.ContextMenuStrip = ContextMenuStrip;
        }

        #endregion
    }


    public class Struct
    {
        private string name { get; set; }
        public string Name
        {
            get => name;
            set {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }
        public int Size { get; set; }
        public IntPtr Address { get; set; }
        public int OffsetJump { get; set; }
        public int OffsetStart { get; set; }
        private Encoding encoding { get; set; }
        public Encoding Encoding
        {
            get => encoding;
            set
            {
                encoding = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Encoding)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Struct(string name, int size, int offsetJump, int offsetStart, IntPtr addreess, Encoding encoding)
        {
            Name = name;
            Size = size;
            Address = addreess;
            Encoding = encoding;
            OffsetJump = offsetJump;
            OffsetStart = offsetStart;
        }

    }
    public class StructRecord
    {
        private int size { get; set; }
        public int Size { 
            get => size;
            set
            {
                size = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Size)));
            } 
        }
        public IntPtr Offset { get; set; }
        public IntPtr Address { get; set; }
        public string OffsetStr { get => Offset.ToString("X4"); }
        public string AddressStr { get => Address.Add(Offset).ToString(Program.AddressHexFormat); }
        public string ValueStr { get; private set; }

        public ValueType ValueType { get; set; }
        public string TypeStr { get => ValueType.ToString(); }
        public bool HasChangedValue { get; private set; }
        public int ValueLength { get; set; }
        public Encoding Encoding { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public StructRecord(IntPtr address, IntPtr offset, ValueType valueType, Encoding encoding)
        {
            Encoding = encoding;

            Offset = offset;
            Address = address;
            ValueType = valueType;
            SetType(valueType);
        }

        public void RefreshValue(IProcessReader process)
        {
            Contract.Requires(process != null);
            bool isWow64 = Program.RemoteProcess.IsWow64;
            var bytes = process.ReadRemoteMemory(Address.Add(Offset), Size);
            switch (ValueType)
            {
                case ValueType.Int8:
                    ValueStr = ToString(GetByte(process));
                    break;
                case ValueType.UInt8:
                    ValueStr = ToString(GetByte(process),true);
                    break;
                case ValueType.Bool:
                    ValueStr = ToString(GetBool(process));
                    break;
                case ValueType.Int16:
                    ValueStr = ToString(Get2Bytes(process));
                    break;
                case ValueType.Int32:
                    ValueStr = ToString(Get4Bytes(process));
                    break;
                case ValueType.Int64:
                    ValueStr = ToString(Get8Bytes(process));
                    break;
                case ValueType.UInt16:
                    ValueStr = ToString(Get2Bytes(process),true);
                    break;
                case ValueType.UInt32:
                    ValueStr = ToString(Get4Bytes(process), true);
                    break;
                case ValueType.UInt64:
                    ValueStr = ToString(Get8Bytes(process), true);
                    break;
                case ValueType.Float:
                    ValueStr = ToString(GetFloat(process));
                    break;
                case ValueType.Double:
                    ValueStr = ToString(GetDouble(process));
                    break;
                case ValueType.String:
                    ValueStr = GetString(process,Size);
                    break;
                case ValueType.BytesArray:
                    ValueStr = ToString(GetBytes(process, Size));
                    break;
                case ValueType.Pointer:
                    ValueStr = ToString(GetPointer(process));
                    break;
                case ValueType.FloatInt32UInt32:
                    var raw32 = new UInt32FloatData { Raw = BitConverter.ToInt32(bytes, 0) };
                    ValueStr = $"{(raw32.FloatValue > -999999.0f && raw32.FloatValue < 999999.0f ? raw32.FloatValue.ToString("0.000") : "#####")} , {raw32.IntValue} , 0x{raw32.UIntPtr.ToUInt64():X}";
                    break;
                case ValueType.FloatInt64UInt64:
                    var raw64 = new UInt64FloatData { Raw = BitConverter.ToInt64(bytes, 0) };
                    ValueStr = $"{(raw64.FloatValue > -999999.0f && raw64.FloatValue < 999999.0f ? raw64.FloatValue.ToString("0.000") : "#####")} , {raw64.IntValue} , 0x{raw64.UIntPtr.ToUInt64():X}";
                    break;
            }
        }

        public void SetType(ValueType type)
        {
            switch (type)
            {
                case ValueType.Int8: Size = 1; break;
                case ValueType.UInt8: Size = 1; break;
                case ValueType.Bool: Size = 1; break;
                case ValueType.Int16: Size = 2; break;
                case ValueType.Int32: Size = 4; break;
                case ValueType.Int64: Size = 8; break;
                case ValueType.UInt16: Size = 2; break;
                case ValueType.UInt32: Size = 4; break;
                case ValueType.UInt64: Size = 8; break;
                case ValueType.Float: Size = 4; break;
                case ValueType.Double: Size = 8; break;
                case ValueType.String: Size = 16; break;
                case ValueType.Pointer: Size = 8; break;
                case ValueType.BytesArray: Size = 16; break;
                case ValueType.FloatInt32UInt32: Size = 4; break;
                case ValueType.FloatInt64UInt64: Size = 8; break;
                default: Size = 1; break;
            }
            ValueType = type;
        }

        public sbyte GetByte(IProcessReader process) => (sbyte)process.ReadRemoteMemory(Address.Add(Offset), 1)[0];
        public short Get2Bytes(IProcessReader process) => BitConverter.ToInt16(process.ReadRemoteMemory(Address.Add(Offset), 2),0);
        public int Get4Bytes(IProcessReader process) => BitConverter.ToInt32(process.ReadRemoteMemory(Address.Add(Offset), 4),0);
        public long Get8Bytes(IProcessReader process) => BitConverter.ToInt64(process.ReadRemoteMemory(Address.Add(Offset), 8),0);
        public byte[] GetBytes(IProcessReader process, int len = 16) => process.ReadRemoteMemory(Address.Add(Offset), len);
        public bool GetBool(IProcessReader process) => BitConverter.ToBoolean(process.ReadRemoteMemory(Address.Add(Offset), 1), 0);
        public IntPtr GetPointer(IProcessReader process) => process.IsWow64 ? (IntPtr)BitConverter.ToInt32(process.ReadRemoteMemory(Address.Add(Offset), 4), 0) : (IntPtr)BitConverter.ToInt32(process.ReadRemoteMemory(Address.Add(Offset), 4), 0);
        public float GetFloat(IProcessReader process) => BitConverter.ToSingle(process.ReadRemoteMemory(Address.Add(Offset), 4), 0);
        public double GetDouble(IProcessReader process) => BitConverter.ToDouble(process.ReadRemoteMemory(Address.Add(Offset), 8), 0);
        public string GetString(IProcessReader process, int len = 16) => Encoding.GetString(process.ReadRemoteMemory(Address.Add(Offset), len));
        public string GetString(IProcessReader process, Encoding encoding, int len = 16) => encoding.GetString(process.ReadRemoteMemory(Address.Add(Offset), len));

        public string ToString(IntPtr value) => $"P->{value.ToInt64().ToString(Program.AddressHexFormat)}";
        public string ToString(bool value) => value.ToString();
        public string ToString(byte[] value) => HexadecimalFormatter.ToString(value);
        public string ToString(float value, string format = null) => string.IsNullOrEmpty(format) ? value.ToString() : value.ToString(format);
        public string ToString(double value, string format = null) => string.IsNullOrEmpty(format) ? value.ToString() : value.ToString(format);
        public string ToString(int value, bool showHex = false) => showHex ? value.ToString("X08") : value.ToString();
        public string ToString(long value, bool showHex = false) => showHex ? value.ToString("X010") : value.ToString();
        public string ToString(byte value, bool showHex = false) => showHex ? value.ToString("X02") : value.ToString();
        public string ToString(sbyte value, bool showHex = false) => showHex ? value.ToString("X02") : value.ToString();
        public string ToString(short value, bool showHex = false) => showHex ? value.ToString("X04") : value.ToString();
    }

    public enum ValueType
    {
        Bool,
        Int8,
        UInt8,
        Int16,
        Int32,
        Int64,
        UInt16,
        UInt32,
        UInt64,
        Float,
        Double,
        String,
        Pointer,
        BytesArray,
        FloatInt32UInt32,
        FloatInt64UInt64,
    }
    [StructLayout(LayoutKind.Explicit)]
    public struct UInt32FloatData
    {
        [FieldOffset(0)]
        public int Raw;

        [FieldOffset(0)]
        public int IntValue;

        public IntPtr IntPtr => (IntPtr)IntValue;

        [FieldOffset(0)]
        public uint UIntValue;

        public UIntPtr UIntPtr => (UIntPtr)UIntValue;

        [FieldOffset(0)]
        public float FloatValue;
    }
    [StructLayout(LayoutKind.Explicit)]
    public struct UInt64FloatData
    {
        [FieldOffset(0)]
        public long Raw;

        [FieldOffset(0)]
        public long IntValue;

        public IntPtr IntPtr => (IntPtr)IntValue;

        [FieldOffset(0)]
        public ulong UIntValue;

        public UIntPtr UIntPtr => (UIntPtr)UIntValue;

        [FieldOffset(0)]
        public float FloatValue;
    }
}
