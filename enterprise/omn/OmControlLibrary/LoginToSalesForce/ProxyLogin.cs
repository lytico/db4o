using System;
using System.Windows.Forms;
using OManager.BusinessLayer.Login;
using System.Net;
using OManager.BusinessLayer.UIHelper;
using OMControlLibrary.Common;
using OME.Logging.Common;

namespace OMControlLibrary.LoginToSalesForce
{
	public partial class ProxyLogin : Form
	{
		private const string CONST_BACKSLASH = "\\";
		public ProxyLogin()
		{
			InitializeComponent();
			try
			{
				ProxyAuthentication proxy = OMEInteraction.RetrieveProxyInfo();
				if (proxy != null)
				{
					textBoxUserID.Text = proxy.UserName;
					textBoxPassword.Focus();
					textBoxPort.Text = proxy.Port;
					textBoxProxy.Text = proxy.ProxyAddress;
					textBoxPassword.Text = Helper.DecryptPass(proxy.PassWord);
				}
				else
				{
					string domain = Environment.UserDomainName;
					string username = Environment.UserName;
					textBoxUserID.Text = domain + CONST_BACKSLASH + username;
					if (((WebProxy)GlobalProxySelection.Select).Address != null)
					{
						int colonIndex = ((WebProxy)GlobalProxySelection.Select).Address.ToString().LastIndexOf(':');
						string proxystr = ((WebProxy)GlobalProxySelection.Select).Address.ToString().Substring(0, colonIndex);
						string port = ((WebProxy)GlobalProxySelection.Select).Address.ToString().Substring(colonIndex + 1, ((WebProxy)GlobalProxySelection.Select).Address.ToString().Length - colonIndex - 1);
						port.TrimEnd('/');
						textBoxPassword.Text = string.Empty;
						textBoxProxy.Text = proxystr;
						textBoxPort.Text = port.Substring(0, 4);
					}
				}
			}
			catch (Exception e)
			{
				LoggingHelper.ShowMessage(e);
			}
		}




	}
}