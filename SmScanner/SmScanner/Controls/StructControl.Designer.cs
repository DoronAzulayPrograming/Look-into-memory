
namespace SmScanner.Controls
{
    partial class StructControl
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
            this.recordsDataGridView = new System.Windows.Forms.DataGridView();
            this.offset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.recordsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // recordsDataGridView
            // 
            this.recordsDataGridView.AllowUserToAddRows = false;
            this.recordsDataGridView.AllowUserToDeleteRows = false;
            this.recordsDataGridView.AllowUserToResizeRows = false;
            this.recordsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.recordsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.offset,
            this.type,
            this.value,
            this.address});
            this.recordsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recordsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.recordsDataGridView.Name = "recordsDataGridView";
            this.recordsDataGridView.ReadOnly = true;
            this.recordsDataGridView.RowHeadersVisible = false;
            this.recordsDataGridView.RowTemplate.Height = 25;
            this.recordsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.recordsDataGridView.Size = new System.Drawing.Size(557, 369);
            this.recordsDataGridView.TabIndex = 0;
            this.recordsDataGridView.RowContextMenuStripNeeded += new System.Windows.Forms.DataGridViewRowContextMenuStripNeededEventHandler(this.recordsDataGridView_RowContextMenuStripNeeded);
            this.recordsDataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.recordsDataGridView_MouseDown);
            // 
            // offset
            // 
            this.offset.DataPropertyName = "OffsetStr";
            this.offset.FillWeight = 50F;
            this.offset.HeaderText = "Offset";
            this.offset.Name = "offset";
            this.offset.ReadOnly = true;
            this.offset.Width = 50;
            // 
            // type
            // 
            this.type.DataPropertyName = "TypeStr";
            this.type.FillWeight = 130F;
            this.type.HeaderText = "Type";
            this.type.Name = "type";
            this.type.ReadOnly = true;
            this.type.Width = 130;
            // 
            // value
            // 
            this.value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.value.DataPropertyName = "ValueStr";
            this.value.HeaderText = "Value";
            this.value.Name = "value";
            this.value.ReadOnly = true;
            // 
            // address
            // 
            this.address.DataPropertyName = "AddressStr";
            this.address.FillWeight = 90F;
            this.address.HeaderText = "Address";
            this.address.Name = "address";
            this.address.ReadOnly = true;
            this.address.Width = 90;
            // 
            // StructControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.recordsDataGridView);
            this.Name = "StructControl";
            this.Size = new System.Drawing.Size(557, 369);
            ((System.ComponentModel.ISupportInitialize)(this.recordsDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView recordsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn offset;
        private System.Windows.Forms.DataGridViewTextBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn value;
        private System.Windows.Forms.DataGridViewTextBoxColumn address;
    }
}
