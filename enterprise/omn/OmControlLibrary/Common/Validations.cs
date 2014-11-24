using System;
using System.Windows.Forms;
using OMAddinDataTransferLayer;
using OMAddinDataTransferLayer.TypeMauplation;
using OME.Logging.Common;

namespace OMControlLibrary.Common
{
	class Validations
	{
		
		public static bool ValidateDataType(string classname, string  fieldname, object data)
		{
			if (null == data && "null"==data.ToString() ) 
				return false  ;
			try
			{
				return AssemblyInspectorObject.DataType.ValidateDataType(classname, fieldname , data);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}

			return false;
		}
        public static bool ValidateDataType(string classname, ProxyType fieldname, object data)
        {
            if (null == data && "null" == data.ToString())
                return false;

            try
            {
                return AssemblyInspectorObject.DataType.ValidateDataType(classname, fieldname.DisplayName, data);
            }
            catch (Exception oEx)
            {
                LoggingHelper.ShowMessage(oEx);
            }

            return false;
        }
        public static bool ValidateDataType(string className, object data)
        {
            if (null == data && "null" == data.ToString())
                return false;

            try
            {
                return AssemblyInspectorObject.DataType.CheckIfObjectCanBeCasted(className, data);
                
            }
            catch (Exception oEx)
            {
                LoggingHelper.ShowMessage(oEx);
            }

            return false;
        }
		public static bool ValidateRemoteLoginParams(ref ToolTipComboBox comboBoxFilePath, ref TextBox textBoxHost, ref TextBox textBoxPort, ref TextBox textBoxUserName, ref TextBox textBoxPassword)
		{
			try
			{
				if (comboBoxFilePath.Text.Trim().Equals(Helper.GetResourceString(Constants.COMBOBOX_DEFAULT_TEXT))
							&& textBoxHost.Text.Trim().Equals(string.Empty)
							&& textBoxPort.Text.Trim().Equals(string.Empty)
							&& textBoxUserName.Text.Trim().Equals(string.Empty)
							&& textBoxPassword.Text.Trim().Equals(string.Empty))
				{
					MessageBox.Show(Helper.GetResourceString(Constants.VALIDATION_MSG_SELECT_REMOTE_CONNECTION),
						Helper.GetResourceString(Constants.PRODUCT_CAPTION),
						MessageBoxButtons.OK, MessageBoxIcon.Information);
					comboBoxFilePath.Focus();
					return false;
				}
				if (textBoxHost.Text.Trim().Equals(string.Empty))
				{
					MessageBox.Show(Helper.GetResourceString(Constants.VALIDATION_MSG_ENTER_HOST),
						Helper.GetResourceString(Constants.PRODUCT_CAPTION),
						MessageBoxButtons.OK, MessageBoxIcon.Information);
					textBoxHost.Focus();
					return false;
				}
				if (textBoxPort.Text.Trim().Equals(string.Empty))
				{
					MessageBox.Show(Helper.GetResourceString(Constants.VALIDATION_MSG_ENTER_PORT),
						Helper.GetResourceString(Constants.PRODUCT_CAPTION),
						MessageBoxButtons.OK, MessageBoxIcon.Information);
					textBoxPort.Focus();
					return false;
				}
				if (textBoxUserName.Text.Trim().Equals(string.Empty))
				{
					MessageBox.Show(Helper.GetResourceString(Constants.VALIDATION_MSG_ENTER_USERNAME),
						Helper.GetResourceString(Constants.PRODUCT_CAPTION),
						MessageBoxButtons.OK, MessageBoxIcon.Information);
					textBoxUserName.Focus();
					return false;
				}
				if (textBoxPassword.Text.Trim().Equals(string.Empty))
				{
					MessageBox.Show(Helper.GetResourceString(Constants.VALIDATION_MSG_ENTER_PASSWORD),
						Helper.GetResourceString(Constants.PRODUCT_CAPTION),
						MessageBoxButtons.OK, MessageBoxIcon.Information);
					textBoxPassword.Focus();
					return false;
				}

				if (!(Convert.ToInt32(textBoxPort.Text.Trim()) >= 1 && Convert.ToInt32(textBoxPort.Text.Trim()) <= 65535))
				{
					MessageBox.Show(Helper.GetResourceString(Constants.VALIDATION_MSG_PORT_RANG),
						Helper.GetResourceString(Constants.PRODUCT_CAPTION),
						MessageBoxButtons.OK, MessageBoxIcon.Information);
					textBoxPort.Focus();
					return false;
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
				return false;
			}
			return true;
		}

		public static bool ValidateLocalLoginParams(ref ToolTipComboBox comboBoxFilePath, ref TextBox textBoxConnection)
		{
			try
			{
				if ((comboBoxFilePath.Text.Trim().Equals(Helper.GetResourceString(Constants.COMBOBOX_DEFAULT_TEXT)) && textBoxConnection.Text.Trim().Equals(string.Empty)) || ((comboBoxFilePath.Text.Trim().Equals(string.Empty) && textBoxConnection.Text.Trim().Equals(string.Empty))))
				{
					MessageBox.Show(Helper.GetResourceString(Constants.VALIDATION_MSG_SELECT_DATABASE), Helper.GetResourceString(Constants.PRODUCT_CAPTION), MessageBoxButtons.OK, MessageBoxIcon.Information);
					comboBoxFilePath.Focus();
					return false;
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
				return false;
			}
			return true;

		}


	}
}
