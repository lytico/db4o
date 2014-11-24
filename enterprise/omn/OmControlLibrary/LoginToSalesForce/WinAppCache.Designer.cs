namespace OMControlLibrary.LoginToSalesForce
{
	partial class WinAppCache
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
			this.textBoxUserID = new System.Windows.Forms.TextBox();
			this.labelDDNUserName = new System.Windows.Forms.Label();
			this.labelDDNPassword = new System.Windows.Forms.Label();
			this.textBoxPassword = new System.Windows.Forms.TextBox();
			this.checkBoxRememberMe = new System.Windows.Forms.CheckBox();
			this.buttonLogin = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.linkLabelPurchase = new System.Windows.Forms.LinkLabel();
			this.label2 = new System.Windows.Forms.Label();
			this.linkLabelForgotPassword = new System.Windows.Forms.LinkLabel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lblUserName = new System.Windows.Forms.Label();
			this.lblPassword = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// textBoxUserID
			// 
			this.textBoxUserID.CausesValidation = false;
			this.textBoxUserID.Location = new System.Drawing.Point(112, 98);
			this.textBoxUserID.Name = "textBoxUserID";
			this.textBoxUserID.Size = new System.Drawing.Size(136, 20);
			this.textBoxUserID.TabIndex = 0;
			this.textBoxUserID.TextChanged += new System.EventHandler(this.textBoxUserID_TextChanged);
			// 
			// labelDDNUserName
			// 
			this.labelDDNUserName.AutoSize = true;
			this.labelDDNUserName.Location = new System.Drawing.Point(49, 102);
			this.labelDDNUserName.Name = "labelDDNUserName";
			this.labelDDNUserName.Size = new System.Drawing.Size(60, 13);
			this.labelDDNUserName.TabIndex = 1;
			this.labelDDNUserName.Text = "User Name";
			// 
			// labelDDNPassword
			// 
			this.labelDDNPassword.AutoSize = true;
			this.labelDDNPassword.Location = new System.Drawing.Point(53, 128);
			this.labelDDNPassword.Name = "labelDDNPassword";
			this.labelDDNPassword.Size = new System.Drawing.Size(56, 13);
			this.labelDDNPassword.TabIndex = 3;
			this.labelDDNPassword.Text = " Password";
			// 
			// textBoxPassword
			// 
			this.textBoxPassword.CausesValidation = false;
			this.textBoxPassword.Location = new System.Drawing.Point(112, 124);
			this.textBoxPassword.Name = "textBoxPassword";
			this.textBoxPassword.Size = new System.Drawing.Size(136, 20);
			this.textBoxPassword.TabIndex = 1;
			this.textBoxPassword.UseSystemPasswordChar = true;
			this.textBoxPassword.TextChanged += new System.EventHandler(this.textBoxPassword_TextChanged);
			// 
			// checkBoxRememberMe
			// 
			this.checkBoxRememberMe.AutoSize = true;
			this.checkBoxRememberMe.Location = new System.Drawing.Point(112, 153);
			this.checkBoxRememberMe.Name = "checkBoxRememberMe";
			this.checkBoxRememberMe.Size = new System.Drawing.Size(98, 17);
			this.checkBoxRememberMe.TabIndex = 3;
			this.checkBoxRememberMe.Text = "Remember Me ";
			this.checkBoxRememberMe.UseVisualStyleBackColor = true;
			// 
			// buttonLogin
			// 
			this.buttonLogin.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonLogin.Location = new System.Drawing.Point(112, 176);
			this.buttonLogin.Name = "buttonLogin";
			this.buttonLogin.Size = new System.Drawing.Size(63, 22);
			this.buttonLogin.TabIndex = 4;
			this.buttonLogin.Text = "&Login";
			this.buttonLogin.UseVisualStyleBackColor = true;
			this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(182, 176);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(66, 22);
			this.buttonCancel.TabIndex = 5;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(-1, 258);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(272, 13);
			this.label1.TabIndex = 10;
			this.label1.Text = "This product requires a dDN Enterprise license";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::OMControlLibrary.Properties.Resources.db4objects_logo_white_2a;
			this.pictureBox1.Location = new System.Drawing.Point(-2, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(323, 82);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 9;
			this.pictureBox1.TabStop = false;
			// 
			// linkLabelPurchase
			// 
			this.linkLabelPurchase.AutoSize = true;
			this.linkLabelPurchase.Location = new System.Drawing.Point(269, 258);
			this.linkLabelPurchase.Name = "linkLabelPurchase";
			this.linkLabelPurchase.Size = new System.Drawing.Size(52, 13);
			this.linkLabelPurchase.TabIndex = 7;
			this.linkLabelPurchase.TabStop = true;
			this.linkLabelPurchase.Text = "Purchase";
			this.linkLabelPurchase.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelPurchase_LinkClicked);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(47, 239);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(226, 13);
			this.label2.TabIndex = 11;
			this.label2.Text = "Press Cancel  to continue to a Reduced Mode";
			// 
			// linkLabelForgotPassword
			// 
			this.linkLabelForgotPassword.AutoSize = true;
			this.linkLabelForgotPassword.Location = new System.Drawing.Point(109, 205);
			this.linkLabelForgotPassword.Name = "linkLabelForgotPassword";
			this.linkLabelForgotPassword.Size = new System.Drawing.Size(95, 13);
			this.linkLabelForgotPassword.TabIndex = 6;
			this.linkLabelForgotPassword.TabStop = true;
			this.linkLabelForgotPassword.Text = "Forgot Password ?";
			this.linkLabelForgotPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelForgotPassword_LinkClicked);
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(-3, 224);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(328, 10);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Location = new System.Drawing.Point(-2, 76);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(329, 10);
			this.groupBox2.TabIndex = 17;
			this.groupBox2.TabStop = false;
			// 
			// lblUserName
			// 
			this.lblUserName.AutoSize = true;
			this.lblUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblUserName.ForeColor = System.Drawing.Color.Red;
			this.lblUserName.Location = new System.Drawing.Point(249, 101);
			this.lblUserName.Name = "lblUserName";
			this.lblUserName.Size = new System.Drawing.Size(13, 15);
			this.lblUserName.TabIndex = 18;
			this.lblUserName.Text = "*";
			// 
			// lblPassword
			// 
			this.lblPassword.AutoSize = true;
			this.lblPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPassword.ForeColor = System.Drawing.Color.Red;
			this.lblPassword.Location = new System.Drawing.Point(249, 127);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(13, 15);
			this.lblPassword.TabIndex = 19;
			this.lblPassword.Text = "*";
			// 
			// WinAppCache
			// 
			this.AcceptButton = this.buttonLogin;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(320, 277);
			this.ControlBox = false;
			this.Controls.Add(this.lblPassword);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.lblUserName);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.linkLabelForgotPassword);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.linkLabelPurchase);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonLogin);
			this.Controls.Add(this.checkBoxRememberMe);
			this.Controls.Add(this.labelDDNPassword);
			this.Controls.Add(this.textBoxPassword);
			this.Controls.Add(this.labelDDNUserName);
			this.Controls.Add(this.textBoxUserID);
			this.Controls.Add(this.groupBox2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WinAppCache";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ObjectManager Enterprise Login";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonCancel;
		public System.Windows.Forms.CheckBox checkBoxRememberMe;
		public System.Windows.Forms.TextBox textBoxUserID;
		public System.Windows.Forms.TextBox textBoxPassword;
		private System.Windows.Forms.Label labelDDNUserName;
		private System.Windows.Forms.Label labelDDNPassword;
		public System.Windows.Forms.Button buttonLogin;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.LinkLabel linkLabelPurchase;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.LinkLabel linkLabelForgotPassword;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label lblUserName;
		private System.Windows.Forms.Label lblPassword;
	}
}

