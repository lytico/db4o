namespace OMControlLibrary
{
	partial class Login : ViewBase
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

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.labelFile = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.labelUserName = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelPort = new System.Windows.Forms.Label();
            this.labelHost = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.textBoxHost = new System.Windows.Forms.TextBox();
            this.groupBoxToggle = new System.Windows.Forms.GroupBox();
            this.comboBoxFilePath = new ToolTipComboBox();
            this.labelType = new System.Windows.Forms.Label();
            this.radioButtonRemote = new System.Windows.Forms.RadioButton();
            this.radioButtonLocal = new System.Windows.Forms.RadioButton();
            this.panelLocal = new System.Windows.Forms.Panel();
            this.chkReadOnly = new System.Windows.Forms.CheckBox();
            this.textBoxConnection = new System.Windows.Forms.TextBox();
            this.labelNewConnection = new System.Windows.Forms.Label();
            this.panelRemote = new System.Windows.Forms.Panel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.toolTipForTextBox = new System.Windows.Forms.ToolTip(this.components);
            this.btnBrowseCustomConfig = new System.Windows.Forms.Button();
            this.btnClearCustomConfig = new System.Windows.Forms.Button();
            this.btnAddAssemblies = new System.Windows.Forms.Button();
            this.toolTipForAssembly = new System.Windows.Forms.ToolTip(this.components);
            this.lblCustomConfig = new System.Windows.Forms.Label();
            this.txtCustomConfigAssemblyPath = new System.Windows.Forms.TextBox();
            this.groupBoxToggle.SuspendLayout();
            this.panelLocal.SuspendLayout();
            this.panelRemote.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.AutoSize = true;
            this.buttonBrowse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonBrowse.Image = global::OMControlLibrary.Properties.Resources.Browse;
            this.buttonBrowse.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonBrowse.Location = new System.Drawing.Point(399, 3);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(22, 22);
            this.buttonBrowse.TabIndex = 7;
            this.toolTipForTextBox.SetToolTip(this.buttonBrowse, "Browse");
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // labelFile
            // 
            this.labelFile.AutoSize = true;
            this.labelFile.Location = new System.Drawing.Point(12, 46);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new System.Drawing.Size(98, 13);
            this.labelFile.TabIndex = 0;
            this.labelFile.Text = "Recent Connection";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Font = new System.Drawing.Font("Tahoma", 8F);
            this.buttonConnect.Location = new System.Drawing.Point(293, 183);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 9;
            this.buttonConnect.Text = "&Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            this.buttonConnect.Enter += new System.EventHandler(this.buttonConnect_Click);
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new System.Drawing.Point(4, 34);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(59, 13);
            this.labelUserName.TabIndex = 7;
            this.labelUserName.Text = "User Name";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(220, 34);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(53, 13);
            this.labelPassword.TabIndex = 6;
            this.labelPassword.Text = "Password";
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(220, 7);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(27, 13);
            this.labelPort.TabIndex = 5;
            this.labelPort.Text = "Port";
            // 
            // labelHost
            // 
            this.labelHost.AutoSize = true;
            this.labelHost.Location = new System.Drawing.Point(4, 7);
            this.labelHost.Name = "labelHost";
            this.labelHost.Size = new System.Drawing.Size(29, 13);
            this.labelHost.TabIndex = 4;
            this.labelHost.Text = "Host";
            // 
            // textBoxPort
            // 
            this.textBoxPort.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxPort.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBoxPort.Location = new System.Drawing.Point(277, 3);
            this.textBoxPort.MaxLength = 5;
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(128, 13);
            this.textBoxPort.TabIndex = 4;
            this.textBoxPort.TextChanged += new System.EventHandler(this.textBoxPort_TextChanged);
            this.textBoxPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPort_KeyDown);
            this.textBoxPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPort_KeyPress);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxPassword.Location = new System.Drawing.Point(279, 30);
            this.textBoxPassword.MaxLength = 8;
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(126, 13);
            this.textBoxPassword.TabIndex = 6;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxUserName.Location = new System.Drawing.Point(81, 30);
            this.textBoxUserName.MaxLength = 20;
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(100, 13);
            this.textBoxUserName.TabIndex = 5;
            // 
            // textBoxHost
            // 
            this.textBoxHost.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxHost.Location = new System.Drawing.Point(81, 3);
            this.textBoxHost.MaxLength = 30;
            this.textBoxHost.Name = "textBoxHost";
            this.textBoxHost.Size = new System.Drawing.Size(100, 13);
            this.textBoxHost.TabIndex = 3;
            // 
            // groupBoxToggle
            // 
            this.groupBoxToggle.Controls.Add(this.comboBoxFilePath);
            this.groupBoxToggle.Controls.Add(this.labelType);
            this.groupBoxToggle.Controls.Add(this.labelFile);
            this.groupBoxToggle.Controls.Add(this.radioButtonRemote);
            this.groupBoxToggle.Controls.Add(this.radioButtonLocal);
            this.groupBoxToggle.Controls.Add(this.panelLocal);
            this.groupBoxToggle.Controls.Add(this.panelRemote);
            this.groupBoxToggle.Font = new System.Drawing.Font("Tahoma", 8F);
            this.groupBoxToggle.Location = new System.Drawing.Point(8, 2);
            this.groupBoxToggle.Name = "groupBoxToggle";
            this.groupBoxToggle.Size = new System.Drawing.Size(441, 133);
            this.groupBoxToggle.TabIndex = 1;
            this.groupBoxToggle.TabStop = false;
            this.groupBoxToggle.Text = "Connection";
            // 
            // comboBoxFilePath
            // 
            this.comboBoxFilePath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFilePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxFilePath.FormattingEnabled = true;
            this.comboBoxFilePath.Location = new System.Drawing.Point(119, 42);
            this.comboBoxFilePath.Name = "comboBoxFilePath";
            this.comboBoxFilePath.Size = new System.Drawing.Size(304, 21);
            this.comboBoxFilePath.TabIndex = 2;
            this.comboBoxFilePath.DropdownItemSelected += new ToolTipComboBox.DropdownItemSelectedEventHandler(this.comboBoxFilePath_DropdownItemSelected);
            this.comboBoxFilePath.SelectedIndexChanged += new System.EventHandler(this.comboBoxFilePath_SelectedIndexChanged);
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(12, 19);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(63, 13);
            this.labelType.TabIndex = 5;
            this.labelType.Text = "Select Type";
            // 
            // radioButtonRemote
            // 
            this.radioButtonRemote.AutoSize = true;
            this.radioButtonRemote.Location = new System.Drawing.Point(177, 17);
            this.radioButtonRemote.Name = "radioButtonRemote";
            this.radioButtonRemote.Size = new System.Drawing.Size(62, 17);
            this.radioButtonRemote.TabIndex = 1;
            this.radioButtonRemote.Text = "Remote";
            this.radioButtonRemote.UseVisualStyleBackColor = true;
            this.radioButtonRemote.Click += new System.EventHandler(this.radioButton_Click);
            // 
            // radioButtonLocal
            // 
            this.radioButtonLocal.AutoSize = true;
            this.radioButtonLocal.Checked = true;
            this.radioButtonLocal.Location = new System.Drawing.Point(120, 17);
            this.radioButtonLocal.Name = "radioButtonLocal";
            this.radioButtonLocal.Size = new System.Drawing.Size(49, 17);
            this.radioButtonLocal.TabIndex = 0;
            this.radioButtonLocal.TabStop = true;
            this.radioButtonLocal.Text = "Local";
            this.radioButtonLocal.UseVisualStyleBackColor = true;
            this.radioButtonLocal.Click += new System.EventHandler(this.radioButton_Click);
            // 
            // panelLocal
            // 
            this.panelLocal.Controls.Add(this.chkReadOnly);
            this.panelLocal.Controls.Add(this.textBoxConnection);
            this.panelLocal.Controls.Add(this.buttonBrowse);
            this.panelLocal.Controls.Add(this.labelNewConnection);
            this.panelLocal.Location = new System.Drawing.Point(6, 69);
            this.panelLocal.Name = "panelLocal";
            this.panelLocal.Size = new System.Drawing.Size(423, 52);
            this.panelLocal.TabIndex = 12;
            this.panelLocal.Visible = false;
            // 
            // chkReadOnly
            // 
            this.chkReadOnly.AutoSize = true;
            this.chkReadOnly.Location = new System.Drawing.Point(9, 31);
            this.chkReadOnly.Name = "chkReadOnly";
            this.chkReadOnly.Size = new System.Drawing.Size(72, 17);
            this.chkReadOnly.TabIndex = 9;
            this.chkReadOnly.Text = "read-only";
            this.chkReadOnly.UseVisualStyleBackColor = true;
            // 
            // textBoxConnection
            // 
            this.textBoxConnection.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxConnection.Location = new System.Drawing.Point(114, 8);
            this.textBoxConnection.MaxLength = 64;
            this.textBoxConnection.Name = "textBoxConnection";
            this.textBoxConnection.Size = new System.Drawing.Size(279, 13);
            this.textBoxConnection.TabIndex = 8;
            // 
            // labelNewConnection
            // 
            this.labelNewConnection.AutoSize = true;
            this.labelNewConnection.Location = new System.Drawing.Point(6, 8);
            this.labelNewConnection.Name = "labelNewConnection";
            this.labelNewConnection.Size = new System.Drawing.Size(85, 13);
            this.labelNewConnection.TabIndex = 8;
            this.labelNewConnection.Text = "New Connection";
            // 
            // panelRemote
            // 
            this.panelRemote.Controls.Add(this.labelUserName);
            this.panelRemote.Controls.Add(this.textBoxUserName);
            this.panelRemote.Controls.Add(this.labelPassword);
            this.panelRemote.Controls.Add(this.textBoxHost);
            this.panelRemote.Controls.Add(this.labelPort);
            this.panelRemote.Controls.Add(this.textBoxPassword);
            this.panelRemote.Controls.Add(this.labelHost);
            this.panelRemote.Controls.Add(this.textBoxPort);
            this.panelRemote.Location = new System.Drawing.Point(8, 68);
            this.panelRemote.Name = "panelRemote";
            this.panelRemote.Size = new System.Drawing.Size(415, 53);
            this.panelRemote.TabIndex = 12;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Font = new System.Drawing.Font("Tahoma", 8F);
            this.buttonCancel.Location = new System.Drawing.Point(374, 182);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // toolTipForTextBox
            // 
            this.toolTipForTextBox.AutoPopDelay = 0;
            this.toolTipForTextBox.InitialDelay = 0;
            this.toolTipForTextBox.ReshowDelay = 0;
            this.toolTipForTextBox.ShowAlways = true;
            // 
            // btnBrowseCustomConfig
            // 
            this.btnBrowseCustomConfig.AutoSize = true;
            this.btnBrowseCustomConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnBrowseCustomConfig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowseCustomConfig.Image = global::OMControlLibrary.Properties.Resources.Browse;
            this.btnBrowseCustomConfig.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBrowseCustomConfig.Location = new System.Drawing.Point(399, 147);
            this.btnBrowseCustomConfig.Name = "btnBrowseCustomConfig";
            this.btnBrowseCustomConfig.Size = new System.Drawing.Size(22, 22);
            this.btnBrowseCustomConfig.TabIndex = 10;
            this.toolTipForTextBox.SetToolTip(this.btnBrowseCustomConfig, "Browse");
            this.btnBrowseCustomConfig.UseVisualStyleBackColor = true;
            this.btnBrowseCustomConfig.Click += new System.EventHandler(this.btnBrowseCustomConfig_Click);
            // 
            // btnClearCustomConfig
            // 
            this.btnClearCustomConfig.AutoSize = true;
            this.btnClearCustomConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClearCustomConfig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClearCustomConfig.Image = global::OMControlLibrary.Properties.Resources.close;
            this.btnClearCustomConfig.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClearCustomConfig.Location = new System.Drawing.Point(427, 147);
            this.btnClearCustomConfig.Name = "btnClearCustomConfig";
            this.btnClearCustomConfig.Size = new System.Drawing.Size(22, 22);
            this.btnClearCustomConfig.TabIndex = 14;
            this.toolTipForTextBox.SetToolTip(this.btnClearCustomConfig, "Browse");
            this.btnClearCustomConfig.UseVisualStyleBackColor = true;
            this.btnClearCustomConfig.Click += new System.EventHandler(this.btnClearCustomConfig_Click);
            // 
            // btnAddAssemblies
            // 
            this.btnAddAssemblies.Location = new System.Drawing.Point(194, 183);
            this.btnAddAssemblies.Name = "btnAddAssemblies";
            this.btnAddAssemblies.Size = new System.Drawing.Size(93, 23);
            this.btnAddAssemblies.TabIndex = 11;
            this.btnAddAssemblies.Text = "Add Assemblies";
            this.btnAddAssemblies.UseVisualStyleBackColor = true;
            this.btnAddAssemblies.Click += new System.EventHandler(this.btnAddAssemblies_Click);
            // 
            // toolTipForAssembly
            // 
            this.toolTipForAssembly.AutoPopDelay = 0;
            this.toolTipForAssembly.InitialDelay = 0;
            this.toolTipForAssembly.ReshowDelay = 0;
            this.toolTipForAssembly.ShowAlways = true;
            // 
            // lblCustomConfig
            // 
            this.lblCustomConfig.AutoSize = true;
            this.lblCustomConfig.Location = new System.Drawing.Point(13, 147);
            this.lblCustomConfig.Name = "lblCustomConfig";
            this.lblCustomConfig.Size = new System.Drawing.Size(147, 13);
            this.lblCustomConfig.TabIndex = 13;
            this.lblCustomConfig.Text = "Custom Config Assembly Path";
            // 
            // txtCustomConfigAssemblyPath
            // 
            this.txtCustomConfigAssemblyPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCustomConfigAssemblyPath.Location = new System.Drawing.Point(166, 147);
            this.txtCustomConfigAssemblyPath.MaxLength = 64;
            this.txtCustomConfigAssemblyPath.Name = "txtCustomConfigAssemblyPath";
            this.txtCustomConfigAssemblyPath.Size = new System.Drawing.Size(227, 13);
            this.txtCustomConfigAssemblyPath.TabIndex = 10;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.btnClearCustomConfig);
            this.Controls.Add(this.txtCustomConfigAssemblyPath);
            this.Controls.Add(this.btnBrowseCustomConfig);
            this.Controls.Add(this.lblCustomConfig);
            this.Controls.Add(this.btnAddAssemblies);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBoxToggle);
            this.Name = "Login";
            this.Size = new System.Drawing.Size(484, 218);
            this.Load += new System.EventHandler(this.Login_Load);
            this.groupBoxToggle.ResumeLayout(false);
            this.groupBoxToggle.PerformLayout();
            this.panelLocal.ResumeLayout(false);
            this.panelLocal.PerformLayout();
            this.panelRemote.ResumeLayout(false);
            this.panelRemote.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBoxToggle;
		private System.Windows.Forms.RadioButton radioButtonRemote;
		private System.Windows.Forms.RadioButton radioButtonLocal;
		private System.Windows.Forms.Button buttonBrowse;
		private System.Windows.Forms.Label labelFile;
		private System.Windows.Forms.Label labelUserName;
		private System.Windows.Forms.Label labelPassword;
		private System.Windows.Forms.Label labelPort;
		private System.Windows.Forms.Label labelHost;
		private System.Windows.Forms.TextBox textBoxPort;
		private System.Windows.Forms.TextBox textBoxPassword;
		private System.Windows.Forms.TextBox textBoxUserName;
		private System.Windows.Forms.TextBox textBoxHost;
		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Label labelType;
		private System.Windows.Forms.Label labelNewConnection;
		private System.Windows.Forms.TextBox textBoxConnection;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Panel panelLocal;
		private System.Windows.Forms.Panel panelRemote;
		internal System.Windows.Forms.ToolTip toolTipForTextBox;
		private ToolTipComboBox comboBoxFilePath;
		private System.Windows.Forms.CheckBox chkReadOnly;
        private System.Windows.Forms.Button btnAddAssemblies;
        private System.Windows.Forms.ToolTip toolTipForAssembly;
        private System.Windows.Forms.Label lblCustomConfig;
        private System.Windows.Forms.Button btnBrowseCustomConfig;
        private System.Windows.Forms.TextBox txtCustomConfigAssemblyPath;
        private System.Windows.Forms.Button btnClearCustomConfig;
        //private ToolTipComboBox comboAssemblies;
	}
}
