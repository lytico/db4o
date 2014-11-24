using System;
using System.Collections; 
using System.Collections.Generic;
using System.Text;
using OManager.BusinessLayer.QueryManager;

namespace OManager.BusinessLayer.Login
{
	[Serializable ] 
    public class ConnectionDetails 
    {
       
        DateTime m_timestamp;
        ConnParams m_connParam;
        List<OMQuery> m_queryList;
        private long m_TimeOfCreation;
	    private string m_customConfigAssemblyPath;

        public string CustomConfigAssemblyPath
        {
            get { return m_customConfigAssemblyPath; }
            set { m_customConfigAssemblyPath = value; }
        }

        public long TimeOfCreation
        {
            get { return m_TimeOfCreation; }
            set { m_TimeOfCreation = value; }
        }
        public ConnectionDetails(ConnParams connParam)
        {
            m_queryList = new List<OMQuery>();
            m_connParam = connParam;
        }
        
        public List<OMQuery> QueryList
        {
            get {
                if( m_queryList!=null)
                {
                    m_queryList.Sort(new CompareQueryTimestamps());
                   return m_queryList;
                }
                return null;
            }
            set { m_queryList = value;}
        }

        public ConnParams ConnParam
        {
            get { return m_connParam; }
            set { m_connParam = value; }
        }

        public DateTime Timestamp
        {
            get { return m_timestamp; }
            set { m_timestamp = value; }
        }
      
    }

   
}
