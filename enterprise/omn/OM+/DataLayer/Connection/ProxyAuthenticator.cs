using System;
using OManager.BusinessLayer.Config;
using OManager.BusinessLayer.Login;
using Db4objects.Db4o;
using OME.Logging.Common;

namespace  OManager.DataLayer.Connection
{
    public class ProxyAuthenticator
    {
       
        ProxyAuthentication _proxyAuthObj;
        public ProxyAuthentication ProxyAuthObj
        {
            get { return _proxyAuthObj; }
            set { _proxyAuthObj = value; }
        }

        public void AddProxyInfoToDb(ProxyAuthentication proxyAuthObj)
        {
            ProxyAuthenticator proxyobj = ReturnProxyAuthenticationInfo();
            if (proxyobj == null)
            {
                Db4oClient.OMNConnection.Store(this);
            }
            else
            {
                proxyobj._proxyAuthObj = proxyAuthObj;
                Db4oClient.OMNConnection.Store(proxyobj);
            }
            Db4oClient.OMNConnection.Commit();
            Db4oClient.OMNConnection.Ext().Refresh(proxyobj, 1);
            Db4oClient.CloseRecentConnectionFile();
        }

        public ProxyAuthenticator ReturnProxyAuthenticationInfo()
        {
            try
            {
               IObjectSet ObjSet = Db4oClient.OMNConnection.QueryByExample(typeof(ProxyAuthenticator));
				return ObjSet.Count > 0 ? (ProxyAuthenticator) ObjSet.Next() : null;
            }
            catch (Exception e)
            {
                LoggingHelper.HandleException(e);
                return null;
            }
            finally
            {
                Db4oClient.CloseRecentConnectionFile();
            }
        }
    }
}
