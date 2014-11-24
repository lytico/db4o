namespace Db4oTestRunner
{
	partial class Db4oTestRunner
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Db4oTestRunner));
			this.cmdClose = new System.Windows.Forms.Button();
			this.cmdRun = new System.Windows.Forms.Button();
			this.db4oVersion2 = new System.Windows.Forms.ComboBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cmdSelectTestAssembly = new System.Windows.Forms.Button();
			this.txtTestAssembly = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cbRunVersion2 = new System.Windows.Forms.CheckBox();
			this.cbRunVersion1 = new System.Windows.Forms.CheckBox();
			this.db4oVersion1 = new System.Windows.Forms.ComboBox();
			this.lbTestClasses = new System.Windows.Forms.CheckedListBox();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.txtResult2 = new System.Windows.Forms.RichTextBox();
			this.txtResult = new System.Windows.Forms.RichTextBox();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdClose
			// 
			this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdClose.Location = new System.Drawing.Point(813, 650);
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
			this.cmdRun.Location = new System.Drawing.Point(12, 650);
			this.cmdRun.Name = "cmdRun";
			this.cmdRun.Size = new System.Drawing.Size(75, 23);
			this.cmdRun.TabIndex = 8;
			this.cmdRun.Text = "&Run Tests";
			this.cmdRun.UseVisualStyleBackColor = true;
			this.cmdRun.Click += new System.EventHandler(this.cmdRun_Click);
			// 
			// db4oVersion2
			// 
			this.db4oVersion2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.db4oVersion2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.db4oVersion2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.db4oVersion2.FormattingEnabled = true;
			this.db4oVersion2.Location = new System.Drawing.Point(27, 50);
			this.db4oVersion2.Name = "db4oVersion2";
			this.db4oVersion2.Size = new System.Drawing.Size(842, 24);
			this.db4oVersion2.TabIndex = 11;
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
			this.splitContainer1.Size = new System.Drawing.Size(876, 355);
			this.splitContainer1.SplitterDistance = 373;
			this.splitContainer1.TabIndex = 15;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.txtDescription);
			this.groupBox1.Controls.Add(this.lbTestClasses);
			this.groupBox1.Controls.Add(this.cmdSelectTestAssembly);
			this.groupBox1.Controls.Add(this.txtTestAssembly);
			this.groupBox1.Location = new System.Drawing.Point(12, 374);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(875, 180);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Test Specification";
			// 
			// cmdSelectTestAssembly
			// 
			this.cmdSelectTestAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdSelectTestAssembly.Location = new System.Drawing.Point(839, 18);
			this.cmdSelectTestAssembly.Name = "cmdSelectTestAssembly";
			this.cmdSelectTestAssembly.Size = new System.Drawing.Size(30, 23);
			this.cmdSelectTestAssembly.TabIndex = 2;
			this.cmdSelectTestAssembly.Text = "...";
			this.cmdSelectTestAssembly.UseVisualStyleBackColor = true;
			this.cmdSelectTestAssembly.Click += new System.EventHandler(this.cmdSelectTestAssembly_Click);
			// 
			// txtTestAssembly
			// 
			this.txtTestAssembly.AllowDrop = true;
			this.txtTestAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTestAssembly.Location = new System.Drawing.Point(6, 20);
			this.txtTestAssembly.Name = "txtTestAssembly";
			this.txtTestAssembly.Size = new System.Drawing.Size(827, 21);
			this.txtTestAssembly.TabIndex = 1;
			this.txtTestAssembly.TextChanged += new System.EventHandler(this.txtTestAssembly_TextChanged);
			this.txtTestAssembly.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtTestAssembly_DragDrop);
			this.txtTestAssembly.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtTestAssembly_DragEnter);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.cbRunVersion2);
			this.groupBox2.Controls.Add(this.cbRunVersion1);
			this.groupBox2.Controls.Add(this.db4oVersion1);
			this.groupBox2.Controls.Add(this.db4oVersion2);
			this.groupBox2.Location = new System.Drawing.Point(12, 560);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(876, 84);
			this.groupBox2.TabIndex = 17;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Db4o Versions";
			// 
			// cbRunVersion2
			// 
			this.cbRunVersion2.AutoSize = true;
			this.cbRunVersion2.Checked = true;
			this.cbRunVersion2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbRunVersion2.Location = new System.Drawing.Point(6, 55);
			this.cbRunVersion2.Name = "cbRunVersion2";
			this.cbRunVersion2.Size = new System.Drawing.Size(15, 14);
			this.cbRunVersion2.TabIndex = 13;
			this.cbRunVersion2.UseVisualStyleBackColor = true;
			this.cbRunVersion2.CheckedChanged += new System.EventHandler(this.VersionStatusToogle);
			// 
			// cbRunVersion1
			// 
			this.cbRunVersion1.AutoSize = true;
			this.cbRunVersion1.Checked = true;
			this.cbRunVersion1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbRunVersion1.Location = new System.Drawing.Point(6, 25);
			this.cbRunVersion1.Name = "cbRunVersion1";
			this.cbRunVersion1.Size = new System.Drawing.Size(15, 14);
			this.cbRunVersion1.TabIndex = 12;
			this.cbRunVersion1.UseVisualStyleBackColor = true;
			this.cbRunVersion1.CheckedChanged += new System.EventHandler(this.VersionStatusToogle);
			// 
			// db4oVersion1
			// 
			this.db4oVersion1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.db4oVersion1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.db4oVersion1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.db4oVersion1.FormattingEnabled = true;
			this.db4oVersion1.Location = new System.Drawing.Point(27, 20);
			this.db4oVersion1.Name = "db4oVersion1";
			this.db4oVersion1.Size = new System.Drawing.Size(842, 24);
			this.db4oVersion1.TabIndex = 11;
			// 
			// lbTestClasses
			// 
			this.lbTestClasses.FormattingEnabled = true;
			this.lbTestClasses.Location = new System.Drawing.Point(6, 56);
			this.lbTestClasses.Name = "lbTestClasses";
			this.lbTestClasses.Size = new System.Drawing.Size(371, 116);
			this.lbTestClasses.TabIndex = 3;
			// 
			// txtDescription
			// 
			this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtDescription.Location = new System.Drawing.Point(381, 56);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.Size = new System.Drawing.Size(452, 116);
			this.txtDescription.TabIndex = 4;
			// 
			// txtResult2
			// 
			this.txtResult2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtResult2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtResult2.Location = new System.Drawing.Point(0, 0);
			this.txtResult2.Name = "txtResult2";
			this.txtResult2.Size = new System.Drawing.Size(499, 355);
			this.txtResult2.TabIndex = 0;
			this.txtResult2.Text = "";
			// 
			// txtResult
			// 
			this.txtResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtResult.Location = new System.Drawing.Point(0, 0);
			this.txtResult.Name = "txtResult";
			this.txtResult.Size = new System.Drawing.Size(373, 355);
			this.txtResult.TabIndex = 0;
			this.txtResult.Text = "";
			// 
			// Db4oTestRunner
			// 
			this.AcceptButton = this.cmdRun;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(900, 685);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.cmdRun);
			this.Controls.Add(this.cmdClose);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Db4oTestRunner";
			this.Text = "Db4oTestRunner";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.Button cmdRun;
		private System.Windows.Forms.ComboBox db4oVersion2;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button cmdSelectTestAssembly;
		private System.Windows.Forms.TextBox txtTestAssembly;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox db4oVersion1;
		private System.Windows.Forms.CheckBox cbRunVersion1;
		private System.Windows.Forms.CheckBox cbRunVersion2;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.CheckedListBox lbTestClasses;
		private System.Windows.Forms.RichTextBox txtResult;
		private System.Windows.Forms.RichTextBox txtResult2;
	}
}

