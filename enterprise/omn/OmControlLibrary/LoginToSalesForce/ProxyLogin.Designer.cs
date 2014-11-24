namespace OMControlLibrary.LoginToSalesForce
{
	partial class ProxyLogin
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
			this.buttonLogin = new System.Windows.Forms.Button();
			this.labelDDNPassword = new System.Windows.Forms.Label();
			this.textBoxPassword = new System.Windows.Forms.TextBox();
			this.labelDomainUser = new System.Windows.Forms.Label();
			this.textBoxUserID = new System.Windows.Forms.TextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblProxy = new System.Windows.Forms.Label();
			this.lblPort = new System.Windows.Forms.Label();
			this.textBoxPort = new System.Windows.Forms.TextBox();
			this.textBoxProxy = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// buttonLogin
			// 
			this.buttonLogin.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonLogin.Location = new System.Drawing.Point(106, 208);
			this.buttonLogin.Name = "buttonLogin";
			this.buttonLogin.Size = new System.Drawing.Size(67, 23);
			this.buttonLogin.TabIndex = 11;
			this.buttonLogin.Text = "&Ok";
			this.buttonLogin.UseVisualStyleBackColor = true;
			// 
			// labelDDNPassword
			// 
			this.labelDDNPassword.AutoSize = true;
			this.labelDDNPassword.Location = new System.Drawing.Point(21, 74);
			this.labelDDNPassword.Name = "labelDDNPassword";
			this.labelDDNPassword.Size = new System.Drawing.Size(59, 13);
			this.labelDDNPassword.TabIndex = 10;
			this.labelDDNPassword.Text = "Password :";
			// 
			// textBoxPassword
			// 
			this.textBoxPassword.Location = new System.Drawing.Point(106, 71);
			this.textBoxPassword.Name = "textBoxPassword";
			this.textBoxPassword.Size = new System.Drawing.Size(152, 20);
			this.textBoxPassword.TabIndex = 9;
			this.textBoxPassword.UseSystemPasswordChar = true;
			// 
			// labelDomainUser
			// 
			this.labelDomainUser.AutoSize = true;
			this.labelDomainUser.Location = new System.Drawing.Point(21, 35);
			this.labelDomainUser.Name = "labelDomainUser";
			this.labelDomainUser.Size = new System.Drawing.Size(61, 13);
			this.labelDomainUser.TabIndex = 8;
			this.labelDomainUser.Text = "Username :";
			// 
			// textBoxUserID
			// 
			this.textBoxUserID.Location = new System.Drawing.Point(108, 32);
			this.textBoxUserID.Name = "textBoxUserID";
			this.textBoxUserID.Size = new System.Drawing.Size(152, 20);
			this.textBoxUserID.TabIndex = 7;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(179, 208);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 12;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// lblProxy
			// 
			this.lblProxy.AutoSize = true;
			this.lblProxy.Location = new System.Drawing.Point(21, 121);
			this.lblProxy.Name = "lblProxy";
			this.lblProxy.Size = new System.Drawing.Size(80, 13);
			this.lblProxy.TabIndex = 13;
			this.lblProxy.Text = "Proxy Address :";
			// 
			// lblPort
			// 
			this.lblPort.AutoSize = true;
			this.lblPort.Location = new System.Drawing.Point(21, 162);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(49, 13);
			this.lblPort.TabIndex = 14;
			this.lblPort.Text = "Port No :";
			// 
			// textBoxPort
			// 
			this.textBoxPort.Location = new System.Drawing.Point(106, 155);
			this.textBoxPort.Name = "textBoxPort";
			this.textBoxPort.Size = new System.Drawing.Size(152, 20);
			this.textBoxPort.TabIndex = 15;
			// 
			// textBoxProxy
			// 
			this.textBoxProxy.Location = new System.Drawing.Point(106, 114);
			this.textBoxProxy.Name = "textBoxProxy";
			this.textBoxProxy.Size = new System.Drawing.Size(152, 20);
			this.textBoxProxy.TabIndex = 16;
			// 
			// ProxyLogin
			// 
			this.AcceptButton = this.buttonLogin;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(294, 280);
			this.ControlBox = false;
			this.Controls.Add(this.textBoxProxy);
			this.Controls.Add(this.textBoxPort);
			this.Controls.Add(this.lblPort);
			this.Controls.Add(this.lblProxy);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.buttonLogin);
			this.Controls.Add(this.labelDDNPassword);
			this.Controls.Add(this.textBoxPassword);
			this.Controls.Add(this.labelDomainUser);
			this.Controls.Add(this.textBoxUserID);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProxyLogin";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Proxy Login";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.Button buttonLogin;
		private System.Windows.Forms.Label labelDDNPassword;
		public System.Windows.Forms.TextBox textBoxPassword;
		private System.Windows.Forms.Label labelDomainUser;
		public System.Windows.Forms.TextBox textBoxUserID;
		public System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblProxy;
		private System.Windows.Forms.Label lblPort;
		public System.Windows.Forms.TextBox textBoxPort;
		public System.Windows.Forms.TextBox textBoxProxy;
	}
}