using System;
using System.Windows.Forms;
using OMControlLibrary.Common;
using System.Reflection;
using EnvDTE80;

using OME.Logging.Common;

namespace OMControlLibrary.LoginToSalesForce
{
	public partial class WinAppCache : Form
	{
		readonly CustomCookies customCookies;
		public static bool isPasswordEmpty;
		public static bool isUserNameEmpty;

		public WinAppCache(DTE2 AppObj)
		{
			customCookies = new CustomCookies();

			InitializeComponent();
		}

		public WinAppCache()
		{
			customCookies = new CustomCookies();
			SetStyle(
				ControlStyles.CacheText |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.UserPaint |
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.Opaque,
				true);

			InitializeComponent();
		}
		private void buttonLogin_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(textBoxUserID.Text.Trim())
						   || string.IsNullOrEmpty(textBoxPassword.Text.Trim()))
				{
					MessageBox.Show(
						Helper.GetResourceString(Constants.VALIDATION_MSG_MANDATORY_FIELDS),
						Helper.GetResourceString(Constants.PRODUCT_CAPTION),
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning);

					if (string.IsNullOrEmpty(textBoxUserID.Text.Trim()))
					{
						textBoxUserID.Focus();
					}
					else if (string.IsNullOrEmpty(textBoxPassword.Text.Trim()))
					{
						textBoxPassword.Focus();
					}

					DialogResult = DialogResult.None;

					return;
				}

				if (checkBoxRememberMe.Checked)
				{
					string logininfo = textBoxUserID.Text + "~" + textBoxPassword.Text;
					customCookies.SetCookies(logininfo);
				}
				else
				{
					customCookies.SetCookies(string.Empty);
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{

		}

		private void linkLabelForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://www.db4o.com/users/retrievePassword.aspx");
		}

		private void linkLabelPurchase_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string filepath = Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8);

			filepath = filepath.Remove(filepath.Length - 21, 21);
			filepath = filepath + @"/ContactSales/ContactSales.htm";
			System.Diagnostics.Process.Start(filepath);

		}

		private void textBoxUserID_TextChanged(object sender, EventArgs e)
		{
			try
			{
				lblUserName.Visible = string.IsNullOrEmpty(textBoxUserID.Text.Trim());
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void textBoxPassword_TextChanged(object sender, EventArgs e)
		{
			try
			{
				lblPassword.Visible = string.IsNullOrEmpty(textBoxPassword.Text.Trim());
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
	}
}