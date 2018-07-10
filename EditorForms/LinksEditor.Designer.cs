namespace OdysseyExt.EditorFroms
{
    partial class LinksEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Links");
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.NewListBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.EditBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "LinksRoot";
            treeNode1.Text = "Links";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeView1.ShowRootLines = false;
            this.treeView1.Size = new System.Drawing.Size(252, 273);
            this.treeView1.TabIndex = 5;
            this.treeView1.DoubleClick += new System.EventHandler(this.Double_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteBtn,
            this.NewListBtn,
            this.EditBtn});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 92);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuOpening);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(152, 22);
            this.DeleteBtn.Text = "Delete";
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // NewListBtn
            // 
            this.NewListBtn.Name = "NewListBtn";
            this.NewListBtn.Size = new System.Drawing.Size(152, 22);
            this.NewListBtn.Text = "New list";
            this.NewListBtn.Click += new System.EventHandler(this.NewListBtn_Click);
            // 
            // EditBtn
            // 
            this.EditBtn.Name = "EditBtn";
            this.EditBtn.Size = new System.Drawing.Size(152, 22);
            this.EditBtn.Text = "Edit";
            this.EditBtn.Click += new System.EventHandler(this.EditBtn_Click);
            // 
            // LinksEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 273);
            this.Controls.Add(this.treeView1);
            this.Name = "LinksEditor";
            this.Text = "LinksEditor";
            this.Load += new System.EventHandler(this.LinksEditor_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem DeleteBtn;
        private System.Windows.Forms.ToolStripMenuItem NewListBtn;
        private System.Windows.Forms.ToolStripMenuItem EditBtn;
    }
}