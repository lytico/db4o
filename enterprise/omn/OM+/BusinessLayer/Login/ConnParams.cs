using System;
using System.Runtime.Serialization;

namespace OManager.BusinessLayer.Login
{
	[Serializable ]
	public class ConnParams 
	{
        private readonly string m_connection;
        private readonly string m_host;
        private readonly int m_port;
        private readonly string m_userName;
        private readonly string m_passWord;
		private bool m_readonly;

	
		

		public ConnParams(string connection,bool readOnly) : this(connection, null, null, null, 0)
		{
			m_readonly = readOnly;
		}

        public ConnParams(string connection, string host, string username, string password, int port)
        {
            m_connection = connection;
            m_host = host;
            m_userName = username;
            m_passWord = password;
            m_port = port; 

        }

        public ConnParams(string connection)
        {

            m_connection = connection;
        }
		public bool ConnectionReadOnly
		{
			get { return m_readonly; }
			set { m_readonly = value; }
		}
        public string Connection
        {
            get { return m_connection; }
        }

        public string Host
        {
            get { return m_host; }           

        }
        public int Port
        {
            get { return m_port; }           
        }

        public string UserName
        {
            get { return m_userName; }            
        }
        public string PassWord
        {

            get { return m_passWord; }
        }
	}
}
