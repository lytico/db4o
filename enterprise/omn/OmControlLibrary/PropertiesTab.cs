/* Copyright (C) 2004 - 2009  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Windows.Forms;
using EnvDTE;
using OMAddinDataTransferLayer;
using OMAddinDataTransferLayer.DataBaseDetails;
using OMAddinDataTransferLayer.TypeMauplation;
using OManager.BusinessLayer.Login;
using OManager.BusinessLayer.UIHelper;
using OMControlLibrary.Common;
using OME.Logging.Common;
using OME.Logging.Tracing;
using Constants=OMControlLibrary.Common.Constants;

namespace OMControlLibrary
{
	public partial class PropertiesTab : ViewBase
	{
		#region Member Variables

		readonly dbDataGridView dbGridViewProperties;

		private bool m_showObjectPropertiesTab;
		private bool m_showClassProperties;

		private static PropertiesTab instance;
		private long objId;

		#endregion

		#region Properties

		public void SelectDefaultTab()
		{
			tabStripProperties.SelectedItem = tabStripProperties.Items[0];
		}

		public static PropertiesTab Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new PropertiesTab();
				}
				return instance;
			}
		}

		public bool ShowObjectPropertiesTab
		{
			get { return m_showObjectPropertiesTab; }
			set
			{
				m_showObjectPropertiesTab = value;
				tabItemObjectProperties.Visible = m_showObjectPropertiesTab;
			}
		}

		public bool ShowClassProperties
		{
			get { return m_showClassProperties; }
			set
			{
				m_showClassProperties = value;
				tabItemClassProperties.Visible =
					m_showClassProperties;
			}
		}

		#endregion

		#region Constructor
		public PropertiesTab()
		{
			InitializeComponent();
			tabStripProperties.HideMenuGlyph = true;
			dbGridViewProperties = new dbDataGridView();
			dbGridViewProperties.Dock = DockStyle.Fill;

			tabStripProperties.AlwaysShowMenuGlyph = false;
		}

		#endregion

		#region Private Methods

		public void DisplayDatabaseProperties()
		{
			try
			{
				OMETrace.WriteFunctionStart();

				dbGridViewProperties.Size = panelDatabaseProperties.Size;
				dbGridViewProperties.Rows.Clear();
				dbGridViewProperties.Columns.Clear();

				dbGridViewProperties.PopulateDisplayGrid(Constants.VIEW_DBPROPERTIES, null);

				dbGridViewProperties.Rows.Add(1);
				if (AssemblyInspectorObject.Connection.GetTotalDbSize() == -1)
				{
					dbGridViewProperties.Rows[0].Cells[0].Value = "NA for Client/Server";
				}
				else
				{
					dbGridViewProperties.Rows[0].Cells[0].Value = AssemblyInspectorObject.Connection.GetTotalDbSize() + " bytes";
				}

				dbGridViewProperties.Rows[0].Cells[1].Value = AssemblyInspectorObject.Connection.NoOfClassesInDb().ToString();
				if (AssemblyInspectorObject.Connection.GetFreeSizeOfDb()    == -1)
				{
					dbGridViewProperties.Rows[0].Cells[2].Value = "NA for Client/Server";
				}
				else
				{
					dbGridViewProperties.Rows[0].Cells[2].Value = AssemblyInspectorObject.Connection.GetFreeSizeOfDb() + " bytes";
				}

				if (!panelDatabaseProperties.Controls.Contains(dbGridViewProperties))
					panelDatabaseProperties.Controls.Add(dbGridViewProperties);

				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void DisplayClassProperties()
		{
			try
			{
				OMETrace.WriteFunctionStart();

				if (Helper.ClassName != null)
				{
					if (OMEInteraction.GetCurrentConnParams() != null)
					{
						if (OMEInteraction.GetCurrentConnParams() != null)
						{
							buttonSaveIndex.Enabled = !OMEInteraction.GetCurrentConnParams().ConnectionReadOnly &&
                                                    !AssemblyInspectorObject.Connection.CheckForClientServer()   ; 

							labelNoOfObjects.Text = "Number of objects : " +
							                        AssemblyInspectorObject.ClassProperties.GetObjectCountForAClass(Helper.ClassName);
							dbGridViewProperties.Size = Size;
							dbGridViewProperties.Rows.Clear();
							dbGridViewProperties.Columns.Clear();

							ArrayList fieldPropertiesList = GetFieldsForAllClass();
							dbGridViewProperties.ReadOnly = false;
							dbGridViewProperties.PopulateDisplayGrid(Constants.VIEW_CLASSPOPERTY, fieldPropertiesList);

							//Enable Disable IsIndexed Checkboxes
							foreach (DataGridViewRow row in dbGridViewProperties.Rows)
							{
								ProxyType type = row.Cells["Type"].Value as ProxyType;
                               if( type.IsEditable &&  (type.IsPrimitive || type.IsNullable))
								{
									row.Cells[2].ReadOnly = false;
								}
								else
									row.Cells[2].ReadOnly = true;
							}

							if (!panelForClassPropTable.Controls.Contains(dbGridViewProperties))
								panelForClassPropTable.Controls.Add(dbGridViewProperties);
							dbGridViewProperties.Dock = DockStyle.Fill;

						}
					}

				}
				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private static ArrayList GetFieldsForAllClass()
		{
			ClassProperties classProp = null;
			try
			{
				classProp = AssemblyInspectorObject.ClassProperties.GetClassProperties(Helper.ClassName);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}

			return classProp.FieldEntries;
		}

		private void DisplayObjectProperties()
		{
			try
			{
				OMETrace.WriteFunctionStart();

				if (objId != 0)
				{
					ArrayList objectProperties = new ArrayList();
					ObjectProperties objTable = AssemblyInspectorObject.ObjectProperties.GetObjectProperties(objId ); 
					objectProperties.Add(objTable);

					dbGridViewProperties.Rows.Clear();
					dbGridViewProperties.Columns.Clear();
					dbGridViewProperties.Size = panelDatabaseProperties.Size;

					dbGridViewProperties.PopulateDisplayGrid(Constants.VIEW_OBJECTPROPERTIES, objectProperties);

					if (!panelObjectProperties.Controls.Contains(dbGridViewProperties))
						panelObjectProperties.Controls.Add(dbGridViewProperties);

					dbGridViewProperties.Refresh();
				}

				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		#endregion

		#region Event Handlers
		private void tabControlProperties_Resize(object sender, EventArgs e)
		{
			try
			{
				if (dbGridViewProperties != null)
					dbGridViewProperties.Size = Size;
				if (panelDataGrid != null)
				{
					panelDataGrid.Height = Height - tableLayoutPanelClassProp.Height;
					panelDataGrid.Width = Width;
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}

		}

		private void PropertiesTab_Load(object sender, EventArgs e)
		{
			try
			{
				CheckForIllegalCrossThreadCalls = false;
				dbGridViewProperties.Dock = DockStyle.Fill;

				LoadProperties();

				
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void LoadProperties()
		{
			tabItemObjectProperties.Visible = instance.m_showObjectPropertiesTab;
			if (!tabItemClassProperties.Visible)
				tabItemClassProperties.Visible = instance.m_showClassProperties;

			if (Helper.Tab_index.Equals(0))
			{
				DisplayDatabaseProperties();
			}
			else if (Helper.Tab_index.Equals(1))
			{
				DisplayClassProperties();
                buttonSaveIndex.Enabled = !OMEInteraction.GetCurrentConnParams().ConnectionReadOnly && !AssemblyInspectorObject.Connection.CheckForClientServer();    
			}
			else if (Helper.Tab_index.Equals(2))
			{
				DisplayObjectProperties();
			}

			tabStripProperties.SelectedItem = tabStripProperties.Items[Helper.Tab_index];

			instance = this;
		}

		

		private void tabStripProperties_TabStripItemSelectionChanged(TabStripItemChangedEventArgs e)
		{
			try
			{
				if (e.Item != null && e.ChangeType == OMETabStripItemChangeTypes.SelectionChanged)
					RefreshPropertiesTab(objId );
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		#endregion

		#region Public Methods
		public void RefreshPropertiesTab(long  id)
		{
			try
			{
				this.objId  = id;

				if (tabItemDatabaseProperties.Visible &&
					tabStripProperties.SelectedItem.Equals(tabItemDatabaseProperties))
				{
					DisplayDatabaseProperties();
				}
				else if (tabItemClassProperties.Visible &&
					tabStripProperties.SelectedItem.Equals(tabItemClassProperties))
				{
					DisplayClassProperties();
				}
				else if (tabItemObjectProperties.Visible &&
					tabStripProperties.SelectedItem.Equals(tabItemObjectProperties))
				{
					DisplayObjectProperties();
				}
				else
					tabStripProperties.SelectedItem = tabStripProperties.Items[0];

				Helper.Tab_index = Convert.ToInt32(tabStripProperties.SelectedItem.Tag.ToString());
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
		#endregion



		private void SaveIndex()
		{
           
		    ConnParams conparam = null;
		    bool customConfig = false;
		    SaveIndexClass saveIndexInstance = null;
			try
			{
				saveIndexInstance = new SaveIndexClass(Helper.ClassName);
				foreach (DataGridViewRow row in dbGridViewProperties.Rows)
				{
					bool boolValue = Convert.ToBoolean(row.Cells[2].Value);
					saveIndexInstance.Fieldname.Add(row.Cells[0].Value.ToString());
					saveIndexInstance.Indexed.Add(boolValue);
				}

				CloseQueryResultToolWindows();

				conparam = OMEInteraction.GetCurrentConnParams();
			    OMEInteraction.CloseOMEdb();
                customConfig = AssemblyInspectorObject.Connection.CheckForCustomConfig();   
				AssemblyInspectorObject.Connection.Closedb();
			    AssemblyInspectorObject.Connection.SaveIndex(saveIndexInstance.Fieldname, saveIndexInstance.Classname,
			                                                 saveIndexInstance.Indexed, conparam.Connection,customConfig );
                MessageBox.Show("Index Saved Successfully!", Helper.GetResourceString(Constants.PRODUCT_CAPTION), MessageBoxButtons.OK);
            }
            catch (Exception oEx)
            {
                LoggingHelper.ShowMessage(oEx);
            }
            try
            {
                ConnectionDetails currConnectionDetails = new ConnectionDetails(conparam);
                 long id = OMEInteraction.ChkIfRecentConnIsInDb();
                 if (id > 0)
                 {
                     ConnectionDetails tempConnectionDetails = OMEInteraction.GetConnectionDetailsObject(id);

                     if (tempConnectionDetails != null)
                         currConnectionDetails = tempConnectionDetails;
                     currConnectionDetails.Timestamp = DateTime.Now;


                     AssemblyInspectorObject.Connection.ConnectToDatabase(currConnectionDetails, customConfig);
                     OMEInteraction.SetCurrentRecentConnection(currConnectionDetails.ConnParam );

                     if (ObjectBrowser.Instance.ToolStripButtonAssemblyView.Checked)
                         ObjectBrowser.Instance.DbtreeviewObject.FindNSelectNode(
                             ObjectBrowser.Instance.DbAssemblyTreeView.Nodes[0], saveIndexInstance.Classname,
                             ObjectBrowser.Instance.DbAssemblyTreeView);
                     else
                         ObjectBrowser.Instance.DbtreeviewObject.FindNSelectNode(
                             ObjectBrowser.Instance.DbtreeviewObject.Nodes[0], saveIndexInstance.Classname,
                             ObjectBrowser.Instance.DbtreeviewObject);

                     tabStripProperties.SelectedItem = tabItemClassProperties;
                 }
            }
            catch (Exception Ex)
            {
                LoggingHelper.ShowMessage(Ex);
            }
			
		}

		private void CloseQueryResultToolWindows()
		{
			foreach (Window entry in GetAllPluginWindows())
			{
				switch(entry.Caption  )
				{
					case Constants.QUERYBUILDER:
					case Constants.DB4OPROPERTIES :
					case Constants.DB4OBROWSER:
						break;
					default:
						if (entry.Visible )
						{
							entry.Close(vsSaveChanges.vsSaveChangesNo);
						}
						break;
				}
					
			}
		}


		private void buttonSaveIndex_Click(object sender, EventArgs e)
		{
			try
			{
				if (ListofModifiedObjects.Instance.Count > 0)
				{
					DialogResult dialogRes = MessageBox.Show("This will close all the Query result windows. Do you want to continue?", Helper.GetResourceString(Constants.PRODUCT_CAPTION), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (dialogRes == DialogResult.Yes)
					{
						SaveIndex();

					}
				}
				else
				{
					SaveIndex();
				}

			}
			catch (Exception e1)
			{
				LoggingHelper.ShowMessage(e1);
			}
		}




	}
}
