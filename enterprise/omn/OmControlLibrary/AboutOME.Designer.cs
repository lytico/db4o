namespace OMControlLibrary
{
	partial class AboutOME
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
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.labeldb4o = new System.Windows.Forms.Label();
            this.labelOME = new System.Windows.Forms.Label();
            this.labelCopyRight = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBoxLogo.Image = global::OMControlLibrary.Properties.Resources.db4objects_logo_white_2a;
            this.pictureBoxLogo.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(323, 82);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            // 
            // labeldb4o
            // 
            this.labeldb4o.AutoSize = true;
            this.labeldb4o.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.labeldb4o.Location = new System.Drawing.Point(107, 133);
            this.labeldb4o.Name = "labeldb4o";
            this.labeldb4o.Size = new System.Drawing.Size(102, 13);
            this.labeldb4o.TabIndex = 10;
            this.labeldb4o.Text = "db4o  (6.3.500.0)";
            // 
            // labelOME
            // 
            this.labelOME.AutoSize = true;
            this.labelOME.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.labelOME.Location = new System.Drawing.Point(67, 105);
            this.labelOME.Name = "labelOME";
            this.labelOME.Size = new System.Drawing.Size(198, 13);
            this.labelOME.TabIndex = 8;
            this.labelOME.Text = "ObjectManager Enterprise  (3.0.0)";
            this.labelOME.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCopyRight
            // 
            this.labelCopyRight.AutoSize = true;
            this.labelCopyRight.Font = new System.Drawing.Font("Tahoma", 8F);
            this.labelCopyRight.Location = new System.Drawing.Point(2, 249);
            this.labelCopyRight.Name = "labelCopyRight";
            this.labelCopyRight.Size = new System.Drawing.Size(176, 13);
            this.labelCopyRight.TabIndex = 14;
            this.labelCopyRight.Text = "Copyright © 2008 db4objects, Inc.";
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Font = new System.Drawing.Font("Tahoma", 8F);
            this.buttonOk.Location = new System.Drawing.Point(257, 244);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(54, 23);
            this.buttonOk.TabIndex = 16;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(-4, 228);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(329, 10);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(-2, 76);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(329, 10);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // AboutOME
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 273);
            this.Controls.Add(this.labeldb4o);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.labelCopyRight);
            this.Controls.Add(this.labelOME);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.groupBox2);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AboutOME";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About ObjectManager Enterprise";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.PictureBox pictureBoxLogo;
		private System.Windows.Forms.Label labeldb4o;
		private System.Windows.Forms.Label labelOME;
        private System.Windows.Forms.Label labelCopyRight;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
	}
}