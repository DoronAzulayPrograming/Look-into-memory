
namespace SmScanner.Forms
{
    partial class ProcessBrowserForm
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
            this.checkBoxFilterDuplicateWindows = new System.Windows.Forms.CheckBox();
            this.checkBoxFilter64Bit = new System.Windows.Forms.CheckBox();
            this.checkBoxFilter32Bit = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelPreviousProcessLink = new System.Windows.Forms.LinkLabel();
            this.btnRefreshProcessList = new System.Windows.Forms.Button();
            this.textBoxProcessName = new System.Windows.Forms.TextBox();
            this.checkBoxFilter = new System.Windows.Forms.CheckBox();
            this.labell = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAttachToProcess = new System.Windows.Forms.Button();
            this.dataGridViewProcesses = new System.Windows.Forms.DataGridView();
            this.icon = new System.Windows.Forms.DataGridViewImageColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProcesses)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBoxFilterDuplicateWindows);
            this.groupBox1.Controls.Add(this.checkBoxFilter64Bit);
            this.groupBox1.Controls.Add(this.checkBoxFilter32Bit);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.labelPreviousProcessLink);
            this.groupBox1.Controls.Add(this.btnRefreshProcessList);
            this.groupBox1.Controls.Add(this.textBoxProcessName);
            this.groupBox1.Controls.Add(this.checkBoxFilter);
            this.groupBox1.Controls.Add(this.labell);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(14, 14);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(612, 147);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search For Process";
            // 
            // checkBoxFilterDuplicateWindows
            // 
            this.checkBoxFilterDuplicateWindows.AutoSize = true;
            this.checkBoxFilterDuplicateWindows.Checked = true;
            this.checkBoxFilterDuplicateWindows.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFilterDuplicateWindows.Location = new System.Drawing.Point(319, 85);
            this.checkBoxFilterDuplicateWindows.Name = "checkBoxFilterDuplicateWindows";
            this.checkBoxFilterDuplicateWindows.Size = new System.Drawing.Size(126, 19);
            this.checkBoxFilterDuplicateWindows.TabIndex = 12;
            this.checkBoxFilterDuplicateWindows.Text = "Duplicate windows";
            this.checkBoxFilterDuplicateWindows.UseVisualStyleBackColor = true;
            this.checkBoxFilterDuplicateWindows.CheckedChanged += new System.EventHandler(this.btnRefreshProcessList_Click);
            // 
            // checkBoxFilter64Bit
            // 
            this.checkBoxFilter64Bit.AutoSize = true;
            this.checkBoxFilter64Bit.Location = new System.Drawing.Point(258, 85);
            this.checkBoxFilter64Bit.Name = "checkBoxFilter64Bit";
            this.checkBoxFilter64Bit.Size = new System.Drawing.Size(55, 19);
            this.checkBoxFilter64Bit.TabIndex = 11;
            this.checkBoxFilter64Bit.Text = "64 Bit";
            this.checkBoxFilter64Bit.UseVisualStyleBackColor = true;
            this.checkBoxFilter64Bit.CheckedChanged += new System.EventHandler(this.checkBoxFilter_CheckedChanged);
            // 
            // checkBoxFilter32Bit
            // 
            this.checkBoxFilter32Bit.AutoSize = true;
            this.checkBoxFilter32Bit.Location = new System.Drawing.Point(197, 85);
            this.checkBoxFilter32Bit.Name = "checkBoxFilter32Bit";
            this.checkBoxFilter32Bit.Size = new System.Drawing.Size(55, 19);
            this.checkBoxFilter32Bit.TabIndex = 10;
            this.checkBoxFilter32Bit.Text = "32 Bit";
            this.checkBoxFilter32Bit.UseVisualStyleBackColor = true;
            this.checkBoxFilter32Bit.CheckedChanged += new System.EventHandler(this.checkBoxFilter_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "Exclude: ";
            // 
            // labelPreviousProcessLink
            // 
            this.labelPreviousProcessLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPreviousProcessLink.Location = new System.Drawing.Point(122, 58);
            this.labelPreviousProcessLink.Name = "labelPreviousProcessLink";
            this.labelPreviousProcessLink.Size = new System.Drawing.Size(481, 15);
            this.labelPreviousProcessLink.TabIndex = 8;
            this.labelPreviousProcessLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labelPreviousProcessLink_LinkClicked);
            // 
            // btnRefreshProcessList
            // 
            this.btnRefreshProcessList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshProcessList.Image = global::SmScanner.Properties.Resources.B16x16_Arrow_Refresh;
            this.btnRefreshProcessList.Location = new System.Drawing.Point(7, 111);
            this.btnRefreshProcessList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRefreshProcessList.Name = "btnRefreshProcessList";
            this.btnRefreshProcessList.Size = new System.Drawing.Size(596, 27);
            this.btnRefreshProcessList.TabIndex = 4;
            this.btnRefreshProcessList.Text = "Refresh";
            this.btnRefreshProcessList.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRefreshProcessList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRefreshProcessList.UseVisualStyleBackColor = true;
            this.btnRefreshProcessList.Click += new System.EventHandler(this.btnRefreshProcessList_Click);
            // 
            // textBoxProcessName
            // 
            this.textBoxProcessName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProcessName.Location = new System.Drawing.Point(122, 25);
            this.textBoxProcessName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBoxProcessName.Name = "textBoxProcessName";
            this.textBoxProcessName.Size = new System.Drawing.Size(481, 23);
            this.textBoxProcessName.TabIndex = 3;
            this.textBoxProcessName.TextChanged += new System.EventHandler(this.textBoxProcessName_TextChanged);
            // 
            // checkBoxFilter
            // 
            this.checkBoxFilter.AutoSize = true;
            this.checkBoxFilter.Checked = true;
            this.checkBoxFilter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFilter.Location = new System.Drawing.Point(68, 85);
            this.checkBoxFilter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxFilter.Name = "checkBoxFilter";
            this.checkBoxFilter.Size = new System.Drawing.Size(122, 19);
            this.checkBoxFilter.TabIndex = 2;
            this.checkBoxFilter.Text = "Commne process.";
            this.checkBoxFilter.UseVisualStyleBackColor = true;
            this.checkBoxFilter.CheckedChanged += new System.EventHandler(this.checkBoxFilter_CheckedChanged);
            // 
            // labell
            // 
            this.labell.AutoSize = true;
            this.labell.Location = new System.Drawing.Point(7, 58);
            this.labell.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labell.Name = "labell";
            this.labell.Size = new System.Drawing.Size(101, 15);
            this.labell.TabIndex = 1;
            this.labell.Text = "Previous Process: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Process Name: ";
            // 
            // btnAttachToProcess
            // 
            this.btnAttachToProcess.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAttachToProcess.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAttachToProcess.Image = global::SmScanner.Properties.Resources.B16x16_Accept;
            this.btnAttachToProcess.Location = new System.Drawing.Point(14, 554);
            this.btnAttachToProcess.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAttachToProcess.Name = "btnAttachToProcess";
            this.btnAttachToProcess.Size = new System.Drawing.Size(612, 27);
            this.btnAttachToProcess.TabIndex = 7;
            this.btnAttachToProcess.Text = "Attach";
            this.btnAttachToProcess.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAttachToProcess.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAttachToProcess.UseVisualStyleBackColor = true;
            // 
            // dataGridViewProcesses
            // 
            this.dataGridViewProcesses.AllowUserToAddRows = false;
            this.dataGridViewProcesses.AllowUserToDeleteRows = false;
            this.dataGridViewProcesses.AllowUserToResizeColumns = false;
            this.dataGridViewProcesses.AllowUserToResizeRows = false;
            this.dataGridViewProcesses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewProcesses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewProcesses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewProcesses.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.icon,
            this.name,
            this.id,
            this.path});
            this.dataGridViewProcesses.Location = new System.Drawing.Point(14, 167);
            this.dataGridViewProcesses.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dataGridViewProcesses.MultiSelect = false;
            this.dataGridViewProcesses.Name = "dataGridViewProcesses";
            this.dataGridViewProcesses.ReadOnly = true;
            this.dataGridViewProcesses.RowHeadersVisible = false;
            this.dataGridViewProcesses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewProcesses.Size = new System.Drawing.Size(612, 381);
            this.dataGridViewProcesses.TabIndex = 1;
            this.dataGridViewProcesses.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewProcesses_CellDoubleClick);
            // 
            // icon
            // 
            this.icon.DataPropertyName = "Icon";
            this.icon.HeaderText = "";
            this.icon.Name = "icon";
            this.icon.ReadOnly = true;
            // 
            // name
            // 
            this.name.DataPropertyName = "Name";
            this.name.HeaderText = "Process";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // id
            // 
            this.id.DataPropertyName = "Id";
            this.id.HeaderText = "PID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // path
            // 
            this.path.DataPropertyName = "Path";
            this.path.HeaderText = "Path";
            this.path.Name = "path";
            this.path.ReadOnly = true;
            // 
            // ProcessBrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 593);
            this.Controls.Add(this.dataGridViewProcesses);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnAttachToProcess);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(630, 580);
            this.Name = "ProcessBrowserForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Process Browser";
            this.Load += new System.EventHandler(this.ProcessBrowserForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProcesses)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRefreshProcessList;
        private System.Windows.Forms.TextBox textBoxProcessName;
        private System.Windows.Forms.CheckBox checkBoxFilter;
        private System.Windows.Forms.Label labell;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewProcesses;
        private System.Windows.Forms.DataGridViewImageColumn icon;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn path;
        private System.Windows.Forms.Button btnAttachToProcess;
        private System.Windows.Forms.LinkLabel labelPreviousProcessLink;
        private System.Windows.Forms.CheckBox checkBoxFilter64Bit;
        private System.Windows.Forms.CheckBox checkBoxFilter32Bit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxFilterDuplicateWindows;
    }
}