using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YGO233
{
    public partial class frmYGO233Main : Form
    {
        public frmYGO233Main()
        {
            InitializeComponent();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
