package com.db4o.omplus.datalayer.queryBuilder;

import com.db4o.omplus.datalayer.OMPlusConstants;

/**
 * Stores the invidual query clause.
 * 
 * @author prameela_nair
 *
 */

public class QueryClause 
{

	private String field 	= "";
	private String condition 	= "";
	private Object value 		= "";
	private String operator="";	
	  
	public QueryClause() 
	{		
		super();
	}
	public QueryClause(String string) {		
		super();
		setField(string);
	}

	public String getField() {
		return field;
	}

	public void setField(String field) {
		this.field = field;
	}

	public String getCondition() {
		return condition;
	}

	public void setCondition(String condition) {
		this.condition = condition;
	}

	public Object getValue() {
		return value;
	}

	public void setValue(Object ownervalue) {
		this.value = ownervalue;
	}

	public String getOperator() {
		return operator;
	}

	public void setOperator(String operator) {
		this.operator = operator;
	}
	
	public String getFieldNameWithoutPackageInfo()
	{
		if(field!=null && field.trim().length()>0 )
		{
			int colonIndex = field.indexOf(OMPlusConstants.REGEX);
			String packageName = field.substring(0,colonIndex);
			String fieldName = field.substring(colonIndex+1);
			fieldName = fieldName.replace(':', '.');
			String className = packageName.substring(packageName.lastIndexOf('.')+1);
			
			return className+"."+fieldName;
		}
		return "";
	}
	
	public String toString(){
		StringBuilder sb = new StringBuilder("(");
		sb.append(field);sb.append(" ");
		sb.append(condition);sb.append(" ");
		if(value != null)
			sb.append(value.toString());
		sb.append(")");
		return sb.toString();
	}

	
	
}
