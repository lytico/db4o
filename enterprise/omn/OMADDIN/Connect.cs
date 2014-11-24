/* Copyright (C) 2004 - 2009  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using OMAddinDataTransferLayer;
using OManager.BusinessLayer.UIHelper;
using OManager.DataLayer.CommonDatalayer;
using OMControlLibrary;
using OMControlLibrary.Common;
using OMControlLibrary.LoginToSalesForce;
using OManager.BusinessLayer.Login;
using OME.Logging.Common;
using OME.Logging.ExceptionLogging;
using OME.Logging.Tracing;
using stdole;
using Constants = OMControlLibrary.Common.Constants;

namespace OMAddin
{
    public class Connect : IDTExtensibility2, IDTCommandTarget
	{
		#region Private Variables

		private DTE2 _applicationObject;
		private AddIn _addInInstance;
		private CommandBar omToolbar;
		private CommandBarPopup oPopup;
		private CommandBarEvents dbConnectControlHandler;
		private CommandBarEvents db4oControlHandler;
		private CommandBarEvents db4oDnldControlHandler;
		private CommandBarEvents salesForceControlHandler;
		private CommandBarEvents omHelpControlHandler;
		private CommandBarEvents reqConsultationControlHandler;
		private CommandBarEvents omProxyConfigHandler;
		private CommandBarEvents omBackupControlHandler;
		private CommandBarEvents omObjectBrowserControlHandler;
		private CommandBarEvents db4oDeveloperControlHandler;
		private CommandBarEvents omAboutControlHandler;

		private CommandBarEvents dbCreateDemoDbControlHandler;
		private CommandBarEvents omQueryBuilderControlHandler;
		private CommandBarEvents omPropertiesControlHandler;

		private CommandBarControl dbCreateDemoDbControl;
		private CommandBarControl connectDatabaseMenu;
		private CommandBarControl omProxyConfigControl;
		private CommandBarControl omBackupControl;

		private CommandBarControl omObjectBrowserControl;
		private CommandBarControl omQueryBuilderControl;
		private CommandBarControl omPropertiesControl;

		private CommandBarControl reqConsultationControl;
		private CommandBarControl db4oDeveloperControl;
		private CommandBarControl db4oDnldControl;
		private CommandBarControl omHelpControl;
		private CommandBarControl omAboutControl;

		private CommandBarButton connectDatabaseButton;
		private CommandBarButton db4oHelpControlButton;
		private CommandBarButton salesForceControlButton;
		private CommandBarButton reqConsultationControlButton;
		private CommandBarControl salesForceControl;

		private Window windb4oHome;
		private Window windb4oDownloads;
		private Window winSupportdb4o;
		private Window winRequestConsultation;
		private Window winHelp;
		private Window windb4oDeveloper;

		private WindowEvents _windowsEvents;


		#endregion


		#region Private Constants

		private const string IMAGE_CONNECT = "OMAddin.Images.DBconnect.gif";
		private const string IMAGE_CONNECT_MASKED = "OMAddin.Images.DBconnect_Masked.bmp";

		private const string IMAGE_DISCONNECT = "OMAddin.Images.DB_DISCONNECT2_a.GIF";
		private const string IMAGE_DISCONNECT_MASKED = "OMAddin.Images.DB_DISCONNECT2_b.BMP";

		private const string IMAGE_XTREMECONNECT = "OMAddin.Images.XtremeConnct_2.gif";
		private const string IMAGE_XTREMECONNECT_MASKED = "OMAddin.Images.XtremeConnct_2_Masked.bmp";

		private const string IMAGE_SUPPORTCASES = "OMAddin.Images.SupportCases.gif";
		private const string IMAGE_SUPPORTCASES_MASKED = "OMAddin.Images.SupportCases_Masked.bmp";

		private const string IMAGE_HELP = "OMAddin.Images.support1.gif";
		private const string IMAGE_HELP_MASKED = "OMAddin.Images.support1_Masked.bmp";

		private const string COMMAND_NAME = "OMAddin.Connect.ObjectManager Enterprise";
		private const string DB4O_HOMEPAGE = "db4objects Homepage";
		private const string TOOLS = "Tools";
		private const string CONNECT = "Connect";
		private const string XTREME_CONNECT = "XtremeConnect";
		private const string SUPPORT_CASES = "Support Cases";
		private const string MAINTAINANCE = "Maintainance";
		private const string OPTIONS = "Options";
		private const string PROXYCONFIGURATIONS = "Proxy Configurations";
		private const string ASSEMBLY_SEARCH_PATH_CONFIG = "Assembly search path...";


		private const string BACKUP = "Backup";
		private const string DB4O_DEVELOPER_COMMUNITY = "db4objects Developer Community";
		private const string DB4O_DOWNLOADS = "db4objects Downloads";
		private const string DB4O_HELP = "Help";
		private const string ABOUT_OME = "About ObjectManager Enterprise";

		private const string CREATE_DEMO_DB = "Create Demo Database";

		private const string CONTACT_SALES = @"/ContactSales/ContactSales.htm";
		private const string FAQ_PATH = @"FAQ/FAQ.htm";

		private const string URL_DB4O_DEVELOPER = "http://developer.db4o.com";
		private const string URL_DB4O_DOWNLOADS = "http://developer.db4o.com/Downloads.aspx";
		private const string URL_DB4O_HOMEPAGE = "http://db4o.com";

		#endregion

		
	
		//FIXME: Do not catch "Exception" everywhere.
		#region Connect Constructor
		/// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
		public Connect()
		{
            try
            {
                OMETrace.Initialize();
            }
            catch (Exception ex)
            {
                
                ex.ToString();
            }

			try
			{
				ExceptionHandler.Initialize();
			}
			catch (Exception ex)
			{
				ex.ToString();//ignore
			}

			try
			{
				ApplicationManager.CheckLocalAndSetLanguage();
			}
			catch (Exception ex)
			{
				ex.ToString();//ignore
			}
		}
        
		#endregion
		DTEEvents _eve;
		#region Connect Event Handlers

		#region OnConnection
		/// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being                       loaded.</summary>
		/// <param term='application'>Root object of the host application.</param>
		/// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
		/// <param term='addInInst'>Object representing this Add-in.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
			_applicationObject = (DTE2)application;
			_addInInstance = (AddIn)addInInst;
			ViewBase.ResetToolWindowList();
			OutputWindow.Initialize(_applicationObject);

			try
			{
				if (connectMode == ext_ConnectMode.ext_cm_AfterStartup || connectMode == ext_ConnectMode.ext_cm_Startup)
				{
					CreateMenu();


                   
					try
					{
                        CommandBars toolBarCommandBars = ((CommandBars)_applicationObject.CommandBars);
                        string toolbarName = Helper.GetResourceString(OMControlLibrary.Common.Constants.PRODUCT_CAPTION);
                        try
                        {
                             
                            omToolbar = toolBarCommandBars.Add(toolbarName, MsoBarPosition.msoBarTop, Type.Missing, false);
                        }
                        catch (ArgumentException)
                        {
                            omToolbar = toolBarCommandBars[toolbarName];
                        }
						CreateToolBar();
						omToolbar.Visible = true;

					}
					catch (Exception oEx)
					{
						LoggingHelper.HandleException(oEx);
					}

					try
					{
						Events events = _applicationObject.Events;
						_windowsEvents = events.get_WindowEvents(null);

						_windowsEvents.WindowActivated += OnWindowActivated;

						_eve = _applicationObject.Events.DTEEvents;
						_eve.ModeChanged += DesignDebugModeChanged;

                        
					}
					catch (Exception oEx)
					{
						LoggingHelper.HandleException(oEx);
					}

					try
					{
						//This function checks whether user already logged in.
						ViewBase.ApplicationObject = _applicationObject;
						//enable disable connect button while checking cfredentials
						connectDatabaseMenu.Enabled = false;
						connectDatabaseButton.Enabled = false;
						Cursor.Current = Cursors.WaitCursor;
						Cursor.Current = Cursors.Default;
						connectDatabaseMenu.Enabled = true;
						connectDatabaseButton.Enabled = true;
					}
					catch (Exception oEx)
					{
						LoggingHelper.HandleException(oEx);
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}

		}



		static void DesignDebugModeChanged(vsIDEMode LastMode)
		{
			//TODO:
			//Since there is no way to kill toolwindows until the IDE closes, sometimes tool windows of other database also appears
			// to avoid such situation first make all the toolwindows invisible which are not for current db and then 
			//play around with current databse toolwindow
			//Find a way if this can be avoided.

			MakeAllQueryResultWindowsInvisibleWhichDoNotBelongToCurrentDatabase();

			if ( AssemblyInspectorObject.Connection!=null && AssemblyInspectorObject.Connection.DbConnectionStatus() )
			{

				ForEachOMNWindow(delegate(Window window)
									{
										window.Visible = (window.Caption != Constants.LOGIN);
									});

			}
			else
			{
				ForEachOMNWindow(delegate(Window window)
									{
										window.Visible = (window.Caption == Constants.LOGIN && Helper.CheckIfLoginWindowIsVisible);
									});

			}
			DockWindowsIfRequired();

		}

		private static void MakeAllQueryResultWindowsInvisibleWhichDoNotBelongToCurrentDatabase()
		{

			foreach (Window win in ViewBase.ApplicationObject.ToolWindows.DTE.Windows)
			{
				if (win.Object is QueryResult && !ViewBase.GetAllPluginWindows().Contains(win))
				{
					win.Visible = false;

				}
			}
		}

		private static void DockWindowsIfRequired()
		{
			ForEachOMNWindow(delegate(Window window)
			{
				if (window.Object is QueryResult)
				{
					window.IsFloating = false;
					window.Linkable = false;
				}
			});
		}

		private static void ForEachOMNWindow(Action<Window> action)
		{

			foreach (Window w in ViewBase.GetAllPluginWindows())
			{
				action(w);
			}

		}

		#endregion

		#region OnDisconnection
		/// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being                     unloaded.</summary>
		/// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{
			try
			{
				//This function Aborts the current session
				CloseAllToolWindows();
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
			try
			{
				if (oPopup != null)
				{
					oPopup.Delete(null);
					oPopup = null;
				}
			}
			catch (System.Runtime.InteropServices.InvalidComObjectException oEx)
			{
				oEx.ToString(); // Ignore
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
            try
            {
                if (omToolbar != null)
                    omToolbar.Delete();
            }
            catch (System.Runtime.InteropServices.InvalidComObjectException oEx)
            {
                oEx.ToString(); // Ignore
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
		}
		#endregion

		#region OnAddInsUpdate
		/// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of                           Add-ins has changed.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />		
		public void OnAddInsUpdate(ref Array custom)
		{
		}

		#endregion

		#region OnStartupComplete
		/// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host                             application has completed loading.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref Array custom)
		{
			if (omToolbar != null)
			{
				omToolbar.Visible = true;
			}
		}
		#endregion

		#region OnBeginShutdown
		/// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host                                   application is being unloaded.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref Array custom)
		{
			CloseAllToolWindows();
		}
		#endregion

		#region QueryStatus
		/// <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is                      updated</summary>
		/// <param term='commandName'>The name of the command to determine state for.</param>
		/// <param term='neededText'>Text that is needed for the command.</param>
		/// <param term='status'>The state of the command in the user interface.</param>
		/// <param term='commandText'>Text requested by the neededText parameter.</param>
		/// <seealso class='Exec' />
		public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
		{
			try
			{
				if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
				{
					if (commandName == COMMAND_NAME)
					{
						status = vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
						return;
					}
					status = vsCommandStatus.vsCommandStatusUnsupported;
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}

		}
		#endregion

		#region Exec
		/// <summary>Implements the Exec method of the IDTCommandTarget interface. This is called when the command is invoked.</summary>
		/// <param term='commandName'>The name of the command to execute.</param>
		/// <param term='executeOption'>Describes how the command should be run.</param>
		/// <param term='varIn'>Parameters passed from the caller to the command handler.</param>
		/// <param term='varOut'>Parameters passed from the command handler to the caller.</param>
		/// <param term='handled'>Informs the caller if the command was handled or not.</param>
		/// <seealso class='Exec' />
		public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
		{
			try
			{
				handled = false;
				if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
				{
					if (commandName == COMMAND_NAME)
					{
						
						// Add your command execution here 
						handled = true;
						return;
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}

		}
		#endregion

		#endregion

		#region Event Handlers

		#region OnWindowActivated
		/// <summary>
		/// This event handler gets the Activated event of tool window.
		/// When db4o Browser opens for first time, it creates menu option under ObjectManager Enterprise menu.
		/// Using this menu option, user can get back to db4o Browser if he has closed it before.
		/// </summary>
		/// <param name="gotFocus"></param>
		/// <param name="lostFocus"></param>
		void OnWindowActivated(Window gotFocus, Window lostFocus)
		{
			try
			{

				//TODO: Move this code closer to window instantiation.
				if (oPopup != null)
				{
					if (gotFocus.Caption.Equals(Constants.DB4OBROWSER) && omObjectBrowserControl == null)
					{

						omObjectBrowserControlHandler = AddControlToToolbar(ref omObjectBrowserControl, Constants.DB4O_BROWSER_CAPTION);
						omObjectBrowserControlHandler.Click += omObjectBrowserControlHandler_Click;

					}
					else if (gotFocus.Caption.Equals(Constants.QUERYBUILDER) && omQueryBuilderControl == null)
					{

						omQueryBuilderControlHandler = AddControlToToolbar(ref omQueryBuilderControl, Constants.QUERY_BUILDER_CAPTION);
						omQueryBuilderControlHandler.Click += omQueryBuilderControlHandler_Click;

					}
					else if (gotFocus.Caption.Equals(Constants.DB4OPROPERTIES) && omPropertiesControl == null)
					{
						omPropertiesControlHandler = AddControlToToolbar(ref omPropertiesControl, Constants.PROPERTIES_TAB_CAPTION);
						omPropertiesControlHandler.Click += omPropertiesControlHandler_Click;

					}
				}


			}
			catch (System.Runtime.InteropServices.COMException)
			{
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		private CommandBarEvents AddControlToToolbar(ref CommandBarControl ctrl, string caption)
		{
			ctrl = oPopup.Controls.Add(MsoControlType.msoControlButton,
														 Missing.Value,
														 Missing.Value,
														 11, true);

			ctrl.BeginGroup = true;
			ctrl.Caption = Helper.GetResourceString(caption);
			return (CommandBarEvents)_applicationObject.Events.get_CommandBarEvents(ctrl);
		}

		#endregion


		#region dbConnectControlHandler_Click
		/// <summary>
		/// This event handler opens the Login tool window.
		/// </summary>
		/// <param name="CommandBarControl"></param>
		/// <param name="Handled"></param>
		/// <param name="CancelDefault"></param>
		void dbConnectControlHandler_Click(object CommandBarControl, ref bool Handled, ref bool CancelDefault)
		{
			Cursor.Current = Cursors.WaitCursor;

			ConnectToDatabaseOrServer((_CommandBarButton)CommandBarControl);
			Cursor.Current = Cursors.Default;

		}
		#endregion
		#region omButton_Click
		void omButton_Click(CommandBarButton Ctrl, ref bool CancelDefault)
		{
			Cursor.Current = Cursors.WaitCursor;

			ConnectToDatabaseOrServer(Ctrl);
			Cursor.Current = Cursors.Default;

		}
		#endregion

		#region reqConsultationControlHandler_Click
		void reqConsultationControlHandler_Click(object CommandBarControl, ref bool Handled, ref bool CancelDefault)
		{
			Cursor.Current = Cursors.WaitCursor;
			XtremeConnect();
			Cursor.Current = Cursors.Default;

		}
		#endregion
		#region reqConsultationControlButton_Click
		void reqConsultationControlButton_Click(CommandBarButton Ctrl, ref bool CancelDefault)
		{
			Cursor.Current = Cursors.WaitCursor;
			XtremeConnect();
			Cursor.Current = Cursors.Default;
		}
		#endregion

		#region salesForceControlHandler_Click
		void salesForceControlHandler_Click(object CommandBarControl, ref bool Handled, ref bool CancelDefault)
		{
			Cursor.Current = Cursors.WaitCursor;
			SupportCases();
			Cursor.Current = Cursors.Default;
		}
		#endregion
		#region salesForceControlButton_Click
		void salesForceControlButton_Click(CommandBarButton Ctrl, ref bool CancelDefault)
		{
			Cursor.Current = Cursors.WaitCursor;
			SupportCases();
			Cursor.Current = Cursors.Default;
		}
		#endregion

		#region omBackupControlHandler_Click
		/// <summary>
		/// This event handler takes backup of currently connected db4o database.
		/// </summary>
		/// <param name="CommandBarControl"></param>
		/// <param name="Handled"></param>
		/// <param name="CancelDefault"></param>
		void omBackupControlHandler_Click(object CommandBarControl, ref bool Handled, ref bool CancelDefault)
		{
			BackupDatabase();
		}
		#endregion


		#region db4oControlHandler_Click
		void db4oControlHandler_Click(object CommandBarControl, ref bool Handled, ref bool CancelDefault)
		{
			Cursor.Current = Cursors.WaitCursor;
			Opendb4oHomepage();
			Cursor.Current = Cursors.Default;
		}
		#endregion
		#region db4oControlButton_Click

		#endregion

		#region db4oDeveloperControlHandler_Click
		void db4oDeveloperControlHandler_Click(object CommandBarControl, ref bool Handled, ref bool CancelDefault)
		{
			Cursor.Current = Cursors.WaitCursor;
			Opendb4oDeveloper();
			Cursor.Current = Cursors.Default;
		}

		#endregion

		#region db4oDnldControlHandler_Click
		void db4oDnldControlHandler_Click(object CommandBarControl, ref bool Handled, ref bool CancelDefault)
		{
			Cursor.Current = Cursors.WaitCursor;
			Opendb4oDownloads();
			Cursor.Current = Cursors.Default;
		}
		#endregion
		#region db4oDnldControlButton_Click

		#endregion

		#region omHelpControlHandler_Click
		void omHelpControlHandler_Click(object CommandBarControl, ref bool Handled, ref bool CancelDefault)
		{
			Cursor.Current = Cursors.WaitCursor;
			OpenHelp();
			Cursor.Current = Cursors.Default;
		}
		#endregion
		#region db4oHelpControlButton_Click
		void db4oHelpControlButton_Click(CommandBarButton Ctrl, ref bool CancelDefault)
		{
			Cursor.Current = Cursors.WaitCursor;
			OpenHelp();
			Cursor.Current = Cursors.Default;
		}
		#endregion

		#region omAboutControlHandler_Click

		static void omAboutControlHandler_Click(object CommandBarControl, ref bool Handled, ref bool CancelDefault)
		{
			Cursor.Current = Cursors.WaitCursor;
			OpenOMEAboutBox();
			Cursor.Current = Cursors.Default;

		}
		#endregion

		#region omObjectBrowserControlHandler_Click
		/// <summary>
		/// This event handler reopens db4o tool window. 
		/// </summary>
		/// <param name="CommandBarControl"></param>
		/// <param name="Handled"></param>
		/// <param name="CancelDefault"></param>
		static void omObjectBrowserControlHandler_Click(object CommandBarControl, ref bool Handled, ref bool CancelDefault)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				ObjectBrowserToolWin.CreateObjectBrowserToolWindow();
				Cursor.Current = Cursors.Default;
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		static void omQueryBuilderControlHandler_Click(object CommandBarControl, ref bool Handled, ref bool CancelDefault)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				Login.CreateQueryBuilderToolWindow();
				Cursor.Current = Cursors.Default;
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		static void omPropertiesControlHandler_Click(object CommandBarControl, ref bool Handled, ref bool CancelDefault)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				//RecentQueries currRecentConnection = OMEInteraction.GetCurrentRecentConnection();
				//AssemblyInspectorObject.Connection.ConnectToDatabase(currRecentConnection);
				PropertyPaneToolWin.CreatePropertiesPaneToolWindow(true);
				Cursor.Current = Cursors.Default;
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		#endregion

		#endregion

		#region Private Methods

		#region AddSubMenu
		private int AddSubMenu(out CommandBarControl menuItem, CommandBarPopup parent, out CommandBarEvents eventHandler, int position, string caption)
		{
			return AddSubMenu(out menuItem, parent, out eventHandler, position, caption, string.Empty, string.Empty);
		}

		private int AddSubMenu(out CommandBarControl menuItem, CommandBarPopup parent, out CommandBarEvents eventHandler, int position, string caption, string imagePath, string maskedImagePath)
		{
			menuItem = parent.Controls.Add(MsoControlType.msoControlButton, Missing.Value, Missing.Value, position, true);
			menuItem.Caption = caption;

			eventHandler = (CommandBarEvents)_applicationObject.Events.get_CommandBarEvents(menuItem);

			if (!string.IsNullOrEmpty(imagePath))
			{
				Helper.SetPicture(Assembly.GetExecutingAssembly(), (CommandBarButton)menuItem.Control, imagePath, maskedImagePath);
			}
			else if (string.Equals(caption, DB4O_HOMEPAGE))
			{
				menuItem.BeginGroup = true;
			}

			return position + 1;
		}
		#endregion

		#region CreateMenu
		/// <summary>
		/// Creates Menu & Submenus under Tools menu.
		/// </summary>
		private void CreateMenu()
		{
			try
			{
				#region Creates ObjectManager Enterprise Menu item
				try
				{
					CommandBar oCommandBar = ((CommandBars)_applicationObject.CommandBars)[TOOLS];

					oPopup = (CommandBarPopup)oCommandBar.Controls.Add(MsoControlType.msoControlPopup, Missing.Value, Missing.Value, 1, true);
					oPopup.Caption = Helper.GetResourceString(OMControlLibrary.Common.Constants.PRODUCT_CAPTION);
				}
				catch (Exception oEx)
				{
					LoggingHelper.HandleException(oEx);
				}
				#endregion

				#region Creates submenu for Connect/Disconnect
				int position = AddSubMenu(out connectDatabaseMenu, oPopup, out dbConnectControlHandler, 1, CONNECT, IMAGE_CONNECT, IMAGE_CONNECT_MASKED);
				dbConnectControlHandler.Click += dbConnectControlHandler_Click;
				#endregion

				#region Creates submenu for XtremeConnect
				position = AddSubMenu(out reqConsultationControl, oPopup, out reqConsultationControlHandler, position, XTREME_CONNECT, IMAGE_XTREMECONNECT, IMAGE_XTREMECONNECT_MASKED);
				reqConsultationControlHandler.Click += reqConsultationControlHandler_Click;
				#endregion

				#region Creates submenu for Support Cases
				position = AddSubMenu(out salesForceControl, oPopup, out salesForceControlHandler, position, SUPPORT_CASES, IMAGE_SUPPORTCASES, IMAGE_SUPPORTCASES_MASKED);
				salesForceControlHandler.Click += salesForceControlHandler_Click;
				#endregion

				#region Creates submenu for Maintainance
				CommandBarPopup oPopupMaintainance = (CommandBarPopup)oPopup.Controls.Add(MsoControlType.msoControlPopup,
												 Missing.Value,
												 Missing.Value,
												 position, true);
				oPopupMaintainance.Caption = MAINTAINANCE;
				#endregion


				#region Creates submenu for Backup under Maintainance
				AddSubMenu(out omBackupControl, oPopupMaintainance, out omBackupControlHandler, 1, BACKUP);
				omBackupControlHandler.Click += omBackupControlHandler_Click;
				omBackupControl.Enabled = false;
				#endregion

				#region Creates submenu for db4objects Homepage

				CommandBarControl db4oHomePageControl;
				position = AddSubMenu(out db4oHomePageControl, oPopup, out db4oControlHandler, position, DB4O_HOMEPAGE);
				db4oControlHandler.Click += db4oControlHandler_Click;
				#endregion

				#region Creates submenu for db4objects Developer Community
				position = AddSubMenu(out db4oDeveloperControl, oPopup, out db4oDeveloperControlHandler, position, DB4O_DEVELOPER_COMMUNITY);
				db4oDeveloperControlHandler.Click += db4oDeveloperControlHandler_Click;
				#endregion

				#region Creates submenu for db4objects Downloads
				position = AddSubMenu(out db4oDnldControl, oPopup, out db4oDnldControlHandler, position, DB4O_DOWNLOADS);
				db4oDnldControlHandler.Click += db4oDnldControlHandler_Click;
				#endregion

				#region Creates submenu for Help
				position = AddSubMenu(out omHelpControl, oPopup, out omHelpControlHandler, position, DB4O_HELP, IMAGE_HELP, IMAGE_HELP_MASKED);
				omHelpControlHandler.Click += omHelpControlHandler_Click;
				#endregion

				#region Creates submenu for ObjectManager Enterprise About Box
				position = AddSubMenu(out omAboutControl, oPopup, out omAboutControlHandler, position, ABOUT_OME);
				omAboutControlHandler.Click += omAboutControlHandler_Click;
				#endregion

				#region Creates submenu for Creating demo db
				position = AddSubMenu(out dbCreateDemoDbControl, oPopup, out dbCreateDemoDbControlHandler, position, CREATE_DEMO_DB);
				dbCreateDemoDbControlHandler.Click += dbCreateDemoDbControlHandler_Click;
				dbCreateDemoDbControl.Enabled = true;
				#endregion

				#region Creates submenu for Options
				CommandBarPopup oPopupOptions = (CommandBarPopup)oPopup.Controls.Add(MsoControlType.msoControlPopup,
												   Missing.Value,
												   Missing.Value,
												   position, true);
				oPopupOptions.Caption = OPTIONS;
				#endregion

				#region Creates submenu for Proxy Configurations under Maintainance
				AddSubMenu(out omProxyConfigControl, oPopupOptions, out omProxyConfigHandler, 1, PROXYCONFIGURATIONS);
				omProxyConfigHandler.Click += omProxyConfigHandler_Click;
				omProxyConfigControl.Enabled = true;
				#endregion

				
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		

		static void omProxyConfigHandler_Click(object CommandBarControl, ref bool Handled, ref bool CancelDefault)
		{
			try
			{
				ProxyLogin pLoginwin = new ProxyLogin();
				pLoginwin.Text = "Proxy Login Configurations";
				pLoginwin.buttonLogin.Text = "&Save";
				pLoginwin.ShowDialog();
				if (pLoginwin.DialogResult == DialogResult.OK)
				{
					ProxyAuthentication pAuth = new ProxyAuthentication();
					pAuth.Port = pLoginwin.textBoxPort.Text;
					pAuth.ProxyAddress = pLoginwin.textBoxProxy.Text;
					pAuth.UserName = pLoginwin.textBoxUserID.Text;
					pAuth.PassWord = Helper.EncryptPass(pLoginwin.textBoxPassword.Text);
					OMEInteraction.SetProxyInfo(pAuth);
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		#region Creating demo db handler click
		void dbCreateDemoDbControlHandler_Click(object CommandBarControl, ref bool Handled, ref bool CancelDefault)
		{
            try
            {
                Cursor.Current = Cursors.WaitCursor;
              
                if (Login.appDomain == null || Login.appDomain.workerAppDomain == null)
                {
                   
                    Login l = new Login();
                    l.CreateAppDomain();

                }
                CreateDemoDbMethod();

                Cursor.Current = Cursors.Default;
            }
            catch(Exception e)
            {
                LoggingHelper.ShowMessage(e);
            }
		}
		#endregion

		private void CreateDemoDbMethod()
		{
			try
			{
				bw = new BackgroundWorker();
				bw.WorkerSupportsCancellation = true;
				bw.WorkerReportsProgress = true;

				bw.ProgressChanged += bw_ProgressChanged;
				bw.DoWork += bw_DoWork;
				bw.RunWorkerCompleted += bw_RunWorkerCompleted;

				isrunning = true;
				bw.RunWorkerAsync();

				for (double i = 1; i < 10000; i++)
				{
					i++;
					bw.ReportProgress((int)i * 100 / 1000);

					if (isrunning == false)
						break;
				}

			}
			catch (Exception oEx)
			{
				bw.CancelAsync();
				bw = null;
				LoggingHelper.HandleException(oEx);
			}
		}

		#region Function to create demo db
		void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			OpenDemoDb();
			isrunning = false;
			_applicationObject.StatusBar.Clear();
			_applicationObject.StatusBar.Progress(false, "Creation successful", 0, 0);

			_applicationObject.StatusBar.Text = "Creation successful!";
		}

		void createdemo()
		{
			try
			{
				connectDatabaseMenu.Enabled = false;
				connectDatabaseButton.Enabled = false;
				dbCreateDemoDbControl.Enabled = false;

				ViewBase.ApplicationObject = _applicationObject;

				CloseAllToolWindows();

				DemoDb.Create();
				connectDatabaseMenu.Enabled = true;
				connectDatabaseButton.Enabled = true;
				dbCreateDemoDbControl.Enabled = true;


			}
			catch (Exception e1)
			{
				ViewBase.ApplicationObject.StatusBar.Clear();
				ViewBase.ApplicationObject.StatusBar.Animate(false, vsStatusAnimation.vsStatusAnimationBuild);
				LoggingHelper.HandleException(e1);
			}
		}
		#endregion
		void bw_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				_applicationObject.StatusBar.Animate(true, vsStatusAnimation.vsStatusAnimationBuild);
				createdemo();
				_applicationObject.StatusBar.Animate(false, vsStatusAnimation.vsStatusAnimationBuild);
			}
			catch (Exception oEx)
			{
				bw.CancelAsync();
				bw = null;
				LoggingHelper.HandleException(oEx);
			}

		}
		bool isrunning = true;
		BackgroundWorker bw;



		void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			try
			{
				_applicationObject.StatusBar.Progress(true, "Creating Demo database.... ", e.ProgressPercentage * 10, 10000);

			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		private void OpenDemoDb()
		{
			try
			{
				Assembly ThisAssembly = Assembly.GetExecutingAssembly();

				ObjectBrowserToolWin.CreateObjectBrowserToolWindow();
				ObjectBrowserToolWin.ObjBrowserWindow.Visible = true;
				Login.CreateQueryBuilderToolWindow();
				PropertyPaneToolWin.CreatePropertiesPaneToolWindow(true);
				PropertyPaneToolWin.PropWindow.Visible = true;
				dbCreateDemoDbControl.Enabled = false;
				connectDatabaseMenu.Caption = OMControlLibrary.Common.Constants.TOOLBAR_DISCONNECT;
				connectDatabaseButton.Caption = OMControlLibrary.Common.Constants.TOOLBAR_DISCONNECT;
				connectDatabaseButton.TooltipText = OMControlLibrary.Common.Constants.TOOLBAR_DISCONNECT;

				Helper.SetPicture(ThisAssembly, connectDatabaseButton, IMAGE_DISCONNECT, IMAGE_DISCONNECT_MASKED);

				omBackupControl.Enabled = true;

#if !NET_4_0
				((CommandBarButton)connectDatabaseMenu).State = MsoButtonState.msoButtonDown;
				connectDatabaseButton.State = MsoButtonState.msoButtonDown;
#endif

			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}

		}

		#endregion

		#region AddToolBarButton
		private void AddToolBarButton(ref CommandBarButton CommandBarButton, MsoButtonStyle Style, string Caption, string ToolTip, string ImagePath, string MaskImagePath)
		{
			try
			{
				Assembly ThisAssembly = Assembly.GetExecutingAssembly();
				CommandBarButton = (CommandBarButton)omToolbar.Controls.Add(MsoControlType.msoControlButton, 1, "", Type.Missing, true);
				CommandBarButton.Caption = Caption;
				CommandBarButton.TooltipText = ToolTip;
				CommandBarButton.Style = Style;
				CommandBarButton.Visible = true;


				Helper.SetPicture(ThisAssembly, CommandBarButton, ImagePath, MaskImagePath);
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		#endregion

		#region CreateToolBar
		private void CreateToolBar()
		{
			try
			{
				
				AddToolBarButton(ref connectDatabaseButton, MsoButtonStyle.msoButtonIcon, CONNECT, CONNECT, IMAGE_CONNECT, IMAGE_CONNECT_MASKED);
				connectDatabaseButton.Click += omButton_Click;
				connectDatabaseButton.BeginGroup = true;

				AddToolBarButton(ref reqConsultationControlButton, MsoButtonStyle.msoButtonIcon, XTREME_CONNECT, XTREME_CONNECT, IMAGE_XTREMECONNECT, IMAGE_XTREMECONNECT_MASKED);
				reqConsultationControlButton.Click += reqConsultationControlButton_Click;
				reqConsultationControlButton.BeginGroup = true;

				AddToolBarButton(ref salesForceControlButton, MsoButtonStyle.msoButtonIcon, SUPPORT_CASES, SUPPORT_CASES, IMAGE_SUPPORTCASES, IMAGE_SUPPORTCASES_MASKED);
				salesForceControlButton.Click += salesForceControlButton_Click;
				salesForceControlButton.BeginGroup = true;

				AddToolBarButton(ref db4oHelpControlButton, MsoButtonStyle.msoButtonIcon, DB4O_HELP, DB4O_HELP, IMAGE_HELP, IMAGE_HELP_MASKED);
				db4oHelpControlButton.Click += db4oHelpControlButton_Click;
				db4oHelpControlButton.BeginGroup = true;

				
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}

		}

		#endregion

		#region ConnectToDatabaseOrServer
		private void ConnectToDatabaseOrServer(_CommandBarButton Ctrl)
		{
			try
			{
				Assembly assembly = Assembly.GetExecutingAssembly();
				if (Ctrl.Caption.Equals(CONNECT))
				{
					ViewBase.ApplicationObject = _applicationObject;
					Login.CreateLoginToolWindow(connectDatabaseMenu, connectDatabaseButton, assembly, omBackupControl,
												dbCreateDemoDbControl);
				}
				else
				{
					
					Helper.SaveDataIfRequired();
					try
					{
						Helper.SetPicture(assembly, (CommandBarButton)connectDatabaseMenu.Control, IMAGE_CONNECT, IMAGE_CONNECT_MASKED);
					}
					catch (Exception)
					{
					}
					try
					{
						Helper.SetPicture(assembly, connectDatabaseButton, IMAGE_CONNECT, IMAGE_CONNECT_MASKED);
					}
					catch (Exception)
					{
					}

					connectDatabaseMenu.Caption = connectDatabaseButton.Caption = CONNECT;
					connectDatabaseMenu.TooltipText = connectDatabaseButton.TooltipText = CONNECT;
#if !NET_4_0

                    connectDatabaseButton.State = ((CommandBarButton)connectDatabaseMenu).State = MsoButtonState.msoButtonUp;
#endif
					dbCreateDemoDbControl.Enabled = true;
					omBackupControl.Enabled = false;

					CloseAllToolWindows();
                    AppDomain.Unload(Login.appDomain.workerAppDomain);
                    Login.appDomain.workerAppDomain = null;
                    AssemblyInspectorObject.ClearAll();


					
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}




		#endregion

		#region CloseAllToolWindows
		private void CloseAllToolWindows()
		{
			try
			{



				AssemblyInspectorObject.Connection.Closedb(); 

				OMEInteraction.SetCurrentRecentConnection(null);
				Helper.ClearAllCachedAttributes();

				ForEachOMNWindow(delegate(Window window)
									{

                                        window.Close(vsSaveChanges.vsSaveChangesYes);    
									});


				CloseMiscToolwindows();

				if (oPopup != null)
				{
					RemoveMenuControls();
					omObjectBrowserControl = null;
					omQueryBuilderControl = null;
					omPropertiesControl = null;
				}
				RemoveAllQueryResultToolwindows();

			}

			catch (COMException)
			{
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		private void CloseMiscToolwindows()
		{
			CloseWindow(ref windb4oHome);
			CloseWindow(ref windb4oDownloads);
			CloseWindow(ref winSupportdb4o);
			CloseWindow(ref winRequestConsultation);
			CloseWindow(ref winHelp);
			CloseWindow(ref windb4oDeveloper);

		}

		void CloseWindow(ref Window win)
		{
			try
			{
				if (win != null)
				{
					win.Close(vsSaveChanges.vsSaveChangesNo);
					win = null;
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}


        void RemoveMenuControls()
        {
            IList<CommandBarControl> list = new List<CommandBarControl>();
            try
            {
                for (int i = 1; i <= oPopup.Controls.Count; i++)
                {
                    switch (oPopup.Controls[i].Caption.Trim())
                    {
                        case Constants.QUERYBUILDER:
                        case Constants.PROPERTIES:
                        case Constants.DB4OBROWSER:
                            {

                                CommandBarControl commandBarControl = oPopup.Controls[i];
                                list.Add(commandBarControl);

                            }
                            break;

                    }
                }

                foreach (CommandBarControl ctrl in list)
                    ctrl.Delete(null);
            }
            catch (ArgumentException)
            {
            }
        }

	    void RemoveAllQueryResultToolwindows()
		{
			Window[] lst = ((List<Window>)ViewBase.GetAllPluginWindows()).ToArray();
			foreach (Window win in lst)
			{
				if (win.Object is QueryResult)
				{

					ViewBase.GetAllPluginWindows().Remove(win);
				}

			}
		}


		#endregion

		#region OpenOMEAboutBox
		private static void OpenOMEAboutBox()
		{
			using (AboutOME objectAbout = new AboutOME(Regex.Replace(DataLayerCommon.Db4o_Version, @"(.*?)\s(?<version>.*)", @"${version}")))
			{
				objectAbout.ShowDialog();
			}
            
		}
		#endregion

		//FIXME: Add the window to OM window list. See ViewBase.CreateToolWindow() for details.
		private void Opendb4oDeveloper()
		{
			try
			{
				if (windb4oDeveloper == null || windb4oDeveloper.Visible == false)
					windb4oDeveloper = _applicationObject.DTE.ItemOperations.Navigate(URL_DB4O_DEVELOPER, vsNavigateOptions.vsNavigateOptionsNewWindow);
				else
					windb4oDeveloper.Visible = true;

			}
			catch
			{
				windb4oDeveloper = _applicationObject.DTE.ItemOperations.Navigate(URL_DB4O_DEVELOPER, vsNavigateOptions.vsNavigateOptionsNewWindow);
			}
		}

		#region BackupDatabase
		private void BackupDatabase()
		{
			try
			{

				omBackupControl.Enabled = false;
				Backup backUp = new Backup();
				backUp.BackUpDataBase();
				omBackupControl.Enabled = true;

			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		#endregion



		#region OpenHelp

		private WindowVisibilityEvents helpWindowEvents;

		//FIXME: Add the window to OM window list. See ViewBase.CreateToolWindow() for details.
		private void OpenHelp()
		{
			string filepath = Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8);
			filepath = Path.Combine(Path.GetDirectoryName(filepath), FAQ_PATH);

			if (winHelp == null || winHelp.Visible == false)
			{
				winHelp = _applicationObject.DTE.ItemOperations.Navigate(filepath, vsNavigateOptions.vsNavigateOptionsNewWindow);

				helpWindowEvents = ((Events2)_applicationObject.Events).get_WindowVisibilityEvents(null);
				helpWindowEvents.WindowHiding += helpWindowEvents_WindowHiding;
			}
			else
				winHelp.Visible = true;
		}

		void helpWindowEvents_WindowHiding(Window window)
		{
			if (winHelp == window)
				winHelp = null;
		}
		#endregion

		private void SupportCases()
		{
			try
			{
				ConnectToSupport();
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		//FIXME: Add the window to OM window list. See ViewBase.CreateToolWindow() for details.
		private void ConnectToSupport()
		{
			try
			{
				if (winSupportdb4o == null || winSupportdb4o.Visible == false)
					winSupportdb4o = _applicationObject.DTE.ItemOperations.Navigate("https://customer.db4o.com/Support/Default.aspx", vsNavigateOptions.vsNavigateOptionsNewWindow);
				else
					winSupportdb4o.Visible = true;
			}
			catch
			{
				winSupportdb4o = _applicationObject.DTE.ItemOperations.Navigate("https://customer.db4o.com/Support/Default.aspx", vsNavigateOptions.vsNavigateOptionsNewWindow);
			}
		}

		#region XtremeConnect
		private void XtremeConnect()
		{
			try
			{
				ConnectToXtremeConnect();
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		//FIXME: Add the window to OM window list. See ViewBase.CreateToolWindow() for details.
		private void ConnectToXtremeConnect()
		{
			try
			{
				if (winRequestConsultation == null || winRequestConsultation.Visible == false)
					winRequestConsultation = _applicationObject.DTE.ItemOperations.Navigate("https://customer.db4o.com/Peer/Default.aspx", vsNavigateOptions.vsNavigateOptionsNewWindow);
				else
					winRequestConsultation.Visible = true;
			}
			catch
			{
				winRequestConsultation = _applicationObject.DTE.ItemOperations.Navigate("https://customer.db4o.com/Peer/Default.aspx", vsNavigateOptions.vsNavigateOptionsNewWindow);
			}
		}
		#endregion

		//FIXME: Add the window to OM window list. See ViewBase.CreateToolWindow() for details.
		private void Opendb4oDownloads()
		{
			try
			{
				if (windb4oDownloads == null || windb4oDownloads.Visible == false)
					windb4oDownloads = _applicationObject.DTE.ItemOperations.Navigate(URL_DB4O_DOWNLOADS, vsNavigateOptions.vsNavigateOptionsNewWindow);
				else
					windb4oDownloads.Visible = true;

			}
			catch
			{
				windb4oDownloads = _applicationObject.DTE.ItemOperations.Navigate(URL_DB4O_DOWNLOADS, vsNavigateOptions.vsNavigateOptionsNewWindow);
			}
		}

		//FIXME: Add the window to OM window list. See ViewBase.CreateToolWindow() for details.
		private void Opendb4oHomepage()
		{
			try
			{
				if (windb4oHome == null || windb4oHome.Visible == false)
					windb4oHome = _applicationObject.DTE.ItemOperations.Navigate(URL_DB4O_HOMEPAGE, vsNavigateOptions.vsNavigateOptionsNewWindow);
				else
					windb4oHome.Visible = true;
			}
			catch
			{
				windb4oHome = _applicationObject.DTE.ItemOperations.Navigate(URL_DB4O_HOMEPAGE, vsNavigateOptions.vsNavigateOptionsNewWindow);
			}
		}

		#endregion
	}
	
	
}