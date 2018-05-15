using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OdysseyEditor
{
    public partial class FrmCredits : Form
    {
        public FrmCredits()
        {
            InitializeComponent();
        }

        private void FrmCredits_Load(object sender, EventArgs e)
        {
            label4.Text = "V. " + Application.ProductVersion;
        }
    }
}
