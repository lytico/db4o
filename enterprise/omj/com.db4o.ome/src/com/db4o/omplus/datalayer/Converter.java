package com.db4o.omplus.datalayer;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

import com.db4o.omplus.datalayer.queryBuilder.QueryBuilderConstants;

public class Converter {

	private SimpleDateFormat sdf = new SimpleDateFormat(OMPlusConstants.DATE_FORMAT);
	
	private final String P_BYTE		=	"byte";
	private final String P_SHORT	=	"short";
	private final String P_CHAR		=	"char";
	private final String P_INT		=	"int";
	private final String P_LONG		=	"long";
	private final String P_FLOAT	=	"float";
	private final String P_DOUBLE	=	"double";
	private final String P_BOOLEAN	=	"boolean";
	private final String STRING		=	String.class.getName();
	private final String STR_BUFFER = 	StringBuffer.class.getName();
	private final String STR_BUILDER = 	StringBuilder.class.getName();
	private final String BYTE		=	Byte.class.getName();
	private final String SHORT		=	Short.class.getName();
	private final String CHAR		=	Character.class.getName();
	private final String INT		=	Integer.class.getName();
	private final String LONG		=	Long.class.getName();
	private final String FLOAT		=	Float.class.getName();
	private final String DOUBLE		=	Double.class.getName();
	public final String DATE		=	Date.class.getName();
	private final String BOOLEAN	=	Boolean.class.getName();
	
	
	//ObjectTreeBuilder ->updateValue();
	//2 fun in this file
	//ModifyObject -> updateValue
	//TODO: Replace the return types by constants in QueryBuilder
	/*public int getType(String fieldType){
		if(fieldType.equals(STRING))
			return 1;			
		else if(fieldType.equals(BYTE) || fieldType.equals(P_BYTE))
			return 2;
		else if(fieldType.equals(SHORT) || fieldType.equals(P_SHORT))
			return 3;
		else if(fieldType.equals(CHAR) || fieldType.equals(P_CHAR))
			return 4;
		else if(fieldType.equals(INT) || fieldType.equals(P_INT))
			return 5;
		else if(fieldType.equals(LONG) || fieldType.equals(P_LONG))
			return 6;
		else if(fieldType.equals(DOUBLE) || fieldType.equals(P_DOUBLE))
			return 7;
		else if(fieldType.equals(FLOAT) || fieldType.equals(P_FLOAT))
			return 8;
		else if(fieldType.equals(BOOLEAN) || fieldType.equals(P_BOOLEAN))
			return 9;
		else if(fieldType.equals(DATE))
			return 10;
		return 0;
	}*/
	
	//Prameela replaced with constants
	public int getType(String fieldType) {
		if(fieldType.equals(STRING) || fieldType.equals(STR_BUFFER)
				|| fieldType.equals(STR_BUILDER))
			return QueryBuilderConstants.DATATYPE_STRING;
		else if(fieldType.equals(BYTE) || fieldType.equals(P_BYTE))
			return QueryBuilderConstants.DATATYPE_BYTE;
		else if(fieldType.equals(SHORT) || fieldType.equals(P_SHORT))
			return QueryBuilderConstants.DATATYPE_SHORT;
		else if(fieldType.equals(CHAR) || fieldType.equals(P_CHAR))
			return QueryBuilderConstants.DATATYPE_CHARACTER;
		else if(fieldType.equals(INT) || fieldType.equals(P_INT))
			return QueryBuilderConstants.DATATYPE_INT;
		else if(fieldType.equals(LONG) || fieldType.equals(P_LONG))
			return QueryBuilderConstants.DATATYPE_LONG;
		else if(fieldType.equals(DOUBLE) || fieldType.equals(P_DOUBLE))
			return QueryBuilderConstants.DATATYPE_DOUBLE;
		else if(fieldType.equals(FLOAT) || fieldType.equals(P_FLOAT))
			return QueryBuilderConstants.DATATYPE_FLOAT;
		else if(fieldType.equals(BOOLEAN) || fieldType.equals(P_BOOLEAN))
			return QueryBuilderConstants.DATATYPE_BOOLEAN;
		else if(fieldType.equals(DATE))
			return QueryBuilderConstants.DATATYPE_DATE_TIME;
		return -1;
	}
	
	
	
	//Prameela replaced by constants
	public Object getValue(String type, String value) {
		switch(getType(type)){
		
		case QueryBuilderConstants.DATATYPE_STRING:
			return value; 
		
		case QueryBuilderConstants.DATATYPE_BYTE:	
			return new Byte(value);
		case QueryBuilderConstants.DATATYPE_SHORT:	
			return new Short(value);
		
		case QueryBuilderConstants.DATATYPE_CHARACTER:	
//			TODO to be changed in future @blunder
			char[] temp = value.toCharArray();
			return new Character(temp[0]);
		
		case QueryBuilderConstants.DATATYPE_INT:	
			return new Integer(value);
		
		case QueryBuilderConstants.DATATYPE_LONG:	
			return new Long(value);
		case QueryBuilderConstants.DATATYPE_DOUBLE:
			return new Double(value);
		
		case QueryBuilderConstants.DATATYPE_FLOAT:	
			return new Float(value);
		case QueryBuilderConstants.DATATYPE_BOOLEAN:	
			return new Boolean(value);
			
		case QueryBuilderConstants.DATATYPE_DATE_TIME:	
//			TODO need to change this
			try {
					return sdf.parse(value);
				} catch (ParseException e) {
				}
		default :
			return value;
		}
	}

	//TODO: replace with constants
	/*public Object getPrimitiveValue(String type, String value) {
		switch(getType(type)){
		case 2:
			return new Byte(value);
		case 3:
			return new Short(value);
		case 4:
//			TODO: to be changed in future @blunder
			char[] temp = value.toCharArray();
			return new Character(temp[0]);
		case 5:
			return new Integer(value);
		case 6:
			return new Long(value);
		case 7:
			return new Double(value);
		case 8:
			return new Float(value);
		case 9:
			return new Boolean(value);
		default :
			return value;
		}
	}	*/
	
	//Prameela replaced with constants
	public Object getPrimitiveValue(String type, String value) {
		switch(getType(type)){
		case QueryBuilderConstants.DATATYPE_BYTE:	
			return new Byte(value);
		case QueryBuilderConstants.DATATYPE_SHORT:
			return new Short(value);
		case QueryBuilderConstants.DATATYPE_CHARACTER:
//			TODO: to be changed in future @blunder
			char[] temp = value.toCharArray();
			return new Character(temp[0]);
		case QueryBuilderConstants.DATATYPE_INT:
			return new Integer(value);
		case QueryBuilderConstants.DATATYPE_LONG:
			return new Long(value);
		case QueryBuilderConstants.DATATYPE_DOUBLE:
			return new Double(value);
		case QueryBuilderConstants.DATATYPE_FLOAT:
			return new Float(value);
		case QueryBuilderConstants.DATATYPE_BOOLEAN:
			return new Boolean(value);
		default :
			return value;
		}
	}
}
