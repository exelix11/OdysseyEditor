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
		Dictionary<string, dynamic> baseDB;

		dynamic currenteEntry;

		public string ObjectName => comboBox1.Text;
		public string FileName => (currenteEntry!=null &&currenteEntry["isBase"]) ? tbFileName.Text: comboBox1.Text;
		public string ModelName => (currenteEntry != null && currenteEntry["allowsModels"]) ? tbModelName.Text : null;
		public AddObjectDialog()
		{
			InitializeComponent();
			var jss = new System.Web.Script.Serialization.JavaScriptSerializer();

			baseDB = (jss.Deserialize(System.IO.File.ReadAllText("objParamDump.json"), null) as Dictionary<string, dynamic>);

			comboBox1.Items.Clear();

			foreach(string key in baseDB.Keys)
			{
				comboBox1.Items.Add(key);
			}
		}

		private void comboBox1_TextUpdate(object sender, EventArgs e)
		{
			if(comboBox1.Text == "" || comboBox1.Text.Contains(" "))
			{
				currenteEntry = null;
				btnCreate.Enabled = false;
				tbDescription.Text = "Not a valid object name.";
			}
			else if (baseDB.ContainsKey(comboBox1.Text))
			{
				btnCreate.Enabled = true;
				btnCreate.Text = "Create";
				currenteEntry = baseDB[comboBox1.Text];
				tbFileName.Enabled = currenteEntry["isBase"];
				tbModelName.Enabled = currenteEntry["allowsModels"];
				tbDescription.Text = "No Desription Provided.";
			}
			else
			{
				currenteEntry = null;
				btnCreate.Enabled = true;
				btnCreate.Text = "Create Custom";

				tbFileName.Enabled = true;
				tbModelName.Enabled = true;

				tbDescription.Text = "This Object seems not to be in the game.\n" +
									 "You will create an empty object with the common properties only (like Position, Rotation, Scale etc.).";
			}
		}
	}
}
