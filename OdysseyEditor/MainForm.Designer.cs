namespace OdysseyEditor
{
    partial class MainForm
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
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Cap Kingdom");
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Cascade Kingdom");
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("MoEye moving Platform");
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Challenge Subrooms", new System.Windows.Forms.TreeNode[] {
            treeNode28});
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("Pyramid(Starting Area)");
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("Pyramid(Bullet Bill Parkour)");
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("Ice Underground before Boss");
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("Ice Underground Boss");
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("Sand Kindom", new System.Windows.Forms.TreeNode[] {
            treeNode29,
            treeNode30,
            treeNode31,
            treeNode32,
            treeNode33});
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("LakeKingdom (Town Area)");
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("Lake Kingdom(Starting Area)", new System.Windows.Forms.TreeNode[] {
            treeNode35});
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("Wodded Kingdom");
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("Cloud Kingdom(1. Bowser Fight)");
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("Lost Kingdom");
            System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("Theater (smb 1-1)");
            System.Windows.Forms.TreeNode treeNode41 = new System.Windows.Forms.TreeNode("Metro Kingdom", new System.Windows.Forms.TreeNode[] {
            treeNode40});
            System.Windows.Forms.TreeNode treeNode42 = new System.Windows.Forms.TreeNode("Snow Kingdom");
            System.Windows.Forms.TreeNode treeNode43 = new System.Windows.Forms.TreeNode("Seaside Kingdom");
            System.Windows.Forms.TreeNode treeNode44 = new System.Windows.Forms.TreeNode("Luncheon Kingdom");
            System.Windows.Forms.TreeNode treeNode45 = new System.Windows.Forms.TreeNode("Ruined Kingdom");
            System.Windows.Forms.TreeNode treeNode46 = new System.Windows.Forms.TreeNode("Bowser\'s Kingdom");
            System.Windows.Forms.TreeNode treeNode47 = new System.Windows.Forms.TreeNode("Moon Kingdom");
            System.Windows.Forms.TreeNode treeNode48 = new System.Windows.Forms.TreeNode("Mushroom Kingdom");
            System.Windows.Forms.TreeNode treeNode49 = new System.Windows.Forms.TreeNode("Dark Side of the Moon");
            System.Windows.Forms.TreeNode treeNode50 = new System.Windows.Forms.TreeNode("Darker Side of the Moon");
            this.btnOpenLevel = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.btnOpenFromFile = new System.Windows.Forms.Button();
            this.btnChangeGamePath = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOpenLevel
            // 
            this.btnOpenLevel.Location = new System.Drawing.Point(12, 12);
            this.btnOpenLevel.Name = "btnOpenLevel";
            this.btnOpenLevel.Size = new System.Drawing.Size(75, 23);
            this.btnOpenLevel.TabIndex = 0;
            this.btnOpenLevel.Text = "Open Level";
            this.btnOpenLevel.UseVisualStyleBackColor = true;
            this.btnOpenLevel.Click += new System.EventHandler(this.btnOpenLevel_Click);
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.treeView1.Location = new System.Drawing.Point(0, 53);
            this.treeView1.Name = "treeView1";
            treeNode26.Name = "Node0";
            treeNode26.Tag = "CapWorldHomeStage";
            treeNode26.Text = "Cap Kingdom";
            treeNode27.Name = "Node1";
            treeNode27.Tag = "WaterfallWorldHomeStage";
            treeNode27.Text = "Cascade Kingdom";
            treeNode28.Name = "Node4";
            treeNode28.Tag = "MeganeLiftExStage";
            treeNode28.Text = "MoEye moving Platform";
            treeNode29.Name = "Node3";
            treeNode29.Text = "Challenge Subrooms";
            treeNode30.Name = "Node7";
            treeNode30.Tag = "SandWorldPyramid000Stage";
            treeNode30.Text = "Pyramid(Starting Area)";
            treeNode31.Name = "Node8";
            treeNode31.Tag = "SandWorldPyramid001Stage";
            treeNode31.Text = "Pyramid(Bullet Bill Parkour)";
            treeNode32.Name = "Node9";
            treeNode32.Tag = "SandWorldUnderground000Stage";
            treeNode32.Text = "Ice Underground before Boss";
            treeNode33.Name = "Node10";
            treeNode33.Tag = "SandWorldUnderground001Stage";
            treeNode33.Text = "Ice Underground Boss";
            treeNode34.Name = "Node2";
            treeNode34.Tag = "SandWorldHomeStage";
            treeNode34.Text = "Sand Kindom";
            treeNode35.Name = "Node0";
            treeNode35.Tag = "LakeWorldTownZone";
            treeNode35.Text = "LakeKingdom (Town Area)";
            treeNode36.Name = "Node3";
            treeNode36.Tag = "LakeWorldHomeStage";
            treeNode36.Text = "Lake Kingdom(Starting Area)";
            treeNode37.Name = "Node4";
            treeNode37.Tag = "ForestWorldHomeStage";
            treeNode37.Text = "Wodded Kingdom";
            treeNode38.Name = "Node5";
            treeNode38.Tag = "DemoCrashHomeFallStage";
            treeNode38.Text = "Cloud Kingdom(1. Bowser Fight)";
            treeNode39.Name = "Node6";
            treeNode39.Tag = "ClashWorldHomeStage";
            treeNode39.Text = "Lost Kingdom";
            treeNode40.Name = "Node1";
            treeNode40.Tag = "Theater2DExStage";
            treeNode40.Text = "Theater (smb 1-1)";
            treeNode41.Name = "Node7";
            treeNode41.Tag = "CityWorldHomeStage";
            treeNode41.Text = "Metro Kingdom";
            treeNode42.Name = "Node8";
            treeNode42.Tag = "SnowWorldHomeStage";
            treeNode42.Text = "Snow Kingdom";
            treeNode43.Name = "Node9";
            treeNode43.Tag = "SeaWorldHomeStage";
            treeNode43.Text = "Seaside Kingdom";
            treeNode44.Name = "Node10";
            treeNode44.Tag = "LavaWorldHomeStage";
            treeNode44.Text = "Luncheon Kingdom";
            treeNode45.Name = "Node11";
            treeNode45.Tag = "BossRaidWorldHomeStage";
            treeNode45.Text = "Ruined Kingdom";
            treeNode46.Name = "Node12";
            treeNode46.Tag = "SkyWorldHomeStage";
            treeNode46.Text = "Bowser\'s Kingdom";
            treeNode47.Name = "Node13";
            treeNode47.Tag = "MoonWorldHomeStage";
            treeNode47.Text = "Moon Kingdom";
            treeNode48.Name = "Node14";
            treeNode48.Tag = "PeachWorldHomeStage";
            treeNode48.Text = "Mushroom Kingdom";
            treeNode49.Name = "Node15";
            treeNode49.Tag = "Special1WorldHomeStage";
            treeNode49.Text = "Dark Side of the Moon";
            treeNode50.Name = "Node16";
            treeNode50.Tag = "Special2WorldHomeStage";
            treeNode50.Text = "Darker Side of the Moon";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode26,
            treeNode27,
            treeNode34,
            treeNode36,
            treeNode37,
            treeNode38,
            treeNode39,
            treeNode41,
            treeNode42,
            treeNode43,
            treeNode44,
            treeNode45,
            treeNode46,
            treeNode47,
            treeNode48,
            treeNode49,
            treeNode50});
            this.treeView1.Size = new System.Drawing.Size(646, 610);
            this.treeView1.TabIndex = 1;
            this.treeView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDoubleClick);
            // 
            // btnOpenFromFile
            // 
            this.btnOpenFromFile.Location = new System.Drawing.Point(93, 12);
            this.btnOpenFromFile.Name = "btnOpenFromFile";
            this.btnOpenFromFile.Size = new System.Drawing.Size(115, 23);
            this.btnOpenFromFile.TabIndex = 2;
            this.btnOpenFromFile.Text = "Open Level From File";
            this.btnOpenFromFile.UseVisualStyleBackColor = true;
            this.btnOpenFromFile.Click += new System.EventHandler(this.btnOpenFromFile_Click);
            // 
            // btnChangeGamePath
            // 
            this.btnChangeGamePath.Location = new System.Drawing.Point(215, 12);
            this.btnChangeGamePath.Name = "btnChangeGamePath";
            this.btnChangeGamePath.Size = new System.Drawing.Size(122, 23);
            this.btnChangeGamePath.TabIndex = 3;
            this.btnChangeGamePath.Text = "Change Game Path";
            this.btnChangeGamePath.UseVisualStyleBackColor = true;
            this.btnChangeGamePath.Click += new System.EventHandler(this.btnChangeGamePath_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 663);
            this.Controls.Add(this.btnChangeGamePath);
            this.Controls.Add(this.btnOpenFromFile);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.btnOpenLevel);
            this.Name = "MainForm";
            this.Text = "Odyssey Editor";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpenLevel;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button btnOpenFromFile;
        private System.Windows.Forms.Button btnChangeGamePath;
    }
}