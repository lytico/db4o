using System;
using OManager.BusinessLayer.Login;
using Db4objects.Db4o;
using OME.Logging.Common;


namespace OManager.DataLayer.Connection
{
    class DBConnect
    {
       
        public string dbConnection(ConnParams login, bool customConfig)
        {
            try
            {
                Db4oClient.CurrentConnParams  = login; 
                Db4oClient.CustomConfig = customConfig;
                IObjectContainer objectContainer = Db4oClient.Client;
                return Db4oClient.ExceptionConnection;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return "";
            }
        }
    }
}
