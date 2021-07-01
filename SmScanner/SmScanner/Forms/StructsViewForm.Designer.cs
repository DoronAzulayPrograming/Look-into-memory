
using System;

namespace SmScanner.Forms
{
    partial class StructsViewForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.Int8 = new SmScanner.Controls.IconButton();
            this.Int16 = new SmScanner.Controls.IconButton();
            this.Int32 = new SmScanner.Controls.IconButton();
            this.Int64 = new SmScanner.Controls.IconButton();
            this.UInt64 = new SmScanner.Controls.IconButton();
            this.UInt32 = new SmScanner.Controls.IconButton();
            this.UInt16 = new SmScanner.Controls.IconButton();
            this.UInt8 = new SmScanner.Controls.IconButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.String = new SmScanner.Controls.IconButton();
            this.Double = new SmScanner.Controls.IconButton();
            this.Float = new SmScanner.Controls.IconButton();
            this.Bool = new SmScanner.Controls.IconButton();
            this.Pointer = new SmScanner.Controls.IconButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.newStructToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.structOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.structSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.structDeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.textSizeTextBox = new System.Windows.Forms.TextBox();
            this.BytesArray = new SmScanner.Controls.IconButton();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editRecordValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.valueTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bytes2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bytes4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bytes8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.hexByteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexBytes2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexBytes4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexBytes8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.boolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.floatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doubleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.pointerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.arrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Location = new System.Drawing.Point(12, 55);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(698, 441);
            this.tabControl.TabIndex = 5;
            // 
            // Int8
            // 
            this.Int8.Image = global::SmScanner.Properties.Resources.B16x16_Button_Int_8;
            this.Int8.Location = new System.Drawing.Point(12, 27);
            this.Int8.Name = "Int8";
            this.Int8.Pressed = false;
            this.Int8.Selected = false;
            this.Int8.Size = new System.Drawing.Size(23, 22);
            this.Int8.TabIndex = 1000;
            this.Int8.Click += new System.EventHandler(this.btnSetValueType_Click);
            // 
            // Int16
            // 
            this.Int16.Image = global::SmScanner.Properties.Resources.B16x16_Button_Int_16;
            this.Int16.Location = new System.Drawing.Point(41, 27);
            this.Int16.Name = "Int16";
            this.Int16.Pressed = false;
            this.Int16.Selected = false;
            this.Int16.Size = new System.Drawing.Size(23, 22);
            this.Int16.TabIndex = 1001;
            this.Int16.Click += new System.EventHandler(this.btnSetValueType_Click);
            // 
            // Int32
            // 
            this.Int32.Image = global::SmScanner.Properties.Resources.B16x16_Button_Int_32;
            this.Int32.Location = new System.Drawing.Point(70, 27);
            this.Int32.Name = "Int32";
            this.Int32.Pressed = false;
            this.Int32.Selected = false;
            this.Int32.Size = new System.Drawing.Size(23, 22);
            this.Int32.TabIndex = 1001;
            this.Int32.Click += new System.EventHandler(this.btnSetValueType_Click);
            // 
            // Int64
            // 
            this.Int64.Image = global::SmScanner.Properties.Resources.B16x16_Button_Int_64;
            this.Int64.Location = new System.Drawing.Point(99, 27);
            this.Int64.Name = "Int64";
            this.Int64.Pressed = false;
            this.Int64.Selected = false;
            this.Int64.Size = new System.Drawing.Size(23, 22);
            this.Int64.TabIndex = 1001;
            this.Int64.Click += new System.EventHandler(this.btnSetValueType_Click);
            // 
            // UInt64
            // 
            this.UInt64.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.UInt64.Image = global::SmScanner.Properties.Resources.B16x16_Button_UInt_64;
            this.UInt64.Location = new System.Drawing.Point(222, 27);
            this.UInt64.Name = "UInt64";
            this.UInt64.Pressed = false;
            this.UInt64.Selected = false;
            this.UInt64.Size = new System.Drawing.Size(23, 22);
            this.UInt64.TabIndex = 1003;
            this.UInt64.Click += new System.EventHandler(this.btnSetValueType_Click);
            // 
            // UInt32
            // 
            this.UInt32.Image = global::SmScanner.Properties.Resources.B16x16_Button_UInt_32;
            this.UInt32.Location = new System.Drawing.Point(193, 27);
            this.UInt32.Name = "UInt32";
            this.UInt32.Pressed = false;
            this.UInt32.Selected = false;
            this.UInt32.Size = new System.Drawing.Size(23, 22);
            this.UInt32.TabIndex = 1004;
            this.UInt32.Click += new System.EventHandler(this.btnSetValueType_Click);
            // 
            // UInt16
            // 
            this.UInt16.Image = global::SmScanner.Properties.Resources.B16x16_Button_UInt_16;
            this.UInt16.Location = new System.Drawing.Point(164, 27);
            this.UInt16.Name = "UInt16";
            this.UInt16.Pressed = false;
            this.UInt16.Selected = false;
            this.UInt16.Size = new System.Drawing.Size(23, 22);
            this.UInt16.TabIndex = 1005;
            this.UInt16.Click += new System.EventHandler(this.btnSetValueType_Click);
            // 
            // UInt8
            // 
            this.UInt8.Image = global::SmScanner.Properties.Resources.B16x16_Button_UInt_8;
            this.UInt8.Location = new System.Drawing.Point(135, 27);
            this.UInt8.Name = "UInt8";
            this.UInt8.Pressed = false;
            this.UInt8.Selected = false;
            this.UInt8.Size = new System.Drawing.Size(23, 22);
            this.UInt8.TabIndex = 1002;
            this.UInt8.Click += new System.EventHandler(this.btnSetValueType_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(128, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 21);
            this.panel1.TabIndex = 1006;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(251, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1, 21);
            this.panel2.TabIndex = 1011;
            // 
            // String
            // 
            this.String.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.String.Image = global::SmScanner.Properties.Resources.B16x16_Button_Text;
            this.String.Location = new System.Drawing.Point(345, 27);
            this.String.Name = "String";
            this.String.Pressed = false;
            this.String.Selected = false;
            this.String.Size = new System.Drawing.Size(23, 22);
            this.String.TabIndex = 1008;
            this.String.Click += new System.EventHandler(this.btnSetValueType_Click);
            // 
            // Double
            // 
            this.Double.Image = global::SmScanner.Properties.Resources.B16x16_Button_Double;
            this.Double.Location = new System.Drawing.Point(316, 27);
            this.Double.Name = "Double";
            this.Double.Pressed = false;
            this.Double.Selected = false;
            this.Double.Size = new System.Drawing.Size(23, 22);
            this.Double.TabIndex = 1009;
            this.Double.Click += new System.EventHandler(this.btnSetValueType_Click);
            // 
            // Float
            // 
            this.Float.Image = global::SmScanner.Properties.Resources.B16x16_Button_Float;
            this.Float.Location = new System.Drawing.Point(287, 27);
            this.Float.Name = "Float";
            this.Float.Pressed = false;
            this.Float.Selected = false;
            this.Float.Size = new System.Drawing.Size(23, 22);
            this.Float.TabIndex = 1010;
            this.Float.Click += new System.EventHandler(this.btnSetValueType_Click);
            // 
            // Bool
            // 
            this.Bool.Image = global::SmScanner.Properties.Resources.B16x16_Button_Bool;
            this.Bool.Location = new System.Drawing.Point(258, 27);
            this.Bool.Name = "Bool";
            this.Bool.Pressed = false;
            this.Bool.Selected = false;
            this.Bool.Size = new System.Drawing.Size(23, 22);
            this.Bool.TabIndex = 1007;
            this.Bool.Click += new System.EventHandler(this.btnSetValueType_Click);
            // 
            // Pointer
            // 
            this.Pointer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Pointer.Image = global::SmScanner.Properties.Resources.B16x16_Button_Pointer;
            this.Pointer.Location = new System.Drawing.Point(374, 27);
            this.Pointer.Name = "Pointer";
            this.Pointer.Pressed = false;
            this.Pointer.Selected = false;
            this.Pointer.Size = new System.Drawing.Size(23, 22);
            this.Pointer.TabIndex = 1009;
            this.Pointer.Click += new System.EventHandler(this.btnSetValueType_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newStructToolStripMenuItem,
            this.structOptionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(722, 24);
            this.menuStrip1.TabIndex = 1012;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // newStructToolStripMenuItem
            // 
            this.newStructToolStripMenuItem.Name = "newStructToolStripMenuItem";
            this.newStructToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.newStructToolStripMenuItem.Text = "&New";
            this.newStructToolStripMenuItem.Click += new System.EventHandler(this.newStructToolStripMenuItem_Click);
            // 
            // structOptionsToolStripMenuItem
            // 
            this.structOptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.structSettingsToolStripMenuItem,
            this.structDeleteToolStripMenuItem});
            this.structOptionsToolStripMenuItem.Name = "structOptionsToolStripMenuItem";
            this.structOptionsToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.structOptionsToolStripMenuItem.Text = "&Struct options";
            // 
            // structSettingsToolStripMenuItem
            // 
            this.structSettingsToolStripMenuItem.Name = "structSettingsToolStripMenuItem";
            this.structSettingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.structSettingsToolStripMenuItem.Text = "&Settings";
            this.structSettingsToolStripMenuItem.Click += new System.EventHandler(this.structSettingsToolStripMenuItem_Click);
            // 
            // structDeleteToolStripMenuItem
            // 
            this.structDeleteToolStripMenuItem.Name = "structDeleteToolStripMenuItem";
            this.structDeleteToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.structDeleteToolStripMenuItem.Text = "Delete";
            this.structDeleteToolStripMenuItem.Click += new System.EventHandler(this.structDeleteToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(621, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 15);
            this.label1.TabIndex = 1013;
            this.label1.Text = "Text Size:";
            // 
            // textSizeTextBox
            // 
            this.textSizeTextBox.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textSizeTextBox.Location = new System.Drawing.Point(681, 27);
            this.textSizeTextBox.Name = "textSizeTextBox";
            this.textSizeTextBox.Size = new System.Drawing.Size(29, 22);
            this.textSizeTextBox.TabIndex = 1014;
            // 
            // BytesArray
            // 
            this.BytesArray.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BytesArray.Image = global::SmScanner.Properties.Resources.B16x16_Button_Array;
            this.BytesArray.Location = new System.Drawing.Point(403, 27);
            this.BytesArray.Name = "BytesArray";
            this.BytesArray.Pressed = false;
            this.BytesArray.Selected = false;
            this.BytesArray.Size = new System.Drawing.Size(23, 22);
            this.BytesArray.TabIndex = 1010;
            this.BytesArray.Click += new System.EventHandler(this.btnSetValueType_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editRecordValueToolStripMenuItem,
            this.valueTypesToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(104, 48);
            // 
            // editRecordValueToolStripMenuItem
            // 
            this.editRecordValueToolStripMenuItem.Name = "editRecordValueToolStripMenuItem";
            this.editRecordValueToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.editRecordValueToolStripMenuItem.Text = "&Edit";
            // 
            // valueTypesToolStripMenuItem
            // 
            this.valueTypesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.byteToolStripMenuItem,
            this.bytes2ToolStripMenuItem,
            this.bytes4ToolStripMenuItem,
            this.bytes8ToolStripMenuItem,
            this.toolStripSeparator1,
            this.hexByteToolStripMenuItem,
            this.hexBytes2ToolStripMenuItem,
            this.hexBytes4ToolStripMenuItem,
            this.hexBytes8ToolStripMenuItem,
            this.toolStripSeparator2,
            this.boolToolStripMenuItem,
            this.floatToolStripMenuItem,
            this.doubleToolStripMenuItem,
            this.toolStripSeparator3,
            this.pointerToolStripMenuItem,
            this.arrayToolStripMenuItem,
            this.stringToolStripMenuItem});
            this.valueTypesToolStripMenuItem.Name = "valueTypesToolStripMenuItem";
            this.valueTypesToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.valueTypesToolStripMenuItem.Text = "&Types";
            // 
            // byteToolStripMenuItem
            // 
            this.byteToolStripMenuItem.Name = "byteToolStripMenuItem";
            this.byteToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.byteToolStripMenuItem.Text = "Byte";
            this.byteToolStripMenuItem.Click += new System.EventHandler(this.byteToolStripMenuItem_Click);
            // 
            // bytes2ToolStripMenuItem
            // 
            this.bytes2ToolStripMenuItem.Name = "bytes2ToolStripMenuItem";
            this.bytes2ToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.bytes2ToolStripMenuItem.Text = "2 Bytes";
            this.bytes2ToolStripMenuItem.Click += new System.EventHandler(this.bytes2ToolStripMenuItem_Click);
            // 
            // bytes4ToolStripMenuItem
            // 
            this.bytes4ToolStripMenuItem.Name = "bytes4ToolStripMenuItem";
            this.bytes4ToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.bytes4ToolStripMenuItem.Text = "4 Bytes";
            this.bytes4ToolStripMenuItem.Click += new System.EventHandler(this.bytes4ToolStripMenuItem_Click);
            // 
            // bytes8ToolStripMenuItem
            // 
            this.bytes8ToolStripMenuItem.Name = "bytes8ToolStripMenuItem";
            this.bytes8ToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.bytes8ToolStripMenuItem.Text = "8 Bytes";
            this.bytes8ToolStripMenuItem.Click += new System.EventHandler(this.bytes8ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(140, 6);
            // 
            // hexByteToolStripMenuItem
            // 
            this.hexByteToolStripMenuItem.Name = "hexByteToolStripMenuItem";
            this.hexByteToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.hexByteToolStripMenuItem.Text = "(Hex) Byte";
            this.hexByteToolStripMenuItem.Click += new System.EventHandler(this.hexByteToolStripMenuItem_Click);
            // 
            // hexBytes2ToolStripMenuItem
            // 
            this.hexBytes2ToolStripMenuItem.Name = "hexBytes2ToolStripMenuItem";
            this.hexBytes2ToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.hexBytes2ToolStripMenuItem.Text = "(Hex) 2 Bytes";
            this.hexBytes2ToolStripMenuItem.Click += new System.EventHandler(this.hexBytes2ToolStripMenuItem_Click);
            // 
            // hexBytes4ToolStripMenuItem
            // 
            this.hexBytes4ToolStripMenuItem.Name = "hexBytes4ToolStripMenuItem";
            this.hexBytes4ToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.hexBytes4ToolStripMenuItem.Text = "(Hex) 4 Bytes";
            this.hexBytes4ToolStripMenuItem.Click += new System.EventHandler(this.hexBytes4ToolStripMenuItem_Click);
            // 
            // hexBytes8ToolStripMenuItem
            // 
            this.hexBytes8ToolStripMenuItem.Name = "hexBytes8ToolStripMenuItem";
            this.hexBytes8ToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.hexBytes8ToolStripMenuItem.Text = "(Hex) 8 Bytes";
            this.hexBytes8ToolStripMenuItem.Click += new System.EventHandler(this.hexBytes8ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(140, 6);
            // 
            // boolToolStripMenuItem
            // 
            this.boolToolStripMenuItem.Name = "boolToolStripMenuItem";
            this.boolToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.boolToolStripMenuItem.Text = "Bool";
            this.boolToolStripMenuItem.Click += new System.EventHandler(this.boolToolStripMenuItem_Click);
            // 
            // floatToolStripMenuItem
            // 
            this.floatToolStripMenuItem.Name = "floatToolStripMenuItem";
            this.floatToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.floatToolStripMenuItem.Text = "Float";
            this.floatToolStripMenuItem.Click += new System.EventHandler(this.floatToolStripMenuItem_Click);
            // 
            // doubleToolStripMenuItem
            // 
            this.doubleToolStripMenuItem.Name = "doubleToolStripMenuItem";
            this.doubleToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.doubleToolStripMenuItem.Text = "Double";
            this.doubleToolStripMenuItem.Click += new System.EventHandler(this.doubleToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(140, 6);
            // 
            // pointerToolStripMenuItem
            // 
            this.pointerToolStripMenuItem.Name = "pointerToolStripMenuItem";
            this.pointerToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.pointerToolStripMenuItem.Text = "Pointer:";
            this.pointerToolStripMenuItem.Click += new System.EventHandler(this.pointerToolStripMenuItem_Click);
            // 
            // arrayToolStripMenuItem
            // 
            this.arrayToolStripMenuItem.Name = "arrayToolStripMenuItem";
            this.arrayToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.arrayToolStripMenuItem.Text = "Array:";
            this.arrayToolStripMenuItem.Click += new System.EventHandler(this.arrayToolStripMenuItem_Click);
            // 
            // stringToolStripMenuItem
            // 
            this.stringToolStripMenuItem.Name = "stringToolStripMenuItem";
            this.stringToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.stringToolStripMenuItem.Text = "String:";
            this.stringToolStripMenuItem.Click += new System.EventHandler(this.stringToolStripMenuItem_Click);
            // 
            // StructsViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 508);
            this.Controls.Add(this.BytesArray);
            this.Controls.Add(this.textSizeTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Pointer);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.String);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Double);
            this.Controls.Add(this.UInt64);
            this.Controls.Add(this.Float);
            this.Controls.Add(this.Int64);
            this.Controls.Add(this.Bool);
            this.Controls.Add(this.UInt32);
            this.Controls.Add(this.Int32);
            this.Controls.Add(this.UInt16);
            this.Controls.Add(this.Int16);
            this.Controls.Add(this.UInt8);
            this.Controls.Add(this.Int8);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(738, 547);
            this.Name = "StructsViewForm";
            this.Text = "Structs View";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private Controls.IconButton Int8;
        private Controls.IconButton Int16;
        private Controls.IconButton Int32;
        private Controls.IconButton Int64;
        private Controls.IconButton UInt64;
        private Controls.IconButton UInt32;
        private Controls.IconButton UInt16;
        private Controls.IconButton UInt8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Controls.IconButton String;
        private Controls.IconButton Double;
        private Controls.IconButton Float;
        private Controls.IconButton Bool;
        private Controls.IconButton Pointer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newStructToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem structOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem structSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem structDeleteToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textSizeTextBox;
        private Controls.IconButton BytesArray;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editRecordValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem valueTypesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bytes2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bytes4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bytes8ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem hexByteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hexBytes2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hexBytes4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hexBytes8ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem floatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem doubleToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem arrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pointerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boolToolStripMenuItem;
    }
}