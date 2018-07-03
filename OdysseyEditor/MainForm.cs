using OdysseyEditor.EditorFroms;
using RedCarpet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OdysseyEditor
{
    public partial class MainForm : Form
    {
        private string[] startUpArgs;
        public MainForm(string[] args)
        {
            startUpArgs = args;
            InitializeComponent();
        }

        void GamePathCheck()
        {
            if (Properties.Settings.Default.GamePath == "" || !Directory.Exists(Properties.Settings.Default.GamePath))
            {
                MessageBox.Show("Select the path of the game, it will be used to display the models from the game");
                btnChangeGamePath.PerformClick();
                MessageBox.Show("You can change it from the tools menu later");
                this.Focus();
            }
        }

        private void btnOpenLevel_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;
            string file = Properties.Settings.Default.GamePath + "StageData\\" + treeView1.SelectedNode.Tag+"Map.szs";

            Console.WriteLine(file);

            if (File.Exists(file))
            {
                new EditorForm(file).Show();
            }
        }

        private void btnOpenFromFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "szs file | *.szs";
            if (openFile.ShowDialog() != DialogResult.OK) return;
            new EditorForm(openFile.FileName).Show();
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnOpenLevel.PerformClick();
        }

        private void btnChangeGamePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string GameFolder = dlg.SelectedPath;
                if (!GameFolder.EndsWith("\\")) GameFolder += "\\";
                Properties.Settings.Default.GamePath = GameFolder;
                Properties.Settings.Default.Save();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GamePathCheck();

            foreach (string file in startUpArgs)
            {
                if (File.Exists(file))
                {
                    if (file.EndsWith("byml") || file.EndsWith("byaml"))
                    {
                        ByamlViewer.OpenByml(file);
                    }
                    else if (file.EndsWith(".szs"))
                    {
                        new EditorForm(file).Show();
                        break;
                    }
                }
            }
        }
    }
}
