namespace OMControlLibrary
{
	partial class SearchPathDialog
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
			this.listSearchPath = new System.Windows.Forms.ListBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnAddPath = new System.Windows.Forms.Button();
			this.btnRemovePath = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// listSearchPath
			// 
			this.listSearchPath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listSearchPath.FormattingEnabled = true;
			this.listSearchPath.Location = new System.Drawing.Point(12, 12);
			this.listSearchPath.Name = "listSearchPath";
			this.listSearchPath.Size = new System.Drawing.Size(437, 355);
			this.listSearchPath.TabIndex = 0;
			this.listSearchPath.SelectedIndexChanged += new System.EventHandler(this.listSearchPath_SelectedIndexChanged);
			this.listSearchPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listSearchPath_KeyDown);
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(455, 344);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(71, 23);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "&OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// btnAddPath
			// 
			this.btnAddPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddPath.Location = new System.Drawing.Point(455, 12);
			this.btnAddPath.Name = "btnAddPath";
			this.btnAddPath.Size = new System.Drawing.Size(71, 23);
			this.btnAddPath.TabIndex = 2;
			this.btnAddPath.Text = "&Add...";
			this.btnAddPath.UseVisualStyleBackColor = true;
			this.btnAddPath.Click += new System.EventHandler(this.btnAddPath_Click);
			// 
			// btnRemovePath
			// 
			this.btnRemovePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemovePath.Enabled = false;
			this.btnRemovePath.Location = new System.Drawing.Point(455, 41);
			this.btnRemovePath.Name = "btnRemovePath";
			this.btnRemovePath.Size = new System.Drawing.Size(71, 23);
			this.btnRemovePath.TabIndex = 3;
			this.btnRemovePath.Text = "&Remove";
			this.btnRemovePath.UseVisualStyleBackColor = true;
			this.btnRemovePath.Click += new System.EventHandler(this.btnRemovePath_Click);
			// 
			// SearchPathDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(538, 377);
			this.Controls.Add(this.btnRemovePath);
			this.Controls.Add(this.btnAddPath);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.listSearchPath);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SearchPathDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Assemblies Search Path";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox listSearchPath;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnAddPath;
		private System.Windows.Forms.Button btnRemovePath;
	}
}