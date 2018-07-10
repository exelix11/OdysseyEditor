﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OdysseyExt.EditorFroms
{
    public partial class AddObjList : Form
    {
        public string Result = null;
        public AddObjList()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Result = comboBox1.Text;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Result = null;
            Close();
        }
    }
}
