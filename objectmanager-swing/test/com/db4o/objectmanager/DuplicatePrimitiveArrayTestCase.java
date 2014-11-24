package com.db4o.objectmanager;

import db4ounit.extensions.AbstractDb4oTestCase;
import db4ounit.Assert;
import com.db4o.ext.StoredField;
import com.db4o.ext.StoredClass;
import com.db4o.ObjectContainer;
import com.db4o.objectmanager.api.impl.DatabaseInspectorImpl;
import com.db4o.objectmanager.api.DatabaseInspector;
import com.db4o.reflect.ReflectClass;
import com.db4o.reflect.ReflectField;
import com.spaceprogram.db4o.sql.util.ReflectHelper;

import javax.swing.tree.DefaultMutableTreeNode;
import java.util.List;

import objectmanager.test.Data;

/**
 * User: treeder
 * Date: Mar 6, 2007
 * Time: 12:44:06 PM
 */
public class DuplicatePrimitiveArrayTestCase extends AbstractDb4oTestCase {

	public static void main(String[] args) {
		new DuplicatePrimitiveArrayTestCase().runSolo();
	}

	protected void store() throws Exception {
		store(new Data(new boolean[]{true, false}));
	}

	/**
	 * For: http://tracker.db4o.com/browse/OMR-70
	 */
	public void testDuplicateArray() {
		StoredClass sc = db().storedClass(Data.class);
		for(StoredField field : sc.getStoredFields()) {
			System.out.println(field.getName() + " : " + field.getStoredType());
		}
		Assert.areEqual(1, sc.getStoredFields().length);

		List<ReflectClass> classesStored = inspector().getClassesStored();
		Assert.areEqual(1, classesStored.size());
		for(int i = 0; i < classesStored.size(); i++) {
			ReflectClass storedClass = classesStored.get(i);
			if(storedClass.getName().equals(Data.class.getName())) {
				ReflectField[] fields = ReflectHelper.getDeclaredFieldsInHeirarchy(storedClass);
				Assert.areEqual(1, fields.length);
			}
		}
	}

	private DatabaseInspector inspector() {
		return new DatabaseInspectorImpl(db());
	}

}
