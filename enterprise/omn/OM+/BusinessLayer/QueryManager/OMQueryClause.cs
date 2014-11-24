using System;
using System.Collections.Generic;
using System.Text;
using OManager.BusinessLayer.Common;
using OManager.DataLayer.CommonDatalayer;
namespace OManager.BusinessLayer.QueryManager
{
	[Serializable ]
    public class OMQueryClause
    {

        private string m_Classname;//This will be a fully qualified classname like pilot.car.name

        public string Classname
        {
            get { return m_Classname; }
        }
        private string m_Operator;

        public string Operator
        {
            get { return m_Operator; }
        }
        private string m_Value;

        public string Value
        {
            get { return m_Value; }
        }
        private string m_Fieldname;

        public string Fieldname
        {
            get { return m_Fieldname; }
        }
        private string m_FieldType;

        public string FieldType
        {
            get { return m_FieldType; }
        }

        private CommonValues.LogicalOperators m_clauseLogicalOperator;

        public CommonValues.LogicalOperators ClauseLogicalOperator
        {
            get { return m_clauseLogicalOperator; }
            set { m_clauseLogicalOperator = value; }
        }

        public OMQueryClause(string classname, string fieldname, string fieldoperator, string fieldvalue, CommonValues.LogicalOperators clauseLogicalOperator,string fieldtype)
        {
            m_clauseLogicalOperator = clauseLogicalOperator;
            m_Classname = DataLayerCommon.RemoveGFromClassName(classname);
            m_Fieldname = fieldname;
            m_Operator = fieldoperator;
            m_Value = fieldvalue;
            m_FieldType = fieldtype;  
        }
        

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", m_Fieldname, m_Operator, m_Value, m_clauseLogicalOperator);
        }


    }
}
