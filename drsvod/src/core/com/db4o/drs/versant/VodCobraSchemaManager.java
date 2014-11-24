/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant;

import java.lang.reflect.*;

import com.db4o.drs.versant.jdo.reflect.*;
import com.versant.odbms.*;
import com.versant.odbms.model.*;
import com.versant.odbms.model.transcriber.*;

public class VodCobraSchemaManager {

	public static UserSchemaModel buildUserSchemaModel(Class clazz) {
		
		UserSchemaModel userSchemaModel = new UserSchemaModel();
		UserSchemaClass userSchemaClass = new UserSchemaClass(clazz.getName(),
				new UserSchemaClass[0], userSchemaModel);

		Field[] declaredFields = clazz.getDeclaredFields();
		for (Field field : declaredFields) {
			if(field.isSynthetic()){
				continue;
			}
			if (Modifier.isTransient(field.getModifiers())) {
				continue;
			}
			if(JdoMirror.isJdoInjectedField(field.getName())){
				continue;
			}
			int nullFlags = SchemaFieldConstants.NULL_ALLOWED
					| SchemaFieldConstants.NULL_ELEMENTS_ALLOWED;
			if (field.getType() == String.class) {
				nullFlags |= SchemaFieldConstants.USE_DEFAULT_VALUE_AS_NULL;
			}
			UserSchemaField userSchemaField;
			if (field.getType().isArray()) {
				Class arrayType = field.getType().getComponentType();
				UserSchemaClass valueTypeDomain = userSchemaModel
						.getSchemaClass(arrayType.getName());
				userSchemaField = createNewArrayField(userSchemaClass,
						field.getName(), arrayType, valueTypeDomain, nullFlags);

			} else {
				SchemaClass fieldDomain = userSchemaModel.getSchemaClass(field.getType().getName());
				if (fieldDomain == null) {
					userSchemaField = userSchemaClass.newField(field.getName(), field.getType(), nullFlags);
				} else {
					userSchemaField = userSchemaClass.newField(field.getName(),
							fieldDomain);
				}
			}

			if (field.getAnnotation(Indexed.class) != null) {
				
				// System.out.println("Creating index " + clazz.getSimpleName() + "_" + field.getName());
				
				userSchemaClass.newIndex(
						clazz.getSimpleName() + "_" + field.getName(), 
						new UserSchemaField[]{userSchemaField}, 
						SchemaIndexType.BTREE_INDEX);
			}
		}
		
		return userSchemaModel;
	}

	public static void defineSchema(DatastoreManagerFactory dmf, UserSchemaModel userSchemaModel) {
		DatastoreManager dsi = null;
		try {
			dsi = dmf.getDatastoreManager();
			dsi.beginTransaction();
			SchemaEditor editor = dsi.getSchemaEditor();
			editor.define(userSchemaModel, false, SchemaEditor.NO_OPTIONS);
			dsi.commitTransaction();
		} catch (Exception x) {
			throw new RuntimeException(x);
		} finally {
			if (dsi != null && dsi.isTransactionActive()) {
				try {
					dsi.rollbackTransaction();
				} catch (Exception e) {
					// ignore
				}
			}
			if (dsi != null) {
				dsi.close();
			}
		}
	}

	public static UserSchemaField createNewArrayField(
			UserSchemaClass ownerClass, String fieldName, Class<?> elementType,
			SchemaClass elementTypeDomain, int nullity) {

		Class<?> arrayType = null;
		if (elementType != null) {
			arrayType = Array.newInstance(elementType, 0).getClass();
		}
		if (TranscriberAdapterFactory.getSupportedFieldTypes().contains(
				arrayType)) {
			return ownerClass.newField(fieldName, arrayType, nullity);
		} else {
			if (elementTypeDomain == null) {
				elementTypeDomain = SystemSchemaClass.NULL_DOMAIN;
			}
			UserSchemaField result = ownerClass.newDynamicField(fieldName,
					elementTypeDomain, nullity);
			return result;
		}
	}

}
