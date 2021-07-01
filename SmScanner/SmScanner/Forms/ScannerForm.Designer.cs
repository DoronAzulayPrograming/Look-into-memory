
using System;

namespace SmScanner.Forms
{
    partial class ScannerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScannerForm));
            this.labelProcessSelected = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.resultListContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemAddSelectedResultsToAddressList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRemoveSelectedRecords = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemChangeValue = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemChangeDescription = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemCopyAddress = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopyAddressText = new System.Windows.Forms.ToolStripMenuItem();
            this.resultCountLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.compareTypeComboBox = new SmScanner.Forms.ScannerForm.ScanCompareTypeComboBox();
            this.valueTypeComboBox = new SmScanner.Forms.ScannerForm.ScanValueTypeComboBox();
            this.dualValueBox = new SmScanner.Controls.DualValueBox();
            this.isHexCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.floatingOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.roundTruncateRadioButton = new System.Windows.Forms.RadioButton();
            this.roundLooseRadioButton = new System.Windows.Forms.RadioButton();
            this.roundStrictRadioButton = new System.Windows.Forms.RadioButton();
            this.stringOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.caseSensitiveCheckBox = new System.Windows.Forms.CheckBox();
            this.encodingUtf32RadioButton = new System.Windows.Forms.RadioButton();
            this.encodingUtf16RadioButton = new System.Windows.Forms.RadioButton();
            this.encodingUtf8RadioButton = new System.Windows.Forms.RadioButton();
            this.scanOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.fastScanAlignmentTextBox = new System.Windows.Forms.TextBox();
            this.fastScanCheckBox = new System.Windows.Forms.CheckBox();
            this.scanExecutableCheckBox = new System.Windows.Forms.CheckBox();
            this.scanWritableCheckBox = new System.Windows.Forms.CheckBox();
            this.scanMappedCheckBox = new System.Windows.Forms.CheckBox();
            this.scanImageCheckBox = new System.Windows.Forms.CheckBox();
            this.scanPrivateCheckBox = new System.Windows.Forms.CheckBox();
            this.stopAddressTextBox = new System.Windows.Forms.TextBox();
            this.startAddressTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnFirstScan = new System.Windows.Forms.Button();
            this.btnNextScan = new System.Windows.Forms.Button();
            this.labelFliker = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnCencelScan = new System.Windows.Forms.Button();
            this.updateValuesTimer = new System.Windows.Forms.Timer(this.components);
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.btnTransferSelectedResultToAddressList = new SmScanner.Controls.IconButton();
            this.resultsMemoryRecordList = new SmScanner.Controls.MemoryRecordList();
            this.btnProcessBrowser = new SmScanner.Controls.IconButton();
            this.btnClearAddressList = new SmScanner.Controls.IconButton();
            this.addressListMemoryRecordList = new SmScanner.Controls.MemoryRecordList();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attacthProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reattachProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detachToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processViewMemoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.processResumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processSuspendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processKillToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resultListContextMenuStrip.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.floatingOptionsGroupBox.SuspendLayout();
            this.stringOptionsGroupBox.SuspendLayout();
            this.scanOptionsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelProcessSelected
            // 
            this.labelProcessSelected.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelProcessSelected.Location = new System.Drawing.Point(0, 0);
            this.labelProcessSelected.Name = "labelProcessSelected";
            this.labelProcessSelected.Size = new System.Drawing.Size(688, 13);
            this.labelProcessSelected.TabIndex = 1;
            this.labelProcessSelected.Text = "No Process Selected";
            this.labelProcessSelected.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(49, 16);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(624, 13);
            this.progressBar.TabIndex = 2;
            // 
            // resultListContextMenuStrip
            // 
            this.resultListContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAddSelectedResultsToAddressList,
            this.toolStripMenuItemRemoveSelectedRecords,
            this.toolStripMenuItemChangeValue,
            this.toolStripMenuItemChangeDescription,
            this.toolStripSeparator1,
            this.toolStripMenuItemCopyAddress,
            this.toolStripMenuItemCopyAddressText});
            this.resultListContextMenuStrip.Name = "resultListContextMenuStrip";
            this.resultListContextMenuStrip.Size = new System.Drawing.Size(258, 142);
            this.resultListContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.resultListContextMenuStrip_Opening);
            // 
            // toolStripMenuItemAddSelectedResultsToAddressList
            // 
            this.toolStripMenuItemAddSelectedResultsToAddressList.Image = global::SmScanner.Properties.Resources.B16x16_Tree_Expand;
            this.toolStripMenuItemAddSelectedResultsToAddressList.Name = "toolStripMenuItemAddSelectedResultsToAddressList";
            this.toolStripMenuItemAddSelectedResultsToAddressList.Size = new System.Drawing.Size(257, 22);
            this.toolStripMenuItemAddSelectedResultsToAddressList.Text = " Add selected results to address list";
            this.toolStripMenuItemAddSelectedResultsToAddressList.Click += new System.EventHandler(this.addSelectedResultsToAddressListToolStripMenuItem_Click);
            // 
            // toolStripMenuItemRemoveSelectedRecords
            // 
            this.toolStripMenuItemRemoveSelectedRecords.Image = global::SmScanner.Properties.Resources.B16x16_Button_Delete;
            this.toolStripMenuItemRemoveSelectedRecords.Name = "toolStripMenuItemRemoveSelectedRecords";
            this.toolStripMenuItemRemoveSelectedRecords.Size = new System.Drawing.Size(257, 22);
            this.toolStripMenuItemRemoveSelectedRecords.Text = "Remove selected records";
            this.toolStripMenuItemRemoveSelectedRecords.Click += new System.EventHandler(this.removeSelectedRecordsToolStripMenuItem_Click);
            // 
            // toolStripMenuItemChangeValue
            // 
            this.toolStripMenuItemChangeValue.Image = global::SmScanner.Properties.Resources.B16x16_Textfield_Rename;
            this.toolStripMenuItemChangeValue.Name = "toolStripMenuItemChangeValue";
            this.toolStripMenuItemChangeValue.Size = new System.Drawing.Size(257, 22);
            this.toolStripMenuItemChangeValue.Text = "Change value";
            this.toolStripMenuItemChangeValue.Click += new System.EventHandler(this.changeValueToolStripMenuItem_Click);
            // 
            // toolStripMenuItemChangeDescription
            // 
            this.toolStripMenuItemChangeDescription.Image = global::SmScanner.Properties.Resources.B16x16_Textfield_Rename;
            this.toolStripMenuItemChangeDescription.Name = "toolStripMenuItemChangeDescription";
            this.toolStripMenuItemChangeDescription.Size = new System.Drawing.Size(257, 22);
            this.toolStripMenuItemChangeDescription.Text = "Change description";
            this.toolStripMenuItemChangeDescription.Click += new System.EventHandler(this.changeDescriptionToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(254, 6);
            // 
            // toolStripMenuItemCopyAddress
            // 
            this.toolStripMenuItemCopyAddress.Image = global::SmScanner.Properties.Resources.B16x16_Page_Copy;
            this.toolStripMenuItemCopyAddress.Name = "toolStripMenuItemCopyAddress";
            this.toolStripMenuItemCopyAddress.Size = new System.Drawing.Size(257, 22);
            this.toolStripMenuItemCopyAddress.Text = "Copy address";
            this.toolStripMenuItemCopyAddress.Click += new System.EventHandler(this.copyAddressToolStripMenuItem_Click);
            // 
            // toolStripMenuItemCopyAddressText
            // 
            this.toolStripMenuItemCopyAddressText.Image = global::SmScanner.Properties.Resources.B16x16_Page_Copy;
            this.toolStripMenuItemCopyAddressText.Name = "toolStripMenuItemCopyAddressText";
            this.toolStripMenuItemCopyAddressText.Size = new System.Drawing.Size(257, 22);
            this.toolStripMenuItemCopyAddressText.Text = "Copy address text";
            this.toolStripMenuItemCopyAddressText.Click += new System.EventHandler(this.copyAddressTextToolStripMenuItem_Click);
            // 
            // resultCountLabel
            // 
            this.resultCountLabel.AutoSize = true;
            this.resultCountLabel.Location = new System.Drawing.Point(12, 40);
            this.resultCountLabel.Name = "resultCountLabel";
            this.resultCountLabel.Size = new System.Drawing.Size(49, 13);
            this.resultCountLabel.TabIndex = 5;
            this.resultCountLabel.Text = "Found: 0";
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel.Controls.Add(this.groupBox1);
            this.flowLayoutPanel.Controls.Add(this.floatingOptionsGroupBox);
            this.flowLayoutPanel.Controls.Add(this.stringOptionsGroupBox);
            this.flowLayoutPanel.Controls.Add(this.scanOptionsGroupBox);
            this.flowLayoutPanel.Location = new System.Drawing.Point(404, 85);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(269, 389);
            this.flowLayoutPanel.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.compareTypeComboBox);
            this.groupBox1.Controls.Add(this.valueTypeComboBox);
            this.groupBox1.Controls.Add(this.dualValueBox);
            this.groupBox1.Controls.Add(this.isHexCheckBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 110);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // compareTypeComboBox
            // 
            this.compareTypeComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.compareTypeComboBox.Location = new System.Drawing.Point(76, 54);
            this.compareTypeComboBox.Name = "compareTypeComboBox";
            this.compareTypeComboBox.Size = new System.Drawing.Size(182, 21);
            this.compareTypeComboBox.TabIndex = 5;
            this.compareTypeComboBox.SelectionChangeCommitted += new System.EventHandler(this.scanTypeComboBox_SelectionChangeCommitted);
            // 
            // valueTypeComboBox
            // 
            this.valueTypeComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.valueTypeComboBox.Location = new System.Drawing.Point(76, 80);
            this.valueTypeComboBox.Name = "valueTypeComboBox";
            this.valueTypeComboBox.Size = new System.Drawing.Size(182, 21);
            this.valueTypeComboBox.TabIndex = 8;
            this.valueTypeComboBox.SelectionChangeCommitted += new System.EventHandler(this.valueTypeComboBox_SelectionChangeCommitted);
            // 
            // dualValueBox
            // 
            this.dualValueBox.Location = new System.Drawing.Point(76, 14);
            this.dualValueBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dualValueBox.Name = "dualValueBox";
            this.dualValueBox.ShowSecondInputField = false;
            this.dualValueBox.Size = new System.Drawing.Size(182, 34);
            this.dualValueBox.TabIndex = 3;
            this.dualValueBox.Value1 = "";
            this.dualValueBox.Value2 = "";
            // 
            // isHexCheckBox
            // 
            this.isHexCheckBox.AutoSize = true;
            this.isHexCheckBox.Location = new System.Drawing.Point(5, 31);
            this.isHexCheckBox.Name = "isHexCheckBox";
            this.isHexCheckBox.Size = new System.Drawing.Size(56, 17);
            this.isHexCheckBox.TabIndex = 2;
            this.isHexCheckBox.Text = "Is Hex";
            this.isHexCheckBox.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Value Type: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Scan Type: ";
            // 
            // floatingOptionsGroupBox
            // 
            this.floatingOptionsGroupBox.Controls.Add(this.roundTruncateRadioButton);
            this.floatingOptionsGroupBox.Controls.Add(this.roundLooseRadioButton);
            this.floatingOptionsGroupBox.Controls.Add(this.roundStrictRadioButton);
            this.floatingOptionsGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.floatingOptionsGroupBox.Location = new System.Drawing.Point(3, 119);
            this.floatingOptionsGroupBox.Name = "floatingOptionsGroupBox";
            this.floatingOptionsGroupBox.Size = new System.Drawing.Size(266, 60);
            this.floatingOptionsGroupBox.TabIndex = 8;
            this.floatingOptionsGroupBox.TabStop = false;
            this.floatingOptionsGroupBox.Visible = false;
            // 
            // roundTruncateRadioButton
            // 
            this.roundTruncateRadioButton.AutoSize = true;
            this.roundTruncateRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.roundTruncateRadioButton.Location = new System.Drawing.Point(85, 40);
            this.roundTruncateRadioButton.Name = "roundTruncateRadioButton";
            this.roundTruncateRadioButton.Size = new System.Drawing.Size(68, 17);
            this.roundTruncateRadioButton.TabIndex = 2;
            this.roundTruncateRadioButton.Text = "Truncate";
            this.roundTruncateRadioButton.UseVisualStyleBackColor = true;
            // 
            // roundLooseRadioButton
            // 
            this.roundLooseRadioButton.AutoSize = true;
            this.roundLooseRadioButton.Checked = true;
            this.roundLooseRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.roundLooseRadioButton.Location = new System.Drawing.Point(85, 24);
            this.roundLooseRadioButton.Name = "roundLooseRadioButton";
            this.roundLooseRadioButton.Size = new System.Drawing.Size(103, 17);
            this.roundLooseRadioButton.TabIndex = 1;
            this.roundLooseRadioButton.TabStop = true;
            this.roundLooseRadioButton.Text = "Rounded (loose)";
            this.roundLooseRadioButton.UseVisualStyleBackColor = true;
            // 
            // roundStrictRadioButton
            // 
            this.roundStrictRadioButton.AutoSize = true;
            this.roundStrictRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.roundStrictRadioButton.Location = new System.Drawing.Point(85, 7);
            this.roundStrictRadioButton.Name = "roundStrictRadioButton";
            this.roundStrictRadioButton.Size = new System.Drawing.Size(100, 17);
            this.roundStrictRadioButton.TabIndex = 0;
            this.roundStrictRadioButton.Text = "Rounded (strict)";
            this.roundStrictRadioButton.UseVisualStyleBackColor = true;
            // 
            // stringOptionsGroupBox
            // 
            this.stringOptionsGroupBox.Controls.Add(this.caseSensitiveCheckBox);
            this.stringOptionsGroupBox.Controls.Add(this.encodingUtf32RadioButton);
            this.stringOptionsGroupBox.Controls.Add(this.encodingUtf16RadioButton);
            this.stringOptionsGroupBox.Controls.Add(this.encodingUtf8RadioButton);
            this.stringOptionsGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.stringOptionsGroupBox.Location = new System.Drawing.Point(3, 185);
            this.stringOptionsGroupBox.Name = "stringOptionsGroupBox";
            this.stringOptionsGroupBox.Size = new System.Drawing.Size(266, 60);
            this.stringOptionsGroupBox.TabIndex = 9;
            this.stringOptionsGroupBox.TabStop = false;
            this.stringOptionsGroupBox.Visible = false;
            // 
            // caseSensitiveCheckBox
            // 
            this.caseSensitiveCheckBox.AutoSize = true;
            this.caseSensitiveCheckBox.Checked = true;
            this.caseSensitiveCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.caseSensitiveCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.caseSensitiveCheckBox.Location = new System.Drawing.Point(153, 8);
            this.caseSensitiveCheckBox.Name = "caseSensitiveCheckBox";
            this.caseSensitiveCheckBox.Size = new System.Drawing.Size(96, 17);
            this.caseSensitiveCheckBox.TabIndex = 3;
            this.caseSensitiveCheckBox.Text = "Case Sensitive";
            this.caseSensitiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // encodingUtf32RadioButton
            // 
            this.encodingUtf32RadioButton.AutoSize = true;
            this.encodingUtf32RadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.encodingUtf32RadioButton.Location = new System.Drawing.Point(85, 40);
            this.encodingUtf32RadioButton.Name = "encodingUtf32RadioButton";
            this.encodingUtf32RadioButton.Size = new System.Drawing.Size(61, 17);
            this.encodingUtf32RadioButton.TabIndex = 2;
            this.encodingUtf32RadioButton.Text = "UTF-32";
            this.encodingUtf32RadioButton.UseVisualStyleBackColor = true;
            // 
            // encodingUtf16RadioButton
            // 
            this.encodingUtf16RadioButton.AutoSize = true;
            this.encodingUtf16RadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.encodingUtf16RadioButton.Location = new System.Drawing.Point(85, 24);
            this.encodingUtf16RadioButton.Name = "encodingUtf16RadioButton";
            this.encodingUtf16RadioButton.Size = new System.Drawing.Size(61, 17);
            this.encodingUtf16RadioButton.TabIndex = 1;
            this.encodingUtf16RadioButton.Text = "UTF-16";
            this.encodingUtf16RadioButton.UseVisualStyleBackColor = true;
            // 
            // encodingUtf8RadioButton
            // 
            this.encodingUtf8RadioButton.AutoSize = true;
            this.encodingUtf8RadioButton.Checked = true;
            this.encodingUtf8RadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.encodingUtf8RadioButton.Location = new System.Drawing.Point(85, 7);
            this.encodingUtf8RadioButton.Name = "encodingUtf8RadioButton";
            this.encodingUtf8RadioButton.Size = new System.Drawing.Size(55, 17);
            this.encodingUtf8RadioButton.TabIndex = 0;
            this.encodingUtf8RadioButton.TabStop = true;
            this.encodingUtf8RadioButton.Text = "UTF-8";
            this.encodingUtf8RadioButton.UseVisualStyleBackColor = true;
            // 
            // scanOptionsGroupBox
            // 
            this.scanOptionsGroupBox.Controls.Add(this.fastScanAlignmentTextBox);
            this.scanOptionsGroupBox.Controls.Add(this.fastScanCheckBox);
            this.scanOptionsGroupBox.Controls.Add(this.scanExecutableCheckBox);
            this.scanOptionsGroupBox.Controls.Add(this.scanWritableCheckBox);
            this.scanOptionsGroupBox.Controls.Add(this.scanMappedCheckBox);
            this.scanOptionsGroupBox.Controls.Add(this.scanImageCheckBox);
            this.scanOptionsGroupBox.Controls.Add(this.scanPrivateCheckBox);
            this.scanOptionsGroupBox.Controls.Add(this.stopAddressTextBox);
            this.scanOptionsGroupBox.Controls.Add(this.startAddressTextBox);
            this.scanOptionsGroupBox.Controls.Add(this.label5);
            this.scanOptionsGroupBox.Controls.Add(this.label4);
            this.scanOptionsGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.scanOptionsGroupBox.Location = new System.Drawing.Point(3, 251);
            this.scanOptionsGroupBox.Name = "scanOptionsGroupBox";
            this.scanOptionsGroupBox.Size = new System.Drawing.Size(266, 133);
            this.scanOptionsGroupBox.TabIndex = 8;
            this.scanOptionsGroupBox.TabStop = false;
            this.scanOptionsGroupBox.Text = "Scan Options";
            // 
            // fastScanAlignmentTextBox
            // 
            this.fastScanAlignmentTextBox.Location = new System.Drawing.Point(134, 106);
            this.fastScanAlignmentTextBox.Name = "fastScanAlignmentTextBox";
            this.fastScanAlignmentTextBox.Size = new System.Drawing.Size(23, 20);
            this.fastScanAlignmentTextBox.TabIndex = 10;
            // 
            // fastScanCheckBox
            // 
            this.fastScanCheckBox.AutoSize = true;
            this.fastScanCheckBox.Location = new System.Drawing.Point(6, 109);
            this.fastScanCheckBox.Name = "fastScanCheckBox";
            this.fastScanCheckBox.Size = new System.Drawing.Size(129, 17);
            this.fastScanCheckBox.TabIndex = 9;
            this.fastScanCheckBox.Text = "Fast Scan, Alignment:";
            this.fastScanCheckBox.UseVisualStyleBackColor = true;
            // 
            // scanExecutableCheckBox
            // 
            this.scanExecutableCheckBox.AutoSize = true;
            this.scanExecutableCheckBox.Checked = true;
            this.scanExecutableCheckBox.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.scanExecutableCheckBox.Location = new System.Drawing.Point(83, 87);
            this.scanExecutableCheckBox.Name = "scanExecutableCheckBox";
            this.scanExecutableCheckBox.Size = new System.Drawing.Size(85, 17);
            this.scanExecutableCheckBox.TabIndex = 8;
            this.scanExecutableCheckBox.Text = "Executeable";
            this.scanExecutableCheckBox.ThreeState = true;
            this.scanExecutableCheckBox.UseVisualStyleBackColor = true;
            // 
            // scanWritableCheckBox
            // 
            this.scanWritableCheckBox.AutoSize = true;
            this.scanWritableCheckBox.Location = new System.Drawing.Point(6, 87);
            this.scanWritableCheckBox.Name = "scanWritableCheckBox";
            this.scanWritableCheckBox.Size = new System.Drawing.Size(71, 17);
            this.scanWritableCheckBox.TabIndex = 7;
            this.scanWritableCheckBox.Text = "Writeable";
            this.scanWritableCheckBox.UseVisualStyleBackColor = true;
            // 
            // scanMappedCheckBox
            // 
            this.scanMappedCheckBox.AutoSize = true;
            this.scanMappedCheckBox.Location = new System.Drawing.Point(163, 65);
            this.scanMappedCheckBox.Name = "scanMappedCheckBox";
            this.scanMappedCheckBox.Size = new System.Drawing.Size(65, 17);
            this.scanMappedCheckBox.TabIndex = 6;
            this.scanMappedCheckBox.Text = "Mapped";
            this.scanMappedCheckBox.UseVisualStyleBackColor = true;
            // 
            // scanImageCheckBox
            // 
            this.scanImageCheckBox.AutoSize = true;
            this.scanImageCheckBox.Location = new System.Drawing.Point(83, 65);
            this.scanImageCheckBox.Name = "scanImageCheckBox";
            this.scanImageCheckBox.Size = new System.Drawing.Size(55, 17);
            this.scanImageCheckBox.TabIndex = 5;
            this.scanImageCheckBox.Text = "Image";
            this.scanImageCheckBox.UseVisualStyleBackColor = true;
            // 
            // scanPrivateCheckBox
            // 
            this.scanPrivateCheckBox.AutoSize = true;
            this.scanPrivateCheckBox.Location = new System.Drawing.Point(6, 65);
            this.scanPrivateCheckBox.Name = "scanPrivateCheckBox";
            this.scanPrivateCheckBox.Size = new System.Drawing.Size(59, 17);
            this.scanPrivateCheckBox.TabIndex = 4;
            this.scanPrivateCheckBox.Text = "Private";
            this.scanPrivateCheckBox.UseVisualStyleBackColor = true;
            // 
            // stopAddressTextBox
            // 
            this.stopAddressTextBox.Location = new System.Drawing.Point(41, 41);
            this.stopAddressTextBox.Name = "stopAddressTextBox";
            this.stopAddressTextBox.Size = new System.Drawing.Size(217, 20);
            this.stopAddressTextBox.TabIndex = 3;
            // 
            // startAddressTextBox
            // 
            this.startAddressTextBox.Location = new System.Drawing.Point(41, 18);
            this.startAddressTextBox.Name = "startAddressTextBox";
            this.startAddressTextBox.Size = new System.Drawing.Size(217, 20);
            this.startAddressTextBox.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Stop: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Start: ";
            // 
            // btnFirstScan
            // 
            this.btnFirstScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFirstScan.Location = new System.Drawing.Point(402, 56);
            this.btnFirstScan.Name = "btnFirstScan";
            this.btnFirstScan.Size = new System.Drawing.Size(80, 23);
            this.btnFirstScan.TabIndex = 8;
            this.btnFirstScan.Text = "First Scan";
            this.btnFirstScan.UseVisualStyleBackColor = true;
            this.btnFirstScan.Click += new System.EventHandler(this.btnFirstScan_Click);
            // 
            // btnNextScan
            // 
            this.btnNextScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextScan.Location = new System.Drawing.Point(488, 56);
            this.btnNextScan.Name = "btnNextScan";
            this.btnNextScan.Size = new System.Drawing.Size(80, 23);
            this.btnNextScan.TabIndex = 9;
            this.btnNextScan.Text = "Next Scan";
            this.btnNextScan.UseVisualStyleBackColor = true;
            this.btnNextScan.Click += new System.EventHandler(this.btnNextScan_Click);
            // 
            // labelFliker
            // 
            this.labelFliker.Location = new System.Drawing.Point(8, 5);
            this.labelFliker.Name = "labelFliker";
            this.labelFliker.Size = new System.Drawing.Size(35, 32);
            this.labelFliker.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::SmScanner.Properties.Resources.proc_chip_256_png;
            this.pictureBox1.Location = new System.Drawing.Point(576, 35);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(97, 58);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // btnCencelScan
            // 
            this.btnCencelScan.Image = global::SmScanner.Properties.Resources.B16x16_Button_Remove;
            this.btnCencelScan.Location = new System.Drawing.Point(402, 31);
            this.btnCencelScan.Name = "btnCencelScan";
            this.btnCencelScan.Size = new System.Drawing.Size(166, 23);
            this.btnCencelScan.TabIndex = 11;
            this.btnCencelScan.Text = "Abort";
            this.btnCencelScan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCencelScan.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCencelScan.UseVisualStyleBackColor = true;
            this.btnCencelScan.Visible = false;
            this.btnCencelScan.Click += new System.EventHandler(this.btnCencelScan_Click);
            // 
            // updateValuesTimer
            // 
            this.updateValuesTimer.Enabled = true;
            this.updateValuesTimer.Interval = 330;
            this.updateValuesTimer.Tick += new System.EventHandler(this.updateValuesTimer_Tick);
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(0, 27);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.btnTransferSelectedResultToAddressList);
            this.splitContainer.Panel1.Controls.Add(this.resultsMemoryRecordList);
            this.splitContainer.Panel1.Controls.Add(this.btnProcessBrowser);
            this.splitContainer.Panel1.Controls.Add(this.progressBar);
            this.splitContainer.Panel1.Controls.Add(this.labelFliker);
            this.splitContainer.Panel1.Controls.Add(this.btnCencelScan);
            this.splitContainer.Panel1.Controls.Add(this.pictureBox1);
            this.splitContainer.Panel1.Controls.Add(this.resultCountLabel);
            this.splitContainer.Panel1.Controls.Add(this.btnNextScan);
            this.splitContainer.Panel1.Controls.Add(this.btnFirstScan);
            this.splitContainer.Panel1.Controls.Add(this.labelProcessSelected);
            this.splitContainer.Panel1.Controls.Add(this.btnClearAddressList);
            this.splitContainer.Panel1.Controls.Add(this.flowLayoutPanel);
            this.splitContainer.Panel1MinSize = 455;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.addressListMemoryRecordList);
            this.splitContainer.Panel2MinSize = 100;
            this.splitContainer.Size = new System.Drawing.Size(688, 666);
            this.splitContainer.SplitterDistance = 499;
            this.splitContainer.TabIndex = 14;
            // 
            // btnTransferSelectedResultToAddressList
            // 
            this.btnTransferSelectedResultToAddressList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTransferSelectedResultToAddressList.Image = global::SmScanner.Properties.Resources.arrows;
            this.btnTransferSelectedResultToAddressList.Location = new System.Drawing.Point(402, 452);
            this.btnTransferSelectedResultToAddressList.Name = "btnTransferSelectedResultToAddressList";
            this.btnTransferSelectedResultToAddressList.Pressed = false;
            this.btnTransferSelectedResultToAddressList.Selected = false;
            this.btnTransferSelectedResultToAddressList.Size = new System.Drawing.Size(24, 22);
            this.btnTransferSelectedResultToAddressList.TabIndex = 15;
            this.btnTransferSelectedResultToAddressList.Click += new System.EventHandler(this.btnTransferSelectedResultToAddressList_Click);
            // 
            // resultsMemoryRecordList
            // 
            this.resultsMemoryRecordList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsMemoryRecordList.ContextMenuStrip = this.resultListContextMenuStrip;
            this.resultsMemoryRecordList.IsResultTable = true;
            this.resultsMemoryRecordList.Location = new System.Drawing.Point(12, 56);
            this.resultsMemoryRecordList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.resultsMemoryRecordList.Name = "resultsMemoryRecordList";
            this.resultsMemoryRecordList.ShowAddressColumn = false;
            this.resultsMemoryRecordList.ShowDescriptionColumn = false;
            this.resultsMemoryRecordList.ShowModuleNameColumn = true;
            this.resultsMemoryRecordList.ShowPreviousValueColumn = true;
            this.resultsMemoryRecordList.ShowValueColumn = true;
            this.resultsMemoryRecordList.ShowValueTypeColumn = false;
            this.resultsMemoryRecordList.Size = new System.Drawing.Size(386, 418);
            this.resultsMemoryRecordList.TabIndex = 13;
            this.resultsMemoryRecordList.RecordDoubleClick += new SmScanner.Controls.MemorySearchResultControlResultDoubleClickEventHandler(this.resultsMemoryRecordList_RecordDoubleClick);
            // 
            // btnProcessBrowser
            // 
            this.btnProcessBrowser.Image = global::SmScanner.Properties.Resources.B32x32_Magnifier;
            this.btnProcessBrowser.Location = new System.Drawing.Point(12, 9);
            this.btnProcessBrowser.Name = "btnProcessBrowser";
            this.btnProcessBrowser.Pressed = false;
            this.btnProcessBrowser.Selected = false;
            this.btnProcessBrowser.Size = new System.Drawing.Size(27, 24);
            this.btnProcessBrowser.TabIndex = 12;
            this.btnProcessBrowser.Click += new System.EventHandler(this.attacthProcessToolStripMenuItem_Click);
            // 
            // btnClearAddressList
            // 
            this.btnClearAddressList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearAddressList.Image = global::SmScanner.Properties.Resources.B16x16_Button_Delete;
            this.btnClearAddressList.Location = new System.Drawing.Point(12, 476);
            this.btnClearAddressList.Name = "btnClearAddressList";
            this.btnClearAddressList.Pressed = false;
            this.btnClearAddressList.Selected = false;
            this.btnClearAddressList.Size = new System.Drawing.Size(20, 20);
            this.btnClearAddressList.TabIndex = 14;
            this.btnClearAddressList.Click += new System.EventHandler(this.btnClearAddressList_Click);
            // 
            // addressListMemoryRecordList
            // 
            this.addressListMemoryRecordList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addressListMemoryRecordList.ContextMenuStrip = this.resultListContextMenuStrip;
            this.addressListMemoryRecordList.IsResultTable = false;
            this.addressListMemoryRecordList.Location = new System.Drawing.Point(12, 3);
            this.addressListMemoryRecordList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.addressListMemoryRecordList.Name = "addressListMemoryRecordList";
            this.addressListMemoryRecordList.ShowAddressColumn = true;
            this.addressListMemoryRecordList.ShowDescriptionColumn = true;
            this.addressListMemoryRecordList.ShowModuleNameColumn = false;
            this.addressListMemoryRecordList.ShowPreviousValueColumn = false;
            this.addressListMemoryRecordList.ShowValueColumn = true;
            this.addressListMemoryRecordList.ShowValueTypeColumn = true;
            this.addressListMemoryRecordList.Size = new System.Drawing.Size(661, 148);
            this.addressListMemoryRecordList.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.processToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(688, 24);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.attacthProcessToolStripMenuItem,
            this.reattachProcessToolStripMenuItem,
            this.detachToolStripMenuItem,
            this.toolStripSeparator,
            this.settingsToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            this.fileToolStripMenuItem.DropDownOpening += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // attacthProcessToolStripMenuItem
            // 
            this.attacthProcessToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Magnifier;
            this.attacthProcessToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.attacthProcessToolStripMenuItem.Name = "attacthProcessToolStripMenuItem";
            this.attacthProcessToolStripMenuItem.ShowShortcutKeys = false;
            this.attacthProcessToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.attacthProcessToolStripMenuItem.Text = "&Attach Process";
            this.attacthProcessToolStripMenuItem.Click += new System.EventHandler(this.attacthProcessToolStripMenuItem_Click);
            // 
            // reattachProcessToolStripMenuItem
            // 
            this.reattachProcessToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Magnifier_Arrow;
            this.reattachProcessToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.reattachProcessToolStripMenuItem.Name = "reattachProcessToolStripMenuItem";
            this.reattachProcessToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.reattachProcessToolStripMenuItem.ShowShortcutKeys = false;
            this.reattachProcessToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.reattachProcessToolStripMenuItem.Text = "&Re-Attach to";
            this.reattachProcessToolStripMenuItem.Click += new System.EventHandler(this.reattachProcessToolStripMenuItem_Click);
            // 
            // detachToolStripMenuItem
            // 
            this.detachToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Magnifier_Remove;
            this.detachToolStripMenuItem.Name = "detachToolStripMenuItem";
            this.detachToolStripMenuItem.ShowShortcutKeys = false;
            this.detachToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.detachToolStripMenuItem.Text = "&Detach";
            this.detachToolStripMenuItem.Click += new System.EventHandler(this.processDetachToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(136, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Cogs;
            this.settingsToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.settingsToolStripMenuItem.ShowShortcutKeys = false;
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.settingsToolStripMenuItem.Text = "&Settings";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(136, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Quit;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShowShortcutKeys = false;
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // processToolStripMenuItem
            // 
            this.processToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processInformationToolStripMenuItem,
            this.processViewMemoryToolStripMenuItem,
            this.toolStripSeparator4,
            this.processResumeToolStripMenuItem,
            this.processSuspendToolStripMenuItem,
            this.processKillToolStripMenuItem});
            this.processToolStripMenuItem.Name = "processToolStripMenuItem";
            this.processToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.processToolStripMenuItem.Text = "&Process";
            this.processToolStripMenuItem.DropDownOpening += new System.EventHandler(this.processToolStripMenuItem_Click);
            // 
            // processInformationToolStripMenuItem
            // 
            this.processInformationToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Category;
            this.processInformationToolStripMenuItem.Name = "processInformationToolStripMenuItem";
            this.processInformationToolStripMenuItem.ShowShortcutKeys = false;
            this.processInformationToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.processInformationToolStripMenuItem.Text = "&Information";
            this.processInformationToolStripMenuItem.Click += new System.EventHandler(this.processInformationToolStripMenuItem_Click);
            // 
            // processViewMemoryToolStripMenuItem
            // 
            this.processViewMemoryToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Matrix_Type;
            this.processViewMemoryToolStripMenuItem.Name = "processViewMemoryToolStripMenuItem";
            this.processViewMemoryToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.processViewMemoryToolStripMenuItem.Text = "&View Memory";
            this.processViewMemoryToolStripMenuItem.Click += new System.EventHandler(this.processViewMemoryToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(134, 6);
            // 
            // processResumeToolStripMenuItem
            // 
            this.processResumeToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Control_Play;
            this.processResumeToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.processResumeToolStripMenuItem.Name = "processResumeToolStripMenuItem";
            this.processResumeToolStripMenuItem.ShowShortcutKeys = false;
            this.processResumeToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.processResumeToolStripMenuItem.Text = "&Resume";
            this.processResumeToolStripMenuItem.Click += new System.EventHandler(this.controlProcessToolStripMenuItem_Click);
            // 
            // processSuspendToolStripMenuItem
            // 
            this.processSuspendToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Control_Pause;
            this.processSuspendToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.processSuspendToolStripMenuItem.Name = "processSuspendToolStripMenuItem";
            this.processSuspendToolStripMenuItem.ShowShortcutKeys = false;
            this.processSuspendToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.processSuspendToolStripMenuItem.Text = "&Suspend";
            this.processSuspendToolStripMenuItem.Click += new System.EventHandler(this.controlProcessToolStripMenuItem_Click);
            // 
            // processKillToolStripMenuItem
            // 
            this.processKillToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Control_Stop;
            this.processKillToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.processKillToolStripMenuItem.Name = "processKillToolStripMenuItem";
            this.processKillToolStripMenuItem.ShowShortcutKeys = false;
            this.processKillToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.processKillToolStripMenuItem.Text = "&Kill";
            this.processKillToolStripMenuItem.Click += new System.EventHandler(this.controlProcessToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::SmScanner.Properties.Resources.B16x16_Information1;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            // 
            // ScannerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 693);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(704, 732);
            this.Name = "ScannerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scanner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScannerForm_FormClosing);
            this.Load += new System.EventHandler(this.ScannerForm_Load);
            this.resultListContextMenuStrip.ResumeLayout(false);
            this.flowLayoutPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.floatingOptionsGroupBox.ResumeLayout(false);
            this.floatingOptionsGroupBox.PerformLayout();
            this.stringOptionsGroupBox.ResumeLayout(false);
            this.stringOptionsGroupBox.PerformLayout();
            this.scanOptionsGroupBox.ResumeLayout(false);
            this.scanOptionsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelProcessSelected;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label resultCountLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox isHexCheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private SmScanner.Forms.ScannerForm.ScanValueTypeComboBox valueTypeComboBox;
        private SmScanner.Forms.ScannerForm.ScanCompareTypeComboBox compareTypeComboBox;
        private System.Windows.Forms.GroupBox floatingOptionsGroupBox;
        private System.Windows.Forms.RadioButton roundTruncateRadioButton;
        private System.Windows.Forms.RadioButton roundLooseRadioButton;
        private System.Windows.Forms.RadioButton roundStrictRadioButton;
        private System.Windows.Forms.GroupBox stringOptionsGroupBox;
        private System.Windows.Forms.CheckBox caseSensitiveCheckBox;
        private System.Windows.Forms.RadioButton encodingUtf32RadioButton;
        private System.Windows.Forms.RadioButton encodingUtf16RadioButton;
        private System.Windows.Forms.RadioButton encodingUtf8RadioButton;
        private System.Windows.Forms.GroupBox scanOptionsGroupBox;
        private System.Windows.Forms.TextBox fastScanAlignmentTextBox;
        private System.Windows.Forms.CheckBox fastScanCheckBox;
        private System.Windows.Forms.CheckBox scanExecutableCheckBox;
        private System.Windows.Forms.CheckBox scanWritableCheckBox;
        private System.Windows.Forms.CheckBox scanMappedCheckBox;
        private System.Windows.Forms.CheckBox scanImageCheckBox;
        private System.Windows.Forms.CheckBox scanPrivateCheckBox;
        private System.Windows.Forms.TextBox stopAddressTextBox;
        private System.Windows.Forms.TextBox startAddressTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnFirstScan;
        private System.Windows.Forms.Button btnNextScan;
        private System.Windows.Forms.Label labelFliker;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnCencelScan;
        private System.Windows.Forms.Timer updateValuesTimer;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCopyAddress;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCopyAddressText;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ContextMenuStrip resultListContextMenuStrip;
        public System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAddSelectedResultsToAddressList;
        public System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRemoveSelectedRecords;
        public System.Windows.Forms.ToolStripMenuItem toolStripMenuItemChangeValue;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemChangeDescription;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reattachProcessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detachToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processInformationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem attacthProcessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processResumeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processSuspendToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processKillToolStripMenuItem;
        private Controls.IconButton btnProcessBrowser;
        private Controls.MemoryRecordList resultsMemoryRecordList;
        private Controls.MemoryRecordList addressListMemoryRecordList;
        private Controls.DualValueBox dualValueBox;
        private Controls.IconButton btnClearAddressList;
        private Controls.IconButton btnTransferSelectedResultToAddressList;
        private System.Windows.Forms.ToolStripMenuItem processViewMemoryToolStripMenuItem;
    }
}