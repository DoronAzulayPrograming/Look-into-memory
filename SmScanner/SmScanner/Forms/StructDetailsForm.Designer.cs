
namespace SmScanner.Forms
{
    partial class StructDetailsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sizeTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.offsetJumpTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.offsetStartFromTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.encodingUnicodeRadioButton = new System.Windows.Forms.RadioButton();
            this.encodingBigEndianUnicodeRadioButton = new System.Windows.Forms.RadioButton();
            this.encodingUTF32RadioButton = new System.Windows.Forms.RadioButton();
            this.encodingUTF8RadioButton = new System.Windows.Forms.RadioButton();
            this.encodingUTF7RadioButton = new System.Windows.Forms.RadioButton();
            this.encodingASCIIRadioButton = new System.Windows.Forms.RadioButton();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sizeTextBox
            // 
            this.sizeTextBox.Location = new System.Drawing.Point(97, 75);
            this.sizeTextBox.Name = "sizeTextBox";
            this.sizeTextBox.Size = new System.Drawing.Size(68, 23);
            this.sizeTextBox.TabIndex = 2;
            this.sizeTextBox.Text = "4096";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 15);
            this.label4.TabIndex = 1008;
            this.label4.Text = "Buffer Size:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(219, 113);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(148, 23);
            this.nameTextBox.TabIndex = 4;
            this.nameTextBox.Text = "Unknown";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(171, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 15);
            this.label3.TabIndex = 1007;
            this.label3.Text = "Name:";
            // 
            // addressTextBox
            // 
            this.addressTextBox.Location = new System.Drawing.Point(70, 113);
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(95, 23);
            this.addressTextBox.TabIndex = 3;
            this.addressTextBox.Text = "00400000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 1005;
            this.label2.Text = "Address:";
            // 
            // offsetJumpTextBox
            // 
            this.offsetJumpTextBox.Location = new System.Drawing.Point(97, 46);
            this.offsetJumpTextBox.Name = "offsetJumpTextBox";
            this.offsetJumpTextBox.Size = new System.Drawing.Size(31, 23);
            this.offsetJumpTextBox.TabIndex = 1;
            this.offsetJumpTextBox.Text = "4";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 15);
            this.label1.TabIndex = 1006;
            this.label1.Text = "Offset Jump:";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(212, 154);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // offsetStartFromTextBox
            // 
            this.offsetStartFromTextBox.Location = new System.Drawing.Point(97, 17);
            this.offsetStartFromTextBox.Name = "offsetStartFromTextBox";
            this.offsetStartFromTextBox.Size = new System.Drawing.Size(31, 23);
            this.offsetStartFromTextBox.TabIndex = 0;
            this.offsetStartFromTextBox.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 15);
            this.label5.TabIndex = 1010;
            this.label5.Text = "Offset start:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.encodingUnicodeRadioButton);
            this.groupBox1.Controls.Add(this.encodingBigEndianUnicodeRadioButton);
            this.groupBox1.Controls.Add(this.encodingUTF32RadioButton);
            this.groupBox1.Controls.Add(this.encodingUTF8RadioButton);
            this.groupBox1.Controls.Add(this.encodingUTF7RadioButton);
            this.groupBox1.Controls.Add(this.encodingASCIIRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(171, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(196, 95);
            this.groupBox1.TabIndex = 1011;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Encoding";
            // 
            // encodingUnicodeRadioButton
            // 
            this.encodingUnicodeRadioButton.AutoSize = true;
            this.encodingUnicodeRadioButton.Location = new System.Drawing.Point(70, 47);
            this.encodingUnicodeRadioButton.Name = "encodingUnicodeRadioButton";
            this.encodingUnicodeRadioButton.Size = new System.Drawing.Size(69, 19);
            this.encodingUnicodeRadioButton.TabIndex = 10;
            this.encodingUnicodeRadioButton.Text = "Unicode";
            this.encodingUnicodeRadioButton.UseVisualStyleBackColor = true;
            // 
            // encodingBigEndianUnicodeRadioButton
            // 
            this.encodingBigEndianUnicodeRadioButton.AutoSize = true;
            this.encodingBigEndianUnicodeRadioButton.Location = new System.Drawing.Point(70, 70);
            this.encodingBigEndianUnicodeRadioButton.Name = "encodingBigEndianUnicodeRadioButton";
            this.encodingBigEndianUnicodeRadioButton.Size = new System.Drawing.Size(122, 19);
            this.encodingBigEndianUnicodeRadioButton.TabIndex = 11;
            this.encodingBigEndianUnicodeRadioButton.Text = "BigEndianUnicode";
            this.encodingBigEndianUnicodeRadioButton.UseVisualStyleBackColor = true;
            // 
            // encodingUTF32RadioButton
            // 
            this.encodingUTF32RadioButton.AutoSize = true;
            this.encodingUTF32RadioButton.Location = new System.Drawing.Point(7, 70);
            this.encodingUTF32RadioButton.Name = "encodingUTF32RadioButton";
            this.encodingUTF32RadioButton.Size = new System.Drawing.Size(57, 19);
            this.encodingUTF32RadioButton.TabIndex = 8;
            this.encodingUTF32RadioButton.Text = "UTF32";
            this.encodingUTF32RadioButton.UseVisualStyleBackColor = true;
            // 
            // encodingUTF8RadioButton
            // 
            this.encodingUTF8RadioButton.AutoSize = true;
            this.encodingUTF8RadioButton.Location = new System.Drawing.Point(7, 47);
            this.encodingUTF8RadioButton.Name = "encodingUTF8RadioButton";
            this.encodingUTF8RadioButton.Size = new System.Drawing.Size(51, 19);
            this.encodingUTF8RadioButton.TabIndex = 7;
            this.encodingUTF8RadioButton.Text = "UTF8";
            this.encodingUTF8RadioButton.UseVisualStyleBackColor = true;
            // 
            // encodingUTF7RadioButton
            // 
            this.encodingUTF7RadioButton.AutoSize = true;
            this.encodingUTF7RadioButton.Location = new System.Drawing.Point(7, 22);
            this.encodingUTF7RadioButton.Name = "encodingUTF7RadioButton";
            this.encodingUTF7RadioButton.Size = new System.Drawing.Size(51, 19);
            this.encodingUTF7RadioButton.TabIndex = 6;
            this.encodingUTF7RadioButton.Text = "UTF7";
            this.encodingUTF7RadioButton.UseVisualStyleBackColor = true;
            // 
            // encodingASCIIRadioButton
            // 
            this.encodingASCIIRadioButton.AutoSize = true;
            this.encodingASCIIRadioButton.Checked = true;
            this.encodingASCIIRadioButton.Location = new System.Drawing.Point(70, 22);
            this.encodingASCIIRadioButton.Name = "encodingASCIIRadioButton";
            this.encodingASCIIRadioButton.Size = new System.Drawing.Size(53, 19);
            this.encodingASCIIRadioButton.TabIndex = 9;
            this.encodingASCIIRadioButton.TabStop = true;
            this.encodingASCIIRadioButton.Text = "ASCII";
            this.encodingASCIIRadioButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(293, 154);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 12;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // StructDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 189);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.offsetStartFromTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.sizeTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.addressTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.offsetJumpTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(393, 228);
            this.Name = "StructDetailsForm";
            this.Text = "Struct Form";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox sizeTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox addressTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox offsetJumpTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox offsetStartFromTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton encodingUTF8RadioButton;
        private System.Windows.Forms.RadioButton encodingUTF7RadioButton;
        private System.Windows.Forms.RadioButton encodingASCIIRadioButton;
        private System.Windows.Forms.RadioButton encodingUnicodeRadioButton;
        private System.Windows.Forms.RadioButton encodingBigEndianUnicodeRadioButton;
        private System.Windows.Forms.RadioButton encodingUTF32RadioButton;
        private System.Windows.Forms.Button cancelButton;
    }
}