using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SmScanner.Forms
{
    public partial class HexViewAbout : Form
    {
        public HexViewAbout()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CenterToParent();
        }
    }
}
