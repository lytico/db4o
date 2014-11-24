package com.db4o.omplus.datalayer.propertyViewer;


public class PropertyViewerConstants
{
	/***** DB properties Constants ****/
	public static final String DBSIZE ="DBSize (bytes)";
	public static final String NUM_CLASSES = "No of Classes";
	public static final String FREE_SPACE = "Free Space (bytes)";
	
	public static final String[] DB_PROPERTY_COLUMN_ARRAY = {DBSIZE, NUM_CLASSES, FREE_SPACE};
	public static final int[] DB_PROPERTY_COLUMN_WIDTHS = {130,100,130};
	
	
	public static final int DBSIZE_ID = 0;
	public static final int NUM_CLASSES_ID = 1;
	public static final int FREE_SPACE_ID = 2;
	
	/***** Class properties Constants ****/
	public static final String FIELD ="Field";
	public static final String DATATYPE = "Datatype";
	public static final String INDEXED = "Is Indexed";
	public static final String ACCESS_MODIFIER = "Is Public";
	
	public static final String[] CLASS_PROPERTY_COLUMN_ARRAY = {FIELD,DATATYPE,INDEXED, ACCESS_MODIFIER};
	public static final int[] CLASS_PROPERTY_COLUMN_WIDTHS = {100,150,100,100};
	
	public static final int FIELD_ID = 0;
	public static final int DATATYPE_ID = 1;
	public static final int INDEXED_ID = 2;
	public static final int ACCESS_MODIFIER_ID = 3;
	
	
	/***** Object properties Constants ****/
	public static final String UUID ="Uuid";
	public static final String LOCAL_IDENTIFIER = "LocalId";
	public static final String DEPTH = "Depth";
	public static final String VERSION = "Version";
	
	public static final String[] OBJECT_PROPERTY_COLUMN_ARRAY = {UUID,LOCAL_IDENTIFIER,DEPTH,VERSION};
	
	public static final int UUID_ID = 0;
	public static final int LOCAL_IDENTIFIER_ID = 1;
	public static final int DEPTH_ID = 2;
	public static final int VERSION_ID = 3;
	
	
	
}
