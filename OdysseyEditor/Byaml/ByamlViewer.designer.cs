namespace RedCarpet
{
    partial class ByamlViewer
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
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.CopyNode = new System.Windows.Forms.ToolStripMenuItem();
			this.editValueNodeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportJsonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(284, 261);
			this.treeView1.TabIndex = 0;
			this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.BeforeExpand);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopyNode,
            this.addNodeToolStripMenuItem,
            this.editValueNodeMenuItem,
            this.deleteNodeToolStripMenuItem,
            this.exportJsonToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveAsToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(181, 164);
			this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuOpening);
			// 
			// CopyNode
			// 
			this.CopyNode.Name = "CopyNode";
			this.CopyNode.Size = new System.Drawing.Size(168, 22);
			this.CopyNode.Text = "Copy node as text";
			this.CopyNode.Click += new System.EventHandler(this.CopyNode_Click);
			// 
			// editValueNodeMenuItem
			// 
			this.editValueNodeMenuItem.Name = "editValueNodeMenuItem";
			this.editValueNodeMenuItem.Size = new System.Drawing.Size(168, 22);
			this.editValueNodeMenuItem.Text = "Edit value";
			this.editValueNodeMenuItem.Click += new System.EventHandler(this.editValueNodeMenuItem_Click);
			// 
			// exportJsonToolStripMenuItem
			// 
			this.exportJsonToolStripMenuItem.Name = "exportJsonToolStripMenuItem";
			this.exportJsonToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
			this.exportJsonToolStripMenuItem.Text = "Export json";
			this.exportJsonToolStripMenuItem.Visible = false;
			this.exportJsonToolStripMenuItem.Click += new System.EventHandler(this.exportJsonToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(165, 6);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
			this.saveAsToolStripMenuItem.Text = "Save as..";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// addNodeToolStripMenuItem
			// 
			this.addNodeToolStripMenuItem.Name = "addNodeToolStripMenuItem";
			this.addNodeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.addNodeToolStripMenuItem.Text = "Add Node";
			this.addNodeToolStripMenuItem.Click += new System.EventHandler(this.addNodeToolStripMenuItem_Click);
			// 
			// deleteNodeToolStripMenuItem
			// 
			this.deleteNodeToolStripMenuItem.Name = "deleteNodeToolStripMenuItem";
			this.deleteNodeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.deleteNodeToolStripMenuItem.Text = "Delete node";
			this.deleteNodeToolStripMenuItem.Click += new System.EventHandler(this.deleteNodeToolStripMenuItem_Click);
			// 
			// ByamlViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.treeView1);
			this.Name = "ByamlViewer";
			this.Text = "ByamlEditor";
			this.Load += new System.EventHandler(this.ByamlViewer_Load);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem CopyNode;
        private System.Windows.Forms.ToolStripMenuItem exportJsonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editValueNodeMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem addNodeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteNodeToolStripMenuItem;
	}
}