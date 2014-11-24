package com.db4o.omplus.datalayer.queryresult;

import java.util.Date;

import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.ModifyObject;
import com.db4o.omplus.datalayer.OMPlusConstants;
import com.db4o.omplus.datalayer.ReflectHelper;
import com.db4o.reflect.ReflectClass;
import com.db4o.reflect.ReflectField;

// TODO doesn't make sense to send values and GenericObject at the same time
// 
public class QueryResultRow {
	
	private int id;
	
	private Object[] values;
	
	private boolean[]	isCellModifiable;	
	
	private	Object	resultObj;
	
//	private SimpleDateFormat sdf = new SimpleDateFormat(OMPlusConstants.DATE_FORMAT);
	
	public Object getValue(int index){
		if(index > -1 && index < values.length)
			return values[index];
		return null;
	}

	public Object getResultObj() {
		return resultObj;
	}

	public void setResultObj(Object resultObj) {
		this.resultObj = resultObj;
	}

	public void setValues(Object[] values) {
		this.values = values;
	}
	


	// date modification? isn't there a better way
	public Object updateValue(String fieldName, Object newValue, QueryResultList list)	{
		ReflectHelper reflectHelper = Activator.getDefault().dbModel().db().reflectHelper();
		Object obj = null, parent = null;
		Object rowObj = resultObj;
		if((newValue != null) && (fieldName != null) ){
			ReflectClass clazz = null;
			String[] hierarchy = fieldName.split(OMPlusConstants.REGEX);
			int length = hierarchy.length;
			obj = rowObj;
			if(length > 1){
				int count = 1;
				while(count < length) {
					if(obj != null) {
						parent = obj;
						clazz = reflectHelper.getReflectClazz(parent);
						obj = ReflectHelper.getFieldValueInHierarchy(hierarchy[count++], clazz, parent);
					}
				}
			}else{
				parent = obj;
				clazz = reflectHelper.getReflectClazz(parent);
				obj = ReflectHelper.getFieldValueInHierarchy(hierarchy[0], clazz, parent);
			}
//			if(obj != null){
			if(obj != null && newValue.equals(obj.toString()))
				return null;
			ReflectField rField = ReflectHelper.getDeclaredFieldInHeirarchy(clazz, hierarchy[length-1]);
			/*Object newVal = new Converter().getPrimitiveValue(rField.getFieldType().getName(), (String)newValue);
			rField.set(parent, newVal);*/
			String type =  rField.getFieldType().getName();
			if(type.equals(Date.class.getName())) {
				if(newValue != null ){ // && ((String)newValue).trim().length() > 0 
					/*rField = ReflectHelper.getReflectClazz(Date.class.getName()).getDeclaredField("fastTime");
					rField.setAccessible();*/
//					try {
//						Date newDate = sdf.parse(newValue.toString());
//						rField.set(parent, newDate);
						rField.set(parent, newValue);
						int index = list.getFieldIndex(fieldName);
						this.values[index] = newValue;
					/*} catch (ParseException e) {
						return null;
					}*/
				}
			}else {
				// FIXME same code as in OMJ-135 - just call field#set() here, too?
				ModifyObject modify = new ModifyObject(reflectHelper);
				if(obj != null){
					modify.updateValue(obj, newValue, type);
				} else {
					Object newObj = modify.createNewValue(newValue, type);
					if(newObj != null)
						rField.set(parent, newObj);
					else
						return null;
				}
			}
//			}
		}
		return parent;
	}

	public boolean getIsCellModifiable(int index) {
		if(index > -1 && index < values.length)
			return isCellModifiable[index];
		return false;
	}
	
	public boolean[] getIsCellModifiable() {
		return isCellModifiable;
	}

	public void setIsCellModifiable(boolean[] isCellModifiable) {
		this.isCellModifiable = isCellModifiable;
	}

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public Object[] getValues() {
		return values;
	}

}
