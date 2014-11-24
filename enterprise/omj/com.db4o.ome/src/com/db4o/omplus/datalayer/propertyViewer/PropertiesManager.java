package com.db4o.omplus.datalayer.propertyViewer;

import java.util.*;

import com.db4o.ext.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.datalayer.propertyViewer.classProperties.*;
import com.db4o.omplus.datalayer.propertyViewer.dbProperties.*;
import com.db4o.omplus.datalayer.propertyViewer.objectProperties.*;
import com.db4o.reflect.*;


public class PropertiesManager {

	private final IDbInterface db;
	private final HashMap<String, ClassProperties> properties = new HashMap<String, ClassProperties>();
	
	public PropertiesManager(IDbInterface db){
		this.db = db;
	}
	
	public ObjectProperties getObjectProperties(Object obj){
		if(obj == null) {
			return null;
		}
		ObjectProperties objProp = new ObjectProperties();;
		ExtObjectContainer eoc = db.getDB().ext();
		objProp.setLocalID(eoc.getID(obj));
		ObjectInfo objInfo = eoc.getObjectInfo(obj);
		if(objInfo != null){
			objProp.setUuid(objInfo.getUUID());
			objProp.setVersion(objInfo.getVersion());
		}
	//			TODO: check depth and reference
		return objProp;
	}
	
	public ClassProperties getClassProperties(String className){
		ClassProperties clsProperties = null;
		if((className != null) && (className.trim().length() > 0)){
			
			if(properties.containsKey(className)){
				clsProperties = properties.get(className);
				clsProperties.setNumberOfObjects(getNumberOfObj(className));
				return clsProperties;
			}
			else{
				clsProperties = buildClassProperties(className);
				if(clsProperties != null)
					properties.put(className, clsProperties);
			}
		}
		return clsProperties;
	}	
	
	public DBProperties getDBProperties(){		
		DBProperties dbProperties = new DBProperties();
		dbProperties.setDbSize(db.getDBSize());
		dbProperties.setFreeSpace(db.getFreespaceSize());
		Object[] classes = null;
		try {
			classes = db.getStoredClasses();
			dbProperties.setNoOfClasses(classes.length);
		} 
		catch (Exception e) {
		}
		return dbProperties;
	}

//	TODO: move them to another Property builder class
	private ClassProperties buildClassProperties(String className) {
		ClassProperties clsProperties = new ClassProperties();
		clsProperties.setClassname(className);
		ReflectClass clazz = db.reflectHelper().getReflectClazz(className);
		clsProperties.setNumberOfObjects(getNumberOfObj(className));
		clsProperties.setFields(buildFieldProperties(clazz));
		return clsProperties;
	}

	private FieldProperties[] buildFieldProperties(ReflectClass clazz) {
		
		ReflectField []fields =  ReflectHelper.getDeclaredFieldsInHierarchy(clazz);
		FieldProperties []fp = new FieldProperties[fields.length];
		int count = 0;
		for(ReflectField rfield : fields){
			FieldProperties fieldProp = new FieldProperties();
			fieldProp.setFieldName(rfield.getName());
			if(rfield.isPublic())
			{	
				fieldProp.setAccessModifier("Public");
			}
			else
				fieldProp.setAccessModifier("NP");
			ReflectClass fieldType = rfield.getFieldType();
			fieldProp.setFieldDataType(fieldType.getName());
			fieldProp.setPrimitive(fieldType.isPrimitive());
			// Add further properties required
			fieldProp.setIndexed(isIndexed(clazz.getName(), rfield.getName()));
			fp[count++] = fieldProp;
		}
		return fp;
	}

	private boolean isIndexed(String clsname, String fname) {
		StoredClass storedClass = db.getDB().ext().storedClass(clsname);
		if(storedClass != null){
			StoredField[] fields = ReflectHelper.getDeclaredFieldsInHeirarchy(storedClass);
			if(fields != null){
				for( StoredField storedFld : fields){
					if(storedFld.getName().equals(fname)){
						return storedFld.hasIndex();
					}
				}
			}
		}
		return false;
	}

/*	private String[] getIndexedFieldNames(className, fieldName) {
		//String fieldNames
		return null;
	}*/

	private int getNumberOfObj(String className) {
		return db.getNumOfObjects(className);
	}
	
	public int getDepthForObj(Object obj){
//		@blunder has to be recursive
	/*	int objDepth = 0, depth = 0;
		if(obj != null){
			ReflectClass clazz = ReflectHelper.getReflectClazz(obj);
			if(clazz != null){
				objDepth = depth = 1;
				ReflectField []fields = ReflectHelper.getDeclaredFieldsInHierarchy(clazz);
				for(ReflectField rField : fields){
					
				}
			}
		}*/
		return 0;
	}
}
