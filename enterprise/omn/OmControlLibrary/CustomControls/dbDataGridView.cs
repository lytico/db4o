using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using OMAddinDataTransferLayer;
using OMAddinDataTransferLayer.TypeMauplation;
using OManager.BusinessLayer.Common;
using OManager.BusinessLayer.ObjectExplorer;
using OME.Logging.Common;
using OME.Logging.Tracing;

using Type=System.Type;

namespace OMControlLibrary.Common
{
	public partial class dbDataGridView : DataGridView
	{
		#region Private Members

		private ContextMenuStrip m_columnContextMenuStrip; //column context menu
		private ContextMenuStrip m_rowContextMenuStrip; //Row context menu
		private const string SHOW_ALL_COLUMN = "SHOW_ALL_COLUMN";
		private bool m_CellClick;
		private int m_CurrentRowNumber;


		public event EventHandler<dbDataGridViewEventArgs> OnDBGridCellClick;

		//Constants
		private const string MENU_SEPARATOR = "MENU_SEPARATOR";
		private const string COLUMN_NO_NAME = "No.";
		private const string VALUE_NULL = "null";
		#endregion

		#region Constructor

		public dbDataGridView()
		{
			SetStyle(ControlStyles.CacheText | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Opaque, true);
			SetDefaultProperties();

		}

		#endregion

		#region Override Methods

		protected override void OnDragDrop(DragEventArgs e)
		{
			try
			{
				base.OnDragDrop(e);

				Point ptToClient = PointToClient(new Point(e.X, e.Y));

				DragHelper.ImageList_DragMove(ptToClient.X, ptToClient.Y);
				e.Effect = DragDropEffects.Move;

			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);

			e.Effect = DragDropEffects.Move;
		}

		protected override void OnDragEnter(DragEventArgs e)
		{
			try
			{

				base.OnDragEnter(e);
				e.Effect = DragDropEffects.Move;
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			try
			{
				//Reset m_CellClick, as when user left clicks on the same cell,
				//(mostly in case of multiple row selection) we need to fire the
				//OnDBGridCellClick event
				m_CellClick = false;
				base.OnMouseDown(e);

				DoubleBuffered = false;

				//set to false for users to receive visual feedback when when reordering columns.
				HitTestInfo hitTestInfo = HitTest(e.X, e.Y);

				//setting member variable IsHeaderClicked = true, to set cancel 
				// contextmenu on ColumnHeader.
				DataGridViewHitTestType hitTestType = hitTestInfo.Type;
				if (e.Button == MouseButtons.Right &&
					hitTestType == DataGridViewHitTestType.ColumnHeader ||
					hitTestType == DataGridViewHitTestType.RowHeader ||
					hitTestType == DataGridViewHitTestType.TopLeftHeader)
				{
					ContextMenuStrip = m_columnContextMenuStrip;
				}
				else if (hitTestType == DataGridViewHitTestType.Cell)
				{
					ContextMenuStrip = m_rowContextMenuStrip;
				}

				if (Rows.Count < 1)
				{
					if (ContextMenuStrip != null)
						ContextMenuStrip = null;

					return;
				}

				//If user clicks on blank space below grid, then remove the context menu
				if (hitTestInfo.Type == DataGridViewHitTestType.None)
				{
					ContextMenuStrip = null;
					return;
				}

				if (hitTestInfo.Type != DataGridViewHitTestType.Cell && CurrentRow != null)
				{
					m_CurrentRowNumber = CurrentRow.Index;
					return;
				}

				m_CurrentRowNumber = hitTestInfo.RowIndex;
				if (hitTestInfo.RowIndex == Constants.INVALID_INDEX_VALUE)
					return;

				if (e.Button == MouseButtons.Right)
				{
					if (SelectedRows.Count < 2)
						ClearSelection();
					if (SelectedRows.Count < 1)
						Rows[hitTestInfo.RowIndex].Selected = true;
				}

				if (CurrentRow != null)
					CurrentRow.Visible = true;

				//fireing OnDBCellClick event.
				if (!m_CellClick && e.Button == MouseButtons.Left)
				{
					if (OnDBGridCellClick != null)
					{
						OnDBGridCellClick(this, new dbDataGridViewEventArgs(CurrentCell));
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
				ContextMenuStrip = null;
			}
		}

		protected override void OnCurrentCellChanged(EventArgs e)
		{
			base.OnCurrentCellChanged(e);

			//fireing OnDBCellClick event.
			try
			{
				if (CurrentCell == null)
					return;

				//Does not fire OnAICellClick event if current row is not 
				//chage for the cell or cell is header cell.
				if (CurrentCell.RowIndex == Constants.INVALID_INDEX_VALUE ||
					CurrentCell.RowIndex == m_CurrentRowNumber)
				{
					return;
				}

				m_CurrentRowNumber = CurrentCell.RowIndex;

				CurrentRow.Visible = true;

				if (!m_CellClick)
				{
					if (OnDBGridCellClick != null)
					{
						OnDBGridCellClick(this, new dbDataGridViewEventArgs(CurrentCell));
					}
					m_CellClick = true;
				}
				else
					m_CellClick = false;
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
		#endregion

		#region Public Method

		/// <summary>
		/// Build context menu for column to set column visibility.
		/// </summary>
		public void BuildColumnContextMenu(string showAllMenuText, params string[] alwaysVisibleColumns)
		{
			DataGridViewColumn column;
			int colCount = ColumnCount;
			int visibaleColumnCount = 0;

			m_columnContextMenuStrip = new ContextMenuStrip();

			string showAllColumnText = showAllMenuText;
			ToolStripMenuItem objMenu;

			try
			{
				OMETrace.WriteFunctionStart();

				string menuText;
				string menuName;
				for (int colIndex = 0; colIndex < colCount; ++colIndex)
				{
					column = Columns[colIndex];
					menuText = column.HeaderText;
					menuName = column.Name;

					if (column.Visible && string.IsNullOrEmpty(menuText) == false)
					{
						++visibaleColumnCount;
						objMenu = new ToolStripMenuItem(menuText);
						objMenu.Name = menuName;
						objMenu.Checked = true;
						m_columnContextMenuStrip.Items.Add(objMenu);
					}
				}

				//if only visible column count is one then do not need any context menu.
				//to show and hide it.
				if (visibaleColumnCount < 2)
				{
					m_columnContextMenuStrip = null;
					return;
				}

				//disable menu item for column 
				for (int count = 0; count < alwaysVisibleColumns.Length; count++)
				{
					if (m_columnContextMenuStrip.Items.ContainsKey(alwaysVisibleColumns[count]))
						m_columnContextMenuStrip.Items[alwaysVisibleColumns[count]].Enabled = false;
				}

				if (m_columnContextMenuStrip.Items.Count > 0)
				{
					//add separator before showall menu item
					ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
					toolStripSeparator.Name = MENU_SEPARATOR;
					m_columnContextMenuStrip.Items.Add(toolStripSeparator);

					//add show all menu item
					menuText = showAllColumnText;
					menuName = SHOW_ALL_COLUMN;
					objMenu = new ToolStripMenuItem(menuText);
					objMenu.Name = menuName;
					m_columnContextMenuStrip.Items.Add(objMenu);
				}

				ContextMenuStrip = m_columnContextMenuStrip;
				ContextMenuStrip.ItemClicked += ContextMenuStrip_ItemClicked;

				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		/// <summary>
		/// Sync. the column visibility while toggeling between perespective view and
		/// normal view.
		/// </summary>
		public void SetColumnVisible()
		{
			try
			{
				if (m_columnContextMenuStrip == null)
					return;

				DataGridViewColumn column;
				int menuCount = m_columnContextMenuStrip.Items.Count;
				ToolStripMenuItem objMenuItem;
				for (int iMenuIndex = 0; iMenuIndex < menuCount; ++iMenuIndex)
				{
					objMenuItem =
						m_columnContextMenuStrip.Items[iMenuIndex] as ToolStripMenuItem;

					if (objMenuItem != null && objMenuItem.Enabled &&
						objMenuItem.Name != SHOW_ALL_COLUMN)
					{
						column = Columns[objMenuItem.Name];
						if (objMenuItem.Checked)
						{
							column.Visible = true;
							objMenuItem.Checked = true;
						}
						else
						{
							column.Visible = false;
							objMenuItem.Checked = false;
						}
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}


		public void BuildRowContextMenu()
		{

			m_rowContextMenuStrip = new ContextMenuStrip();

			string menuName = string.Empty;
			string menuText = string.Empty;

			ToolStripMenuItem objMenu;

			try
			{
				menuText = Helper.GetResourceString(Constants.CONTEXT_MENU_CAPTION.Replace(Constants.HOT_KEY_CHAR, string.Empty));
				menuName = Constants.CONTEXT_MENU_DATAGRID_DELETE;

				objMenu = new ToolStripMenuItem(menuText);
				objMenu.Name = menuName;

				m_rowContextMenuStrip.Items.Add(objMenu);

				ContextMenuStrip = m_rowContextMenuStrip;
				ContextMenuStrip.ItemClicked += ContextMenuStrip_ItemClicked;
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}

		}
		#endregion

		#region Private Events

		private void ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			DataGridViewColumn column;
			string menuItemName = e.ClickedItem.Name;

			try
			{
				if (Columns.Contains(menuItemName))
				{
					column = Columns[menuItemName];

					//do not process if column index is zero.
					//making sure atleast one column should be visible.
					ToolStripMenuItem menuItem = ContextMenuStrip.Items[menuItemName] as ToolStripMenuItem;

					if (column.Visible)
					{
						column.Visible = false;
						menuItem.Checked = false;
					}
					else
					{
						column.Visible = true;
						menuItem.Checked = true;
					}
				}
				else if (menuItemName == SHOW_ALL_COLUMN)
				{
					//show all columns when show all column menu is clicked.
					ShowAllColumn();
				}

				if (e.ClickedItem.Name == Constants.CONTEXT_MENU_DATAGRID_DELETE)
				{
					ToolStripMenuItem menuItem = (ToolStripMenuItem)ContextMenuStrip.Items[0];
					menuItem.Checked = false;

					ContextMenuStrip.Dispose();
					if (SelectedRows.Count > 1)
					{
						List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();

						int rowCount = Rows.Count;
						for (int i = 0; i < rowCount; i++)
						{
							if (Rows[i].Selected)
								selectedRows.Add(Rows[i]);
						}

						for (int i = 0; i < selectedRows.Count; i++)
						{
							Rows.Remove(selectedRows[i]);
						}
						selectedRows.Clear();
					}
					else
						Rows.RemoveAt(m_CurrentRowNumber);
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
			finally
			{
				ContextMenu = null;
				ContextMenuStrip = null;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Shows all the columns 
		/// fires clicked on the "Show all columns" from column context menu.
		/// </summary>
		private void ShowAllColumn()
		{
			try
			{
				if (m_columnContextMenuStrip == null)
					return;

				DataGridViewColumn column;
				int menuCount = m_columnContextMenuStrip.Items.Count;
				ToolStripMenuItem objMenuItem;
				for (int iMenuIndex = 0; iMenuIndex < menuCount; iMenuIndex++)
				{
					objMenuItem = m_columnContextMenuStrip.Items[iMenuIndex] as ToolStripMenuItem;

					if (objMenuItem != null && objMenuItem.Enabled &&
						objMenuItem.Name != SHOW_ALL_COLUMN)
					{
						column = Columns[objMenuItem.Name];
						column.Visible = true;
						objMenuItem.Checked = true;
					}
				}

			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		/// <summary>
		/// This method is used to set default behaviour of the DataGridView.
		/// </summary>
		private void SetDefaultProperties()
		{
			BackgroundColor = Color.White;
			RowHeadersVisible = false;
			AutoGenerateColumns = false;
			AllowUserToResizeRows = false;
			AllowUserToAddRows = false;
			AllowUserToOrderColumns = false;
			AllowDrop = true;
			MultiSelect = false;
			EditMode = DataGridViewEditMode.EditOnEnter;
			ScrollBars = ScrollBars.Both;
			ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			AllowUserToDeleteRows = false;
			AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			Visible = true;
			AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			AutoGenerateColumns = false;
			EnableHeadersVisualStyles = false;
			DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
			DataGridViewCellStyle cellstyle = new DataGridViewCellStyle();
			cellstyle.BackColor = Color.FromArgb(218, 232, 241);
			SetColumnHeaderStyle();
		}

		public void SetColumnHeaderStyle()
		{
			try
			{
				DataGridViewCellStyle headerCellStyle = new DataGridViewCellStyle();
				const float fontSize = 8;
				Font headerFont = new Font("Tahoma", fontSize, FontStyle.Regular);
				headerCellStyle.ForeColor = Color.Black;
				headerCellStyle.BackColor = SystemColors.Control;
				headerCellStyle.Font = headerFont;
				ColumnHeadersHeight = 20;
				ColumnHeadersDefaultCellStyle = headerCellStyle;

				DataGridViewCellStyle cellstyle = new DataGridViewCellStyle();
				cellstyle.Font = headerFont;
				DefaultCellStyle = cellstyle;
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		#endregion

		#region Fill QueryResult Datagrid methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hashColumn"></param>
		/// <param name="className"></param>
		/// <param name="hashAttributes"></param>
		internal void SetDataGridColumnHeader(Hashtable hashColumn, string className, Hashtable hashAttributes)
		{
			try
			{
				Columns.Add(Constants.QUERY_GRID_ISEDITED_HIDDEN, Constants.QUERY_GRID_ISEDITED_HIDDEN);
				Columns.Add(COLUMN_NO_NAME, COLUMN_NO_NAME);
				Columns[Constants.QUERY_GRID_ISEDITED_HIDDEN].Visible = false;
				Columns[Constants.QUERY_GRID_ISEDITED_HIDDEN].Width = 0;
				Columns[COLUMN_NO_NAME].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

				foreach (DictionaryEntry entry in hashColumn)
				{
					if (entry.Key.ToString().Equals(BusinessConstants.DB4OBJECTS_REF)) 
						continue;
					
                    dbDataGridViewDateTimePickerColumn valueTextBoxColumn = CreateDateTimeAndComboBoxColumn(entry.Key.ToString(), entry.Key.ToString(), DataGridViewColumnSortMode.Automatic);
					Columns.Add(valueTextBoxColumn);

					if (hashAttributes.Count != 0)
					{
						string strTag = hashAttributes[entry.Key.ToString()] as string;
						Columns[entry.Key.ToString()].Tag = strTag;
					}
					else
					{
						Columns[entry.Key.ToString()].Tag = className;
					}

					BuildColumnContextMenu(Helper.GetResourceString(Constants.GRID_SHOW_ALL_COLUMN), COLUMN_NO_NAME);
				}
				Columns[Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
		
		internal void SetDataGridColumnHeader(List<Hashtable> resultList, string className, Hashtable hashAttributes)
		{
			Rows.Clear();
			Columns.Clear();
			SetDataGridColumnHeader(BiggestFieldList(resultList), className, hashAttributes);
		}

		private static Hashtable BiggestFieldList(IList<Hashtable> resultList)
		{
			if (resultList.Count == 0)
			{
				return new Hashtable();
			}

			Hashtable hColumn = resultList[0];
			foreach (Hashtable current in resultList)
			{
				if (current.Count < hColumn.Count)
				{
					hColumn = current;
				}
			}
			
			return hColumn;
		}

		internal void SetDatagridRows(List<Hashtable> resultList, string className, Hashtable hashAttributes, int index)
		{
			try
			{
				Rows.Clear();
			    foreach (Hashtable fields in resultList)
			    {
				    AddFields(fields, className, index++, hashAttributes);
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

	    private void AddFields(Hashtable fields, string containingClass, int index, Hashtable hashAttributes)
	    {
	        DataGridViewRow row = NewRow(index);
	        foreach (DictionaryEntry entry in fields)
	        {
	            string fieldName = entry.Key.ToString();
	            if (!fieldName.Equals(BusinessConstants.DB4OBJECTS_REF))
	            {
	                ProxyType  fieldType = FieldTypeFor(containingClass, fieldName, hashAttributes);
	                if (fieldType == null)
	                    continue;

	                DataGridViewCell cell = row.Cells[fieldName];
	                cell.Tag = fieldType;

	                cell.ReadOnly = !(fieldType.IsEditable);
                    if (fieldType.IsEditable  )
	                {
						cell.Value =  AssemblyInspectorObject.DataType.CastedValueOrNullConstant(entry.Value, fieldName, fieldType.ContainingClassName);
	                }
	                else
	                {
	                    cell.Value = entry.Value != null 
	                                     ? entry.Value.ToString()
	                                     : VALUE_NULL;
	                }
	            }
	            else
	                row.Tag = entry.Value;
	        }
	    }

	    private ProxyType FieldTypeFor(string className, string fieldName, Hashtable hashAttributes)
	    {
	        ProxyType fieldType = null;
	        if (hashAttributes.Count == 0)
	        {
				fieldType = AssemblyInspectorObject.DataType.GetFieldType(className, fieldName);
	        	fieldType.ContainingClassName = className;
	        }
	        else
	        {
	            int intIndex = fieldName.LastIndexOf('.');
	            string strAttribName = fieldName.Substring(intIndex + 1);
	            string clsName = Columns[fieldName].Tag.ToString();
				if (clsName != null && strAttribName != null)
				{
					fieldType = AssemblyInspectorObject.DataType.GetFieldType(clsName, strAttribName);
					fieldType.ContainingClassName = clsName;
				}

	        }
	        return fieldType;
	    }

	    private DataGridViewRow NewRow(int index)
	    {
	        int rowIndex = Rows.Add(1);

	        DataGridViewRow row = Rows[rowIndex];

	        row.Cells[COLUMN_NO_NAME].Value = index;
            row.Cells[COLUMN_NO_NAME].ReadOnly = true;
            
            // For checking if row is edited or not
            row.Cells[Constants.QUERY_GRID_ISEDITED_HIDDEN].Value = false;
	        row.Cells[Constants.QUERY_GRID_ISEDITED_HIDDEN].Tag = 1; //(for saving object, this will work as updatedepth)

	        return row;
	    }
	    

	    #endregion

		#region Listing Helper Class

		#region Member variables

		#region Column Header Variables

		//QueryBuilder
		private string m_FieldHeaderText;
		private string m_ConditionHeaderText;
		private string m_ValueHeaderText;
		private string m_OperatorHeaderText;

		//Property Table for Class
		private string m_FieldNameHeaderText;
		private string m_DataTypeHeaderText;
		private string m_IsIndexedHeaderText;
		private string m_IsPublicHeaderText;
	    private string m_TypeNameHeaderText;

		//Class Property Table for Objects
		private string m_UUIDHeaderText;
		private string m_LocalIDHeaderText;
		private string m_VersionHeaderText;

		private string m_AttributesHeaderText;

		//Database Property Table for Objects
		private string m_DataBaseSizeHeaderText;
		private string m_NumberOfClassesHeaderText;
		private string m_FreeSpaceHeaderText;

		#endregion

		#endregion

		#region Initialization Columns Methods

		/// <summary>
		/// Initializing Class Properties columns
		/// </summary>
		internal void InitClassPropertyColumns()
		{
			try
			{
				SetLiteralsClassPropertyColumn();

				Columns.Clear();

				AutoGenerateColumns = false;
				AllowUserToDeleteRows = false;

				DataGridViewColumn fieldNameTextBoxColumn = New<DataGridViewTextBoxColumn>(
																	m_FieldNameHeaderText, 
																	m_FieldNameHeaderText, 
																	DataGridViewColumnSortMode.NotSortable, 
																	DataGridViewAutoSizeColumnMode.Fill);
				fieldNameTextBoxColumn.ReadOnly = true;
				DataGridViewColumn dataTypeTextBoxColumn = New<DataGridViewTextBoxColumn>(
																	m_DataTypeHeaderText, 
																	m_DataTypeHeaderText, 
																	DataGridViewColumnSortMode.NotSortable, 
																	DataGridViewAutoSizeColumnMode.Fill);
				dataTypeTextBoxColumn.ReadOnly = true;

				DataGridViewCheckBoxColumn isIndexedCheckBoxColumn = CreateCheckBoxColumn(
																			m_IsIndexedHeaderText, 
																			m_IsIndexedHeaderText, 
																			DataGridViewColumnSortMode.NotSortable);
				isIndexedCheckBoxColumn.ReadOnly = false;

				DataGridViewCheckBoxColumn isPublicCheckBoxColumn = CreateCheckBoxColumn(
																			m_IsPublicHeaderText, 
																			m_IsPublicHeaderText, 
																			DataGridViewColumnSortMode.NotSortable);
				isPublicCheckBoxColumn.ReadOnly = true;
                DataGridViewColumn typeTextBoxColumn = New<DataGridViewTextBoxColumn>(
                                                                    m_TypeNameHeaderText,
                                                                    m_TypeNameHeaderText,
                                                                    DataGridViewColumnSortMode.NotSortable,
                                                                    DataGridViewAutoSizeColumnMode.Fill);
                typeTextBoxColumn.Visible = false;
                Columns.AddRange(new DataGridViewColumn[] { fieldNameTextBoxColumn, dataTypeTextBoxColumn, isIndexedCheckBoxColumn, isPublicCheckBoxColumn, typeTextBoxColumn });

			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Initializing Class QueryBuilder columns
		/// </summary>
		internal void InitQueryBuilderColumns()
		{
			try
			{
				//Set header text for the Query grid columns
				SetLiteralsQueryGridColumn();

				//Clear all the columns before creating columns
				Columns.Clear();

				AutoGenerateColumns = false;
				AllowUserToAddRows = false;
				AllowUserToDeleteRows = false;

				DataGridViewTextBoxColumn fieldNameTextBoxColumn =
					CreateTextBoxColumn(m_FieldHeaderText, m_FieldHeaderText, DataGridViewColumnSortMode.NotSortable);
				fieldNameTextBoxColumn.ReadOnly = true;

				DataGridViewComboBoxColumn conditionComboBoxColumn =
					CreateComboBoxColumn(m_ConditionHeaderText, m_ConditionHeaderText, DataGridViewColumnSortMode.NotSortable);
				dbDataGridViewDateTimePickerColumn valueTextBoxColumn =
					CreateDateTimeAndComboBoxColumn(m_ValueHeaderText, m_ValueHeaderText, DataGridViewColumnSortMode.NotSortable);
				DataGridViewComboBoxColumn operatorComboBoxColumn =
					 CreateComboBoxColumn(m_OperatorHeaderText, m_OperatorHeaderText, DataGridViewColumnSortMode.NotSortable);
				DataGridViewTextBoxColumn classNameTextBoxColumn =
					CreateTextBoxColumn(string.Empty, Constants.QUERY_GRID_CALSSNAME_HIDDEN, DataGridViewColumnSortMode.NotSortable);
				classNameTextBoxColumn.Visible = false;

                DataGridViewTextBoxColumn DisplayclassNameTextBoxColumn =
                    CreateTextBoxColumn(string.Empty, Constants.QUERY_GRID_FIELDTYPE_DISPLAY_HIDDEN, DataGridViewColumnSortMode.NotSortable);
                DisplayclassNameTextBoxColumn.Visible = false;

				DataGridViewTextBoxColumn fieldTypeTextBoxColumn =
								   CreateTextBoxColumn(string.Empty, Constants.QUERY_GRID_FIELDTYPE_HIDDEN,
								   DataGridViewColumnSortMode.NotSortable);
				fieldTypeTextBoxColumn.Visible = false;

				fieldNameTextBoxColumn.Width = 230;
				conditionComboBoxColumn.Width = 100;
				valueTextBoxColumn.Width = 210;
				operatorComboBoxColumn.Width = 97;

				Columns.AddRange(
					new DataGridViewColumn[] {
                    fieldNameTextBoxColumn, 
                    conditionComboBoxColumn,
                    valueTextBoxColumn,
                    operatorComboBoxColumn,
                    classNameTextBoxColumn,
                    DisplayclassNameTextBoxColumn,
                    fieldTypeTextBoxColumn
                });

				//Prepare Oprator Column
				FillOperatorComboBox();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Initializing Class Attributes columns
		/// </summary>
		internal void InitAttributeColumns()
		{
			try
			{
				//Set header text for the attribute grid column
				SetLitralsAdttributesColumn();

				//Clear all the columns before creating columns
				Columns.Clear();

				AutoGenerateColumns = false;
				//this.AllowUserToAddRows = false;
				AllowUserToDeleteRows = true;
				ReadOnly = true;

				DataGridViewTextBoxColumn fieldNameTextBoxColumn =
					CreateTextBoxColumn(m_AttributesHeaderText, m_AttributesHeaderText, DataGridViewColumnSortMode.NotSortable);

				Columns.Add(fieldNameTextBoxColumn);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Initializing Database Property columns
		/// </summary>
		internal void InitDatabasePropertyColumns()
		{
			try
			{
				SetLitralsDatabasePropertiesColumn();

				Columns.Clear();

				AutoGenerateColumns = false;
				AllowUserToDeleteRows = false;
				ReadOnly = true;

				DataGridViewTextBoxColumn databaseSizeTextBoxColumn =
					CreateTextBoxColumn(m_DataBaseSizeHeaderText, m_DataBaseSizeHeaderText,
										DataGridViewColumnSortMode.NotSortable);

				DataGridViewTextBoxColumn noOfClasessTextBoxColumn =
					CreateTextBoxColumn(m_NumberOfClassesHeaderText, m_NumberOfClassesHeaderText,
										DataGridViewColumnSortMode.NotSortable);

				DataGridViewTextBoxColumn freeSpaceTextBoxColumn = New<DataGridViewTextBoxColumn>(
																		m_FreeSpaceHeaderText, 
																		m_FreeSpaceHeaderText,
																		DataGridViewColumnSortMode.NotSortable,
																		DataGridViewAutoSizeColumnMode.Fill);

				Columns.AddRange(
				   new DataGridViewColumn[] {
                    databaseSizeTextBoxColumn, 
                    noOfClasessTextBoxColumn,
                    freeSpaceTextBoxColumn
                });
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Initializing Objects Property columns
		/// </summary>
		internal void InitObjectsPropertyColumns()
		{
			try
			{
				SetLitralsObjectProperties();

				Columns.Clear();

				AutoGenerateColumns = false;
				AllowUserToDeleteRows = false;
				ReadOnly = true;

				DataGridViewTextBoxColumn uuidTextBoxColumn =New<DataGridViewTextBoxColumn>(
																m_UUIDHeaderText, 
																m_UUIDHeaderText, 
																DataGridViewColumnSortMode.NotSortable, 
																DataGridViewAutoSizeColumnMode.Fill);

				DataGridViewTextBoxColumn localIDTextBoxColumn = CreateTextBoxColumn(
																	m_LocalIDHeaderText, 
																	m_LocalIDHeaderText, 
																	DataGridViewColumnSortMode.NotSortable);

				DataGridViewTextBoxColumn versionTextBoxColumn = CreateTextBoxColumn(
																	m_VersionHeaderText, 
																	m_VersionHeaderText, 
																	DataGridViewColumnSortMode.NotSortable);

				Columns.AddRange(new DataGridViewColumn[]  { uuidTextBoxColumn, localIDTextBoxColumn, versionTextBoxColumn });
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// Set litrals for class properties
		/// </summary>
		private void SetLiteralsClassPropertyColumn()
		{
			try
			{
				m_FieldNameHeaderText = Helper.GetResourceString(Constants.CLASS_PROPERTY_FIELD_NAME);
				m_DataTypeHeaderText = Helper.GetResourceString(Constants.CLASS_PROPERTY_DATA_TYPE);
				m_IsIndexedHeaderText = Helper.GetResourceString(Constants.CLASS_PROPERTY_ISINDEXED);
				m_IsPublicHeaderText = Helper.GetResourceString(Constants.CLASS_PROPERTY_ISPUBLIC);
                m_TypeNameHeaderText = "Type";
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}


		private void SetLiteralsQueryGridColumn()
		{
			try
			{
				m_FieldHeaderText = Helper.GetResourceString(Constants.QUERY_GRID_FIELD);
				m_ConditionHeaderText = Helper.GetResourceString(Constants.QUERY_GRID_CONDITION);
				m_ValueHeaderText = Helper.GetResourceString(Constants.QUERY_GRID_VALUE);
				m_OperatorHeaderText = Helper.GetResourceString(Constants.QUERY_GRID_OPERATOR);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void SetLitralsAdttributesColumn()
		{
			try
			{
				m_AttributesHeaderText = Helper.GetResourceString(Constants.ATTRIB_TEXT);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void SetLitralsDatabasePropertiesColumn()
		{
			try
			{
				m_DataBaseSizeHeaderText = Helper.GetResourceString(Constants.DB_PROPERTY_SIZE);
				m_NumberOfClassesHeaderText = Helper.GetResourceString(Constants.DB_PROPERTY_CLASSES);
				m_FreeSpaceHeaderText = Helper.GetResourceString(Constants.DB_PROPERTY_FREESPACE);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void SetLitralsObjectProperties()
		{
			try
			{
				m_UUIDHeaderText = Helper.GetResourceString(Constants.OBJECT_PROPERTY_UUID);
				m_LocalIDHeaderText = Helper.GetResourceString(Constants.OBJECT_PROPERTY_LOCALID);
				m_VersionHeaderText = Helper.GetResourceString(Constants.OBJECT_PROPERTY_VERSION);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
		/// <summary>
		/// Create a new data grid view column of type DataGridViewTextboxColumn
		/// </summary>
		/// <param name="headertext"></param>
		/// <param name="name"></param>
		/// <param name="sortMode"></param>
		/// <returns></returns> 
		internal static DataGridViewTextBoxColumn CreateTextBoxColumn(string headertext, string name, DataGridViewColumnSortMode sortMode)
		{
			return New<DataGridViewTextBoxColumn>(headertext, name, sortMode, DataGridViewAutoSizeColumnMode.None);
		}

        internal static T New<T>(string headertext, string name, DataGridViewColumnSortMode sortMode, DataGridViewAutoSizeColumnMode autoSizeMode) where T : DataGridViewColumn, new()
        {
            T newColumn = new T();
            newColumn.HeaderText = headertext;
            newColumn.AutoSizeMode = autoSizeMode;
            newColumn.SortMode = sortMode;
            newColumn.Name = name;

            return newColumn;
        }

        internal static dbDataGridViewDateTimePickerColumn CreateDateTimeAndComboBoxColumn(string headertext, string name, DataGridViewColumnSortMode sortMode)
        {
            return New<dbDataGridViewDateTimePickerColumn>(headertext, name, sortMode, DataGridViewAutoSizeColumnMode.None);
        //    //dbDataGridViewDateTimePickerColumn newDataGridViewTextBoxColumn;
        //    //newDataGridViewTextBoxColumn = new dbDataGridViewDateTimePickerColumn();
        //    //newDataGridViewTextBoxColumn.HeaderText = headertext;
        //    //newDataGridViewTextBoxColumn.AutoSizeMode =
        //    //    DataGridViewAutoSizeColumnMode.None;
        //    //newDataGridViewTextBoxColumn.SortMode = sortMode;
        //    //newDataGridViewTextBoxColumn.Name = name;
        //    //return newDataGridViewTextBoxColumn;
        }
		internal static DataGridViewComboBoxColumn CreateComboBoxColumn(string headertext, string name, DataGridViewColumnSortMode sortMode)
		{
			DataGridViewComboBoxColumn newColumn = New<DataGridViewComboBoxColumn>(headertext, name, sortMode, DataGridViewAutoSizeColumnMode.None);
			newColumn.Resizable = DataGridViewTriState.True;
			newColumn.FlatStyle = FlatStyle.Popup;

			return newColumn;
		}

		internal static DataGridViewCheckBoxColumn CreateCheckBoxColumn(string headertext, string name, DataGridViewColumnSortMode sortMode)
		{
			return New<DataGridViewCheckBoxColumn>(headertext, name, sortMode, DataGridViewAutoSizeColumnMode.ColumnHeader);
		}


		internal static DataGridViewImageColumn CreateImageColumn(string headertext, string name, DataGridViewColumnSortMode sortMode)
		{
			DataGridViewImageColumn newDataGridViewImageColumn = New<DataGridViewImageColumn>(headertext, name, sortMode, DataGridViewAutoSizeColumnMode.None);
			newDataGridViewImageColumn.Resizable = DataGridViewTriState.False;
			newDataGridViewImageColumn.Image = dbImages.Delete;
			newDataGridViewImageColumn.Width = dbImages.Delete.Width + 5;
			
			return newDataGridViewImageColumn;
		}

		/// <summary>
		/// Create a new data grid view column of type DataGridViewTextboxColumn
		/// </summary>
		/// <param name="headertext"></param>
		/// <param name="name"></param>
		/// <param name="sortMode"></param>
		/// <returns></returns>
        //internal static dbDataGridViewDateTimePickerColumn CreateDateTimeAndComboBoxColumn(string headertext, string name, DataGridViewColumnSortMode sortMode)
        //{
        //    return New<dbDataGridViewDateTimePickerColumn>(headertext, name, sortMode, DataGridViewAutoSizeColumnMode.None);
        //}
		#endregion

		#region Public Method

		internal void PopulateDisplayGrid(string viewName, ArrayList dataSource)
		{
			try
			{
				if (Columns.Count < 1)
				{
					switch (viewName)
					{
						case Constants.VIEW_CLASSPOPERTY:
							InitClassPropertyColumns();
							break;
						case Constants.VIEW_QUERYBUILDER:
							InitQueryBuilderColumns();
							break;
						case Constants.VIEW_ATTRIBUTES:
							InitAttributeColumns();
							break;
						case Constants.VIEW_DBPROPERTIES:
							InitDatabasePropertyColumns();
							break;
						case Constants.VIEW_OBJECTPROPERTIES:
							InitObjectsPropertyColumns();
							break;
						default:
							break;
					}
				}

				if (dataSource != null)
				{
					DataSource = null;

					if (!AutoGenerateColumns)
						SetData(dataSource);
				}

			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		/// <summary>
		/// SetData : It is used to bind data to datagrid view (e.g. simple listing for souces,services etc.)
		/// </summary>

		/// <param name="data">ArrayList : arryaList of entity object</param>
		private void SetData(IList data)
		{
			try
			{
				int colCount = Columns.Count;
				if (colCount <= 0)
					return;

				if (null == data)
					return;

				for (int dataIndex = 0; dataIndex < data.Count; dataIndex++)
				{
					object dataobject = data[dataIndex];
					Rows.Add(new DataGridViewRow());

					for (int colIndex = 0; colIndex < colCount; colIndex++)
					{
						DataGridViewColumn col = Columns[colIndex];

						Type dataobjectType = dataobject.GetType();
						PropertyInfo pi = dataobjectType.GetProperty(col.Name);
						if (pi != null)
						{
							FillDataIntoDataGridCell(dataobject, col.Name, dataIndex, col.GetType(), pi);
						}
					}

					//Adjust the height of specified row
					if (Rows[dataIndex] != null && dataIndex != -1)
						AutoResizeRow(dataIndex);
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void FillDataIntoDataGridCell(object dataobject, string colname, int dataIndex, Type coltype, PropertyInfo pi)
		{
			try
			{
				string coltypename = coltype.ToString();
				object dataObjectValue = pi.GetValue(dataobject, null);

				//Check for the object type.
				if (dataObjectValue != null)
				{
					switch (coltypename)
					{
						//case Constants.DBDATAGRIDVIEW_TEXTBOX_COLUMN:
						//     break;
						case Constants.DBDATAGRIDVIEW_COMBOBOX_COLUMN:
							#region Image Combobox
							#endregion Image Combobox
							break;
						case Constants.DBDATAGRIDVIEW_CHECKBOX_COLUMN:
							#region Checkbox column
							Rows[dataIndex].Cells[colname].Value = dataObjectValue.ToString();
							#endregion Checkbox column
							break;
						default:
							#region Text without image
							//only text to be displayed in the cell
							Rows[dataIndex].Cells[colname].Style.WrapMode = DataGridViewTriState.True;
							
							AutoResizeRow(dataIndex);
                            if (dataObjectValue is ProxyType  )
							Rows[dataIndex].Cells[colname].Value = dataObjectValue;
                            else
                            {
                               Rows[dataIndex].Cells[colname].Value = dataObjectValue.ToString();
                            }

					        #endregion Text without image
							break;
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		#endregion

		#region Query Helper Methods

		internal void FillConditionsCombobox(ProxyType  type, int selectedrowindex)
		{
			try
			{
                string typeName = AssemblyInspectorObject.DataType.ReturnDisplayNameOfType(type.FullName );

                string[] conditionList = QueryHelper.ConditionsFor(typeName);

				string conditionColumnName = Helper.GetResourceString(Constants.QUERY_GRID_CONDITION);
				DataGridViewComboBoxCell conditionColumn = (DataGridViewComboBoxCell)Rows[selectedrowindex].Cells[conditionColumnName];
				conditionColumn.OwningColumn.MinimumWidth = 40;
				conditionColumn.MaxDropDownItems = conditionList.Length;
				conditionColumn.Items.Clear();
				conditionColumn.Items.AddRange(conditionList);
				Rows[selectedrowindex].Cells[conditionColumnName].Value = conditionList[0];
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		public void FillOperatorComboBox()
		{
			try
			{
				string[] opratorList = QueryHelper.GetOperators();

				string operatorColumnName = Helper.GetResourceString(Constants.QUERY_GRID_OPERATOR);
				DataGridViewComboBoxColumn operatorColumn = (DataGridViewComboBoxColumn)Columns[operatorColumnName];
				operatorColumn.MaxDropDownItems = opratorList.Length;
				operatorColumn.Items.Clear();
				operatorColumn.Items.AddRange(opratorList);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}

		}



		#endregion

		#endregion

		internal bool AddToQueryBuilder(TreeNode tempTreeNode, QueryBuilder queryBuilder)
		{
			string className = string.Empty;
			try
			{
				ProxyType itemType = AssemblyInspectorObject.DataType.ResolveType(tempTreeNode.Tag.ToString());
					
				if (!(itemType.IsEditable &&  (itemType.IsPrimitive || itemType.IsNullable) ) )
					return false;
				if (tempTreeNode.Parent != null)
				{
					className = FullyQualifiedClassNameFor(tempTreeNode.Parent);
				}

				//If field is not selected and Query Group has no clauses then reset the base class. 
				if (queryBuilder.QueryGroupCount == 0 && Rows.Count == 0 && queryBuilder.AttributeCount == 0)
				{
					Helper.HashTableBaseClass.Clear();
				}

				//Get full path of selected node
				string fullpath = Helper.GetFullPath(tempTreeNode);

				//Chech whether dragged item is from same class or not,
				//if not dont allow to drop that item in query builder
				if (Helper.HashTableBaseClass.Count > 0 && !Helper.HashTableBaseClass.Contains(Helper.BaseClass))
						return false;

				AddRowsToGrid(itemType, className, fullpath);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
				return false;
			}

			return Rows.Count > 0;
		}

        private static string FullyQualifiedClassNameFor(TreeNode node)
        {
			ProxyType  type = AssemblyInspectorObject.DataType.ResolveType(node.Tag.ToString()); 
            return type == null ? (node.Parent != null && !node.Parent.Tag.ToString().Equals("Fav Folder")  ? node.Parent.Name : node.Text) : type.FullName;
        }

	    internal bool AddAllItemsOfClassToQueryBuilder(TreeNode tempTreeNode, QueryBuilder queryBuilder)
		{
			try
			{
				//Get the type of sselected item from tree view

				Helper.BaseClass = Helper.FindRootNode(tempTreeNode);
				if (Helper.HashTableBaseClass.Count > 0)
					if (!Helper.HashTableBaseClass.Contains(Helper.BaseClass))
						return false;

				string className = FullyQualifiedClassNameFor(tempTreeNode);
				Hashtable storedfields = AssemblyInspectorObject.Connection.FetchStoredFields(className);
					
				if (storedfields != null)
				{
					IDictionaryEnumerator eNum = storedfields.GetEnumerator();
					while (eNum.MoveNext())
					{
						ProxyType  itemType = AssemblyInspectorObject.DataType.ResolveType(eNum.Value.ToString());
                        if (!(itemType.IsEditable && (itemType.IsPrimitive || itemType.IsNullable) ))
							continue;
							
						if (queryBuilder.QueryGroupCount == 0 && Rows.Count == 0 && queryBuilder.AttributeCount == 0)
						{
							Helper.HashTableBaseClass.Clear();
						}

						//Chech whether dragged item is from same class or not,
						//if not dont allow to drop that item in query builder
						string parentName = Helper.FormulateParentName(tempTreeNode, eNum);
                        AddRowsToGrid(itemType, className, parentName);
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
				return false;
			}

			return Rows.Count > 0;
		}

		private void AddRowsToGrid(ProxyType  type, string className, string parentName)
		{
			try
			{
				
				Rows.Add(1);
				int index = Rows.Count - 1;
				Rows[index].Cells[0].Value = parentName;
				Rows[index].Cells[Constants.QUERY_GRID_CALSSNAME_HIDDEN].Value = className;
				Rows[index].Cells[Constants.QUERY_GRID_FIELDTYPE_HIDDEN].Value = type.DisplayName  ;

			    Rows[index].Cells[Constants.QUERY_GRID_FIELDTYPE_DISPLAY_HIDDEN].Value = type;
			   // Rows[index].Tag = type;
			    string nullableStringType = string.Empty;
				if (type.IsNullable)
			   {
			       nullableStringType = CommonValues.GetSimpleNameForNullable(type.FullName );
			   }

                if (AssemblyInspectorObject.DataType.CheckTypeIsSame(type.DisplayName , typeof(Boolean)) || nullableStringType == "System.Boolean")
				{
					Rows[index].Cells[2].Value = "True";
				}
                if (AssemblyInspectorObject.DataType.CheckTypeIsSame(type.DisplayName , typeof(DateTime)))
                {
					Rows[index].Cells[2].Value =DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
				}

				ClearSelection();
				Rows[index].Cells[0].Selected = true;

				if (!Helper.HashTableBaseClass.Contains(Helper.BaseClass))
					Helper.HashTableBaseClass.Add(Helper.BaseClass, string.Empty);

				FillConditionsCombobox(type  , index);

				//Select the Default operator for Query
				Rows[index].Cells[3].Value = CommonValues.LogicalOperators.AND.ToString();
				
				if (index > 0)
				{
					Rows[index].Cells[3].Value = Rows[0].Cells[3].Value;
					Rows[index].Cells[3].ReadOnly = true;
				}
				else
				{
					//Select the Default operator for Query
					Rows[index].Cells[3].Value = CommonValues.LogicalOperators.AND.ToString();
				}
			}
			catch (Exception ex)
			{
				LoggingHelper.ShowMessage(ex);
			}
		}
		/// <summary>
		/// dbDataGridViewEventArgs : For handling all  custom events of AIDataGridView
		/// </summary>
		/// 

	}
	public class dbDataGridViewEventArgs : EventArgs
	{
		private readonly object m_data = string.Empty;

		public object Data
		{
			get { return m_data; }
		}

		public dbDataGridViewEventArgs(object data)
		{
			m_data = data;
		}

		public dbDataGridViewEventArgs()
		{
		}
	}
}

