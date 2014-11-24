package com.db4o.omplus.ui.model;

public class ArrayLabelProvider {
	
	public static final String INT_ARRAY = new int[0].getClass().getName();
	public static final String LONG_ARRAY = new long[0].getClass().getName();
	public static final String BYTE_ARRAY = new byte[0].getClass().getName();
	public static final String SHORT_ARRAY = new short[0].getClass().getName();
	public static final String CHAR_ARRAY = new char[0].getClass().getName();
	public static final String FLOAT_ARRAY = new float[0].getClass().getName();
	public static final String DOUBLE_ARRAY = new double[0].getClass().getName();
	public static final String BOOL_ARRAY = new boolean[0].getClass().getName();
	public static final String INT_OBJ_ARRAY = new Integer[0].getClass().getName();
	public static final String LONG_OBJ_ARRAY = new Long[0].getClass().getName();
	public static final String BYTE_OBJ_ARRAY = new Byte[0].getClass().getName();
	public static final String SHORT_OBJ_ARRAY =  new Short[0].getClass().getName();
	public static final String CHAR_OBJ_ARRAY = new Character[0].getClass().getName();
	public static final String FLOAT_OBJ_ARRAY = new Float[0].getClass().getName();
	public static final String DOUBLE_OBJ_ARRAY = new Double[0].getClass().getName();
	public static final String BOOL_OBJ_ARRAY = new Boolean[0].getClass().getName();
	public static final String STRING_ARRAY = new String[0].getClass().getName();
	
	public static final String INT_ARRAY_STR = "int []";
	public static final String LONG_ARRAY_STR = "long []";
	public static final String BYTE_ARRAY_STR = "byte[]";
	public static final String SHORT_ARRAY_STR = "short []";
	public static final String CHAR_ARRAY_STR = "char []";
	public static final String FLOAT_ARRAY_STR = "float []";
	public static final String DOUBLE_ARRAY_STR = "double []";
	public static final String BOOL_ARRAY_STR = "boolean []";
	public static final String INT_OBJ_ARRAY_STR = "Integer []";
	public static final String LONG_OBJ_ARRAY_STR = "Long []";
	public static final String BYTE_OBJ_ARRAY_STR = "Byte []";
	public static final String SHORT_OBJ_ARRAY_STR =  "Short []";
	public static final String CHAR_OBJ_ARRAY_STR = "Character []";
	public static final String FLOAT_OBJ_ARRAY_STR = "Float []";
	public static final String DOUBLE_OBJ_ARRAY_STR = "Double []";
	public static final String BOOL_OBJ_ARRAY_STR = "Boolean []";
	public static final String STRING_ARRAY_STR = "String []";
	
	public static String lookUp(String type) {
		if (type.equals(INT_ARRAY))
			return INT_ARRAY_STR;
		else if(type.equals(INT_OBJ_ARRAY))
				return INT_OBJ_ARRAY_STR;
		else if(type.equals(LONG_ARRAY))
			return LONG_ARRAY_STR;
		else if(type.equals(LONG_OBJ_ARRAY))
			return LONG_OBJ_ARRAY_STR;
		else if(type.equals(BYTE_ARRAY))
			return BYTE_ARRAY_STR;
		else if(type.equals(BYTE_OBJ_ARRAY))
			return BYTE_OBJ_ARRAY_STR;
		else if(type.equals(SHORT_ARRAY))
			return SHORT_ARRAY_STR;
		else if(type.equals(SHORT_OBJ_ARRAY))
			return SHORT_OBJ_ARRAY_STR;
		else if(type.equals(CHAR_ARRAY))
			return CHAR_ARRAY_STR;
		else if(type.equals(CHAR_OBJ_ARRAY))
			return CHAR_OBJ_ARRAY_STR;
		else if(type.equals(DOUBLE_ARRAY))
			return DOUBLE_ARRAY_STR;
		else if(type.equals(DOUBLE_OBJ_ARRAY))
			return DOUBLE_OBJ_ARRAY_STR;
		else if(type.equals(FLOAT_ARRAY))
			return FLOAT_ARRAY_STR;
		else if(type.equals(FLOAT_OBJ_ARRAY))
			return FLOAT_OBJ_ARRAY_STR;
		else if(type.equals(BOOL_ARRAY))
			return BOOL_ARRAY_STR;
		else if(type.equals(BOOL_OBJ_ARRAY))
			return BOOL_OBJ_ARRAY_STR;
		else if(type.equals(STRING_ARRAY))
			return STRING_ARRAY_STR;
		else
			return null;
	}
	
	public String get(int arrType) {
		// TODO Auto-generated method stub
		return null;
	}
	
	
	

}
