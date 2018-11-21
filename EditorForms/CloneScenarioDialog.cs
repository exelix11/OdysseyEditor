using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OdysseyExt.EditorForms
{
	public partial class CloneScenarioDialog : Form
	{
		public int[] result = null;
		public bool clone = true;

		class ScenarioString
		{
			public ScenarioString(int i) { Tag = i; Str = "Scenario " + i; }
			public string Str;
			public int Tag;
			public override string ToString() => Str;
		}

		public CloneScenarioDialog(int curScenario, int ScenarioCount)
		{
			InitializeComponent();
			CurScenarioLbl.Text += curScenario.ToString();
			checkedListBox1.Items.Clear();
			for (int i = 0; i < ScenarioCount; i++)
				if (curScenario != i)
					checkedListBox1.Items.Add(new ScenarioString(i));
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Are you sure ?", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
				return;

			result = new int[checkedListBox1.CheckedItems.Count];
			int i = 0;
			foreach (ScenarioString item in checkedListBox1.CheckedItems)
				result[i++] = item.Tag;
			clone = !checkBox1.Checked;
			this.Close();
		}

		private void CloneScenarioDialog_Load(object sender, EventArgs e)
		{

		}

		private void button2_Click(object sender, EventArgs e)
		{
			result = null;
			this.Close();
		}
	}
}
