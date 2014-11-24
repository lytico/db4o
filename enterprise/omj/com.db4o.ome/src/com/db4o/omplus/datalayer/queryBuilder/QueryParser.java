package com.db4o.omplus.datalayer.queryBuilder;

import java.util.*;

import com.db4o.*;
import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.query.*;
import com.db4o.reflect.*;

public class QueryParser
{
    private final String OR_OPERATOR = "OR";
    private final String AND_OPERATOR = "AND";
    
	private boolean isDate;

    public ObjectSet executeOMQueryList(OMQuery allConstraints)
    {
        Constraint rootConstraint = null;
        Constraint ConCatClauses = null;
        Constraint buildClause = null;
        Constraint buildGroup = null;           
        Constraint conCatGroup = null; ;

        IDbInterface db = Activator.getDefault().dbModel().db();
		Query query = db.getDB().query();
        rootConstraint = null;        
        int groupcount = 0;
        
        ArrayList<QueryGroup> queryGroupList = allConstraints.getConstraintList();
        for(QueryGroup qmGroup: queryGroupList)
        {
            int clausecount = 0;
            groupcount++;
            buildClause = null;
            ArrayList<QueryClause> queryClauseList = qmGroup.getQueryList(); 
            for (QueryClause qmclause : queryClauseList)
            {
                clausecount++;
                // Adds Class Constraint for all groups
                if(rootConstraint == null)
                	rootConstraint = addClassConstraint(query, qmclause.getField(), db.reflectHelper());
                buildClause = addFieldConstraints(query, qmclause);
                if (qmclause.getCondition() != null)
                {
                	if(!isDate){
                		buildClause = applyOperator(buildClause, qmclause.getCondition());
                	}else
                		isDate = false;
                }

                if (qmclause.getOperator().equals(OR_OPERATOR))
                {
                    if (buildClause != null)
                    {
                        if (clausecount == 1)
                        {
                            ConCatClauses = buildClause;
                        }
                        if (clausecount > 1)
                        {
                            ConCatClauses = buildClause.or(ConCatClauses);
                        }

                    }
                }
            }
            if (ConCatClauses != null)
            {
                buildGroup = ConCatClauses;
            }
            else
            {
                buildGroup = buildClause;
            }


            if (qmGroup.getGroupOperator() != null && qmGroup.getGroupOperator().trim().length()!=0 )
            {
            	  if (buildGroup != null)
                  {
            		  if (qmGroup.getGroupOperator().equals(OR_OPERATOR))
                      {
            			  conCatGroup = conCatGroup.or(buildGroup);
                      }else if (qmGroup.getGroupOperator().equals(AND_OPERATOR))
                      {
                    	  conCatGroup = conCatGroup.and(buildGroup);
                      }
                  }
            } else	{
            	conCatGroup = buildGroup;
            }
        }
        ObjectSet objSet = query.execute();
        return objSet;
    }

    private Constraint addClassConstraint(Query query, String field, ReflectHelper reflectHelper) {
    	String className = field.split(OMPlusConstants.REGEX)[0];
    	ReflectClass clazz = reflectHelper.getReflectClazz(className);
		return query.constrain(clazz);
	}

	public Constraint applyOperator(Constraint cons, String db4oOperator)
    {	
        switch (QueryBuilderConstants.getConstraintsIdentifier(db4oOperator))
        {
        	case QueryBuilderConstants.EQUALS_IDENTIFIER:
        		cons.equal();
        		break;
        	case QueryBuilderConstants.NOT_EQUALS_IDENTIFIER:
        		cons.equal().not();
        		break;
        	case QueryBuilderConstants.GREATER_THAN_IDENTIFIER:
        		cons.greater();
        		break;
        	case QueryBuilderConstants.LESS_THAN_IDENTIFIER:
        		cons.smaller();
        		break;
        	case QueryBuilderConstants.GREATER_THAN_EQUAL_IDENTIFIER:
        		cons.equal().greater();
        		break;
        	case QueryBuilderConstants.LESS_THAN_EQUAL_IDENTIFIER:
        		cons.equal().smaller();
        		break;
        	case QueryBuilderConstants.ENDS_WITH_IDENTIFIER:
        		cons.endsWith(false);
        		break;
        	case QueryBuilderConstants.STARTS_WITH_IDENTIFIER:
        		cons.startsWith(false);
        		break;
        	case QueryBuilderConstants.CONTAINS_IDENTIFIER:
        		cons.like();
        		break;  
        }
        return cons; 
    }


	@SuppressWarnings({ "deprecation" })
	public Constraint addFieldConstraints(Query query, QueryClause clause)
    {
    	 String fieldname = clause.getField();
    	 Object value = clause.getValue();
        String[] str = fieldname.split(OMPlusConstants.REGEX);
        Query q = addAsDescends(query, str);
        Constraint cons = null;
        Date temp = new Date();
        Object valueType =  value;
        if(valueType instanceof Date)
        { //clean up
        	temp.setTime(0);temp.setDate(((Date)valueType).getDate());
        	temp.setHours(((Date)valueType).getHours());temp.setMinutes(((Date)valueType).getMinutes());
        	temp.setSeconds(((Date)valueType).getSeconds());temp.setMonth(((Date)valueType).getMonth());
        	temp.setYear(((Date)valueType).getYear());
        	Date d2 = (Date)((Date)temp).clone();
        	d2.setSeconds(temp.getSeconds()+1);
           	if (clause.getCondition().equals(QueryBuilderConstants.EQUALS)){
        		temp.setTime(temp.getTime() - 1);
                cons = q.constrain(d2).smaller().and(q.constrain(temp).greater());
        	}else if(clause.getCondition().equals(QueryBuilderConstants.GREATER_THAN)){
        		long ms = d2.getTime();
        		d2.setTime(ms - 1);
        		cons = q.constrain(d2).greater();
        	} else if(clause.getCondition().equals(QueryBuilderConstants.LESS_THAN)){
        		cons = q.constrain(temp).smaller();
        	}
        	isDate = true;
        }
        else {
        	valueType = convertToValue(str, value.toString());
        	cons = q.constrain(valueType);
        }
        	
        return cons;
    }

    public Object convertToValue(String[] str, String value)
    {
    	Object fValue = null;
    	Converter convert = new Converter();
    	ReflectClass clazz = getFieldType(str);
    	String type = clazz.getName();
    	if(clazz.isPrimitive()){
    		fValue = convert.getPrimitiveValue(type, value);
    	}else{
    		fValue =  convert.getValue(type, value);
    	}
       	return fValue;
    }
    
    private ReflectClass getFieldType(String []strArray) {
		Reflector rf = Activator.getDefault().dbModel().db().reflector();
		ReflectClass clz = rf.forName(strArray[0]);
		int length = strArray.length;
		int count = 1;
		while(count < length ){
			clz = ReflectHelper.getDeclaredFieldInHeirarchy(clz, strArray[count++]).getFieldType();
		}
		return clz;
	}

	public Query addAsDescends(Query query, String[] str)
    {
        Query result = query;
        int count = 1;
        while (str.length > 0 && count <= str.length - 1)
        {
            result = result.descend(str[count]);
            count++;
        }
        return result;
    }

	public ObjectSet execute(ReflectClass clazz)
	{
		ObjectContainer objectContainer = Activator.getDefault().dbModel().db().getDB();
        Query query = objectContainer.query();
        query.constrain(clazz);
		return query.execute();
	}
}