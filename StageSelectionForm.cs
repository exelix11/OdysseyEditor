using EditorCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OdysseyExt
{
	public partial class StageSelectionForm : Form 
	{
		public string Result = null;

		public StageSelectionForm()
		{
			InitializeComponent();
		}
		
		private void button2_Click(object sender, EventArgs e)
		{
			Result = null;
			this.Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (treeView1.SelectedNode == null)
			{
				MessageBox.Show("Select a level");
				return;
			}
			Result = treeView1.SelectedNode.Tag as string;
			this.Close();
		}

		private void treeView1_DoubleClick(object sender, EventArgs e)
		{
			if (treeView1.SelectedNode == null)
				return;
			Result = treeView1.SelectedNode.Tag as string;
			this.Close();
		}
	}
}
