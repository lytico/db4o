using System;
using System.Collections.Generic;
using System.Text;
using OManager.BusinessLayer.Common;


namespace OMAddinDataTransferLayer.QueryData
{
	[Serializable ]
    public class ProxyOMQueryGroup
    {
        private List<ProxyOMQueryClause> m_listQueryClauses;
        private CommonValues.LogicalOperators m_groupLogicalOperator;


		public List<ProxyOMQueryClause> ListQueryClauses
        {
            get { return m_listQueryClauses; }
            set { m_listQueryClauses = value; }
        }

        public CommonValues.LogicalOperators GroupLogicalOperator
        {
            get { return m_groupLogicalOperator; }
            set { m_groupLogicalOperator = value; }
        }

        public void AddOMQueryClause(ProxyOMQueryClause omQueryClause)
        {

            m_listQueryClauses.Add(omQueryClause);
        }

	
    }
}
