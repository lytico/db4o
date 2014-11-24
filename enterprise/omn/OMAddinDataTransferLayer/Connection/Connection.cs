using System;
using System.Collections;
using OManager.BusinessLayer.Login;
using OManager.BusinessLayer.UIHelper;
using OME.Logging.ExceptionLogging;
using OME.Logging.Tracing;
using OManager.DataLayer.Connection;
using OManager.DataLayer.DBInfo;

namespace OMAddinDataTransferLayer.Connection
{

    public class Connection : MarshalByRefObject, IConnection
    {
        public string ConnectToDatabase(ConnectionDetails currConnectionDetails, bool checkCustomConfig)
        {
            string exceptionString = dbInteraction.ConnectoToDB(currConnectionDetails, checkCustomConfig);

            try
            {
                OMETrace.Initialize();
            }
            catch (Exception ex)
            {
                ex.ToString(); //ignore
            }
            try
            {
                ExceptionHandler.Initialize();
            }
            catch (Exception ex)
            {
                ex.ToString(); //ignore
            }
            return exceptionString;
        }

        public int GetFieldCount(string classname)
        {
            return dbInteraction.GetFieldCount(classname);
        }

        public Hashtable FetchAllStoredClasses()
        {
            return DbInformation.StoredClasses();
        }

        public Hashtable FetchAllStoredClassesForAssembly()
        {
            return DbInformation.StoredClassesByAssembly();
        }

        public Hashtable FetchStoredFields(string nodeName)
        {
            return dbInteraction.FetchStoredFields(nodeName);
        }

        public bool DbConnectionStatus()
        {
            return dbInteraction.CheckIfDbConnected();
        }

        public bool CheckForClientServer()
        {
            return dbInteraction.CheckIfClientServer();
        }

        public int NoOfClassesInDb()
        {
            return DbInformation.GetNumberOfClassesinDB();
        }

        public long GetFreeSizeOfDb()
        {
            return DbInformation.GetFreeSizeofDatabase();
        }

        public long GetTotalDbSize()
        {
            return DbInformation.getTotalDatabaseSize();
        }

        public long DbCreationTime()
        {
            return DbInformation.getDatabaseCreationTime();
        }

        public void Closedb()
        {
            Db4oClient.CloseConnection();
        }

        public void SaveIndex(ArrayList fieldnames, string classname, ArrayList indexed, string path,bool customConfig)
        {
            dbInteraction.SetIndexedConfiguration(fieldnames, classname, indexed, path, customConfig);
        }
        public bool  CheckForCustomConfig()
        {
            return Db4oClient.CustomConfig;
        }
        public override object InitializeLifetimeService()
        {

            return null;
        }
    }
}