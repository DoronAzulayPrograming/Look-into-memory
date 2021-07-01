using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows.Forms;

namespace SmScanner.Forms
{
    public partial class MemoryViewSettingsForm : Form
    {
        public MemoryViewSettingsForm()
        {
            InitializeComponent();
            SetGeneralBindings();
        }

        private void SetGeneralBindings()
        {
            SetBinding(hexBufferTextBox, nameof(TextBox.Text), Program.Settings, nameof(Settings.HexBufferSize));
            SetBinding(disassemblerBuffertextBox, nameof(TextBox.Text), Program.Settings, nameof(Settings.DissassemblerBufferSize));
        }
        private static void SetBinding(IBindableComponent control, string propertyName, object dataSource, string dataMember)
        {
            Contract.Requires(control != null);
            Contract.Requires(propertyName != null);
            Contract.Requires(dataSource != null);
            Contract.Requires(dataMember != null);

            control.DataBindings.Add(propertyName, dataSource, dataMember, true, DataSourceUpdateMode.OnPropertyChanged);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CenterToParent();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
        private void cencelButton_Click(object sender, System.EventArgs e)
        {
            Hide();
        }
    }
}
