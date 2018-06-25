namespace OdysseyEditor
{
    partial class Settings
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
			this.SettingsPanel = new System.Windows.Forms.Panel();
			this.drawDistance = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.btn_url_Default = new System.Windows.Forms.Button();
			this.ChbStartupUpdate = new System.Windows.Forms.CheckBox();
			this.label11 = new System.Windows.Forms.Label();
			this.tbUrl = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.ChbStartupDb = new System.Windows.Forms.CheckBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.RotSenUpDown = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.ZoomSenUpDown = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.cbCameraMode = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.ChbDebugInfo = new System.Windows.Forms.CheckBox();
			this.ChbTriCount = new System.Windows.Forms.CheckBox();
			this.ChbFps = new System.Windows.Forms.CheckBox();
			this.CamInertiaUpDown = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.SettingsPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.drawDistance)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.RotSenUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ZoomSenUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.CamInertiaUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// SettingsPanel
			// 
			this.SettingsPanel.BackColor = System.Drawing.SystemColors.ControlDark;
			this.SettingsPanel.Controls.Add(this.drawDistance);
			this.SettingsPanel.Controls.Add(this.label1);
			this.SettingsPanel.Controls.Add(this.btn_url_Default);
			this.SettingsPanel.Controls.Add(this.ChbStartupUpdate);
			this.SettingsPanel.Controls.Add(this.label11);
			this.SettingsPanel.Controls.Add(this.tbUrl);
			this.SettingsPanel.Controls.Add(this.label10);
			this.SettingsPanel.Controls.Add(this.ChbStartupDb);
			this.SettingsPanel.Controls.Add(this.label9);
			this.SettingsPanel.Controls.Add(this.label8);
			this.SettingsPanel.Controls.Add(this.RotSenUpDown);
			this.SettingsPanel.Controls.Add(this.label7);
			this.SettingsPanel.Controls.Add(this.ZoomSenUpDown);
			this.SettingsPanel.Controls.Add(this.label6);
			this.SettingsPanel.Controls.Add(this.cbCameraMode);
			this.SettingsPanel.Controls.Add(this.label5);
			this.SettingsPanel.Controls.Add(this.ChbDebugInfo);
			this.SettingsPanel.Controls.Add(this.ChbTriCount);
			this.SettingsPanel.Controls.Add(this.ChbFps);
			this.SettingsPanel.Controls.Add(this.CamInertiaUpDown);
			this.SettingsPanel.Controls.Add(this.label4);
			this.SettingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SettingsPanel.Location = new System.Drawing.Point(0, 0);
			this.SettingsPanel.Name = "SettingsPanel";
			this.SettingsPanel.Size = new System.Drawing.Size(397, 420);
			this.SettingsPanel.TabIndex = 18;
			this.SettingsPanel.Visible = false;
			// 
			// drawDistance
			// 
			this.drawDistance.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.drawDistance.Location = new System.Drawing.Point(313, 141);
			this.drawDistance.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.drawDistance.Name = "drawDistance";
			this.drawDistance.Size = new System.Drawing.Size(75, 20);
			this.drawDistance.TabIndex = 22;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(26, 135);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(89, 26);
			this.label1.TabIndex = 21;
			this.label1.Text = "Draw distance\r\nDefault 0 (Infinity)";
			// 
			// btn_url_Default
			// 
			this.btn_url_Default.Location = new System.Drawing.Point(339, 326);
			this.btn_url_Default.Name = "btn_url_Default";
			this.btn_url_Default.Size = new System.Drawing.Size(57, 23);
			this.btn_url_Default.TabIndex = 20;
			this.btn_url_Default.Text = "Default";
			this.btn_url_Default.UseVisualStyleBackColor = true;
			this.btn_url_Default.Visible = false;
			// 
			// ChbStartupUpdate
			// 
			this.ChbStartupUpdate.AutoSize = true;
			this.ChbStartupUpdate.Location = new System.Drawing.Point(21, 290);
			this.ChbStartupUpdate.Name = "ChbStartupUpdate";
			this.ChbStartupUpdate.Size = new System.Drawing.Size(163, 17);
			this.ChbStartupUpdate.TabIndex = 19;
			this.ChbStartupUpdate.Text = "Check for updates on startup";
			this.ChbStartupUpdate.UseVisualStyleBackColor = true;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(22, 351);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(347, 39);
			this.label11.TabIndex = 18;
			this.label11.Text = "this is both for startup update and manual update from help -> download \r\nlatest " +
    "objectsDb\r\nif this is empty the database will be downloaded from the github repo" +
    "";
			this.label11.Visible = false;
			// 
			// tbUrl
			// 
			this.tbUrl.Location = new System.Drawing.Point(141, 329);
			this.tbUrl.Name = "tbUrl";
			this.tbUrl.Size = new System.Drawing.Size(192, 20);
			this.tbUrl.TabIndex = 17;
			this.tbUrl.Visible = false;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(11, 332);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(124, 13);
			this.label10.TabIndex = 16;
			this.label10.Text = "Database download link:";
			this.label10.Visible = false;
			// 
			// ChbStartupDb
			// 
			this.ChbStartupDb.AutoSize = true;
			this.ChbStartupDb.Location = new System.Drawing.Point(21, 312);
			this.ChbStartupDb.Name = "ChbStartupDb";
			this.ChbStartupDb.Size = new System.Drawing.Size(299, 17);
			this.ChbStartupDb.TabIndex = 15;
			this.ChbStartupDb.Text = "Automatically download latest objects database on startup";
			this.ChbStartupDb.UseVisualStyleBackColor = true;
			this.ChbStartupDb.Visible = false;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(3, 274);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(92, 13);
			this.label9.TabIndex = 14;
			this.label9.Text = "Editor settings:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(3, 6);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(105, 13);
			this.label8.TabIndex = 13;
			this.label8.Text = "3D view settings:";
			// 
			// RotSenUpDown
			// 
			this.RotSenUpDown.DecimalPlaces = 2;
			this.RotSenUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.RotSenUpDown.Location = new System.Drawing.Point(313, 107);
			this.RotSenUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.RotSenUpDown.Name = "RotSenUpDown";
			this.RotSenUpDown.Size = new System.Drawing.Size(75, 20);
			this.RotSenUpDown.TabIndex = 11;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(26, 101);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(95, 26);
			this.label7.TabIndex = 10;
			this.label7.Text = "Rotation sensitivity\r\nDefault: 1";
			// 
			// ZoomSenUpDown
			// 
			this.ZoomSenUpDown.DecimalPlaces = 2;
			this.ZoomSenUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this.ZoomSenUpDown.Location = new System.Drawing.Point(313, 70);
			this.ZoomSenUpDown.Name = "ZoomSenUpDown";
			this.ZoomSenUpDown.Size = new System.Drawing.Size(75, 20);
			this.ZoomSenUpDown.TabIndex = 9;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(26, 70);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(85, 26);
			this.label6.TabIndex = 8;
			this.label6.Text = "Zoom sensitivity \r\nDefault: 2";
			// 
			// cbCameraMode
			// 
			this.cbCameraMode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.cbCameraMode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cbCameraMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbCameraMode.FormattingEnabled = true;
			this.cbCameraMode.Items.AddRange(new object[] {
            "Inspect",
            "Walkaround"});
			this.cbCameraMode.Location = new System.Drawing.Point(252, 171);
			this.cbCameraMode.Name = "cbCameraMode";
			this.cbCameraMode.Size = new System.Drawing.Size(136, 21);
			this.cbCameraMode.TabIndex = 7;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(26, 174);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(75, 13);
			this.label5.TabIndex = 5;
			this.label5.Text = "Camera mode:";
			// 
			// ChbDebugInfo
			// 
			this.ChbDebugInfo.AutoSize = true;
			this.ChbDebugInfo.Location = new System.Drawing.Point(187, 209);
			this.ChbDebugInfo.Name = "ChbDebugInfo";
			this.ChbDebugInfo.Size = new System.Drawing.Size(144, 17);
			this.ChbDebugInfo.TabIndex = 4;
			this.ChbDebugInfo.Text = "Show camera debug info";
			this.ChbDebugInfo.UseVisualStyleBackColor = true;
			// 
			// ChbTriCount
			// 
			this.ChbTriCount.AutoSize = true;
			this.ChbTriCount.Location = new System.Drawing.Point(22, 232);
			this.ChbTriCount.Name = "ChbTriCount";
			this.ChbTriCount.Size = new System.Drawing.Size(120, 17);
			this.ChbTriCount.TabIndex = 3;
			this.ChbTriCount.Text = "Show triangle count";
			this.ChbTriCount.UseVisualStyleBackColor = true;
			// 
			// ChbFps
			// 
			this.ChbFps.AutoSize = true;
			this.ChbFps.Location = new System.Drawing.Point(22, 209);
			this.ChbFps.Name = "ChbFps";
			this.ChbFps.Size = new System.Drawing.Size(76, 17);
			this.ChbFps.TabIndex = 2;
			this.ChbFps.Text = "Show FPS";
			this.ChbFps.UseVisualStyleBackColor = true;
			// 
			// CamInertiaUpDown
			// 
			this.CamInertiaUpDown.DecimalPlaces = 2;
			this.CamInertiaUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this.CamInertiaUpDown.Location = new System.Drawing.Point(313, 34);
			this.CamInertiaUpDown.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            131072});
			this.CamInertiaUpDown.Name = "CamInertiaUpDown";
			this.CamInertiaUpDown.Size = new System.Drawing.Size(75, 20);
			this.CamInertiaUpDown.TabIndex = 1;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(19, 34);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(271, 26);
			this.label4.TabIndex = 0;
			this.label4.Text = "Camera inertia factor:\r\n (This controls how much the camera slips) Default: 0,92";
			// 
			// Settings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(397, 420);
			this.Controls.Add(this.SettingsPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Settings";
			this.Text = "Settings";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_closing);
			this.Load += new System.EventHandler(this.Settings_Load);
			this.SettingsPanel.ResumeLayout(false);
			this.SettingsPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.drawDistance)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.RotSenUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ZoomSenUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.CamInertiaUpDown)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel SettingsPanel;
        private System.Windows.Forms.Button btn_url_Default;
        private System.Windows.Forms.CheckBox ChbStartupUpdate;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbUrl;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox ChbStartupDb;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown RotSenUpDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown ZoomSenUpDown;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.ComboBox cbCameraMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox ChbDebugInfo;
        private System.Windows.Forms.CheckBox ChbTriCount;
        private System.Windows.Forms.CheckBox ChbFps;
        private System.Windows.Forms.NumericUpDown CamInertiaUpDown;
        private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown drawDistance;
		private System.Windows.Forms.Label label1;
	}
}