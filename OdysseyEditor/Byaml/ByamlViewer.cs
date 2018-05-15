using Syroot.NintenTools.Byaml.Dynamic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace RedCarpet
{
    public partial class ByamlViewer : Form
    {
        public dynamic byml;
        public ByamlViewer(dynamic by)
        {
            InitializeComponent();
            byml = by;
            if (byml == null) return;
            //the first node should always be a dictionary node
            if (byml is Dictionary<string, dynamic>)
            {
                parseDictNode(byml, treeView1.Nodes);
            }
            else if (byml is List<dynamic>)
            {
                parseArrayNode(byml, treeView1.Nodes);
            }
        }
        
        void parseDictNode(IDictionary<string, dynamic> node, TreeNodeCollection addto)
        {
            foreach (string k in node.Keys)
            {
                TreeNode current = addto.Add(k);
                if (node[k] is IDictionary<string, dynamic>)
                {
                    current.Text += " : <Dictionary>";
                    current.Tag = node[k]; 
                    current.Nodes.Add("✯✯dummy✯✯"); //a text that can't be in a byml
                }
                else if (node[k] is IList<dynamic>)
                {
                    current.Text += " : <Array>";
                    current.Tag = ((IList<dynamic>)node[k]).ToList();
                    current.Nodes.Add("✯✯dummy✯✯");
                }
                else
                {
                    current.Text = current.Text + " : " + (node[k] == null  ? "<NULL>" : node[k].ToString());
                }
            }
        }

        void parseArrayNode(IList<dynamic> list, TreeNodeCollection addto)
        {
            foreach (dynamic k in list)
            {
                if (k is IDictionary<string, dynamic>)
                {
                    TreeNode current = addto.Add("<Dictionary>");
                    current.Tag = ((IDictionary<string, dynamic>)k);
                    current.Nodes.Add("✯✯dummy✯✯");
                }
                else if (k is IList<dynamic>)
                {
                    TreeNode current = addto.Add("<Array>");
                    current.Tag = ((IList<dynamic>)k).ToList();
                    current.Nodes.Add("✯✯dummy✯✯");
                }
                else
                {
                    addto.Add(k == null ? "<NULL>" : k.ToString());
                }
            }
        }

        private void BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "✯✯dummy✯✯")
            {
                e.Node.Nodes.Clear();
                if (((dynamic)e.Node.Tag).Count == 0)
                {
                    e.Node.Nodes.Add("<Empty>");
                    return;
                }
                if (e.Node.Tag is IList<dynamic>) parseArrayNode((IList<dynamic>)e.Node.Tag, e.Node.Nodes);
                else if (e.Node.Tag is IDictionary<string, dynamic>) parseDictNode((IDictionary<string, dynamic>)e.Node.Tag, e.Node.Nodes);
                else throw new Exception("WTF");
            }
        }

        private void ContextMenuOpening(object sender, CancelEventArgs e)
        {
            CopyNode.Enabled = treeView1.SelectedNode != null;
        }

        private void CopyNode_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(treeView1.SelectedNode.Text);
        }

        private void ByamlViewer_Load(object sender, EventArgs e)
        {

        }

        private void exportJsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.Filter = "json file | *.json";
            if (sav.ShowDialog() == DialogResult.OK)
            {
                var jss = new JavaScriptSerializer();
                var tbyml = jss.Serialize(byml);
                File.WriteAllText(sav.FileName, tbyml);
            }
        }

        public static void ImportFromJson()
        {
            OpenFileDialog opn = new OpenFileDialog();
            opn.Filter = "json file | *.json";
            if (opn.ShowDialog() == DialogResult.OK)
            {
                var jss = new JavaScriptSerializer();
                var byml = jss.Deserialize<dynamic>(File.ReadAllText(opn.FileName));
                new ByamlViewer(byml).Show();
            }            
        }

        public static void OpenByml()
        {
            OpenFileDialog opn = new OpenFileDialog();
            opn.Filter = "byml file | *.byml";
            if (opn.ShowDialog() == DialogResult.OK)
            {
                OpenByml(opn.FileName);
            }
        }

        public static void OpenByml(string Filename)
        {
            var byml = ByamlFile.Load(Filename);
            new ByamlViewer(byml).Show();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.Filter = "byml file | *.byml";
            if (sav.ShowDialog() == DialogResult.OK)
            {
                ByamlFile.Save(sav.FileName, byml);
            }
        }
    }
}
