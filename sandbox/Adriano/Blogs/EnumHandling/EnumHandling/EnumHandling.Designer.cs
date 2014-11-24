namespace EnumHandling
{
	partial class EnumHandling
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnumHandling));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cmdClose = new System.Windows.Forms.Button();
			this.cmdRun = new System.Windows.Forms.Button();
			this.db4oVersion1 = new System.Windows.Forms.ComboBox();
			this.db4oVersion2 = new System.Windows.Forms.ComboBox();
			this.comboEnumValues = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.txtResult = new System.Windows.Forms.TextBox();
			this.txtResult2 = new System.Windows.Forms.TextBox();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 256);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Db4o version";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 310);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(140, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Db4o version to run against";
			// 
			// cmdClose
			// 
			this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdClose.Location = new System.Drawing.Point(625, 392);
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size(75, 23);
			this.cmdClose.TabIndex = 7;
			this.cmdClose.Text = "&Close";
			this.cmdClose.UseVisualStyleBackColor = true;
			this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
			// 
			// cmdRun
			// 
			this.cmdRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdRun.Location = new System.Drawing.Point(12, 392);
			this.cmdRun.Name = "cmdRun";
			this.cmdRun.Size = new System.Drawing.Size(75, 23);
			this.cmdRun.TabIndex = 8;
			this.cmdRun.Text = "&Run Tests";
			this.cmdRun.UseVisualStyleBackColor = true;
			this.cmdRun.Click += new System.EventHandler(this.cmdRun_Click);
			// 
			// db4oVersion1
			// 
			this.db4oVersion1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.db4oVersion1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.db4oVersion1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.db4oVersion1.FormattingEnabled = true;
			this.db4oVersion1.Location = new System.Drawing.Point(12, 272);
			this.db4oVersion1.Name = "db4oVersion1";
			this.db4oVersion1.Size = new System.Drawing.Size(688, 24);
			this.db4oVersion1.TabIndex = 10;
			// 
			// db4oVersion2
			// 
			this.db4oVersion2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.db4oVersion2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.db4oVersion2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.db4oVersion2.FormattingEnabled = true;
			this.db4oVersion2.Location = new System.Drawing.Point(12, 327);
			this.db4oVersion2.Name = "db4oVersion2";
			this.db4oVersion2.Size = new System.Drawing.Size(688, 24);
			this.db4oVersion2.TabIndex = 11;
			// 
			// comboEnumValues
			// 
			this.comboEnumValues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.comboEnumValues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboEnumValues.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.comboEnumValues.FormattingEnabled = true;
			this.comboEnumValues.Location = new System.Drawing.Point(148, 362);
			this.comboEnumValues.Name = "comboEnumValues";
			this.comboEnumValues.Size = new System.Drawing.Size(152, 21);
			this.comboEnumValues.TabIndex = 12;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 365);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(127, 13);
			this.label3.TabIndex = 13;
			this.label3.Text = "Enum value to search for";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(12, 13);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.txtResult);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.txtResult2);
			this.splitContainer1.Size = new System.Drawing.Size(688, 240);
			this.splitContainer1.SplitterDistance = 332;
			this.splitContainer1.TabIndex = 15;
			// 
			// txtResult
			// 
			this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtResult.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtResult.Location = new System.Drawing.Point(0, 0);
			this.txtResult.Multiline = true;
			this.txtResult.Name = "txtResult";
			this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtResult.Size = new System.Drawing.Size(332, 240);
			this.txtResult.TabIndex = 1;
			// 
			// txtResult2
			// 
			this.txtResult2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtResult2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtResult2.Location = new System.Drawing.Point(0, 0);
			this.txtResult2.Multiline = true;
			this.txtResult2.Name = "txtResult2";
			this.txtResult2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtResult2.Size = new System.Drawing.Size(352, 240);
			this.txtResult2.TabIndex = 15;
			// 
			// EnumHandling
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(712, 427);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.comboEnumValues);
			this.Controls.Add(this.db4oVersion2);
			this.Controls.Add(this.db4oVersion1);
			this.Controls.Add(this.cmdRun);
			this.Controls.Add(this.cmdClose);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "EnumHandling";
			this.Text = "Enum Test";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.Button cmdRun;
		private System.Windows.Forms.ComboBox db4oVersion1;
		private System.Windows.Forms.ComboBox db4oVersion2;
		private System.Windows.Forms.ComboBox comboEnumValues;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TextBox txtResult;
		private System.Windows.Forms.TextBox txtResult2;
	}
}

