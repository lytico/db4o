package com.db4o.omplus.datalayer;

import java.util.*;

import com.db4o.ext.*;
import com.db4o.omplus.datalayer.queryBuilder.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;

public class ReflectHelper {
	 
	private final Reflector reflector;
	
	public ReflectHelper(Reflector reflector) {
		this.reflector = reflector;
	}

	public static boolean isWrapperClass(String name) {
		if( name.equals(String.class.getName())
				|| name.equals(Integer.class.getName())
				|| name.equals(Double.class.getName())
				|| name.equals(Long.class.getName())
				|| name.equals(Float.class.getName())
				|| name.equals(Short.class.getName())
				|| name.equals(Byte.class.getName())
				|| name.equals(Character.class.getName())
				|| name.equals(Date.class.getName())
				||name.equals(Boolean.class.getName()))
			return true;
		else
			return false;
	}
	
	public int getArraySize(Object obj){
		return reflector.array().getLength(obj);
	}
	
	public int getFieldTypeClass(String fieldname){
		String []hierarchy = ((String)fieldname).split(OMPlusConstants.REGEX);
		int length = hierarchy.length;
		ReflectClass clazz = reflector.forName(hierarchy[0]);
		if(length > 1)
		{
			clazz =  getReflectCass(hierarchy, clazz);
		}
		String type = clazz.getName();
		Converter converter = new Converter();
		int fieldType = converter.getType(type);
		return getFieldType(fieldType);
		
	}
		
	public static int getFieldType(int fieldType) {
		switch(fieldType)
		{
		case QueryBuilderConstants.DATATYPE_STRING:
			return QueryBuilderConstants.DATATYPE_STRING;
		case QueryBuilderConstants.DATATYPE_CHARACTER:
			return QueryBuilderConstants.DATATYPE_CHARACTER;
			
		case QueryBuilderConstants.DATATYPE_BYTE:
		case QueryBuilderConstants.DATATYPE_SHORT:
		case QueryBuilderConstants.DATATYPE_INT:
		case QueryBuilderConstants.DATATYPE_LONG:
		case QueryBuilderConstants.DATATYPE_FLOAT:
		case QueryBuilderConstants.DATATYPE_DOUBLE:
			return QueryBuilderConstants.DATATYPE_NUMBER;
			
		case QueryBuilderConstants.DATATYPE_BOOLEAN:
			return QueryBuilderConstants.DATATYPE_BOOLEAN;
		case QueryBuilderConstants.DATATYPE_DATE_TIME:
			return QueryBuilderConstants.DATATYPE_DATE_TIME;
		}
		return -1;
	}

	public int getNumberType(String fieldname)
	{
		String []hierarchy = ((String)fieldname).split(OMPlusConstants.REGEX);
		int length = hierarchy.length;
		ReflectClass clazz = reflector.forName(hierarchy[0]);
		if(length > 1){
			clazz =  getReflectCass(hierarchy, clazz);
		}
		String type = clazz.getName();
		Converter converter = new Converter();
		int fieldType = converter.getType(type);
		return fieldType;				
	}
	
	private static ReflectClass getReflectCass(String[] hierarchy, ReflectClass clazz) {
		int count = 1;
		ReflectClass rClazz = clazz;
		while(count < hierarchy.length){
			rClazz = getDeclaredFieldInHeirarchy(rClazz, hierarchy[count++]).getFieldType();
		}
		return rClazz;
	}
	
	public ReflectClass getReflectClazz(String className){
		return reflector.forName(className);
	}
	
	public ReflectClass getReflectClazz(Object obj){
		return reflector.forObject(obj);
	}	
	
	public static ReflectField getReflectField(ReflectClass clz, String fieldName) {
		return clz.getDeclaredField(fieldName);
	}
	
	public static ReflectField getDeclaredFieldInHeirarchy(ReflectClass reflectClass, String field) {
		ReflectField rf = reflectClass.getDeclaredField(field);
		if(rf == null) {
			ReflectClass parent = reflectClass.getSuperclass();
			if(parent != null)
				return getDeclaredFieldInHeirarchy(parent, field);
		}
		return rf;
	}
	
	public static ReflectField[] getDeclaredFieldsInHierarchy(ReflectClass clazz) {
		List<ReflectField>  list = getDeclaredFieldsListInHeirarchy(clazz);
		return (ReflectField[])list.toArray(new ReflectField[list.size()]);
	}
	
	 private static List<ReflectField> getDeclaredFieldsListInHeirarchy(ReflectClass reflectclass) {
		 List<ReflectField> list = getDeclaredFieldsList(reflectclass);
	        ReflectClass reflectclass1 = reflectclass.getSuperclass();
	        if(reflectclass1 != null)
	            list.addAll(getDeclaredFieldsListInHeirarchy(reflectclass1));
	        return list;
	}

	private static List<ReflectField> getDeclaredFieldsList(ReflectClass reflectclass) {
		ArrayList<ReflectField> arrayList = new ArrayList<ReflectField>();
		ReflectField fields[] = reflectclass.getDeclaredFields();
		for(int i = 0; i < fields.length; i++) {
			ReflectField reflectField = fields[i];
			if(!(reflectField instanceof GenericVirtualField))
				arrayList.add(reflectField);
		}
	    return arrayList;
	}
	
    public static StoredField[] getDeclaredFieldsInHeirarchy(StoredClass clazz) {
        List<StoredField> ret = getDeclaredFieldsListInHeirarchy(clazz);
        return (StoredField[])ret.toArray(new StoredField[ret.size()]);
    }

    private static List<StoredField> getDeclaredFieldsListInHeirarchy(StoredClass aClass) {
        if(aClass == null)
            return null;
        List<StoredField> ret = getDeclaredFieldsList(aClass);
        StoredClass parent = aClass.getParentStoredClass();
        if(parent != null)
            ret.addAll(getDeclaredFieldsListInHeirarchy(parent));
        return ret;
    }

    public static List<StoredField> getDeclaredFieldsList(StoredClass aClass) {
        List<StoredField> ret = new ArrayList<StoredField>();
        StoredField fields[] = aClass.getStoredFields();
        for(int i = 0; i < fields.length; i++)
        {
            StoredField field = fields[i];
            if(!(field instanceof GenericVirtualField))
                ret.add(field);
        }

        return ret;
    }

/*	public static Object getFieldValue(String fieldName, Object rowObj) {
		Object obj = null;
		if(fieldName != null && fieldName.trim().length() > 0 &&
				rowObj != null){
			Reflector reflector = DbInterface.getInstance().reflector();
			ReflectClass clazz = reflector.forObject(rowObj);
			String []hierarchy = fieldName.split(OMPlusConstants.REGEX);
			int length = hierarchy.length;
			if(length == 1){
				return getFieldValueInHierarchy(hierarchy[0], clazz, rowObj);
			}
			int count = 1;
			do{
				obj = getFieldValueInHierarchy(hierarchy[count], clazz, obj);
				if(obj == null){
					break;
				}
			}while(count < length);
		}
		return obj;
	}*/

	public static Object getFieldValueInHierarchy(String field, ReflectClass clazz, Object rowObj) {
		ReflectField rField = getDeclaredFieldInHeirarchy(clazz, field);
		if(rField != null)
			return rField.get(rowObj);
		return null;
	}

	public boolean checkForHierarchy(Object obj) {
		if(obj != null) {
			ReflectClass clazz = getReflectClazz(obj);
			if(clazz != null){
				ReflectField [] fields =getDeclaredFieldsInHierarchy(clazz);
				for( ReflectField rField : fields) {
					Object value = rField.get(obj);
					if(value != null){
						ReflectClass type = rField.getFieldType();
						String name = type.getName();
						if(type.isPrimitive() || isWrapperClass(name))
							continue;
						else if(type.isArray()) {
							int length = reflector.array().getLength(value);
							for(int i = 0; i < length; i++) {
								Object indexObj = reflector.array().get(value, i);
								if( indexObj instanceof GenericObject){
									return true;
								}
							}
						} else if(type.isCollection()) {
							if( value instanceof Map){
								Map map = (Map)value;
								Iterator iterator = map.keySet().iterator(); 
								while (iterator.hasNext()) {
									Object key = iterator.next();
									Object valueObj = map.get(key);
									if( valueObj instanceof GenericObject)
										return true;
								}
							}else {
								Collection collection = (Collection)value;
								Iterator iterator = collection.iterator();
								while(iterator.hasNext()) {
									Object colObj = iterator.next();
									if(colObj instanceof GenericObject)
										return true;
								}
							}
						} else 
							return true;

					}
				}// end for loop
			}
		}
		return false;
	}

	
}
