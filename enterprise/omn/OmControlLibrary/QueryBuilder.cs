using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.CommandBars;
using OMAddinDataTransferLayer;
using OMAddinDataTransferLayer.TypeMauplation;
using OManager.BusinessLayer.Common;
using OManager.BusinessLayer.QueryManager;
using OManager.BusinessLayer.UIHelper;
using OMControlLibrary.Common;
using OME.Logging.Common;
using OME.Logging.Tracing;
using Constants = OMControlLibrary.Common.Constants;

namespace OMControlLibrary
{
	[ComVisible(true)]
	public partial class QueryBuilder : ViewBase, ILoadData 
	{
		private static QueryBuilder instance;

		public static QueryBuilder Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new QueryBuilder();
				}
				return instance;
			}
		}

		#region Member Variable

		
		private int m_attributeCount;
		private int m_queryGroupCount;

		private OMQuery omQuery;

		//Controls 
		private TableLayoutPanel tableLayoutPanelQueries;


		private dbDataGridView dbDataGridAttributes;
		private DataGridViewGroup dataGridViewGroup;
		private string className;


		private DataGridViewGroup defaultGroup;

		private ToolTip recentQueriesToolTip;

		//Constants 
		
		private WindowEvents _windowsEvents;

		#endregion

		#region Properties

		/// <summary>
		/// Get the Attribute DataGridView
		/// </summary>
		public dbDataGridView DataGridViewAttributes
		{
			get { return dbDataGridAttributes; }
		}

		/// <summary>
		/// Get/Set number of query groups
		/// </summary>
		public int QueryGroupCount
		{
			get { return m_queryGroupCount; }
			set { m_queryGroupCount = value; }
		}

		/// <summary>
		/// Get number of attributes added
		/// </summary>
		public int AttributeCount
		{
			get
			{
				m_attributeCount = dbDataGridAttributes.RowCount;
				return m_attributeCount;
			}
		}

		public string ClassName
		{
			get { return className; }
			set { className = value; }
		}

		public TableLayoutPanel TableLayoutPanelQueries
		{
			get { return tableLayoutPanelQueries; }
			set { tableLayoutPanelQueries = value; }
		}

		public bool EnableRunQuery
		{
			get { return buttonRunQuery.Enabled; }
			set { buttonRunQuery.Enabled = value; }
		}

		#endregion

		#region Event Handlers

		////Handles the Drags events item dragged to the DataGridViewGroup DatagridView
		//internal event EventHandler<DragEventArgs> OnQueryBuilderDragEnter;
		////Event Handler if DataGridView is removed
		//internal event EventHandler<DbEventArgs> OnQueryBuilderRemoveClick;
		////Event Handler for Operator is changed for query group 
		//internal event EventHandler<DbEventArgs> OnQueryBuilderDataGridIndexChanged;
		////Events handles id item is dragged to the Attribute datagridview
		//internal event EventHandler<DragEventArgs> OnAttributesDragEnter;

		internal event EventHandler<DbEventArgs> OnRecentQueryChanged;

		#endregion

		#region Constructor

		public QueryBuilder()
		{
			InitializeComponent();
		}

		#endregion

		#region Query Builder Events

		/// <summary>
		/// Load Event of Query Builder
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void QueryBuilder_Load(object sender, EventArgs e)
		{
			try
			{
				
				//Initialization of Queries tablelayour where the QueryGridGrup will be added
				InitializeQueriesTableLayoutPanel();
				//Initialization of Attribute List
				InitializeAttributesDataGrid();
				InitializeRecentQueries();
				defaultGroup = AddDataGridViewToPanel();
				OMETrace.WriteFunctionStart();
				Events events = ApplicationObject.Events;
				_windowsEvents = events.get_WindowEvents(null);
				_windowsEvents.WindowActivated += _windowsEvents_WindowActivated;
				OMETrace.WriteFunctionEnd();

				SetLiterals();
				recentQueriesToolTip = new ToolTip();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		public void LoadAppropriatedata()
		{

			ClearAllQueries();
			comboboxRecentQueries.SelectedIndex = 0;
			//InitializeAttributesDataGrid();
			instance = this;

		}

	

		private static void _windowsEvents_WindowActivated(Window GotFocus, Window LostFocus)
		{
			if (GotFocus.Caption == Constants.QUERYBUILDER )
			{
                if (AssemblyInspectorObject.Connection!=null && AssemblyInspectorObject.Connection.DbConnectionStatus())
				{
					PropertiesTab.Instance.ShowObjectPropertiesTab = false;
					PropertiesTab.Instance.ShowClassProperties = true;
					PropertiesTab.Instance.SelectDefaultTab();
				}
			}
		}

		/// <summary>
		/// Sets the width of Attribute column
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void QueryBuilder_Resize(object sender, EventArgs e)
		{
			try
			{
				if (dbDataGridAttributes != null)
					dbDataGridAttributes.Columns[0].Width = dbDataGridAttributes.Width - 5;
				comboboxRecentQueries.Refresh();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		#endregion

		#region DataGridViewGroup Events

		/// <summary>
		/// Raise when expressions removed from groups 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dataGridViewGroup_OnRowsRemoved(object sender, EventArgs e)
		{
			DataGridViewGroup dbdataGridViewGroup;

			try
			{
				dbdataGridViewGroup = (DataGridViewGroup)((dbDataGridView)sender).Parent;

				if (tableLayoutPanelQueries.Controls.Count == 1)
				{
					if (dbdataGridViewGroup.DataGridViewQuery.Rows.Count == 0)
						buttonRunQuery.Enabled = false;
				}
				else
				{
					//if Query quilder has more then one group
					if (tableLayoutPanelQueries.Controls.Count != 1 && dbdataGridViewGroup != null &&
						dbdataGridViewGroup.Removable)
					{
						//if all expression removed from the query group
						//get the confirmation from user to remove that Query Group
						if (MessageBox.Show(Helper.GetResourceString(Constants.CONFIRMATION_MSG_REMOVE_QUERY_GROUP),
											Helper.GetResourceString(Constants.PRODUCT_CAPTION),
											MessageBoxButtons.YesNo,
											MessageBoxIcon.Question) == DialogResult.Yes)
						{
							int dataGridViewGroupHeight = dbdataGridViewGroup.Height;

							tableLayoutPanelQueries.Controls.Remove(dbdataGridViewGroup);
							QueryGroupCount--;
							tableLayoutPanelQueries.RowCount = QueryGroupCount;
							tableLayoutPanelQueries.Height = tableLayoutPanelQueries.Height
															 - dataGridViewGroupHeight;
							//reset the expression group names
							RenameQueryGroupCaption();
						}
					}
				}

				CheckForDataGridViewQueryRows();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Raise an event OnQueryBuilderDataGridIndexChanged
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void dataGridViewGroup_OnDataGridViewComboBoxIndexChanged(object sender, DbEventArgs e)
		{
			dbDataGridView datagrid = e.Data as dbDataGridView;

			try
			{
				OMETrace.WriteFunctionStart();

				string operatorColumnName = Helper.GetResourceString(Constants.QUERY_GRID_OPERATOR);
				int operatorColumnIndex = datagrid.Columns[operatorColumnName].Index;

				if (datagrid.CurrentCell.ColumnIndex == operatorColumnIndex)
				{
					if (datagrid.Rows.Count > 1)
					{
						string operatorValue = ((ComboBox)sender).SelectedItem.ToString();

						for (int i = 1; i < datagrid.Rows.Count; i++)
						{
							datagrid.Rows[i].Cells[operatorColumnName].Value = operatorValue;
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

		/// <summary>
		/// Rainse an event OnQueryBuilderRemoveClick
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dataGridViewGroup_OnRemoveClick(object sender, DbEventArgs e)
		{
			try
			{
				if (e.Data is DataGridViewGroup)
				{
					DataGridViewGroup dataGridViewGroup = (DataGridViewGroup)e.Data;
					int dataGridViewGroupHeight = dataGridViewGroup.Height;

					if (dataGridViewGroup.Parent is TableLayoutPanel)
					{
						TableLayoutPanel tableLayoutPanelQueries = dataGridViewGroup.Parent as TableLayoutPanel;
						tableLayoutPanelQueries.Controls.Remove(dataGridViewGroup);
						QueryGroupCount--;
						tableLayoutPanelQueries.RowCount = QueryGroupCount;
						tableLayoutPanelQueries.Height = tableLayoutPanelQueries.Height
														 - dataGridViewGroupHeight;
					}
				}

				CheckForDataGridViewQueryRows();
				RenameQueryGroupCaption();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}


		private void buttonRunQuery_Click(object sender, EventArgs e)
		{
			try
			{
				OMETrace.WriteFunctionStart();
				ExecuteQuery();
				OMETrace.WriteFunctionEnd();
			}
			catch(Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
        
		//------------
		private static void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			ApplicationObject.StatusBar.Progress(true, "Running Query ... ", e.ProgressPercentage * 10, 10000);
		}

		private bool isrunning = true;
		private BackgroundWorker bw = new BackgroundWorker();
		private long[] objectid;

		private void bw_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				objectid = AssemblyInspectorObject.DataPopulation.ExecuteQueryResults(omQuery);
					
				e.Result = objectid;

				for (int i = 0; i < 10000; i++)
				{
					if (bw.CancellationPending )
					{
						e.Cancel = true;
						break;
					}
					bw.ReportProgress(i * 100 / 10000);
				}
				
				isrunning = false;
			}
			catch (Exception oEx)
			{
				bw.CancelAsync();
				bw = null;
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void ClearStatusBar()
		{
			if (bw != null)
			{
				bw.CancelAsync();
				bw = null;
			}

			ApplicationObject.StatusBar.Clear();
			ApplicationObject.StatusBar.Animate(false, vsStatusAnimation.vsStatusAnimationBuild);
		}

		private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
                Helper.CreateQueryResultToolwindow(objectid);
				ObjectBrowser.Instance.Enabled = true;
				PropertiesTab.Instance.Enabled = true;
				Instance.Enabled = true;

				EnableDisableDatabaseConnection(true);

				isrunning = false;
               
               if (bw != null)
                    {
                        bw.CancelAsync();
                        bw.Dispose();
                        bw = null;
                    }
                    ApplicationObject.StatusBar.Clear();
                    ApplicationObject.StatusBar.Progress(false, "Query run successfully!", 0, 0);
                    ApplicationObject.StatusBar.Text = "Query run successfully!";
                    ApplicationObject.StatusBar.Animate(false, vsStatusAnimation.vsStatusAnimationBuild);
                
			}
			catch (Exception oEx)
			{
				ClearStatusBar();
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private static void EnableDisableDatabaseConnection(bool enabled)
		{
			SafeSetEnabled(Login.m_cmdBarCtrlBackup, enabled);
			SafeSetEnabled(Login.m_cmdBarCtrlConnect, enabled);
			SafeSetEnabled(Login.m_cmdBarBtnConnect, enabled);
		}

		private void ExecuteQuery()
		{
			omQuery = new OMQuery(Helper.BaseClass, DateTime.Now);
			try
			{
				string errorMessage;

				//Check the for valid query. user must specifies the values for each query expression
				bool isValidQuery = IsValidQuery(out errorMessage);
				if (!isValidQuery)
				{
					MessageBox.Show(errorMessage,
					                Helper.GetResourceString(Constants.PRODUCT_CAPTION),
					                MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				omQuery = PrepareOMQuery();

				AddQueryToCurrentConnection(omQuery);
				
				ObjectBrowser.Instance.Enabled = false;
				PropertiesTab.Instance.Enabled = false;
				Instance.Enabled = false;

				EnableDisableDatabaseConnection(false);

				bw = new BackgroundWorker();
				
				bw.WorkerReportsProgress = true;
				bw.WorkerSupportsCancellation = true;
				bw.ProgressChanged += bw_ProgressChanged;
				bw.DoWork += bw_DoWork;
				bw.RunWorkerCompleted += bw_RunWorkerCompleted;
				

				bw.RunWorkerAsync();
				ApplicationObject.StatusBar.Animate(true, vsStatusAnimation.vsStatusAnimationBuild);
				isrunning = true;
			
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
				ClearStatusBar();
			}
		}

	
	    /// <summary>
		/// Raise an event OnQueryBuilderDragEnter
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dataGridViewGroup_OnDataGridViewDragEnter(object sender, DragEventArgs e)
		{
			try
			{
				OMETrace.WriteFunctionStart();

				dbDataGridView datagridObject = sender as dbDataGridView;
				if (e.Data.GetDataPresent(typeof(TreeNode).ToString(), true))
				{
					//Get the selected item from classes tree
					TreeNode tempTreeNode = (TreeNode)e.Data.GetData(typeof(TreeNode).ToString(), true);
					if (tempTreeNode != null)
					{
						if (tempTreeNode.Tag != null && tempTreeNode.Tag.ToString() != "Fav Folder" &&
							tempTreeNode.Tag.ToString() != "Assembly View")
						{
							//If dragged item has child node dont allow to be dragged to Query Builder
							bool rowadded = tempTreeNode.Nodes.Count > 0 ? datagridObject.AddAllItemsOfClassToQueryBuilder(tempTreeNode, this) : datagridObject.AddToQueryBuilder(tempTreeNode, this);
							if (rowadded)
								buttonRunQuery.Enabled = true;
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

		#endregion

		#region Event Handlers

		/// <summary>
		/// Adds a new query Group
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonAddQueryGroup_Click(object sender, EventArgs e)
		{
			try
			{
				OMETrace.WriteFunctionStart();

				int queryGroupCount = tableLayoutPanelQueries.Controls.Count;

				if (queryGroupCount == 0)
					return;

				DataGridViewGroup dataGridViewGroup =
					(DataGridViewGroup)tableLayoutPanelQueries.Controls[queryGroupCount - 1];

				if (dataGridViewGroup.DataGridViewQuery.RowCount < 1)
					return;

				DataGridViewGroup dgvGroup = AddDataGridViewToPanel();
				//focus at new added group
				panelQueryGrid.ScrollControlIntoView(dgvGroup);


				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Clears all queries from Query Builder
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonClearAll_Click(object sender, EventArgs e)
		{
			ClearAllQueries();
			comboboxRecentQueries.SelectedIndex = 0;
		}

		/// <summary>
		/// Removes the selected attributes if "Delete" key pressed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dataGridAttributes_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				OMETrace.WriteFunctionStart();

				if (e.KeyCode == Keys.Delete)
				{
					if (dbDataGridAttributes.Rows.Count > 0)
					{
						if (dbDataGridAttributes.SelectedRows.Count > 1)
						{
							List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();

							int rowCount = dbDataGridAttributes.Rows.Count;
							for (int i = 0; i < rowCount; i++)
							{
								if (dbDataGridAttributes.Rows[i].Selected)
									selectedRows.Add(dbDataGridAttributes.Rows[i]);
							}

							for (int i = 0; i < selectedRows.Count; i++)
							{
								dbDataGridAttributes.Rows.Remove(selectedRows[i]);
							}
							selectedRows.Clear();
						}
						else
						{
							dbDataGridAttributes.Rows.RemoveAt(dbDataGridAttributes.SelectedCells[0].OwningRow.Index);
						}
						e.Handled = true;
					}
				}
				else
					e.Handled = false;

				OMETrace.WriteTraceBlockStartEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Calls when row is removed from the Attribute List
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dataGridAttributes_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			CheckForDataGridViewQueryRows();
		}

		/// <summary>
		/// Raise an event OnAttributesDragEnter
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dataGridAttributes_DragEnter(object sender, DragEventArgs e)
		{
			string className = string.Empty;
			if (e.Data.GetDataPresent(typeof(TreeNode).ToString(), true))
			{
				//Get which tree node is dragged from the classes tree
				TreeNode tempTreeNode = (TreeNode)e.Data.GetData(typeof(TreeNode).ToString(), true);
				if (tempTreeNode != null)
				{
					if (tempTreeNode.Tag != null && tempTreeNode.Tag.ToString() != "Fav Folder" &&
						tempTreeNode.Tag.ToString() != "Assembly View")
					{
						Helper.BaseClass = Helper.FindRootNode(tempTreeNode);
						//Don't drag the node if the treenode has childs, because its not premitive type 
						if (tempTreeNode.Nodes.Count > 0)
						{
							AddAllTheElementsofClassIntoAttributeList(tempTreeNode);
						}
						else
						{
							ProxyType fieldType = AssemblyInspectorObject.DataType.ResolveType(tempTreeNode.Tag.ToString());
							
							//If selected item is not a primitive type than dont allow to drage item)
                            if (!(fieldType.IsEditable && (fieldType.IsPrimitive || fieldType.IsNullable)  ))
                              
								return;

							//If field is not selected and Query Group has no clauses then reset the base class.
							if (dbDataGridAttributes.Rows.Count == 0)
							{
								CheckForDataGridViewQueryRows();
							}
							//Check if dragged item is from same class or not, if not then dont allow to drag item
							if (Helper.HashTableBaseClass.Count > 0)
								if (!Helper.HashTableBaseClass.Contains(Helper.BaseClass))
									return;
							//Get the full path of the selected item
							string fullpath = Helper.GetFullPath(tempTreeNode);
							//Check whether attributes is allready added in list, if yes dont allow to added again. 
							if (!Helper.CheckUniqueNessAttributes(fullpath, dbDataGridAttributes))
								return;
							if (tempTreeNode.Parent != null)
							{
								ProxyType type = AssemblyInspectorObject.DataType.ResolveType(tempTreeNode.Parent.Tag.ToString());
							    className = type != null ? type.FullName : tempTreeNode.Parent.Name;
							  
							}
						    //Add a new row and assing required values.
                            Helper.AddElementToAttributeGrid(dbDataGridAttributes, className, fullpath);
							
							if (!Helper.HashTableBaseClass.Contains(Helper.BaseClass))
								Helper.HashTableBaseClass.Add(Helper.BaseClass, string.Empty);
						}
					}
					e.Effect = DragDropEffects.Move;
					
				}
			}
		}

		private void AddAllTheElementsofClassIntoAttributeList(TreeNode tempTreeNode)
		{
			if (dbDataGridAttributes.Rows.Count == 0)
			{
				CheckForDataGridViewQueryRows();
			}
			//Check if dragged item is from same class or not, if not then dont allow to drag item
			if (Helper.HashTableBaseClass.Count > 0)
				if (!Helper.HashTableBaseClass.Contains(Helper.BaseClass))
					return;

			if (!Helper.HashTableBaseClass.Contains(Helper.BaseClass))
				Helper.HashTableBaseClass.Add(Helper.BaseClass, string.Empty);
            
		    string tempClassName = string.Empty;
			ProxyType type = AssemblyInspectorObject.DataType.ResolveType(tempTreeNode.Tag.ToString());
		    tempClassName = type != null
		                        ? type.FullName
		                        : (tempTreeNode.Parent != null ? tempTreeNode.Parent.Name : tempTreeNode.Text);


			Hashtable storedfields = AssemblyInspectorObject.Connection.FetchStoredFields(tempClassName);   
			if (storedfields == null) 
				return;


			IDictionaryEnumerator eNum = storedfields.GetEnumerator();
			if (eNum != null)
			{
				while (eNum.MoveNext())
				{
                    ProxyType itemType = AssemblyInspectorObject.DataType.ResolveType(eNum.Value.ToString());
                    if (!(itemType.IsEditable && (itemType.IsPrimitive || itemType.IsNullable)))
                        continue;
					
						
					//Check whether attributes is allready added in list, if yes dont allow to added again. 

					string parentName = Helper.FormulateParentName(tempTreeNode, eNum);
					if (!Helper.CheckUniqueNessAttributes(parentName, dbDataGridAttributes))
						continue;

                    Helper.AddElementToAttributeGrid(dbDataGridAttributes,tempClassName, parentName);
				}
			}
		}

       
		/// <summary>
		/// Build context menu if its null
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dataGridAttributes_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				DataGridView.HitTestInfo hitTestInfo = dbDataGridAttributes.HitTest(e.X, e.Y);
				DataGridViewHitTestType hitTestType = hitTestInfo.Type;
				if (hitTestType == DataGridViewHitTestType.Cell)
				{
					if (dbDataGridAttributes.ContextMenuStrip == null)
						dbDataGridAttributes.BuildRowContextMenu();
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Populates the recent queries
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboboxRecentQueries_Click(object sender, EventArgs e)
		{
			try
			{
				comboboxRecentQueries.DataSource = null;
				if (comboboxRecentQueries != null)
					comboboxRecentQueries.Items.Clear();
				Helper.PopulateRecentQueryComboBox(comboboxRecentQueries);

			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Handles the changed Recent Queries.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboboxRecentQueries_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (comboboxRecentQueries.SelectedIndex <= 0)
					return;

				if (comboboxRecentQueries.SelectedValue == null ||
					(comboboxRecentQueries.SelectedValue != null && comboboxRecentQueries.SelectedValue.GetType() == typeof(DBNull)))
				{
					ClearAllQueries();
					return;
				}

				if (!string.IsNullOrEmpty(((OMQuery)comboboxRecentQueries.SelectedValue).QueryString))
				{
					//Set tooltip for selected recent query
					recentQueriesToolTip.SetToolTip(comboboxRecentQueries, comboboxRecentQueries.SelectedText);

					//Get the OMQuery for selected query expression
					OMQuery omQuery = (OMQuery)comboboxRecentQueries.SelectedValue;

					//Reset the OMQuery
					SetOMQuery(omQuery);

					//Set the base class name
					Helper.ClassName = omQuery.BaseClass;


					//Clear all the queies from the QueryBuilder 
					ClearAllQueries();

					//Repopulate the query builder
					PopulateQueryBuilderGroup(omQuery);

					if (OnRecentQueryChanged != null)
					{
						OnRecentQueryChanged(sender, new DbEventArgs(omQuery.BaseClass));
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Event to get tooltip of combobox
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboboxRecentQueries_DropdownItemSelected(object sender, ToolTipComboBox.DropdownItemSelectedEventArgs e)
		{
			try
			{
				if (e.SelectedItem < 0 || e.Scrolled) recentQueriesToolTip.Hide(comboboxRecentQueries);
				else if (comboboxRecentQueries.SelectedIndex != 0)
					recentQueriesToolTip.Show(comboboxRecentQueries.Text,
											  comboboxRecentQueries,
											  e.Bounds.Location.X + Cursor.Size.Width,
											  e.Bounds.Location.Y + Cursor.Size.Height);
			}
			catch (Exception ex)
			{
				LoggingHelper.HandleException(ex);
			}
		}

		#endregion

		#region Private Methods DataGridViewFunctionality

		/// <summary>
		/// Initialize Tablelayout panel for each DataGridView Group
		/// </summary>
		private void InitializeQueriesTableLayoutPanel()
		{
			try
			{
				tableLayoutPanelQueries = new TableLayoutPanel();

				tableLayoutPanelQueries.RowCount = 1;
				tableLayoutPanelQueries.ColumnCount = 1;
				tableLayoutPanelQueries.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
				tableLayoutPanelQueries.AutoSize = true;
				panelQueryGrid.BackColor = Color.Gray;
				panelQueryGrid.Controls.Add(tableLayoutPanelQueries);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Initialize DataGridView Group
		/// </summary>
		/// <returns></returns>
		private DataGridViewGroup InitializeDataGridViewGroup()
		{
			dataGridViewGroup = new DataGridViewGroup();
			try
			{
				dataGridViewGroup.Dock = DockStyle.Fill;
				dataGridViewGroup.LabelQueryGroup = Helper.GetResourceString(Constants.QUERY_GROUP_CAPTION) + QueryGroupCount;
				tableLayoutPanelQueries.Width = dataGridViewGroup.Width;

				dataGridViewGroup.OnDataGridViewDragEnter += dataGridViewGroup_OnDataGridViewDragEnter;

				dataGridViewGroup.OnRemoveClick += dataGridViewGroup_OnRemoveClick;

				dataGridViewGroup.OnDataGridViewComboBoxIndexChanged += dataGridViewGroup_OnDataGridViewComboBoxIndexChanged;

				dataGridViewGroup.OnRowsRemoved += dataGridViewGroup_OnRowsRemoved;
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}

			return dataGridViewGroup;
		}


		/// <summary>
		/// Add QueryGroup control
		/// </summary>
		/// <returns></returns>
		private DataGridViewGroup AddDataGridViewToPanel()
		{
			DataGridViewGroup dataGridViewGroup = InitializeDataGridViewGroup();

			try
			{
				OMETrace.WriteFunctionStart();

				tableLayoutPanelQueries.RowStyles.Add(new RowStyle(SizeType.AutoSize, 70));
				tableLayoutPanelQueries.Controls.Add(dataGridViewGroup, 0, QueryGroupCount);
				tableLayoutPanelQueries.SetRow(dataGridViewGroup, QueryGroupCount);

				if (QueryGroupCount == 0)
					dataGridViewGroup.EnableOperator = false;
				else
					dataGridViewGroup.Removable = true;

				//increase row count
				QueryGroupCount++;

				tableLayoutPanelQueries.Height = dataGridViewGroup.Height * QueryGroupCount;

				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}

			return dataGridViewGroup;
		}

		/// <summary>
		/// Initialize/populates Recent queries
		/// </summary>
		private void InitializeRecentQueries()
		{
            try
            {
                comboboxRecentQueries.Items.Clear();  
                comboboxRecentQueries.Items.Add(Helper.GetResourceString(Constants.COMBOBOX_DEFAULT_TEXT));
                comboboxRecentQueries.SelectedIndex = 0;
                comboboxRecentQueries.DropdownItemSelected += comboboxRecentQueries_DropdownItemSelected;
                Helper.PopulateRecentQueryComboBox(comboboxRecentQueries);
            }
            catch (Exception oEx)
            {
                LoggingHelper.ShowMessage(oEx);
            }
		}

		/// <summary>
		/// Set the query to current connection
		/// </summary>
		/// <param name="query"></param>
		private static void AddQueryToCurrentConnection(OMQuery query)
		{
            try
            {
                if (!string.IsNullOrEmpty(query.QueryString))
                    OMEInteraction.AddQueriesToList(query);
            }
            catch (Exception oEx)
            {
                LoggingHelper.ShowMessage(oEx);
            }
		}

		/// <summary>
		/// Prepares the OMQuery Groups
		/// </summary>
		/// <param name="datagridview"></param>
		/// <returns></returns>
		private static OMQueryGroup PrepareQueryCollection(dbDataGridView datagridview)
		{
			//Get all the columns names from resource
			string fieldColumnName = Helper.GetResourceString(Constants.QUERY_GRID_FIELD);
			string conditionColumnName = Helper.GetResourceString(Constants.QUERY_GRID_CONDITION);
			string valueColumnName = Helper.GetResourceString(Constants.QUERY_GRID_VALUE);
			string operatorColumnName = Helper.GetResourceString(Constants.QUERY_GRID_OPERATOR);

		    OMQueryGroup objectManagerQueryGroup = null;

			try
			{
				OMETrace.WriteFunctionStart();

				int rowCount = datagridview.RowCount;
				if (rowCount > 0)
				{
                   
					objectManagerQueryGroup = new OMQueryGroup();
					string stringOperator = datagridview.Rows[0].Cells[operatorColumnName].Value.ToString();
					CommonValues.LogicalOperators clauseOperator = (CommonValues.LogicalOperators)Enum.Parse(typeof(CommonValues.LogicalOperators), stringOperator);
					for (int i = 0; i < rowCount; i++)
					{
                        string fieldName = datagridview.Rows[i].Cells[fieldColumnName].Value.ToString();
						string stringCondition = datagridview.Rows[i].Cells[conditionColumnName].Value.ToString();
						string className = datagridview.Rows[i].Cells[Constants.QUERY_GRID_CALSSNAME_HIDDEN].Value.ToString();
					    string fieldType = FieldTypeFor(datagridview.Rows[i]);
						//get the value for each expression if value not specified then return null
						string stringValue;
						if (datagridview.Rows[i].Cells[valueColumnName].Value != null)
						{
                            
                                stringValue = datagridview.Rows[i].Cells[valueColumnName].Value.ToString();
                           
                           
						}
						else
							return null;

						OMQueryClause queryClause = new OMQueryClause(className, fieldName, stringCondition, stringValue, clauseOperator, fieldType);
						objectManagerQueryGroup.AddOMQueryClause(queryClause);
					}
				}

				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}

			return objectManagerQueryGroup;
		}

	   
        private static string FieldTypeFor(DataGridViewRow row)
        {
			ProxyType fieldType = (ProxyType)row.Cells[Constants.QUERY_GRID_FIELDTYPE_DISPLAY_HIDDEN].Value;
            return fieldType.FullName ;
        }

	    /// <summary>
		/// Initialise the Attribute DataGrid
		/// </summary>
		private void InitializeAttributesDataGrid()
		{
			try
			{
				dbDataGridAttributes = new dbDataGridView();

				dbDataGridAttributes.Dock = DockStyle.Fill;
				dbDataGridAttributes.MultiSelect = true;
				dbDataGridAttributes.ColumnHeadersHeight = 12;
				dbDataGridAttributes.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
				DataGridViewCellStyle style = new DataGridViewCellStyle();
				style.BackColor = SystemColors.Control;
				style.ForeColor = Color.Black;
				style.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
				dbDataGridAttributes.ColumnHeadersDefaultCellStyle = style;
				dbDataGridAttributes.BuildRowContextMenu();
				splitContainerQueryBuilder.Panel2.Controls.Add(dbDataGridAttributes);

				dbDataGridAttributes.PopulateDisplayGrid(Constants.VIEW_ATTRIBUTES, null);

				//Register Event Handlers
				dbDataGridAttributes.DragEnter += dataGridAttributes_DragEnter;
				dbDataGridAttributes.KeyDown += dataGridAttributes_KeyDown;
				dbDataGridAttributes.RowsRemoved += dataGridAttributes_RowsRemoved;
				dbDataGridAttributes.MouseDown += dataGridAttributes_MouseDown;
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Checks if Query builder has groups but expressions are removed from all groups
		/// then allow use to get the attributes from other class
		/// </summary>
		internal void CheckForDataGridViewQueryRows()
		{
			bool rowsNotFound = false;

			try
			{
				int count = tableLayoutPanelQueries.Controls.Count;

				for (int i = 0; i < count; i++)
				{
					DataGridViewGroup dataGridViewGroup = (DataGridViewGroup)tableLayoutPanelQueries.Controls[i];
					dbDataGridView dataGridView = dataGridViewGroup.DataGridViewQuery;
					if (dataGridView.Rows.Count == 0)
					{
						rowsNotFound = true;
					}
					else
					{
						rowsNotFound = false;
						break;
					}
				}

				if (dbDataGridAttributes.Rows.Count == 0 && rowsNotFound)
				{
					//if (Helper.BaseClass != null)
					Helper.HashTableBaseClass.Clear();
					buttonRunQuery.Enabled = false;
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Clear all Query Groups and Expressions 
		/// </summary>
		internal void ClearAllQueries()
		{
			try
			{
				OMETrace.WriteFunctionStart();
				int count = 0;
				//Get all query groups added to query builder
				
				if (tableLayoutPanelQueries != null)
				{
					count = tableLayoutPanelQueries.Controls.Count;
					for (int i = count; i > 1; i--)
					{
						tableLayoutPanelQueries.Controls.RemoveAt(i - 1);
						QueryGroupCount--;
					}
					DataGridViewGroup dataGridViewGroup = (DataGridViewGroup) tableLayoutPanelQueries.Controls[0];
					dbDataGridView dataGridView = dataGridViewGroup.DataGridViewQuery;

					//Clear all query expressions
					if (dataGridView != null && dataGridView.RowCount > 0)
					{
						dataGridView.Rows.Clear();
					}
					tableLayoutPanelQueries.Height = tableLayoutPanelQueries.Height - dataGridViewGroup.Height;
				}
					//Clears the attribute list
					if (dbDataGridAttributes != null && dbDataGridAttributes.RowCount > 0)
					{
						dbDataGridAttributes.Rows.Clear();
					}

					
				// Remove base class from helper base class hashtable
				// so that next time it will set for the new class
				Helper.HashTableBaseClass.Clear();

				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Validate the Query Expression for empty value
		/// </summary>
		/// <returns></returns>
		internal bool IsValidQuery(out string errorMessage)
		{
			errorMessage = string.Empty;
			try
			{
				OMETrace.WriteFunctionStart();

				string valueColumn = Helper.GetResourceString(Constants.QUERY_GRID_VALUE);

				int totalQueryGroups = tableLayoutPanelQueries.Controls.Count;

				for (int i = 0; i < totalQueryGroups; i++)
				{
					DataGridViewGroup dataGridViewGrp = (DataGridViewGroup)tableLayoutPanelQueries.Controls[i];
					dbDataGridView datagridView = dataGridViewGrp.DataGridViewQuery;

					if (totalQueryGroups > 1 && i == 0 && datagridView.Rows.Count == 0)
					{
						errorMessage = dataGridViewGrp.LabelQueryGroup +
									   Helper.GetResourceString(Constants.VALIDATION_DEFAULT_GROUP_IS_EMPTY);
						return false;
					}

					for (int j = 0; j < datagridView.Rows.Count; j++)
					{
						string type = datagridView.Rows[j].Cells[Constants.QUERY_GRID_FIELDTYPE_DISPLAY_HIDDEN ].Value.ToString();
						if (type != BusinessConstants.STRING && type != BusinessConstants.CHAR)
						{
							if (datagridView.Rows[j].Cells[valueColumn].Value == null)
							{
								errorMessage = Helper.GetResourceString(Constants.VALIDATION_MESSAGE_VALUE_NOT_SPECIFIED);
								return false;
							}
							
							if (string.IsNullOrEmpty(datagridView.Rows[j].Cells[valueColumn].Value.ToString()))
							{
								errorMessage = Helper.GetResourceString(Constants.VALIDATION_MESSAGE_VALUE_NOT_SPECIFIED);
								return false;
							}
						}
					}
				}

				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}


			return true;
		}

		/// <summary>
		/// Sets the OMQuery to the cached list
		/// </summary>
		/// <param name="omQuery"></param>
		private static void SetOMQuery(OMQuery omQuery)
		{
			try
			{
				if (Helper.OMResultedQuery.ContainsKey(omQuery.BaseClass))
				{
					Helper.OMResultedQuery[omQuery.BaseClass] = omQuery;
				}
				else
				{
					Helper.OMResultedQuery.Add(omQuery.BaseClass, omQuery);
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Resets the Query Expression Group if any group is deleted
		/// </summary>
		private void RenameQueryGroupCaption()
		{
			try
			{
				for (int i = 0; i < tableLayoutPanelQueries.Controls.Count; i++)
				{
					DataGridViewGroup dataGridViewGrp = (DataGridViewGroup)tableLayoutPanelQueries.Controls[i];
					dataGridViewGrp.LabelQueryGroup = Helper.GetResourceString(Constants.QUERY_GROUP_CAPTION) + i;
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Resets the Query Expression Group if any group is deleted
		/// </summary>
		public List<string> GetAllQueryGroups()
		{
			List<string> groupCollection = new List<string>();
			try
			{
				int totalQueryGroups = tableLayoutPanelQueries.Controls.Count;
				for (int i = 0; i < totalQueryGroups; i++)
				{
					DataGridViewGroup dataGridViewGrp = (DataGridViewGroup)tableLayoutPanelQueries.Controls[i];
					groupCollection.Add(dataGridViewGrp.LabelQueryGroup);
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}

			return groupCollection;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Poulates Query in Query Result pane/recent query selected
		/// </summary>
		/// <param name="omQry"></param>
		public void PopulateQueryBuilderGroup(OMQuery omQry)
		{
			List<OMQueryGroup> listOMQueryGroup;
			Hashtable listQueryAttributes;
			OMQueryGroup omGroup;
			DataGridViewGroup group;
			OMQuery omQuery;

			try
			{
				OMETrace.WriteFunctionStart();

				//Set the omQuery 
				if (omQry != null)
				{
					omQuery = omQry;

					//query is not available then dont do anything
					if (omQuery == null)
						return;

					listOMQueryGroup = omQuery.ListQueryGroup;
					Helper.HashTableBaseClass.Clear();
					Helper.BaseClass = Helper.ClassName = omQuery.BaseClass;

					//Only Add when recent query is populated 
					if (omQuery.ListQueryGroup.Count > 0)
					{
						Helper.HashTableBaseClass.Add(omQuery.BaseClass, string.Empty);
					}

					for (int i = 0; i < listOMQueryGroup.Count; i++)
					{
						//Add DataGridViewGrop
						omGroup = listOMQueryGroup[i];
						group = i != 0 ? AddDataGridViewToPanel() : defaultGroup;

						for (int j = 0; j < omGroup.ListQueryClauses.Count; j++)
						{
							//Get the clauses from the group
							OMQueryClause omQueryClause = omGroup.ListQueryClauses[j];
							dbDataGridView gridQuery = group.DataGridViewQuery;
							gridQuery.Rows.Add(1);

							ProxyType fieldType = AssemblyInspectorObject.DataType.ResolveType(omQueryClause.FieldType);
						
                            
                            //Fill the Conditions depending upon the field name
                            gridQuery.FillConditionsCombobox(fieldType, j);
							gridQuery.FillOperatorComboBox();

							gridQuery.Rows[j].Cells[Helper.GetResourceString(Constants.QUERY_GRID_FIELD)].Value = omQueryClause.Fieldname;
							gridQuery.Rows[j].Cells[Helper.GetResourceString(Constants.QUERY_GRID_CONDITION)].Value = omQueryClause.Operator;
							gridQuery.Rows[j].Cells[Helper.GetResourceString(Constants.QUERY_GRID_VALUE)].Value = omQueryClause.Value;
							gridQuery.Rows[j].Cells[Helper.GetResourceString(Constants.QUERY_GRID_OPERATOR)].Value = omQueryClause.ClauseLogicalOperator.ToString();

							//Make the operator cell readonly for other than 1st Rows
							if (j > 0)
								gridQuery.Rows[j].Cells[Helper.GetResourceString(Constants.QUERY_GRID_OPERATOR)].ReadOnly = true;

							gridQuery.Rows[j].Cells[Constants.QUERY_GRID_CALSSNAME_HIDDEN].Value = omQueryClause.Classname;
                            gridQuery.Rows[j].Cells[Constants.QUERY_GRID_FIELDTYPE_HIDDEN].Value = fieldType.DisplayName;
                            gridQuery.Rows[j].Cells[Constants.QUERY_GRID_FIELDTYPE_DISPLAY_HIDDEN].Value = fieldType;
                            
						}

						//Set the logical operator for Query Group
						group.OperatorComboBox.SelectedItem = omGroup.GroupLogicalOperator.ToString();

						if (group.DataGridViewQuery.Rows.Count > 0)
							buttonRunQuery.Enabled = true;
					}

					listQueryAttributes = omQuery.AttributeList;

					int count = 0;
					if (listQueryAttributes != null)
					{
						IDictionaryEnumerator enumerator = listQueryAttributes.GetEnumerator();
						while (enumerator.MoveNext())
						{
							dbDataGridAttributes.Rows.Add(1);
							dbDataGridAttributes.Rows[count].Cells[0].Value = enumerator.Key.ToString();
							dbDataGridAttributes.Rows[count].Cells[0].Tag = enumerator.Value.ToString();
							count++;
						}
					}

					OMETrace.WriteFunctionEnd();
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Preapers OMQuery object
		/// </summary>
		/// <returns></returns>
		public OMQuery PrepareOMQuery()
		{
			try
			{
				OMETrace.WriteFunctionStart();

				//Get count of query groups 
				int totalQueryGroups = tableLayoutPanelQueries.Controls.Count;

				//iterate through all query groups
				for (int i = 0; i < totalQueryGroups; i++)
				{
					DataGridViewGroup dataGridViewGrp = (DataGridViewGroup)tableLayoutPanelQueries.Controls[i];

					dbDataGridView datagridView = dataGridViewGrp.DataGridViewQuery;

					//Prepare the query expression for each query group
					OMQueryGroup omQueryGroup = PrepareQueryCollection(datagridView);

					//if query builder has only one group get new instance of OMQueryGroup
					//and set the logical operator to empty
					if (i == 0)
					{
						if (omQueryGroup == null)
						{
							omQueryGroup = new OMQueryGroup();
						}
						omQueryGroup.GroupLogicalOperator = CommonValues.LogicalOperators.EMPTY;
					}
					else //Get the selected operatior for each query group
					{
						//Ignore empty group
						if (omQueryGroup == null)
							continue;

						omQueryGroup.GroupLogicalOperator = (CommonValues.LogicalOperators)
															Enum.Parse(typeof(CommonValues.LogicalOperators),
																	   dataGridViewGrp.QueryGroupOperator);
					}

					if (omQuery.BaseClass != null)
						Helper.BaseClass = omQuery.BaseClass;
					else
					{
						if (Helper.HashTableBaseClass.Count > 0)
						{
							IDictionaryEnumerator enumerator = Helper.HashTableBaseClass.GetEnumerator();
							enumerator.MoveNext();
							string baseClass = enumerator.Key.ToString();

							Helper.BaseClass = baseClass;
						}
					}

					omQuery.AttributeList = GetSelectedAttributes(); //Helper.GetQueryAttributes();

					//Add OMQueryGroup to OMQuery
					if (omQueryGroup != null)
						omQuery.AddOMQuery(omQueryGroup);
				}

				omQuery.QueryString = omQuery.ToString();

				//Set the OMQuery object to the Resulted query
				if (Helper.OMResultedQuery.ContainsKey(omQuery.BaseClass))
				{
					//update omquery if query is prepared not for first time
					Helper.OMResultedQuery[omQuery.BaseClass] = omQuery;
				}
				else
				{
					//Add omquery if query is prepared for first time
					Helper.OMResultedQuery.Add(omQuery.BaseClass, omQuery);
				}

				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}

			return omQuery;
		}

		/// <summary>
		/// Fetch the all the Attributes added to Attribute Grid
		/// </summary>
		public Hashtable GetSelectedAttributes()
		{
			Hashtable list = new Hashtable();

			try
			{
				OMETrace.WriteFunctionStart();
				if (dbDataGridAttributes != null)
				{
					for (int attributeCount = 0; attributeCount < dbDataGridAttributes.RowCount; attributeCount++)
					{
						list.Add(dbDataGridAttributes.Rows[attributeCount].Cells[0].Value.ToString()
						         , dbDataGridAttributes.Rows[attributeCount].Cells[0].Tag.ToString());
					}
				}

				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
			return list;
		}

		/// <summary>
		/// Set all litrals for this control
		/// </summary>
		public override void SetLiterals()
		{
			try
			{
				labelRecentQueries.Text = Helper.GetResourceString(Constants.LABEL_RECENT_QUERIES_CAPTION);
				labelBuildQuery.Text = Helper.GetResourceString(Constants.LABEL_QUERY_BUILDER_CAPTION);
				buttonAddQueryGroup.Text = Helper.GetResourceString(Constants.BUTTON_ADD_GROUP_CAPTION);
				buttonClearAll.Text = Helper.GetResourceString(Constants.BUTTON_CLEAR_AAL_CAPTION);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private static void SafeSetEnabled(CommandBarControl control, bool enabled)
		{
			if (control != null)
				control.Enabled = enabled;
		}

		#endregion
        
	}
}