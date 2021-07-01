using SmScanner.Controls;
using SmScanner.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Forms;

namespace SmScanner.Forms
{
    public partial class StructsViewForm : Form
    {
        private Timer refreshRecords_timer;
        private List<StructControl> structs;
        private readonly IRemoteProcess process;

        public StructsViewForm()
        {
            InitializeComponent();

            process = Program.RemoteProcess;

            structOptionsToolStripMenuItem.Enabled = false;
            SetValueButtons(false);
            contextMenuStrip.Opening += new CancelEventHandler(contextMenuStrip_Opening);
            structs = new List<StructControl>();
            refreshRecords_timer = new Timer();
            refreshRecords_timer.Interval = 300;
            refreshRecords_timer.Tick += new EventHandler(refreshRecords_ticks);
            refreshRecords_timer.Start();
        }

        #region Helpers

        public void SetValueButtons(bool enabled)
        {
            Int8.Enabled = enabled;
            UInt8.Enabled = enabled;
            Int16.Enabled = enabled;
            UInt16.Enabled = enabled;
            Int32.Enabled = enabled;
            UInt32.Enabled = enabled;
            Int64.Enabled = enabled;
            UInt64.Enabled = enabled;
            Bool.Enabled = enabled;
            Float.Enabled = enabled;
            Double.Enabled = enabled;
            String.Enabled = enabled;
            Pointer.Enabled = enabled;
            BytesArray.Enabled = enabled;
        }
        private SmScanner.Controls.ValueType GetValueTypeByName(string typeName)
        {
            SmScanner.Controls.ValueType newValueType = SmScanner.Controls.ValueType.Int8;

            if (SmScanner.Controls.ValueType.Int8.ToString().Equals(typeName)) newValueType = SmScanner.Controls.ValueType.Int8;
            if (SmScanner.Controls.ValueType.UInt8.ToString().Equals(typeName)) newValueType = SmScanner.Controls.ValueType.UInt8;
            if (SmScanner.Controls.ValueType.Bool.ToString().Equals(typeName)) newValueType = SmScanner.Controls.ValueType.Bool;
            else if (SmScanner.Controls.ValueType.Int16.ToString().Equals(typeName)) newValueType = SmScanner.Controls.ValueType.Int16;
            else if (SmScanner.Controls.ValueType.Int32.ToString().Equals(typeName)) newValueType = SmScanner.Controls.ValueType.Int32;
            else if (SmScanner.Controls.ValueType.Int64.ToString().Equals(typeName)) newValueType = SmScanner.Controls.ValueType.Int64;
            else if (SmScanner.Controls.ValueType.UInt16.ToString().Equals(typeName)) newValueType = SmScanner.Controls.ValueType.UInt16;
            else if (SmScanner.Controls.ValueType.UInt32.ToString().Equals(typeName)) newValueType = SmScanner.Controls.ValueType.UInt32;
            else if (SmScanner.Controls.ValueType.UInt64.ToString().Equals(typeName)) newValueType = SmScanner.Controls.ValueType.UInt64;
            else if (SmScanner.Controls.ValueType.Float.ToString().Equals(typeName)) newValueType = SmScanner.Controls.ValueType.Float;
            else if (SmScanner.Controls.ValueType.Double.ToString().Equals(typeName)) newValueType = SmScanner.Controls.ValueType.Double;
            else if (SmScanner.Controls.ValueType.String.ToString().Equals(typeName)) newValueType = SmScanner.Controls.ValueType.String;
            else if (SmScanner.Controls.ValueType.Pointer.ToString().Equals(typeName)) newValueType = SmScanner.Controls.ValueType.Pointer;
            else if (SmScanner.Controls.ValueType.BytesArray.ToString().Equals(typeName)) newValueType = SmScanner.Controls.ValueType.BytesArray;

            return newValueType;
        }
        private void SetSelectedRecordsValueType(SmScanner.Controls.ValueType valueType)
        {
            var tab = tabControl.SelectedTab;
            var structControl = tab.Controls[tab.Controls.Count - 1] as StructControl;

            foreach (var record in structControl.SelectedRecords) record.SetType(valueType);

            var selected = structControl.SelectedRecords.Last();
            textSizeTextBox.Text = selected.Size.ToString();

            if (selected.ValueType == SmScanner.Controls.ValueType.String || selected.ValueType == SmScanner.Controls.ValueType.BytesArray)
                textSizeTextBox.Enabled = true;
            else textSizeTextBox.Enabled = false;

            textSizeTextBox.DataBindings.Clear();
            SetBinding(textSizeTextBox, nameof(TextBox.Text), selected, nameof(selected.Size));

            int index = structControl.Records.IndexOf(structControl.SelectedRecords.Last());
            structControl.FixOffsets(index + 1);
        }
        private static void SetBinding(IBindableComponent control, string propertyName, object dataSource, string dataMember)
        {
            Contract.Requires(control != null);
            Contract.Requires(propertyName != null);
            Contract.Requires(dataSource != null);
            Contract.Requires(dataMember != null);

            control.DataBindings.Add(propertyName, dataSource, dataMember, true, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion
        #region Events

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            refreshRecords_timer?.Stop();
            refreshRecords_timer?.Dispose();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }


        private void refreshRecords_ticks(object sender, EventArgs e)
        {
            foreach (var @struct in structs)
            {
                @struct.RefreshVisibleRecords();
            }
        }
        private void structPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            var @struct = sender as Struct;

            if (e.PropertyName.Equals("Name"))
            {
                tabControl.SelectedTab.Text = @struct.Name;
            }
        }


        private void btnSetValueType_Click(object sender, EventArgs e)
        {
            var btn = sender as IconButton;
            string typeName = btn.Name;

            SmScanner.Controls.ValueType newValueType = GetValueTypeByName(typeName);

            SetSelectedRecordsValueType(newValueType);
        }


        private void newStructToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int index = 1;
                string sName = "Unknown";
                while (structs.Where(s => s.Details.Name.Equals(sName)).Count() > 0) sName = $"Unknown{index++}";
                var sf = new StructDetailsForm(sName);
                if (sf.ShowDialog(this) == DialogResult.OK)
                {
                    var @struct = sf.Struct;
                    StructControl structControl = new StructControl(process, ref @struct);
                    structControl.ContextMenuStrip = contextMenuStrip;
                    structControl.Dock = DockStyle.Fill;
                    structControl.Details.PropertyChanged += new PropertyChangedEventHandler(structPropertyChange);
                    structControl.SelectionChanged = new EventHandler((s, e) =>
                    {
                        if (structControl.SelectedRecords.Count <= 0) return;
                        var selected = structControl.SelectedRecords.Last();
                        if(selected.ValueType != SmScanner.Controls.ValueType.String && selected.ValueType != SmScanner.Controls.ValueType.BytesArray) 
                            textSizeTextBox.Enabled = false;
                        else textSizeTextBox.Enabled = true;
                        textSizeTextBox.DataBindings.Clear();
                        SetBinding(textSizeTextBox, nameof(TextBox.Text), selected, nameof(selected.Size));
                    });

                    structs.Add(structControl);
                    tabControl.TabPages.Add(@struct.Name);
                    tabControl.TabPages.Cast<TabPage>().Last().Controls.Add(structControl);
                    tabControl.SelectedIndex = tabControl.TabCount - 1;
                }

                if (!structOptionsToolStripMenuItem.Enabled)
                {
                    structOptionsToolStripMenuItem.Enabled = true;
                    SetValueButtons(true);
                }
            }
            catch (Exception ex)
            {
                Program.ShowException(ex);
            }
        }
        private void structSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tab = tabControl.SelectedTab;
            var structControl = tab.Controls[tab.Controls.Count - 1] as StructControl;

            var details = structControl.Details;
            var sf = new StructDetailsForm(ref details);
            if (sf.ShowDialog(this) == DialogResult.OK)
            {

            }
        }
        private void structDeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tab = tabControl.SelectedTab;
            var structControl = tab.Controls[tab.Controls.Count - 1] as StructControl;

            tabControl.TabPages.Remove(tab);
            structs.Remove(structs.Find(s=>s.Details.Name.Equals(tab.Text)));

            structOptionsToolStripMenuItem.Enabled = structs.Count > 0;
            SetValueButtons(structOptionsToolStripMenuItem.Enabled);
        }


        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            var tab = tabControl.SelectedTab;
            var structControl = tab.Controls[tab.Controls.Count - 1] as StructControl;
            var selected = structControl.SelectedRecord;

            byteToolStripMenuItem.Text = $"Byte: {selected.ToString(selected.GetByte(process))}";
            bytes2ToolStripMenuItem.Text = $"2 Bytes: {selected.ToString(selected.Get2Bytes(process))}";
            bytes4ToolStripMenuItem.Text = $"4 Bytes: {selected.ToString(selected.Get4Bytes(process))}";
            bytes8ToolStripMenuItem.Text = $"8 Bytes: {selected.ToString(selected.Get8Bytes(process))}";

            hexByteToolStripMenuItem.Text = $"(Hex) Byte: {selected.ToString(selected.GetByte(process), true)}";
            hexBytes2ToolStripMenuItem.Text = $"(Hex) 2 Bytes: {selected.ToString(selected.Get2Bytes(process), true)}";
            hexBytes4ToolStripMenuItem.Text = $"(Hex) 4 Bytes: {selected.ToString(selected.Get4Bytes(process), true)}";
            hexBytes8ToolStripMenuItem.Text = $"(Hex) 8 Bytes: {selected.ToString(selected.Get8Bytes(process), true)}";

            boolToolStripMenuItem.Text = $"Bool: {selected.ToString(selected.GetBool(process))}";
            floatToolStripMenuItem.Text = $"Float: {selected.ToString(selected.GetFloat(process))}";
            doubleToolStripMenuItem.Text = $"Double: {selected.ToString(selected.GetDouble(process))}";

            pointerToolStripMenuItem.Text = $"Pointer: {selected.ToString(selected.GetPointer(process))}";
            arrayToolStripMenuItem.Text = $"Array: {selected.ToString(selected.GetBytes(process))}";
            stringToolStripMenuItem.Text = $"String: {selected.GetString(process)}";
        }

        private void arrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedRecordsValueType(SmScanner.Controls.ValueType.BytesArray);
        }
        private void stringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedRecordsValueType(SmScanner.Controls.ValueType.String);
        }
        private void pointerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedRecordsValueType(SmScanner.Controls.ValueType.Pointer);
        }
        private void doubleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedRecordsValueType(SmScanner.Controls.ValueType.Double);
        }
        private void floatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedRecordsValueType(SmScanner.Controls.ValueType.Float);
        }
        private void boolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedRecordsValueType(SmScanner.Controls.ValueType.Bool);
        }
        private void hexBytes8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedRecordsValueType(SmScanner.Controls.ValueType.UInt64);
        }
        private void hexBytes4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedRecordsValueType(SmScanner.Controls.ValueType.UInt32);
        }
        private void hexBytes2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedRecordsValueType(SmScanner.Controls.ValueType.UInt16);
        }
        private void hexByteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedRecordsValueType(SmScanner.Controls.ValueType.UInt8);
        }
        private void bytes8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedRecordsValueType(SmScanner.Controls.ValueType.Int64);
        }
        private void bytes4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedRecordsValueType(SmScanner.Controls.ValueType.Int32);
        }
        private void bytes2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedRecordsValueType(SmScanner.Controls.ValueType.Int16);
        }
        private void byteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedRecordsValueType(SmScanner.Controls.ValueType.Int8);
        }

        #endregion
    }
}
