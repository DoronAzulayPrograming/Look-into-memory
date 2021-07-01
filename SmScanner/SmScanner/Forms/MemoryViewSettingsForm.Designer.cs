
namespace SmScanner.Forms
{
    partial class MemoryViewSettingsForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.disassemblerBuffertextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.hexBufferTextBox = new System.Windows.Forms.TextBox();
            this.cencelButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.disassemblerBuffertextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(216, 59);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Disassembler View";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Buffer Size:";
            // 
            // disassemblerBuffertextBox
            // 
            this.disassemblerBuffertextBox.Location = new System.Drawing.Point(77, 22);
            this.disassemblerBuffertextBox.Name = "disassemblerBuffertextBox";
            this.disassemblerBuffertextBox.Size = new System.Drawing.Size(127, 23);
            this.disassemblerBuffertextBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.hexBufferTextBox);
            this.groupBox2.Location = new System.Drawing.Point(234, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(216, 59);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Hex View";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Buffer Size:";
            // 
            // hexBufferTextBox
            // 
            this.hexBufferTextBox.Location = new System.Drawing.Point(77, 22);
            this.hexBufferTextBox.Name = "hexBufferTextBox";
            this.hexBufferTextBox.Size = new System.Drawing.Size(127, 23);
            this.hexBufferTextBox.TabIndex = 1;
            // 
            // cencelButton
            // 
            this.cencelButton.Location = new System.Drawing.Point(375, 77);
            this.cencelButton.Name = "cencelButton";
            this.cencelButton.Size = new System.Drawing.Size(75, 23);
            this.cencelButton.TabIndex = 3;
            this.cencelButton.Text = "Close";
            this.cencelButton.UseVisualStyleBackColor = true;
            this.cencelButton.Click += new System.EventHandler(this.cencelButton_Click);
            // 
            // MemoryViewSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 109);
            this.Controls.Add(this.cencelButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MemoryViewSettingsForm";
            this.Text = "Memory View Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox disassemblerBuffertextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox hexBufferTextBox;
        private System.Windows.Forms.Button cencelButton;
    }
}