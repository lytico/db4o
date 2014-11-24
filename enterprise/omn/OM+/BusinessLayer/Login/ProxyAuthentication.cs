using System;
using System.Collections.Generic;
using System.Text;

namespace OManager.BusinessLayer.Login
{
    public class ProxyAuthentication
    {
        string m_UserName;
        string m_ProxyAddress;
        string m_Port;
        byte[] m_pass;

        public byte[] PassWord
        {
            get { return m_pass; }
            set { m_pass = value; }
        }
       

        public string UserName
        {
            get { return m_UserName; }
            set { m_UserName = value; }
        }

        public string Port
        {
            get { return m_Port; }
            set { m_Port = value; }
        }

        public string ProxyAddress
        {
            get { return m_ProxyAddress; }
            set { m_ProxyAddress = value; }
        }

        public ProxyAuthentication(string username, string proxyAddr, string port, byte[] password)
        {
            this.UserName = username;
            this.ProxyAddress = proxyAddr;
            this.Port = port;
            this.PassWord = password;
        }
        public ProxyAuthentication()
        {
            this.UserName = string.Empty;
            this.ProxyAddress = string.Empty;
            this.Port = string.Empty;
           
        }
    }
}
