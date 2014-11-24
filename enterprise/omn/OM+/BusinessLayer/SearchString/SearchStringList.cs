using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OManager.BusinessLayer.Login;

namespace OManager.BusinessLayer.SearchString
{
    public class SearchStringList
    {
         ConnParams m_connParam;
        public ConnParams ConnParam
        {
            get { return m_connParam; }
            set { m_connParam = value; }
        }

        private List<SeachString> m_ListSearchString;
       

        public List<SeachString> ListSearchString
        {
            get { return m_ListSearchString; }
            
        }
        private long m_TimeOfCreation;
        public long TimeOfCreation
        {
            get { return m_TimeOfCreation; }
            set { m_TimeOfCreation = value; }
        }

        public SearchStringList(ConnParams connParam)
        {           
            m_connParam = connParam;
            m_ListSearchString = new List<SeachString>(); 
        }

    }
}
