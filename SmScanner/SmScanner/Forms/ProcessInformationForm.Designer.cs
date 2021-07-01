
namespace SmScanner.Forms
{
    partial class ProcessInformationForm
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
            this.modulesDataGridView = new System.Windows.Forms.DataGridView();
            this.icon = new System.Windows.Forms.DataGridViewImageColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageModules = new System.Windows.Forms.TabPage();
            this.tabPageSections = new System.Windows.Forms.TabPage();
            this.sectionsDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.protection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.module = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.modulesDataGridView)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPageModules.SuspendLayout();
            this.tabPageSections.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // modulesDataGridView
            // 
            this.modulesDataGridView.AllowUserToAddRows = false;
            this.modulesDataGridView.AllowUserToDeleteRows = false;
            this.modulesDataGridView.AllowUserToOrderColumns = true;
            this.modulesDataGridView.AllowUserToResizeColumns = false;
            this.modulesDataGridView.AllowUserToResizeRows = false;
            this.modulesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.modulesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.modulesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.icon,
            this.name,
            this.address,
            this.size,
            this.path});
            this.modulesDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modulesDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.modulesDataGridView.Location = new System.Drawing.Point(3, 3);
            this.modulesDataGridView.Name = "modulesDataGridView";
            this.modulesDataGridView.RowHeadersVisible = false;
            this.modulesDataGridView.RowTemplate.Height = 25;
            this.modulesDataGridView.Size = new System.Drawing.Size(620, 385);
            this.modulesDataGridView.TabIndex = 11;
            // 
            // icon
            // 
            this.icon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.icon.DataPropertyName = "Icon";
            this.icon.HeaderText = "Icon";
            this.icon.Name = "icon";
            this.icon.Width = 34;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.name.DataPropertyName = "Name";
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            this.name.Width = 60;
            // 
            // address
            // 
            this.address.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.address.DataPropertyName = "Address";
            this.address.HeaderText = "Address";
            this.address.Name = "address";
            this.address.Width = 70;
            // 
            // size
            // 
            this.size.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.size.DataPropertyName = "Size";
            this.size.HeaderText = "Size";
            this.size.Name = "size";
            this.size.Width = 52;
            // 
            // path
            // 
            this.path.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.path.DataPropertyName = "Path";
            this.path.HeaderText = "Path";
            this.path.Name = "path";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageModules);
            this.tabControl.Controls.Add(this.tabPageSections);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(634, 417);
            this.tabControl.TabIndex = 13;
            // 
            // tabPageModules
            // 
            this.tabPageModules.Controls.Add(this.modulesDataGridView);
            this.tabPageModules.Location = new System.Drawing.Point(4, 22);
            this.tabPageModules.Name = "tabPageModules";
            this.tabPageModules.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageModules.Size = new System.Drawing.Size(626, 391);
            this.tabPageModules.TabIndex = 0;
            this.tabPageModules.Text = "Modules";
            this.tabPageModules.UseVisualStyleBackColor = true;
            // 
            // tabPageSections
            // 
            this.tabPageSections.Controls.Add(this.sectionsDataGridView);
            this.tabPageSections.Location = new System.Drawing.Point(4, 22);
            this.tabPageSections.Name = "tabPageSections";
            this.tabPageSections.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSections.Size = new System.Drawing.Size(626, 391);
            this.tabPageSections.TabIndex = 1;
            this.tabPageSections.Text = "Sections";
            this.tabPageSections.UseVisualStyleBackColor = true;
            // 
            // sectionsDataGridView
            // 
            this.sectionsDataGridView.AllowUserToAddRows = false;
            this.sectionsDataGridView.AllowUserToDeleteRows = false;
            this.sectionsDataGridView.AllowUserToResizeRows = false;
            this.sectionsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sectionsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.protection,
            this.type,
            this.module});
            this.sectionsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionsDataGridView.Location = new System.Drawing.Point(3, 3);
            this.sectionsDataGridView.Name = "sectionsDataGridView";
            this.sectionsDataGridView.RowHeadersVisible = false;
            this.sectionsDataGridView.RowTemplate.Height = 25;
            this.sectionsDataGridView.Size = new System.Drawing.Size(620, 385);
            this.sectionsDataGridView.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Address";
            this.dataGridViewTextBoxColumn1.HeaderText = "Address";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 70;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Size";
            this.dataGridViewTextBoxColumn2.HeaderText = "Size";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 52;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn3.HeaderText = "Name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 60;
            // 
            // protection
            // 
            this.protection.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.protection.DataPropertyName = "Protection";
            this.protection.HeaderText = "Protection";
            this.protection.Name = "protection";
            this.protection.Width = 80;
            // 
            // type
            // 
            this.type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.type.DataPropertyName = "Type";
            this.type.HeaderText = "Type";
            this.type.Name = "type";
            this.type.Width = 56;
            // 
            // module
            // 
            this.module.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.module.DataPropertyName = "Module";
            this.module.HeaderText = "Module";
            this.module.Name = "module";
            // 
            // ProcessInformationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 417);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MinimumSize = new System.Drawing.Size(650, 456);
            this.Name = "ProcessInformationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Process Information";
            ((System.ComponentModel.ISupportInitialize)(this.modulesDataGridView)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPageModules.ResumeLayout(false);
            this.tabPageSections.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sectionsDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView modulesDataGridView;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageModules;
        private System.Windows.Forms.TabPage tabPageSections;
        private System.Windows.Forms.DataGridView sectionsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn protection;
        private System.Windows.Forms.DataGridViewTextBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn module;
        private System.Windows.Forms.DataGridViewImageColumn icon;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn address;
        private System.Windows.Forms.DataGridViewTextBoxColumn size;
        private System.Windows.Forms.DataGridViewTextBoxColumn path;
    }
}