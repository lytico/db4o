using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using OMAddinDataTransferLayer;
using OMAddinDataTransferLayer.TypeMauplation;
using OManager.BusinessLayer.Common;

namespace OMControlLibrary.Common
{
	/// <summary>
	/// This class is associated with dbDataGridViewDateTimePickerColumn.
	/// It is simple DataGridView TextBox Cell but in Edit mode it 
	/// show AIDataGridViewDateTimePickerEditingControl.
	/// </summary>
	public class dbDataGridViewDateTimePickerCell : DataGridViewTextBoxCell
	{
		#region Constructor

		public dbDataGridViewDateTimePickerCell()
			: base()
		{

		}
		#endregion Constructor

		#region Override methods

		/// <summary>
		/// Initialize Editing Control, control type is AIDataGridViewDateTimePickerEditingControl.
		/// </summary>
		/// <param name="rowIndex"></param>
		/// <param name="initialFormattedValue"></param>
		/// <param name="dataGridViewCellStyle"></param>
		public override void InitializeEditingControl(int rowIndex, object
				initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			// Set the value of the editing control to the current cell value.
			try
			{
				base.InitializeEditingControl(rowIndex,
					initialFormattedValue,
					dataGridViewCellStyle);
				string typeOfValue = string.Empty;
                ProxyType type = this.Tag as ProxyType;
                if (type == null || type.IsNullable)
                 type =DataGridView.Rows[rowIndex].Cells[Constants.QUERY_GRID_FIELDTYPE_DISPLAY_HIDDEN].Value as ProxyType ;
                    if(type.IsNullable )
                    {
                        typeOfValue = CommonValues.GetSimpleNameForNullable(type.FullName);
                    }
                    else
                    {
                        typeOfValue = type.DisplayName;
                    }
               // }

                    if (typeOfValue == typeof(DateTime).ToString() || AssemblyInspectorObject.DataType.CheckTypeIsSame(type.DisplayName, typeof(DateTime)))
				{
					dbDataGridViewDateTimePickerEditingControl ctl =
						DataGridView.EditingControl as dbDataGridViewDateTimePickerEditingControl;

					if (this.Value != null && this.Value != this.OwningColumn.DefaultCellStyle)
					{
						try
						{
							
                            DateTimeFormatInfo dateTimeFormatterProvider = DateTimeFormatInfo.CurrentInfo.Clone() as DateTimeFormatInfo;
                            dateTimeFormatterProvider.ShortDatePattern = "MM/dd/yyyy hh:mm:ss tt";
                            DateTime dateTime = DateTime.Parse(Value.ToString(), dateTimeFormatterProvider);
                            ctl.Value = dateTime;
						}
						catch (Exception ex)
						{
							ex.ToString();
							ctl.Value = System.DateTime.Now;
						}
					}
				}
                    else if ((typeOfValue == typeof(Boolean).ToString()) || AssemblyInspectorObject.DataType.CheckTypeIsSame(type.DisplayName, typeof(Boolean)))
				{
					//intializing editing control (DataGridViewComboBoxEditingControl)
					DataGridViewComboBoxEditingControl ctl =
						this.DataGridView.EditingControl as DataGridViewComboBoxEditingControl;

					//setting combox style
					ctl.DropDownStyle = ComboBoxStyle.DropDownList;
					ctl.FlatStyle = FlatStyle.Popup;
					FillBoolColumnValue(ctl);

					if (this.Value != null && this.Value != this.OwningColumn.DefaultCellStyle)
					{
						try
						{
							ctl.EditingControlFormattedValue = this.Value.ToString();
						}
						catch (Exception ex)
						{
							ex.ToString();
							ctl.SelectedItem = ctl.Items[0].ToString();
						}
					}
					ctl.Width = this.OwningColumn.Width;
				}
			}
			catch (Exception oEx)
			{
				string str = oEx.Message;

			}
		}

		public override Type EditType
		{
			get
			{
				// Return the type of the editing contol that AIDataGridViewDateTimePickerCell uses.

				Type controlType = typeof(DataGridViewTextBoxEditingControl);

				try
				{
					string typeOfValue = string.Empty;

					if (this.Tag == null)
					{
						try
						{
							typeOfValue = this.DataGridView.Rows[this.RowIndex].Cells[Constants.QUERY_GRID_FIELDTYPE_HIDDEN].Value.ToString();
						}
						catch
						{
							typeOfValue = string.Empty;
						}
					}
					if (typeOfValue == typeof(System.DateTime).ToString() ||
						(this.Tag != null && this.Tag.ToString() == typeof(System.DateTime).ToString()))
					{
						controlType = typeof(dbDataGridViewDateTimePickerEditingControl);
					}
					else if (typeOfValue == typeof(Boolean).ToString() ||
						(Tag != null && Tag.ToString() == typeof(Boolean).ToString()))
					{
						controlType = typeof(DataGridViewComboBoxEditingControl);
					}
				}
				catch (Exception)
				{
				}
				return controlType;
			}
		}

		public override Type ValueType
		{
			get
			{
				// Return the type of the value that AIDataGridViewDateTimePickerCell contains.
				return typeof(String);
			}
		}

		public void FillBoolColumnValue(DataGridViewComboBoxEditingControl ctrl)
		{
			
				ctrl.Items.Clear();
				ctrl.Items.AddRange(new object[] { BusinessConstants.CONDITION_TRUE,BusinessConstants.CONDITION_FALSE});
				ctrl.SelectedIndex = 0;
			
		}

		#endregion Override methods
	}
}
