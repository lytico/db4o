/* Copyright (C) 2004 - 2009  Versant Inc.  http://www.db4o.com */
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EnvDTE;
using System.Reflection;
using OMAddinDataTransferLayer;
using OMControlLibrary.OMEAppDomain;
using OManager.BusinessLayer.UIHelper;
using OMControlLibrary.Common;
using OManager.BusinessLayer.Login;
using Microsoft.VisualStudio.CommandBars;
using OME.Logging.Common;
using Constants = OMControlLibrary.Common.Constants;


namespace OMControlLibrary
{
	/// <summary>
	/// Using this user control, user can login to ObjectManager Enterprise.
	/// </summary>

	[ComVisible(true)]
	public partial class Login: ILoadData
	{
		#region Member Variables

		//Private static variables
	    private static Window loginToolWindow;
		private static CommandBarControl m_cmdBarCtrlCreateDemoDb;
		internal static CommandBarControl m_cmdBarCtrlConnect;
		internal static CommandBarControl m_cmdBarCtrlBackup;
		internal static CommandBarButton m_cmdBarBtnConnect;
		private static Assembly m_AddIn_Assembly;
		//Private variables
		
		private IList<ConnectionDetails> m_ListrecentConnections;
		//Constants

        private const string IMAGE_DISCONNECT = "OMAddin.Images.DB_DISCONNECT2_a.GIF";
        private const string IMAGE_DISCONNECT_MASKED = "OMAddin.Images.DB_DISCONNECT2_b.BMP";

		private const string OPEN_FILE_DIALOG_FILTER = "db4o Database Files(*.yap, *.db4o)|*.yap;*.db4o|All Files(*.*)|*.*";
        private const string OPEN_FILE_ADDASSEMBLY_FILTER = "Assemblies(*.exe, *.dll)|*.exe;*.dll";
		private const string STRING_SERVER = "server:";
		private const string STRING_COLON = ":";
		private const char CHAR_COLON = ':';

		static Window queryBuilderToolWindow;
        public static AppDomainDetails appDomain;
	    private CustomConfigAppDomain customConfigAppDomain;
	    private bool checkCustomConfig;
		#endregion
        public Login()
		{
			InitializeComponent();
		
		}

		public override void SetLiterals()
		{
			try
			{
				labelFile.Text = Helper.GetResourceString(Common.Constants.LOGIN_RECENTCONNECTION_TEXT);
				labelHost.Text = Helper.GetResourceString(Common.Constants.LOGIN_HOST_TEXT);
				labelPort.Text = Helper.GetResourceString(Common.Constants.LOGIN_PORT_TEXT);
				labelUserName.Text = Helper.GetResourceString(Common.Constants.LOGIN_USERNAME_TEXT);
				labelPassword.Text = Helper.GetResourceString(Common.Constants.LOGIN_PASSWORD_TEXT);
				labelType.Text = Helper.GetResourceString(Common.Constants.LOGIN_TYPE_TEXT);
				labelNewConnection.Text = Helper.GetResourceString(Common.Constants.LOGIN_NEWCONNECTION_TEXT);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
		
		public static void CreateLoginToolWindow(CommandBarControl cmdBarCtrl,
			CommandBarButton cmdBarBtn, Assembly addIn_Assembly,
			CommandBarControl cmdBarCtrlBackup, CommandBarControl dbCreateDemoDbControl)
		{
			try
			{
				m_AddIn_Assembly = addIn_Assembly;
				m_cmdBarCtrlConnect = cmdBarCtrl;
				m_cmdBarBtnConnect = cmdBarBtn;
				m_cmdBarCtrlBackup = cmdBarCtrlBackup;
				m_cmdBarCtrlCreateDemoDb = dbCreateDemoDbControl;

                loginToolWindow = CreateToolWindow(Common.Constants.CLASS_NAME_LOGIN, Common.Constants.LOGIN, NewFormattedGuid());

                if (loginToolWindow.AutoHides)
				{
                    loginToolWindow.AutoHides = false;
				}
                loginToolWindow.Visible = true;
                loginToolWindow.Width = 460;
                loginToolWindow.Height = 210;
				Helper.CheckIfLoginWindowIsVisible = true;
				
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private static string NewFormattedGuid()
		{
			return Guid.NewGuid().ToString(Helper.GetResourceString(Constants.GUID_FORMATTER_STRING));
		}
	
		public static void CreateQueryBuilderToolWindow()
		{
			try
			{
				string caption = Helper.GetResourceString(Constants.QUERY_BUILDER_CAPTION);
				queryBuilderToolWindow = CreateToolWindow(Constants.CLASS_NAME_QUERYBUILDER, caption, Common.Constants.GUID_QUERYBUILDER );
				
				if (queryBuilderToolWindow.AutoHides)
				{
					queryBuilderToolWindow.AutoHides = false;
				}
				queryBuilderToolWindow.IsFloating = false;
				queryBuilderToolWindow.Linkable = false;
				queryBuilderToolWindow.Visible = true;
				
			}
			catch (Exception e)
			{
				LoggingHelper.HandleException(e); 
			}
		}

		public void LoadAppropriatedata()
		{
			ClearPanelControls();
			ShowAppropriatePanel(true);
			m_ListrecentConnections = OMEInteraction.FetchAllConnections(false);
			if (m_ListrecentConnections != null)
			{
				PopulateConnections(m_ListrecentConnections);
				m_cmdBarCtrlBackup.Enabled = false;
				m_cmdBarCtrlCreateDemoDb.Enabled = true;
			}
          
		}

		private void ClearPanelControls()
		{
			toolTipForTextBox.RemoveAll();
		    toolTipForAssembly.RemoveAll();
			comboBoxFilePath.Items.Clear();
			textBoxConnection.Clear();
			textBoxHost.Clear();
			textBoxPassword.Clear();
			textBoxPort.Clear();
			textBoxPassword.Clear();
		    txtCustomConfigAssemblyPath.Clear(); 
			chkReadOnly.Checked = false;
		}
		private void ShowAppropriatePanel(bool param)
		{
			panelLocal.Visible = param;
			panelRemote.Visible = !param;
			radioButtonLocal.Checked = param;
			radioButtonRemote.Checked = !param;
		}
		private void AfterSuccessfullyConnected()
		{
			try
			{
				m_cmdBarCtrlConnect.Caption = Common.Constants.TOOLBAR_DISCONNECT;
				m_cmdBarBtnConnect.Caption = Common.Constants.TOOLBAR_DISCONNECT;
				m_cmdBarBtnConnect.TooltipText = Common.Constants.TOOLBAR_DISCONNECT;
				

				if (radioButtonLocal.Checked)
				{
					m_cmdBarCtrlBackup.Enabled = true;
					m_cmdBarCtrlCreateDemoDb.Enabled = false;
				}
				Helper.SetPicture(m_AddIn_Assembly, (CommandBarButton)m_cmdBarCtrlConnect.Control, IMAGE_DISCONNECT, IMAGE_DISCONNECT_MASKED);
				Helper.SetPicture(m_AddIn_Assembly, (CommandBarButton)m_cmdBarBtnConnect.Control, IMAGE_DISCONNECT, IMAGE_DISCONNECT_MASKED);
				
#if !NET_4_0
                ((CommandBarButton)m_cmdBarCtrlConnect).State = MsoButtonState.msoButtonDown;
				m_cmdBarBtnConnect.State = MsoButtonState.msoButtonDown;
#endif

			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void PopulateConnections(IList<ConnectionDetails> listConnections)
		{
			try
			{
				if (listConnections != null && listConnections.Count > 0)
				{

					comboBoxFilePath.Items.Clear();
					comboBoxFilePath.Items.Add(Helper.GetResourceString(Constants.COMBOBOX_DEFAULT_TEXT));
					foreach (ConnectionDetails connectionDetails in listConnections)
					{
						if (connectionDetails.ConnParam.Host == null)
							comboBoxFilePath.Items.Add(new ComboItem(connectionDetails.ConnParam.Connection, connectionDetails.ConnParam.ConnectionReadOnly,connectionDetails.CustomConfigAssemblyPath ));
                            
						else
						{
							comboBoxFilePath.Items.Add(new ComboItem(connectionDetails.ConnParam.Connection,false,connectionDetails.CustomConfigAssemblyPath   ));
							textBoxHost.Text = connectionDetails.ConnParam.Host;
							textBoxPort.Text = connectionDetails.ConnParam.Port.ToString();
							textBoxUserName.Text = connectionDetails.ConnParam.UserName;
							textBoxPassword.Focus();
						}
					   
					}
					if (comboBoxFilePath.Items.Count > 1)
					{
						comboBoxFilePath.SelectedIndex = 1;
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
	
	private void Login_Load(object sender, EventArgs e)
		{
			try
			{
			
				LoadAppropriatedata();
				SetLiterals();
				
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

	private void radioButton_Click(object sender, EventArgs e)
		{
			try
			{

				ClearPanelControls();
				if (radioButtonLocal.Checked)
				{
					
					PopulateConnections(OMEInteraction.FetchAllConnections(false));
					panelLocal.Visible = true;
					panelRemote.Visible = false;
				}
				else
				{
					
				
					PopulateConnections(OMEInteraction.FetchAllConnections(true));
					panelLocal.Visible = false;
					panelRemote.Visible = true;
					
					
					m_cmdBarCtrlBackup.Enabled = false;
					m_cmdBarCtrlCreateDemoDb.Enabled = true;
				}
				
				
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
		
		private void buttonBrowse_Click(object sender, EventArgs e)
		{
			try
			{
				
				openFileDialog.Filter = OPEN_FILE_DIALOG_FILTER;
				openFileDialog.Title = Helper.GetResourceString(Constants.LOGIN_OPEN_FILE_DIALOG_CAPTION);
				if (openFileDialog.ShowDialog() != DialogResult.Cancel)
				{
				    dbInteraction.SetAssemblyPathtoNull();
					textBoxConnection.Text = openFileDialog.FileName;
					toolTipForTextBox.SetToolTip(textBoxConnection, textBoxConnection.Text);
					if (comboBoxFilePath.Items.Contains(textBoxConnection.Text))
						comboBoxFilePath.SelectedItem = textBoxConnection.Text;
				    txtCustomConfigAssemblyPath.Clear();
				    
				}

			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
		
		private void buttonConnect_Click(object sender, EventArgs e)
		{
            try
            {
                ConnParams conparam;
                checkCustomConfig = false;
                if (radioButtonLocal.Checked)
                {
                    if (!(Validations.ValidateLocalLoginParams(ref comboBoxFilePath, ref textBoxConnection)))
                        return;
                    conparam = new ConnParams(textBoxConnection.Text.Trim(), chkReadOnly.Checked);

                }
                else
                {
                    if (!(Validations.ValidateRemoteLoginParams(ref comboBoxFilePath, ref textBoxHost, ref textBoxPort,
                                                                ref textBoxUserName, ref textBoxPassword)))
                        return;

                    string connection = STRING_SERVER + textBoxHost.Text.Trim() + STRING_COLON + textBoxPort.Text.Trim() +
                                        STRING_COLON +
                                        textBoxUserName.Text.Trim();
                    conparam = new ConnParams(connection, textBoxHost.Text.Trim(), textBoxUserName.Text.Trim(),
                                              textBoxPassword.Text.Trim(), Convert.ToInt32(textBoxPort.Text.Trim()));

                }
                dbInteraction.SetPathForConnection(conparam.Connection);
                CheckCustomConfig();

                if (CreateAppDomain())
                    ConnectAfterCreatingNewAppDomain(conparam);
            }
            catch (Exception oEx)
            {
                LoggingHelper.ShowMessage(oEx);
            }
		}

        private void ConnectAfterCreatingNewAppDomain(ConnParams conparam)
        {
            ConnectionDetails currConnectionDetails = new ConnectionDetails(conparam);
            long id = OMEInteraction.ChkIfRecentConnIsInDb(conparam);
            if (id > 0)
            {
                currConnectionDetails = OMEInteraction.GetConnectionDetailsObject(id);
                if (currConnectionDetails != null)
                    currConnectionDetails.ConnParam.ConnectionReadOnly = chkReadOnly.Checked;
            }

            currConnectionDetails.CustomConfigAssemblyPath = txtCustomConfigAssemblyPath.Text.Trim();
            string exceptionString = AssemblyInspectorObject.Connection.ConnectToDatabase(currConnectionDetails,
                                                                                          checkCustomConfig);

            if (exceptionString == string.Empty)
            {
                SaveConnectionAndCreateToolWindows(currConnectionDetails);
                CreateQueryBuilderToolWindow();
            }
            else
            {
                AssemblyInspectorObject.Connection.Closedb();
                textBoxConnection.Clear();
                MessageBox.Show(exceptionString,
                                Helper.GetResourceString(Common.Constants.PRODUCT_CAPTION),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                AppDomain.Unload(appDomain.workerAppDomain);
                dbInteraction.SetAssemblyPathtoNull();
            }
        }


	    private void SaveConnectionAndCreateToolWindows(ConnectionDetails currConnectionDetails)
	    {
	        OMEInteraction.SetCurrentRecentConnection(currConnectionDetails.ConnParam );
	        OMEInteraction.SaveRecentConnection(currConnectionDetails);
	        dbInteraction.SaveAssemblyPath(currConnectionDetails.ConnParam.Connection);
	        AfterSuccessfullyConnected();
            dbInteraction.SetAssemblyPathtoNull();
	        loginToolWindow.Close(vsSaveChanges.vsSaveChangesNo);
	        Helper.CheckIfLoginWindowIsVisible = false;
	        ObjectBrowserToolWin.CreateObjectBrowserToolWindow();
	        ObjectBrowserToolWin.ObjBrowserWindow.Visible = true;

	        PropertyPaneToolWin.CreatePropertiesPaneToolWindow(true);
	        PropertyPaneToolWin.PropWindow.Visible = true;
	    }

	    private void CheckCustomConfig()
	    {
            if (txtCustomConfigAssemblyPath.Text.Trim() != string.Empty)
            {
                if (!File.Exists(txtCustomConfigAssemblyPath.Text.Trim()))
                    MessageBox.Show(
                        txtCustomConfigAssemblyPath.Text.Trim() +
                        " does not exist. Please remove it using 'delete' button.", Helper.GetResourceString(Constants.PRODUCT_CAPTION),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                else
                {
                    checkCustomConfig = CreateCustomConfigAppDomain();
                    if (!checkCustomConfig)
                    {
                        MessageBox.Show(
                            "Custom Configuration assembly does not contain valid configuration, please add valid configurations and load again. ",
                            Helper.GetResourceString(Constants.PRODUCT_CAPTION),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
	    }

	    public bool CreateAppDomain()
        {
           appDomain = new AppDomainDetails();
           bool check = appDomain.LoadAppDomain(dbInteraction.GetAssemblySearchPath());
            if (checkCustomConfig)
                appDomain.LoadAppDomain(txtCustomConfigAssemblyPath.Text.Trim());
            return check;
        }

	    public bool CreateCustomConfigAppDomain()
        {
            customConfigAppDomain = new CustomConfigAppDomain(radioButtonLocal.Checked  );
            customConfigAppDomain.LoadAppDomain(dbInteraction.GetAssemblySearchPath()  );
	        return customConfigAppDomain.LoadAppDomain(txtCustomConfigAssemblyPath.Text.Trim());
        }

	    private void comboBoxFilePath_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (radioButtonRemote.Checked)
				{
					if (!comboBoxFilePath.Text.Equals(Helper.GetResourceString(Common.Constants.COMBOBOX_DEFAULT_TEXT)))
					{
                        ComboItem comboItem = comboBoxFilePath.SelectedItem as ComboItem;
						string[] strRemote = comboBoxFilePath.Text.Split(CHAR_COLON);
						textBoxHost.Text = strRemote[1];
						textBoxPort.Text = strRemote[2];
						textBoxUserName.Text = strRemote[3];
						textBoxPassword.Focus();
						toolTipForTextBox.SetToolTip(comboBoxFilePath, comboBoxFilePath.SelectedItem.ToString());
                        txtCustomConfigAssemblyPath.Text = comboItem.ConfigPath;
					}
					else
					{
						textBoxHost.Clear();
						textBoxPort.Clear();
						textBoxUserName.Clear();
						textBoxPassword.Clear();
					    txtCustomConfigAssemblyPath.Clear();
					}
				}
				else
				{
					if (!comboBoxFilePath.Text.Equals(Helper.GetResourceString(Common.Constants.COMBOBOX_DEFAULT_TEXT)))
					{
						ComboItem comboItem = comboBoxFilePath.SelectedItem as ComboItem;
						textBoxConnection.Text = comboItem.ToString();
						chkReadOnly.Checked = comboItem.ReadonlyParam;
						toolTipForTextBox.SetToolTip(comboBoxFilePath, comboBoxFilePath.SelectedItem.ToString());
					    txtCustomConfigAssemblyPath.Text = comboItem.ConfigPath;
					}
					else
					{
						textBoxConnection.Clear();
						chkReadOnly.Checked = false;
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void comboBoxFilePath_DropdownItemSelected(object sender, ToolTipComboBox.DropdownItemSelectedEventArgs e)
		{
			try
			{
				if (e.SelectedItem < 0 || e.Scrolled) toolTipForTextBox.Hide(comboBoxFilePath);
				else
					toolTipForTextBox.Show(comboBoxFilePath.Items[e.SelectedItem].ToString(), comboBoxFilePath, e.Bounds.Location.X + Cursor.Size.Width, e.Bounds.Location.Y + Cursor.Size.Height);
			}
			catch (Exception ex)
			{
				LoggingHelper.HandleException(ex);
			}
		}
	
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			try
			{
				textBoxPort.Clear();
				textBoxHost.Clear();
				textBoxConnection.Clear();
				textBoxPassword.Clear();
				textBoxUserName.Clear();
			    txtCustomConfigAssemblyPath.Clear(); 

                loginToolWindow.Close(vsSaveChanges.vsSaveChangesNo);
				Helper.CheckIfLoginWindowIsVisible = false;
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
		
		private void textBoxPort_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				char c = e.KeyChar;

				//Allow only numeric charaters in filter textbox.
				if (!Helper.IsNumeric(c.ToString()))
					e.Handled = true;
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void textBoxPort_TextChanged(object sender, EventArgs e)
		{
			int result;
			if (!Int32.TryParse(textBoxPort.Text.Trim(), out result))
			{
				textBoxPort.Text = string.Empty;
			}
		}

		private void textBoxPort_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Modifiers == Keys.Control)
				e.Handled = true;
		}

		class ComboItem
		{
			private string m_Name;
			private bool m_Value;
		    private string m_customconfig;
			public ComboItem(string name, bool in_value, string config)
			{
				m_Name = name;
				m_Value = in_value;
			    m_customconfig = config;
			}

			public bool ReadonlyParam
			{
				get { return m_Value; }
			}
            public string ConfigPath
            {
                get { return m_customconfig; }
            }
			public override string ToString()
			{
				return m_Name;
			}

		}

        private void btnAddAssemblies_Click(object sender, EventArgs e)
        {
            dbInteraction.SetAssemblyPathtoNull();
            if(radioButtonLocal.Checked  )
            new ChooseAssemblies(textBoxConnection.Text.Trim()).Show();
            else
            {
                string connection = STRING_SERVER + textBoxHost.Text.Trim() + STRING_COLON + textBoxPort.Text.Trim() + STRING_COLON +
					                    textBoxUserName.Text.Trim();
                new ChooseAssemblies(connection).Show( );
            }
        }

       

        private void btnBrowseCustomConfig_Click(object sender, EventArgs e)
        {
            try
            {

                openFileDialog.Filter = OPEN_FILE_ADDASSEMBLY_FILTER;
                openFileDialog.Title = Helper.GetResourceString(Constants.LOGIN_OPEN_FILE_DIALOG_CAPTION);
                if (openFileDialog.ShowDialog() != DialogResult.Cancel)
                    txtCustomConfigAssemblyPath.Text = openFileDialog.FileName;
                 
            }
            catch (Exception oEx)
            {
                LoggingHelper.ShowMessage(oEx);
            }
        }

        private void btnClearCustomConfig_Click(object sender, EventArgs e)
        {
            if (comboBoxFilePath.SelectedItem != null)
            {
                ConnParams param = new ConnParams(comboBoxFilePath.SelectedItem.ToString());
                OMEInteraction.DeleteConfigConnection(txtCustomConfigAssemblyPath.Text.Trim(), param);
            }
            txtCustomConfigAssemblyPath.Clear();
        }
	}
}
