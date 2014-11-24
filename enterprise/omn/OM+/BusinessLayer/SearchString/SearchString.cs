using System;
using System.Collections.Generic;
using System.Text;

namespace OManager.BusinessLayer.SearchString
{
    public class SeachString
    {
        private DateTime m_timestamp;
        internal DateTime Timestamp
        {
            get { return m_timestamp; }
            set { m_timestamp = value; }
        }

        private string m_SearchString;
        internal string SearchString
        {
            get { return m_SearchString; }
            set { m_SearchString = value; }
        }

        public SeachString(DateTime date,string str)
        {
            m_timestamp = date;
            m_SearchString = str;
        }

    }


}
