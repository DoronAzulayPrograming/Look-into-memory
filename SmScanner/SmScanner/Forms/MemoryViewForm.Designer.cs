
using System;

namespace SmScanner.Forms
{
    partial class MemoryViewForm
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.disassembleRecordList = new SmScanner.Controls.DisassembleRecordList();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.goToAddressDisassembleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectCurrentFunctionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyBytesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyOpcodetoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copySelectedsValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAstoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutHexViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutDisassemblerViewtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.structsViewtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexDumpRecordList = new SmScanner.Controls.HexDumpRecordList();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.disassembleRecordList);
            this.splitContainer1.Panel1.Controls.Add(this.menuStrip);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.hexDumpRecordList);
            this.splitContainer1.Size = new System.Drawing.Size(838, 761);
            this.splitContainer1.SplitterDistance = 470;
            this.splitContainer1.TabIndex = 0;
            // 
            // disassembleRecordList
            // 
            this.disassembleRecordList.ContextMenuStrip = this.contextMenuStrip;
            this.disassembleRecordList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.disassembleRecordList.Location = new System.Drawing.Point(0, 24);
            this.disassembleRecordList.Name = "disassembleRecordList";
            this.disassembleRecordList.Size = new System.Drawing.Size(838, 446);
            this.disassembleRecordList.TabIndex = 1;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToAddressDisassembleToolStripMenuItem,
            this.selectCurrentFunctionToolStripMenuItem,
            this.copyToClipboardToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(195, 70);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // goToAddressDisassembleToolStripMenuItem
            // 
            this.goToAddressDisassembleToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Eye;
            this.goToAddressDisassembleToolStripMenuItem.Name = "goToAddressDisassembleToolStripMenuItem";
            this.goToAddressDisassembleToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.goToAddressDisassembleToolStripMenuItem.Text = "Go to address";
            this.goToAddressDisassembleToolStripMenuItem.Click += new System.EventHandler(this.goToAddressDisassembleToolStripMenuItem_Click);
            // 
            // selectCurrentFunctionToolStripMenuItem
            // 
            this.selectCurrentFunctionToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Function_Type;
            this.selectCurrentFunctionToolStripMenuItem.Name = "selectCurrentFunctionToolStripMenuItem";
            this.selectCurrentFunctionToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.selectCurrentFunctionToolStripMenuItem.Text = "Select current function";
            this.selectCurrentFunctionToolStripMenuItem.Click += new System.EventHandler(this.selectCurrentFunctionToolStripMenuItem_Click);
            // 
            // copyToClipboardToolStripMenuItem
            // 
            this.copyToClipboardToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyRecordToolStripMenuItem,
            this.copyAddressToolStripMenuItem,
            this.copyBytesToolStripMenuItem,
            this.copyOpcodetoolStripMenuItem,
            this.copySelectedsValuesToolStripMenuItem});
            this.copyToClipboardToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Page_Copy;
            this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.copyToClipboardToolStripMenuItem.Text = "Copy to clipboard";
            // 
            // copyRecordToolStripMenuItem
            // 
            this.copyRecordToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Page_Copy;
            this.copyRecordToolStripMenuItem.Name = "copyRecordToolStripMenuItem";
            this.copyRecordToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.copyRecordToolStripMenuItem.Text = "Record";
            this.copyRecordToolStripMenuItem.Click += new System.EventHandler(this.copyRecordToolStripMenuItem_Click);
            // 
            // copyAddressToolStripMenuItem
            // 
            this.copyAddressToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Page_Copy;
            this.copyAddressToolStripMenuItem.Name = "copyAddressToolStripMenuItem";
            this.copyAddressToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.copyAddressToolStripMenuItem.Text = "Address";
            this.copyAddressToolStripMenuItem.Click += new System.EventHandler(this.copyAddressToolStripMenuItem_Click);
            // 
            // copyBytesToolStripMenuItem
            // 
            this.copyBytesToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Page_Copy;
            this.copyBytesToolStripMenuItem.Name = "copyBytesToolStripMenuItem";
            this.copyBytesToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.copyBytesToolStripMenuItem.Text = "Bytes";
            this.copyBytesToolStripMenuItem.Click += new System.EventHandler(this.copyBytesToolStripMenuItem_Click);
            // 
            // copyOpcodetoolStripMenuItem
            // 
            this.copyOpcodetoolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Page_Copy;
            this.copyOpcodetoolStripMenuItem.Name = "copyOpcodetoolStripMenuItem";
            this.copyOpcodetoolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.copyOpcodetoolStripMenuItem.Text = "Opcode";
            this.copyOpcodetoolStripMenuItem.Click += new System.EventHandler(this.copyOpcodeToolStripMenuItem_Click);
            // 
            // copySelectedsValuesToolStripMenuItem
            // 
            this.copySelectedsValuesToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Page_Copy;
            this.copySelectedsValuesToolStripMenuItem.Name = "copySelectedsValuesToolStripMenuItem";
            this.copySelectedsValuesToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.copySelectedsValuesToolStripMenuItem.Text = "Selecteds";
            this.copySelectedsValuesToolStripMenuItem.Click += new System.EventHandler(this.copySelectedsValuesToolStripMenuItem_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.structsViewtoolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(838, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveToolStripMenuItem,
            this.saveAstoolStripMenuItem,
            this.toolStripSeparator2,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Magnifier;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(109, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Save;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAstoolStripMenuItem
            // 
            this.saveAstoolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Save_As;
            this.saveAstoolStripMenuItem.Name = "saveAstoolStripMenuItem";
            this.saveAstoolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.saveAstoolStripMenuItem.Text = "&Save as";
            this.saveAstoolStripMenuItem.Click += new System.EventHandler(this.saveAstoolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(109, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Quit;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "&Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutHexViewToolStripMenuItem,
            this.aboutDisassemblerViewtoolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "&About";
            // 
            // aboutHexViewToolStripMenuItem
            // 
            this.aboutHexViewToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Information;
            this.aboutHexViewToolStripMenuItem.Name = "aboutHexViewToolStripMenuItem";
            this.aboutHexViewToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.aboutHexViewToolStripMenuItem.Text = "&Hex View";
            this.aboutHexViewToolStripMenuItem.Click += new System.EventHandler(this.aboutHexViewToolStripMenuItem_Click);
            // 
            // aboutDisassemblerViewtoolStripMenuItem
            // 
            this.aboutDisassemblerViewtoolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Information;
            this.aboutDisassemblerViewtoolStripMenuItem.Name = "aboutDisassemblerViewtoolStripMenuItem";
            this.aboutDisassemblerViewtoolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.aboutDisassemblerViewtoolStripMenuItem.Text = "&Disassembler View";
            this.aboutDisassemblerViewtoolStripMenuItem.Click += new System.EventHandler(this.aboutDisassemblerViewtoolStripMenuItem_Click);
            // 
            // structsViewtoolStripMenuItem
            // 
            this.structsViewtoolStripMenuItem.Name = "structsViewtoolStripMenuItem";
            this.structsViewtoolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.structsViewtoolStripMenuItem.Text = "&Structs View";
            this.structsViewtoolStripMenuItem.Click += new System.EventHandler(this.structsViewtoolStripMenuItem_Click);
            // 
            // hexDumpRecordList
            // 
            this.hexDumpRecordList.ContextMenuStrip = this.contextMenuStrip;
            this.hexDumpRecordList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexDumpRecordList.Location = new System.Drawing.Point(0, 0);
            this.hexDumpRecordList.Name = "hexDumpRecordList";
            this.hexDumpRecordList.Size = new System.Drawing.Size(838, 287);
            this.hexDumpRecordList.TabIndex = 2;
            // 
            // MemoryViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 761);
            this.Controls.Add(this.splitContainer1);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(854, 600);
            this.Name = "MemoryViewForm";
            this.Text = "Memory View";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem goToAddressDisassembleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyRecordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyAddressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyBytesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyOpcodetoolStripMenuItem;
        private Controls.HexDumpRecordList hexDumpRecordList;
        private System.Windows.Forms.ToolStripMenuItem copySelectedsValuesToolStripMenuItem;
        private Controls.DisassembleRecordList disassembleRecordList;
        private System.Windows.Forms.ToolStripMenuItem selectCurrentFunctionToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAstoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutHexViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutDisassemblerViewtoolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem structsViewtoolStripMenuItem;
    }
}