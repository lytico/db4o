using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OME.Logging.Common;

namespace OManager.BusinessLayer.QueryManager
{
	[Serializable ]
    public class OMQuery
    {
        private List<OMQueryGroup> m_listQueryGroup ;
        private string m_baseClass;
        private Hashtable m_attributeList;
        private DateTime m_queryTimeStamp;

        public OMQuery(string baseclass,DateTime queryTimeStamp )
        {
            m_baseClass = baseclass;
            m_queryTimeStamp = queryTimeStamp;
            m_listQueryGroup= new List<OMQueryGroup>();       
            m_attributeList= new Hashtable();
        }
        public DateTime QueryTimestamp
        {
            get { return m_queryTimeStamp; }
            set { m_queryTimeStamp = value; }
        }

        public Hashtable AttributeList
        {
            get { return m_attributeList; }
            set { m_attributeList = value; }
        }

        public string BaseClass
        {
            get { return m_baseClass; }
            set { m_baseClass = value; }
        }
        private string queryString;

        public string QueryString
        {
            get { return queryString; }
            set { queryString = value; }
        }
     
        public List<OMQueryGroup> ListQueryGroup
        {
            get { return m_listQueryGroup; }
            set { m_listQueryGroup = value; } 
        }

        public void AddOMQuery(OMQueryGroup omQuerygroup)
        {
            ListQueryGroup.Add(omQuerygroup);
        }      

       
        public override string ToString()
        {
            try
            {
                string groupString = string.Empty;
                foreach (OMQueryGroup group in m_listQueryGroup)
                {

                    groupString = groupString + string.Format("{0}", group);
                }

                groupString = groupString.Replace("EMPTY", "");

                return groupString;
                
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return string.Empty;  
            }
           
        }
    }
   
    internal class CompareQueryTimestamps : IComparer<OMQuery>
    {
        public   int Compare(OMQuery q1, OMQuery q2)
        {
            
                return q2.QueryTimestamp.CompareTo(q1.QueryTimestamp);
        }
    }
}
