using SmScanner.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SmScanner.Forms
{
    public partial class StructDetailsForm : Form
    {
        public Struct Struct { get; set; }
        public StructDetailsForm()
        {
            InitializeComponent();
        }
        public StructDetailsForm(string struct_name)
        {
            InitializeComponent();

            nameTextBox.Text = struct_name;
        }

        public StructDetailsForm(ref Struct @struct)
        {
            Struct = @struct;

            InitializeComponent();

            offsetStartFromTextBox.Enabled = false;
            offsetJumpTextBox.Enabled = false;
            sizeTextBox.Enabled = false;
            addressTextBox.Enabled = false;

            offsetStartFromTextBox.Text = Struct.OffsetStart.ToString();
            offsetJumpTextBox.Text = Struct.OffsetJump.ToString();
            sizeTextBox.Text = Struct.Size.ToString();
            addressTextBox.Text = Struct.Address.ToInt64().ToString(Program.AddressHexFormat);
            nameTextBox.Text = Struct.Name;

            encodingUTF7RadioButton.Checked = false;
            encodingUTF8RadioButton.Checked = false;
            encodingUTF32RadioButton.Checked = false;
            encodingASCIIRadioButton.Checked = false;
            encodingUnicodeRadioButton.Checked = false;
            encodingBigEndianUnicodeRadioButton.Checked = false;

            if (Struct.Encoding == Encoding.UTF7)
                encodingUTF7RadioButton.Checked = true;
            else if (Struct.Encoding == Encoding.UTF8)
                encodingUTF8RadioButton.Checked = true;
            else if (Struct.Encoding == Encoding.UTF32)
                encodingUTF32RadioButton.Checked = true;
            else if (Struct.Encoding == Encoding.ASCII)
                encodingASCIIRadioButton.Checked = true;
            else if (Struct.Encoding == Encoding.Unicode)
                encodingUnicodeRadioButton.Checked = true;
            else if (Struct.Encoding == Encoding.BigEndianUnicode)
                encodingBigEndianUnicodeRadioButton.Checked = true;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            StartPosition = FormStartPosition.CenterParent;
            CenterToParent();
        }

        private void SetStruct(string name, int size, int offsetJump, int offsetStart, IntPtr address, Encoding encoding)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(size > 0);
            Contract.Requires(offsetJump > 0);
            Contract.Requires(address != IntPtr.Zero);

            Struct = new Struct(name, size, offsetJump, offsetStart, address, encoding);
        }
        private void UpdateStruct(string name, int size, int offsetJump, int offsetStart, IntPtr address, Encoding encoding)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(size > 0);
            Contract.Requires(offsetJump > 0);
            Contract.Requires(offsetStart >= 0);
            Contract.Requires(encoding != null);
            Contract.Requires(address != IntPtr.Zero);

            Struct.Size = size;
            Struct.Name = name;
            Struct.Address = address;
            Struct.Encoding = encoding;
            Struct.OffsetJump = offsetJump;
            Struct.OffsetStart = offsetStart;
        }
        private void okButton_Click(object sender, EventArgs e)
        {
            if(
                !IsValid(offsetStartFromTextBox.Text) ||
                !IsValid(offsetJumpTextBox.Text) ||
                !IsValid(sizeTextBox.Text) ||
                !IsValid(addressTextBox.Text) ||
                !IsValid(nameTextBox.Text)
                )
            {

                Program.ShowException(new Exception("the fields not in the currect format or empty"));
                return;
            }

            try
            {
                Encoding encoding;
                if (encodingUTF7RadioButton.Checked) encoding = Encoding.UTF7;
                else if (encodingUTF8RadioButton.Checked) encoding = Encoding.UTF8;
                else if (encodingUTF32RadioButton.Checked) encoding = Encoding.UTF32;
                else if (encodingASCIIRadioButton.Checked) encoding = Encoding.ASCII;
                else if (encodingUnicodeRadioButton.Checked) encoding = Encoding.Unicode;
                else encoding = Encoding.BigEndianUnicode;

                if (Struct == null)
                {
                    SetStruct(
                        nameTextBox.Text,
                        int.Parse(sizeTextBox.Text),
                        int.Parse(offsetJumpTextBox.Text),
                        int.Parse(offsetStartFromTextBox.Text),
                        (IntPtr)long.Parse(addressTextBox.Text, System.Globalization.NumberStyles.HexNumber),
                        encoding
                        );
                }
                else
                {
                    UpdateStruct(
                        nameTextBox.Text,
                        int.Parse(sizeTextBox.Text),
                        int.Parse(offsetJumpTextBox.Text),
                        int.Parse(offsetStartFromTextBox.Text),
                        (IntPtr)long.Parse(addressTextBox.Text, System.Globalization.NumberStyles.HexNumber),
                        encoding
                       );
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Program.ShowException(ex);
            }
        }

        private bool IsValid(string text) => !string.IsNullOrEmpty(text);
    }
}
