using System.Diagnostics.Contracts;
using System.Windows.Forms;

namespace SmScanner.Forms
{
    public partial class ChangeValueForm : Form
    {
        public string LabelText { get => label1.Text; set => label1.Text = value; }
        public string ValueText { get => valueTextBox.Text; }
        public string HeaderText { get => Text; set => Text = value; }

        public ChangeValueForm(string valueStr, string labelText = null, string headerText = null)
        {
            Contract.Requires(valueStr != null);

            InitializeComponent();

            if (labelText != null)
                label1.Text = labelText;

            if (headerText != null)
                Text = headerText;

            valueTextBox.Text = valueStr;
            valueTextBox.SelectAll();

            valueTextBox.KeyUp += new KeyEventHandler((s,e)=> {
                if (e.KeyCode == Keys.Enter)
                    button1.PerformClick();
            });
        }

    }
}
