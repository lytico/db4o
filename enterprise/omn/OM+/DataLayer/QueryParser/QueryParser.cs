using System;
using System.Globalization;
using OManager.Business.Config;
using OManager.BusinessLayer.QueryManager;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using OManager.DataLayer.Connection;
using OManager.BusinessLayer.Common;
using OManager.DataLayer.CommonDatalayer;
using OME.Logging.Common;
using OManager.DataLayer.Reflection;

namespace OManager.DataLayer.QueryParser
{
    // change name to QueryParser
    public class QueryParser
    {
        IObjectContainer objectContainer;
    	readonly OMQuery m_OmQuery;

        public QueryParser(OMQuery OmQuery)
        {
            objectContainer = Db4oClient.Client;
            m_OmQuery = OmQuery;
        }

        public IObjectSet ExecuteOMQueryList()
        {
            try
            {
                IConstraint ConCatClauses = null;
                IConstraint buildClause;
                IConstraint conCatGroup = null;

                objectContainer = Db4oClient.Client;
                IQuery query = objectContainer.Query();

                //ToCheckQueryDirectly();

                FormulateRootConstraints(query, m_OmQuery.BaseClass);
                int Groupcount = 0;
                foreach (OMQueryGroup qmGroup in m_OmQuery.ListQueryGroup)
                {

                    int clausecount = 0;
                    Groupcount++;
                    buildClause = null;
                    foreach (OMQueryClause qmclause in qmGroup.ListQueryClauses)
                    {
                        clausecount++;
                        buildClause = FormulateFieldConstraints(query, qmclause);
                            //.Classname, qmclause.Fieldname, qmclause.Value);
                        if (qmclause.Operator != null)
                        {
                            if (qmclause.FieldType != BusinessConstants.DATETIME)
                                buildClause = ApplyOperator(buildClause, qmclause.Operator);
                        }

                        if (qmclause.ClauseLogicalOperator == CommonValues.LogicalOperators.OR)
                        {
                            if (buildClause != null)
                            {
                                if (clausecount == 1)
                                {
                                    ConCatClauses = buildClause;
                                }
                                if (clausecount > 1)
                                {
                                    ConCatClauses = buildClause.Or(ConCatClauses);
                                }

                            }
                        }
                        if (qmclause.ClauseLogicalOperator == CommonValues.LogicalOperators.AND)
                        {
                            if (buildClause != null)
                            {
                                if (clausecount == 1)
                                {
                                    ConCatClauses = buildClause;
                                }
                                if (clausecount > 1)
                                {
                                    ConCatClauses = buildClause.And(ConCatClauses);
                                }
                            }
                        }
                    }

                    IConstraint buildGroup = ConCatClauses ?? buildClause;
                    if (qmGroup.GroupLogicalOperator != CommonValues.LogicalOperators.EMPTY)
                    {
                        if (qmGroup.GroupLogicalOperator == CommonValues.LogicalOperators.OR)
                        {

                            if (buildGroup != null)
                            {
                                conCatGroup = conCatGroup.Or(buildGroup);
                            }
                        }
                        if (qmGroup.GroupLogicalOperator == CommonValues.LogicalOperators.AND)
                        {

                            if (buildGroup != null)
                            {
                                conCatGroup = conCatGroup.And(buildGroup);
                            }
                        }
                    }
                    else
                    {
                        conCatGroup = buildGroup;
                    }
                }
                IObjectSet objSet = query.Execute();
                return objSet;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null;
            }
        }


        public IConstraint ApplyOperator(IConstraint cons, string db4oOperator)
        {
            try
            {
                switch (db4oOperator)
                {
                    case BusinessConstants.CONDITION_STARTSWITH: 
                        cons.StartsWith(false);
                        break;
                    case BusinessConstants.CONDITION_ENDSWITH:
                        cons.EndsWith(false);
                        break;
                    case BusinessConstants.CONDITION_EQUALS:
                        cons.Equal();
                        break;
                    case BusinessConstants.CONDITION_NOTEQUALS:
                        cons.Not();
                        break;
                    case BusinessConstants.CONDITION_GREATERTHAN :
                        cons.Greater();
                        break;
                    case BusinessConstants.CONDITION_GREATERTHANEQUAL:
                        cons.Greater().Equal();
                        break;
                    case BusinessConstants.CONDITION_LESSTHAN :
                        cons.Smaller();
                        break;
                    case BusinessConstants.CONDITION_LESSTHANEQUAL:
                        cons.Smaller().Equal();
                        break;
                    case BusinessConstants.CONDITION_CONTAINS:
                        cons.Like();
                        break;
                }
                return cons;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null;
            }
        }


      

        public void FormulateRootConstraints(IQuery query, string classname)
        {
            try
            {
                query.Constrain(DataLayerCommon.ReflectClassForName(classname));
                
                
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                
            }
        }


        public IConstraint FormulateFieldConstraints(IQuery query, OMQueryClause clause)
        {            
            try
            {
                IConstraint cons = null;
                string[] str = clause.Fieldname.Split('.');
                IQuery q = AddAsDescends(query, str);
                IType type= Db4oClient.TypeResolver.Resolve(clause.FieldType);


                switch (type.DisplayName )
                {
                    case BusinessConstants.DATETIME:
                        {
                            IConstraint c1=null, c2=null;
                            DateTimeFormatInfo dateTimeFormatterProvider = DateTimeFormatInfo.CurrentInfo.Clone() as DateTimeFormatInfo;
                            dateTimeFormatterProvider.ShortDatePattern = "MM/dd/yyyy hh:mm:ss tt";
                            DateTime dt = DateTime.Parse(clause.Value.Trim(), dateTimeFormatterProvider);

                            DateTime dt1 = dt.AddDays(-1);
                            DateTime dt2 = dt.AddDays(1);
                            if (clause.Operator.Equals(BusinessConstants.CONDITION_EQUALS))
                                cons = q.Constrain(dt2).Smaller().And(q.Constrain(dt1).Greater());
                           
                            else if (clause.Operator.Equals(BusinessConstants.CONDITION_GREATERTHAN))
                            {
                                c1 = q.Constrain(dt2).Greater();
                                c2=q.Constrain(dt2.AddDays(1)).Smaller().And(q.Constrain(dt).Greater());
                                cons =c1.Or(c2);
                            }
                            else if (clause.Operator.Equals(BusinessConstants.CONDITION_LESSTHAN))
                            {
                                c1 = q.Constrain(dt1).Smaller();
                                c2 = q.Constrain(dt).Smaller().And(q.Constrain(dt1.AddDays(-1)).Greater());
                                cons = c1.Or(c2);
                            }
                                break;
                        }
                    case BusinessConstants.BOOLEAN:
                        {
                            bool check = bool.Parse(clause.Value);
                            cons = q.Constrain(check);
                            break;
                        }



                    default:
                        cons = q.Constrain(type.Cast(clause.Value));
                        break;
                }

                return cons;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null;
            }

        }

        public IQuery AddAsDescends(IQuery query, string[] str)
        {
            try
            {
                IQuery result = query;
                int count = 1;                
                while (str.Length > 0 && count <= str.Length - 1)
                {
                    result = result.Descend(str[count]);
                    count++;
                }
                return result;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null;
            }

        }
    }

}
