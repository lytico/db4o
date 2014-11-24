using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace OMControlLibrary.Common
{

	/// <summary>
	/// This class is used as a editing control for cell dbDataGridViewDateTimePickerCell.
	/// </summary>
	internal class dbDataGridViewDateTimePickerEditingControl :
		DateTimePicker,
		IDataGridViewEditingControl
	{

		#region Member Variables

		private DataGridView m_DataGridView;
		private bool m_ValueChanged = false;
		private int m_RowIndex;
        private const string DATE_FORMAT = "MM/dd/yyyy hh:mm:ss tt";

		#endregion Member Variables

		#region constructor
		/// <summary>
		/// Constructor: Setting some default property for the control AIDateTimePicker.
		/// Setting format of date time is "MM dd, yyyy - hh:mm:ss".
		/// </summary>
		public dbDataGridViewDateTimePickerEditingControl()
		{
			this.CustomFormat = DATE_FORMAT;
			this.Format = DateTimePickerFormat.Custom;  //10/23/2007 10:50:43 AM
			this.ShowUpDown = false;
		}
		#endregion constructor

		#region Methods



		//// Implements the IDataGridViewEditingControl.EditingControlFormattedValue 
		//// property.
		public object EditingControlFormattedValue
		{
			get
			{
			    if (this.Checked)
			        return this.Value;
			    else
			    {
			        string cellValue = string.Empty;
			        if (m_DataGridView.CurrentCell.Value != null)
			        {
			            //string val = this.Value.ToString();
			            cellValue = m_DataGridView.CurrentCell.Value.ToString();
			        }

			        return cellValue;
			    }
			}
			set
			{
				//TODO: need to be change as per the control
				DateTimeConverter dtConverter = new DateTimeConverter();
				Value = (DateTime)dtConverter.ConvertFrom(value);
			}
		}

		//// Implements the 
		//// IDataGridViewEditingControl.GetEditingControlFormattedValue method.
		public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
		{
			this.m_ValueChanged = true;
			this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
			return EditingControlFormattedValue.ToString();
		}

		//// Implements the 
		//// IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
		public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
		{
			this.Font = dataGridViewCellStyle.Font;
			this.ForeColor = dataGridViewCellStyle.ForeColor;
			this.BackColor = dataGridViewCellStyle.BackColor;
		}

		//// Implements the IDataGridViewEditingControl.EditingControlRowIndex 
		//// property.
		public int EditingControlRowIndex
		{
			get
			{
				return m_RowIndex;
			}
			set
			{
				m_RowIndex = value;
			}
		}

		//// Implements the IDataGridViewEditingControl.EditingControlWantsInputKey 
		//// method.
		public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
		{
			return true;
		}

		//// Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit 
		//// method.
		public void PrepareEditingControlForEdit(bool selectAll)
		{
			// No preparation needs to be done.
		}

		//// Implements the IDataGridViewEditingControl.RepositionEditingControlOnValueChange property.
		public bool RepositionEditingControlOnValueChange
		{
			get
			{
				return false;
			}
		}

		//// Implements the IDataGridViewEditingControl
		//// .EditingControlDataGridView property.
		public DataGridView EditingControlDataGridView
		{
			get
			{
				return m_DataGridView;
			}
			set
			{
				m_DataGridView = value;
			}
		}

		//// Implements the IDataGridViewEditingControl
		//// .EditingControlValueChanged property.
		public bool EditingControlValueChanged
		{
			get
			{
				return m_ValueChanged;
			}
			set
			{
				m_ValueChanged = value;
			}
		}

		//// Implements the IDataGridViewEditingControl
		//// .EditingPanelCursor property.
		public Cursor EditingPanelCursor
		{
			get
			{
				return base.Cursor;
			}
		}

		#endregion Methods
	}
}
