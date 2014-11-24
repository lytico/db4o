using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OManager.BusinessLayer.QueryManager;

namespace OMAddinDataTransferLayer.QueryData
{
	[Serializable]
	public class ProxyOMQuery
	{
		private List<ProxyOMQueryGroup> m_listQueryGroup;
		private string m_baseClass;
		private Hashtable m_attributeList;
		private DateTime m_queryTimeStamp;

		
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
		
		public List<ProxyOMQueryGroup> ListQueryGroup
		{
			get { return m_listQueryGroup; }
			set { m_listQueryGroup = value; }
		}

		

		


		
	}
}
