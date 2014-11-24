package com.db4o.omplus.datalayer.propertyViewer;

import com.db4o.ext.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.datalayer.propertyViewer.classProperties.*;
import com.db4o.reflect.*;

public class ConfigureIndex {
	
	DatabaseModel dbModel;
	
	public ConfigureIndex(DatabaseModel dbModel) {
		this.dbModel = dbModel;
	}
	
	public void createIndex(ClassProperties clsProperties){
		ReflectClass clazz = dbModel.db().reflectHelper().getReflectClazz(clsProperties.getClassname());
		StoredClass storedClz = dbModel.db().getStoredClass(clazz.getName());
		if(storedClz != null) {
			for(FieldProperties field : clsProperties.getFields()) {
				StoredField sField = storedClz.storedField(field.getFieldName(), storedClz);
				if(sField.hasIndex() != field.isIndexed()) {
					if(field.isIndexed()) {
						sField.createIndex();
					}
					else {
						sField.dropIndex();
					}
				}
			}
		}
	}
	
}
