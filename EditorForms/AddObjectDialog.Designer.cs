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
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.btnCreate = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.chbProp = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.CbParConfigName = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cbModelName = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "ObjectType (name)";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(15, 68);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(328, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Object ModelName (select from the list, if empty usually leave empty)";
			// 
			// comboBox1
			// 
			this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBox1.DropDownHeight = 250;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.IntegralHeight = false;
			this.comboBox1.Location = new System.Drawing.Point(15, 36);
			this.comboBox1.MaxDropDownItems = 100;
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(401, 21);
			this.comboBox1.Sorted = true;
			this.comboBox1.TabIndex = 6;
			this.comboBox1.TextChanged += new System.EventHandler(this.comboBox1_TextUpdate);
			// 
			// btnCreate
			// 
			this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnCreate.Enabled = false;
			this.btnCreate.Location = new System.Drawing.Point(330, 364);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.Size = new System.Drawing.Size(86, 23);
			this.btnCreate.TabIndex = 7;
			this.btnCreate.Text = "Ok";
			this.btnCreate.UseVisualStyleBackColor = true;
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(238, 364);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(86, 23);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// tbDescription
			// 
			this.tbDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbDescription.Location = new System.Drawing.Point(12, 159);
			this.tbDescription.Multiline = true;
			this.tbDescription.Name = "tbDescription";
			this.tbDescription.ReadOnly = true;
			this.tbDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbDescription.Size = new System.Drawing.Size(404, 199);
			this.tbDescription.TabIndex = 9;
			this.tbDescription.Text = "No Desription Provided";
			// 
			// chbProp
			// 
			this.chbProp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chbProp.AutoSize = true;
			this.chbProp.Location = new System.Drawing.Point(12, 368);
			this.chbProp.Name = "chbProp";
			this.chbProp.Size = new System.Drawing.Size(192, 17);
			this.chbProp.TabIndex = 10;
			this.chbProp.Text = "Don\'t add object-specific properties";
			this.chbProp.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(15, 116);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(397, 13);
			this.label4.TabIndex = 12;
			this.label4.Text = "Object ParameterConfigName (aka object class, use FixMapParts for stage models)";
			// 
			// CbParConfigName
			// 
			this.CbParConfigName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.CbParConfigName.FormattingEnabled = true;
			this.CbParConfigName.Items.AddRange(new object[] {
            "FixMapParts"});
			this.CbParConfigName.Location = new System.Drawing.Point(15, 132);
			this.CbParConfigName.Name = "CbParConfigName";
			this.CbParConfigName.Size = new System.Drawing.Size(401, 21);
			this.CbParConfigName.TabIndex = 13;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(102, 3);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(241, 13);
			this.label5.TabIndex = 14;
			this.label5.Text = "WARNING: ALL NAMES ARE CASE SENSITIVE";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cbModelName
			// 
			this.cbModelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbModelName.DropDownHeight = 250;
			this.cbModelName.FormattingEnabled = true;
			this.cbModelName.IntegralHeight = false;
			this.cbModelName.Location = new System.Drawing.Point(15, 84);
			this.cbModelName.MaxDropDownItems = 100;
			this.cbModelName.Name = "cbModelName";
			this.cbModelName.Size = new System.Drawing.Size(401, 21);
			this.cbModelName.Sorted = true;
			this.cbModelName.TabIndex = 15;
			// 
			// AddObjectDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(428, 399);
			this.ControlBox = false;
			this.Controls.Add(this.cbModelName);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.CbParConfigName);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.chbProp);
			this.Controls.Add(this.tbDescription);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Name = "AddObjectDialog";
			this.Text = "Add Object";
			this.Load += new System.EventHandler(this.comboBox1_TextUpdate);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox tbDescription;
		private System.Windows.Forms.CheckBox chbProp;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox CbParConfigName;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cbModelName;
	}
}