using System;
using System.Collections.Generic;
using OManager.BusinessLayer.Common;
using OME.Logging.Common;


namespace OManager.BusinessLayer.QueryManager
{
	[Serializable ]
    public class OMQueryGroup
    {
        private List<OMQueryClause> m_listQueryClauses;
        private CommonValues.LogicalOperators m_groupLogicalOperator;

        public OMQueryGroup()
        {
           
            m_listQueryClauses = new List<OMQueryClause>();

        }
        public List<OMQueryClause> ListQueryClauses
        {
            get { return m_listQueryClauses; }
            set { m_listQueryClauses = value; }
        }

        public CommonValues.LogicalOperators GroupLogicalOperator
        {
            get { return m_groupLogicalOperator; }
            set { m_groupLogicalOperator = value; }
        }

        public void AddOMQueryClause(OMQueryClause omQueryClause)
        {

            m_listQueryClauses.Add(omQueryClause);
        }

        public override string ToString()
        {
            string strClause = string.Empty;
            try
            {
                foreach (OMQueryClause om in m_listQueryClauses)
                {
                    strClause = strClause + string.Format("{0} ", om.ToString());

                }
                if (strClause != string.Empty)
                {
                    string subStr = strClause.Substring(strClause.Length - 3, 3);

                    if (subStr == "OR ")
                        strClause = strClause.Substring(0, strClause.Length - 3);
                    else
                        strClause = strClause.Substring(0, strClause.Length - 4);


                    strClause = string.Format("{0}({1}) ", GroupLogicalOperator, strClause);

                }
                

                

            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                
            }
            return strClause;
        }
    }
}
