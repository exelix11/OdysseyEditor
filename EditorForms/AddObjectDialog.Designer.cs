namespace OdysseyExt.EditorForms
{
	partial class AddObjectDialog
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
			this.tbFileName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.tbModelName = new System.Windows.Forms.TextBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.btnCreate = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// tbFileName
			// 
			this.tbFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbFileName.Enabled = false;
			this.tbFileName.Location = new System.Drawing.Point(12, 84);
			this.tbFileName.Name = "tbFileName";
			this.tbFileName.Size = new System.Drawing.Size(394, 20);
			this.tbFileName.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(62, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "ObjectType";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 68);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(256, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Object FileName (not many object types support that)";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 122);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(246, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Object ModelName (not many objects support that)";
			// 
			// tbModelName
			// 
			this.tbModelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbModelName.Enabled = false;
			this.tbModelName.Location = new System.Drawing.Point(12, 138);
			this.tbModelName.Name = "tbModelName";
			this.tbModelName.Size = new System.Drawing.Size(394, 20);
			this.tbModelName.TabIndex = 4;
			// 
			// comboBox1
			// 
			this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBox1.DropDownHeight = 250;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.IntegralHeight = false;
			this.comboBox1.Location = new System.Drawing.Point(15, 26);
			this.comboBox1.MaxDropDownItems = 100;
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(391, 21);
			this.comboBox1.Sorted = true;
			this.comboBox1.TabIndex = 6;
			this.comboBox1.TextUpdate += new System.EventHandler(this.comboBox1_TextUpdate);
			this.comboBox1.SelectedValueChanged += new System.EventHandler(this.comboBox1_TextUpdate);
			this.comboBox1.TextChanged += new System.EventHandler(this.comboBox1_TextUpdate);
			// 
			// btnCreate
			// 
			this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnCreate.Location = new System.Drawing.Point(320, 324);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.Size = new System.Drawing.Size(86, 23);
			this.btnCreate.TabIndex = 7;
			this.btnCreate.Text = "Create Custom";
			this.btnCreate.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(228, 324);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(86, 23);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// tbDescription
			// 
			this.tbDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbDescription.Location = new System.Drawing.Point(12, 182);
			this.tbDescription.Multiline = true;
			this.tbDescription.Name = "tbDescription";
			this.tbDescription.ReadOnly = true;
			this.tbDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbDescription.Size = new System.Drawing.Size(394, 115);
			this.tbDescription.TabIndex = 9;
			this.tbDescription.Text = "No Desription Provided";
			// 
			// AddObjectDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(418, 359);
			this.Controls.Add(this.tbDescription);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.tbModelName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbFileName);
			this.Name = "AddObjectDialog";
			this.Text = "Add Object";
			this.Load += new System.EventHandler(this.comboBox1_TextUpdate);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox tbFileName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbModelName;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox tbDescription;
	}
}