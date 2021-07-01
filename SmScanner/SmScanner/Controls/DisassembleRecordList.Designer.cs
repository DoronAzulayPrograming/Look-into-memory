
namespace SmScanner.Controls
{
    partial class DisassembleRecordList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.disassemblerDataGridView = new System.Windows.Forms.DataGridView();
            this.address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bytes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.opcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentAddressLabel = new System.Windows.Forms.Label();
            this.currentCmdDescreptionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.disassemblerDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // disassemblerDataGridView
            // 
            this.disassemblerDataGridView.AllowUserToAddRows = false;
            this.disassemblerDataGridView.AllowUserToDeleteRows = false;
            this.disassemblerDataGridView.AllowUserToResizeRows = false;
            this.disassemblerDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.disassemblerDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.disassemblerDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.disassemblerDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.address,
            this.bytes,
            this.opcode});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.disassemblerDataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.disassemblerDataGridView.Location = new System.Drawing.Point(0, 27);
            this.disassemblerDataGridView.Name = "disassemblerDataGridView";
            this.disassemblerDataGridView.ReadOnly = true;
            this.disassemblerDataGridView.RowHeadersVisible = false;
            this.disassemblerDataGridView.RowTemplate.Height = 25;
            this.disassemblerDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.disassemblerDataGridView.Size = new System.Drawing.Size(563, 303);
            this.disassemblerDataGridView.TabIndex = 1;
            this.disassemblerDataGridView.RowContextMenuStripNeeded += new System.Windows.Forms.DataGridViewRowContextMenuStripNeededEventHandler(this.disassemblerDataGridView_RowContextMenuStripNeeded);
            this.disassemblerDataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.disassemblerDataGridView_MouseDown);
            // 
            // address
            // 
            this.address.DataPropertyName = "address";
            this.address.FillWeight = 125F;
            this.address.HeaderText = "Address";
            this.address.Name = "address";
            this.address.ReadOnly = true;
            this.address.Width = 125;
            // 
            // bytes
            // 
            this.bytes.DataPropertyName = "bytes";
            this.bytes.FillWeight = 180F;
            this.bytes.HeaderText = "Bytes";
            this.bytes.Name = "bytes";
            this.bytes.ReadOnly = true;
            this.bytes.Width = 180;
            // 
            // opcode
            // 
            this.opcode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.opcode.DataPropertyName = "opcode";
            this.opcode.HeaderText = "Opcode";
            this.opcode.Name = "opcode";
            this.opcode.ReadOnly = true;
            // 
            // currentAddressLabel
            // 
            this.currentAddressLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.currentAddressLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.currentAddressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.currentAddressLabel.Location = new System.Drawing.Point(0, 0);
            this.currentAddressLabel.Name = "currentAddressLabel";
            this.currentAddressLabel.Size = new System.Drawing.Size(563, 24);
            this.currentAddressLabel.TabIndex = 2;
            this.currentAddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // currentCmdDescreptionLabel
            // 
            this.currentCmdDescreptionLabel.BackColor = System.Drawing.Color.White;
            this.currentCmdDescreptionLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.currentCmdDescreptionLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.currentCmdDescreptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.currentCmdDescreptionLabel.Location = new System.Drawing.Point(0, 333);
            this.currentCmdDescreptionLabel.Name = "currentCmdDescreptionLabel";
            this.currentCmdDescreptionLabel.Size = new System.Drawing.Size(563, 24);
            this.currentCmdDescreptionLabel.TabIndex = 3;
            this.currentCmdDescreptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DisassembleRecordList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.currentCmdDescreptionLabel);
            this.Controls.Add(this.currentAddressLabel);
            this.Controls.Add(this.disassemblerDataGridView);
            this.Name = "DisassembleRecordList";
            this.Size = new System.Drawing.Size(563, 357);
            ((System.ComponentModel.ISupportInitialize)(this.disassemblerDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView disassemblerDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn address;
        private System.Windows.Forms.DataGridViewTextBoxColumn bytes;
        private System.Windows.Forms.DataGridViewTextBoxColumn opcode;
        private System.Windows.Forms.Label currentAddressLabel;
        private System.Windows.Forms.Label currentCmdDescreptionLabel;
    }
}
