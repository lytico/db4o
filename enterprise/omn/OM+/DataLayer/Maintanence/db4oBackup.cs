using System;
using System.Collections.Generic;
using System.Text;
using OManager.DataLayer.Connection;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using OME.Logging.Common;
using OME.Logging.Tracing;

namespace OManager.DataLayer.Maintanence
{
    public class db4oBackup
    {
        private string m_newbackupLocation;
        IObjectContainer objectContainer;

        public db4oBackup(string Location)
        {
            objectContainer = Db4oClient.Client; 
            m_newbackupLocation = Location;
        }
        public void db4oBackupDatabase()
        {
            objectContainer.Ext().Backup(m_newbackupLocation);
                       
        }

    }
}
