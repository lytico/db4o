using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OManager.BusinessLayer.Login;

namespace OMAddinDataTransferLayer.Connection
{
	[Serializable ]
	public class ProxyConnParam
	{
		public  string DbConnectionStr { get; set; }
		public string Host { get; set; }
		public int Port { get; set; }
		public string UserName { get; set; }
		public string PassWord { get; set; }
		public bool ReadOnly { get; set; }
		public ProxyConnParam CovertoProxy(ConnParams connParams)
		{
			DbConnectionStr = connParams.Connection;
			Host = connParams.Host;
			Port = connParams.Port;
			UserName = connParams.UserName;
			PassWord = connParams.PassWord;
			ReadOnly = connParams.ConnectionReadOnly;  
			return this;
		}
	}
}
