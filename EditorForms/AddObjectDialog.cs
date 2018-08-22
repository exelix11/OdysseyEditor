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

namespace OdysseyExt.EditorForms
{
	public partial class AddObjectDialog : Form
	{		
		public ILevelObj Result;
		
		public AddObjectDialog()
		{
			InitializeComponent();		

			comboBox1.Items.Clear();
			comboBox1.Items.AddRange(OdysseyModule.ObjsDB.Keys.ToArray());
		}

		private void comboBox1_TextUpdate(object sender, EventArgs e)
		{
			if (comboBox1.Text == "" || comboBox1.Text.Contains(" "))
			{
				btnCreate.Enabled = false;
				tbDescription.Text = "Not a valid object name.";
			}
			else if (OdysseyModule.ObjsDB.ContainsKey(comboBox1.Text))
			{
				var desc = OdysseyModule.ObjsDB[comboBox1.Text].Description;
				tbDescription.Text = desc == null ? "No Desription Provided." : desc;
				btnCreate.Enabled = true;
			}
			else
			{
				btnCreate.Enabled = true;
				tbDescription.Text = "This Object seems not to be in the game.\r\n" +
									 "You will create an empty object with the common properties only (like Position, Rotation, Scale etc.).";
			}
		}

		private void btnCreate_Click(object sender, EventArgs e)
		{
			if (comboBox1.Text.Trim() == "")
			{
				MessageBox.Show("This name is not valid");
				return;
			}

			if (!chbProp.Checked && OdysseyModule.ObjsDB.ContainsKey(comboBox1.Text))
				Result = OdysseyModule.ObjsDB.MakeObject(comboBox1.Text);
			else
			{
				Result = new LevelObj();
				Result.Name = comboBox1.Text;
			}

			if (tbFileName.Text.Trim() != "")
				Result[LevelObj.N_PlacementFileName] = tbFileName.Text.Trim();

			if (tbModelName.Text.Trim() != "")
				Result[LevelObj.N_ModelName] = tbModelName.Text.Trim();

			Result[LevelObj.N_UnitConfig][LevelObj.N_UnitConfigBaseClass] = ParConfigName.Text == "" ? comboBox1.Text : ParConfigName.Text;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Result = null;
			this.Close();
		}		
	}
}