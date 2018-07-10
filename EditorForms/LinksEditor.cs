using EditorCore;
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

namespace OdysseyExt.EditorFroms
{
    public partial class LinksEditor : Form, IEditorChild
    {
        IDictionary<string, dynamic> LinksNode;
		public EditorForm ParentEditor { get; set; }

		public LinksEditor(dynamic node, EditorForm _parent)
        {
            InitializeComponent();
			ParentEditor = _parent;
			LinksNode = node;
            UpdateTreeView();
        }

        void UpdateTreeView()
        {
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add("Links");
            foreach (string k in LinksNode.Keys)
            {
                var node = treeView1.Nodes[0].Nodes.Add(k);
                node.Tag = k;
                for (int i = 0; i < LinksNode[k].Count; i++)
                {
                    LevelObj obj = new LevelObj(LinksNode[k][i]);
                    node.Nodes.Add(obj.ToString()).Tag = obj;
                }
            }
            treeView1.Nodes[0].Expand();
        }

        private void LinksEditor_Load(object sender, EventArgs e)
        {
            //Application.OpenForms
        }

        private void Double_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            if (treeView1.SelectedNode.Tag is string)
            {
				ParentEditor.EditList(LinksNode[(string)treeView1.SelectedNode.Tag]);
                this.Close();
            }
        }

        private void ContextMenuOpening(object sender, CancelEventArgs e)
        {
            DeleteBtn.Visible = false;
            EditBtn.Visible = false;
            if (treeView1.SelectedNode == null) return;
            if (treeView1.SelectedNode.Tag is string)
            {
                EditBtn.Visible = true;
                DeleteBtn.Visible = true;
            }
            else if (treeView1.SelectedNode.Tag is LevelObj)
            {
                DeleteBtn.Visible = true;
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            if (treeView1.SelectedNode.Tag is string)
            {
                LinksNode.Remove((string)treeView1.SelectedNode.Tag);
            }
            else if (treeView1.SelectedNode.Tag is LevelObj)
            {
                ((IList<dynamic>)LinksNode[(string)treeView1.SelectedNode.Parent.Tag]).Remove(((LevelObj)treeView1.SelectedNode.Tag).Prop);
            }
            UpdateTreeView();
        }

        private void NewListBtn_Click(object sender, EventArgs e)
        {
            string res = "";
            if (InputDialog.Show("List name", "Enter the list name", ref res) == DialogResult.OK && res != "")
            {
                LinksNode.Add(res, new List<dynamic>());
                UpdateTreeView();
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            Double_Click(sender, e);
        }
    }
}
