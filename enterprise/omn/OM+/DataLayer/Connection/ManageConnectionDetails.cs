using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.Query;
using OME.Logging.Common;
using OManager.BusinessLayer.Login;
using OManager.BusinessLayer.QueryManager;

namespace OManager.DataLayer.Connection
{
    class ManageConnectionDetails
    {
        private ConnParams currConnParams;
        public ManageConnectionDetails(ConnParams currConnParams)
        {
            this.currConnParams = currConnParams;
        }

        internal void RemoveCustomConfigPath(string path)
        {
            try
            {
                IObjectContainer container = Db4oClient.OMNConnection;
                IQuery query = container.Query();
                query.Constrain(typeof(ConnectionDetails));
                query.Descend("m_connParam").Descend("m_connection").Constrain(currConnParams.Connection);
                query.Descend("m_customConfigAssemblyPath").Constrain(path);
                IObjectSet objSet = query.Execute();
                if (objSet.Count > 0)
                {
                    ConnectionDetails q = objSet[0] as ConnectionDetails;
                    q.CustomConfigAssemblyPath = string.Empty;
                    container.Ext().Store(q, 2);
                    container.Commit();
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);}
            finally
            {
                Db4oClient.CloseRecentConnectionFile();
            }
        }

        internal void AddQueryToList(OMQuery query)
        {
            try
            {
                long id = ChkIfRecentConnIsInDb();
                if (id > 0)
                {
                    ConnectionDetails temprc = Db4oClient.OMNConnection.Ext().GetByID(id) as ConnectionDetails;
                    Db4oClient.OMNConnection.Ext().Activate(temprc, 5);
                    MaintainCountofTwentyforQueries(temprc);
                    MaintainCountofFiveForQueries(query, temprc);
                    if (temprc != null)
                    {
                        temprc.Timestamp = DateTime.Now;
                        temprc.QueryList.Add(query);
                    }
                    Db4oClient.OMNConnection.Ext().Store(temprc, 5);
                    Db4oClient.OMNConnection.Commit();
                }

            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
            finally
            {
                Db4oClient.CloseRecentConnectionFile();
            }
        }

        private void MaintainCountofFiveForQueries(OMQuery query, ConnectionDetails connection)
        {
            List<OMQuery> qListForClass = FetchQueriesForAClass(query.BaseClass);
            if (qListForClass != null)
            {
                bool check = false;
                if (qListForClass.Any(qry => qry.QueryString == query.QueryString))
                {
                    check = true;
                    OMQuery result = connection.QueryList.Find(bk => bk.QueryString == query.QueryString);
                    connection.QueryList.Remove(result);
                    DeleteQuery(result);
                }
                if (qListForClass.Count >= 5)

                { 
                    if (!check)
                    {
                        OMQuery q = qListForClass[qListForClass.Count - 1];
                        OMQuery result = connection.QueryList.Find(bk => bk.QueryString == q.QueryString);
                        connection.QueryList.Remove(result);
                        DeleteQuery(result);
                    }
                }

            }
        }

        private void MaintainCountofTwentyforQueries(ConnectionDetails connection)
        {
            List<OMQuery> qList = FetchAllQueries();
            if (qList.Count < 20) return;
            OMQuery q = qList[qList.Count - 1];
            qList.RemoveAt(qList.Count - 1);
            foreach (OMQuery qry1 in connection.QueryList.Where(qry1 => q.QueryString.Equals(qry1.QueryString)))
            {
                connection.QueryList.Remove(qry1);
                DeleteQuery(qry1);
                break;
            }
        }

        private static void DeleteQuery(OMQuery query)
        {
            List<OMQuery> queries = (from OMQuery q in Db4oClient.OMNConnection where q.QueryString == query.QueryString select q).ToList();
            if (queries.Count <= 0) return;
            Db4oClient.OMNConnection.Delete(queries[0]);
            Db4oClient.OMNConnection.Commit();
        }

        private List<OMQuery> FetchAllQueries()
        {
            try
            {
                IObjectContainer container = Db4oClient.OMNConnection;
                IQuery query = container.Query();
                query.Constrain(typeof(ConnectionDetails));
                IObjectSet os = query.Execute();

                List<ConnectionDetails> recConnections = new List<ConnectionDetails>();
                while (os.HasNext())
                {
                    recConnections.Add((ConnectionDetails)os.Next());
                }
                List<OMQuery> QryList = (from recCon in recConnections from q in recCon.QueryList where q != null select q).ToList();

                CompareQueryTimestamps comp = new CompareQueryTimestamps();
                QryList.Sort(comp);
                return QryList;

            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);

                return null;
            }
        }

        private List<OMQuery> FetchQueriesForAClass(string className)
        {
           
            List<OMQuery> qList = new List<OMQuery>();           
            IObjectSet objSet;
            try
            {
                IObjectContainer container = Db4oClient.OMNConnection;
                IQuery query = container.Query();
                query.Constrain(typeof(ConnectionDetails));
                query.Descend("m_connParam").Descend("m_connection").Constrain(currConnParams.Connection);    
                objSet = query.Execute();

				if (objSet != null && objSet.Count>0 )
                {
                    ConnectionDetails connectionDetails = (ConnectionDetails)objSet.Next();
                    qList.AddRange(connectionDetails.QueryList.Where(q => q != null && q.BaseClass.Equals(className)));

                    CompareQueryTimestamps comp = new CompareQueryTimestamps();
                    qList.Sort(comp);
                }
                else
                    return null; 
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
           
            return qList;
        }

        internal  long ReturnTimeWhenRecentQueriesCreated()
        {
            try
            {
                long id = ChkIfRecentConnIsInDb();
                if (id > 0)
                {
                    ConnectionDetails connectionDetails = Db4oClient.OMNConnection.Ext().GetByID(id) as ConnectionDetails;
                     
                    if (connectionDetails != null)
                    {
                        Db4oClient.OMNConnection.Ext().Activate(connectionDetails, 3);
                        return connectionDetails.TimeOfCreation > 0 ? connectionDetails.TimeOfCreation : 0;
                    }
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
            finally
            {
                Db4oClient.CloseRecentConnectionFile();
            }
            return 0;

        }

        internal  long ChkIfRecentConnIsInDb()
        {
            long id = 0;
            try
            {
               IObjectContainer container = Db4oClient.OMNConnection;
                IQuery qry = container.Query();
                qry.Constrain(typeof (ConnectionDetails));
                IObjectSet objSet;
                if (currConnParams.Host == null)
                {
                    qry.Descend("m_connParam").Descend("m_connection").Constrain(currConnParams.Connection);
                    objSet = qry.Execute();
                }
                else
                {
                    qry.Descend("m_connParam").Descend("m_host").Constrain(currConnParams.Host);
                    qry.Descend("m_connParam").Descend("m_port").Constrain(currConnParams.Port);
                    qry.Descend("m_connParam").Descend("m_userName").Constrain(currConnParams.UserName);
                    qry.Descend("m_connParam").Descend("m_passWord").Constrain(currConnParams.PassWord);
                    objSet = qry.Execute();
                }
                if (objSet.Count > 0)
                {
                    ConnectionDetails connectionDetails = (ConnectionDetails)objSet.Next();
                    id = Db4oClient.OMNConnection.Ext().GetID(connectionDetails);
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
            finally
            {
                Db4oClient.CloseRecentConnectionFile();
            }
            return id;

        }

        internal  ConnectionDetails GetConnectionDetails()
        {
            try
            {
                IObjectContainer container = Db4oClient.OMNConnection;
                IQuery qry = container.Query();
                qry.Constrain(typeof (ConnectionDetails));
                IObjectSet objSet;
                if (currConnParams.Host == null)
                {
                    qry.Descend("m_connParam").Descend("m_connection").Constrain(currConnParams.Connection);
                    objSet = qry.Execute();
                }
                else
                {
                    qry.Descend("m_connParam").Descend("m_host").Constrain(currConnParams.Host);
                    qry.Descend("m_connParam").Descend("m_port").Constrain(currConnParams.Port);
                    qry.Descend("m_connParam").Descend("m_userName").Constrain(currConnParams.UserName);
                    qry.Descend("m_connParam").Descend("m_passWord").Constrain(currConnParams.PassWord);
                    objSet = qry.Execute();
                }
                if (objSet.Count > 0)
                {
                   return (ConnectionDetails) objSet.Next();
                    
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
            finally
            {
                Db4oClient.CloseRecentConnectionFile();
            }
            return null ;

        }
        internal  void DeleteRecentQueriesForAConnection()
        {
            try
            {
                IQuery query = Db4oClient.OMNConnection.Query();
                query.Constrain(typeof(ConnectionDetails));
                query.Descend("m_connParam").Descend("m_connection").Constrain(currConnParams.Connection);
                IObjectSet objSet = query.Execute();
                if (objSet != null)
                {
                    ConnectionDetails connectionDetails = (ConnectionDetails)objSet.Next();
                    foreach (OMQuery q in connectionDetails.QueryList.Where(q => q != null))
                    {
                        Db4oClient.OMNConnection.Delete(q);
                    }
                    connectionDetails.QueryList.Clear();
                    connectionDetails.TimeOfCreation = Sharpen.Runtime.CurrentTimeMillis();
                    Db4oClient.OMNConnection.Store(connectionDetails);
                    Db4oClient.OMNConnection.Commit();
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
            finally
            {
                Db4oClient.CloseRecentConnectionFile();
            }
        }
        public bool SaveConnectionDetails(ConnectionDetails connectionDetails)
        {
            try
            {
                ManageConnectionDetails manageConnectionDetails = new ManageConnectionDetails(connectionDetails.ConnParam);
                long id = manageConnectionDetails.ChkIfRecentConnIsInDb();
                IObjectContainer dbrecentConn = Db4oClient.OMNConnection;
                ConnectionDetails temprc = null;
                if (id > 0)
                {
                    temprc = dbrecentConn.Ext().GetByID(id) as ConnectionDetails;
                    dbrecentConn.Ext().Activate(temprc, 3);
                    if (temprc != null)
                    {
                        temprc.Timestamp = DateTime.Now;
                        temprc.ConnParam.ConnectionReadOnly = connectionDetails.ConnParam.ConnectionReadOnly;
                    }
                }
                else
                {
                    temprc = connectionDetails;
                    temprc.Timestamp = DateTime.Now;  
                    temprc.TimeOfCreation = Sharpen.Runtime.CurrentTimeMillis();
                }
                dbrecentConn.Store(temprc);
                dbrecentConn.Commit();

            }

            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return false;
            }
            finally
            {
                Db4oClient.CloseRecentConnectionFile();
            }
            return true;
        }


        public List<ConnectionDetails> GetAllConnections(bool remote)
        {
            List<ConnectionDetails> recConnections = null;
            try
            {
#if DEBUG
                CreateOMNDirectory();
#endif
                IObjectContainer dbrecentConn = Db4oClient.OMNConnection;
                IQuery query = dbrecentConn.Query();
                query.Constrain(typeof(ConnectionDetails));
                if (remote)
                {
                    query.Descend("m_connParam").Descend("m_host").Constrain(null).Not();
                    query.Descend("m_connParam").Descend("m_port").Constrain(0).Not();
                }
                else
                {
                    query.Descend("m_connParam").Descend("m_host").Constrain(null);
                    query.Descend("m_connParam").Descend("m_port").Constrain(0);
                }
                IObjectSet os = query.Execute();

                if (os.Count > 0)
                {
                    recConnections = new List<ConnectionDetails>();
                    while (os.HasNext())
                    {
                        recConnections.Add((ConnectionDetails)os.Next());
                    }
                    CompareTimestamps comparator = new CompareTimestamps();
                    recConnections.Sort(comparator);
                }

            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);

                return null;
            }
            finally
            {
                Db4oClient.CloseRecentConnectionFile();
            }
            return recConnections;

        }
        private void CreateOMNDirectory()
        {
            string omnConfigFolderPath = Path.GetDirectoryName(Db4oClient.GetOMNConfigdbPath());
            if (!Directory.Exists(omnConfigFolderPath))
            {
                Directory.CreateDirectory(omnConfigFolderPath);
            }
        }
    }

    public class CompareTimestamps : IComparer<ConnectionDetails>
    {
        public int Compare(ConnectionDetails con1, ConnectionDetails con2)
        {
            
                return con2.Timestamp.CompareTo(con1.Timestamp);
        }
    }
    
}
