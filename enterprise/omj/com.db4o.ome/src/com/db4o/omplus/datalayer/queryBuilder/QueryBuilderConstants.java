package com.db4o.omplus.datalayer.queryBuilder;

/**
 * Stores all constants needed just for the QueryBuilder module
 * 
 * @author prameela_nair
 *
 */

public class QueryBuilderConstants 
{
	public static final String EQUALS = "Equal";
	public static final String NOT_EQUALS = "Not Equal";
	public static final String GREATER_THAN = "Greater Than";
	public static final String LESS_THAN = "Less Than";
	public static final String GREATER_THAN_EQUAL = "Greater Than Equal";
	public static final String LESS_THAN_EQUAL = "Less Than Equal";
	public static final String STARTS_WITH = "Starts With";
	public static final String ENDS_WITH = "Ends With";
	public static final String CONTAINS = "Contains";
	
	public static final int EQUALS_IDENTIFIER = 0;
	public static final int NOT_EQUALS_IDENTIFIER = 1;
	public static final int GREATER_THAN_IDENTIFIER = 2;
	public static final int LESS_THAN_IDENTIFIER = 3;
	public static final int GREATER_THAN_EQUAL_IDENTIFIER = 4;
	public static final int LESS_THAN_EQUAL_IDENTIFIER = 5;
	public static final int ENDS_WITH_IDENTIFIER = 6;
	public static final int STARTS_WITH_IDENTIFIER = 7;
	public static final int CONTAINS_IDENTIFIER = 8;
		
	public static final int DATATYPE_CHARACTER = 0;
	public static final int DATATYPE_STRING = 1;
	public static final int DATATYPE_NUMBER = 2;
	public static final int DATATYPE_DATE_TIME = 3;
	public static final int DATATYPE_BOOLEAN = 4;
	
	public static final int DATATYPE_BYTE= 22;
	public static final int DATATYPE_SHORT = 33;
	public static final int DATATYPE_INT = 55;
	public static final int DATATYPE_LONG = 66;
	public static final int DATATYPE_FLOAT = 88;
	public static final int DATATYPE_DOUBLE = 77;
	
	
	public static final  String[] CHARACTER_CONDITION_ARRAY= {EQUALS,NOT_EQUALS};
	public static final  String[] STRING_CONDITION_ARRAY= {EQUALS,NOT_EQUALS,STARTS_WITH,ENDS_WITH,CONTAINS};
	public static final  String[] NUMERIC_CONDITION_ARRAY= {EQUALS,NOT_EQUALS,GREATER_THAN,GREATER_THAN_EQUAL, LESS_THAN, LESS_THAN_EQUAL};
	public static final  String[] BOOLEAN_CONDITION_ARRAY = {EQUALS,NOT_EQUALS};
	public static final  String[] DATE_TIME_CONDITION_ARRAY = {EQUALS,GREATER_THAN, LESS_THAN};
	
	
	public static int getConstraintsIdentifier(String constraint)
	{
		if(constraint.equals(EQUALS))
			return EQUALS_IDENTIFIER;
		else if(constraint.equals(NOT_EQUALS))
			return NOT_EQUALS_IDENTIFIER;
		else if(constraint.equals(GREATER_THAN))
			return GREATER_THAN_IDENTIFIER;
		else if(constraint.equals(GREATER_THAN_EQUAL))
			return GREATER_THAN_EQUAL_IDENTIFIER;
		else if(constraint.equals(LESS_THAN))
			return LESS_THAN_IDENTIFIER;
		else if(constraint.equals(LESS_THAN_EQUAL))
			return LESS_THAN_EQUAL_IDENTIFIER;
		else if(constraint.equals(STARTS_WITH))
			return STARTS_WITH_IDENTIFIER;
		else if(constraint.equals(ENDS_WITH))
			return ENDS_WITH_IDENTIFIER;	
		else if(constraint.equals(CONTAINS))
			return CONTAINS_IDENTIFIER;	
		else
			return -1;
		
	}
	
	public static String[] OPERATOR_ARRAY = { "AND", "OR"};
	public static final int OPERATOR_AND = 0;
	public static final int OPERATOR_OR = 1;
	public static int getOPeratorIdentifier(String operator)
	{
		if(operator.equals(OPERATOR_ARRAY[0]))
			//return 0;
			return OPERATOR_AND;
		else if(operator.equals(OPERATOR_ARRAY[1]))
			//return 1;
			return OPERATOR_OR;		
		else
			return -1;
		
	}
	
	public static final int FIELD = 0;
	public static final int CONDITION = 1;
	public static final int VALUE = 2;
	public static final int OPERATOR = 3;
	public static String[] columnNames = new String[] { "Field","Condition","Value","Operator"};
	
	public static String DATE_FORMAT = "mm/dd/yyyy";	
	
}
