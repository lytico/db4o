package com.db4o.objectmanager.api.impl;

import com.db4o.*;
import com.db4o.ext.StoredClass;
import com.db4o.ext.StoredField;
import com.db4o.objectmanager.api.DatabaseInspector;
import com.db4o.objectmanager.api.helpers.ReflectHelper2;

import java.util.List;

/**
 * User: treeder
 * Date: Aug 9, 2006
 * Time: 11:49:54 AM
 */
public class DatabaseInspectorImpl implements DatabaseInspector {
	
    private ObjectContainer oc;
    
    private final InsideDb4o inside;

    public DatabaseInspectorImpl(ObjectContainer oc) {
        this.oc = oc;
        inside = new InsideDb4o(oc);
    }

    public int getNumberOfClasses() {
        return getClassesStored().size();
    }

    public List getClassesStored() {
        return ReflectHelper2.getUserStoredClasses(oc);
    }

    /*public int getNumberOfObjectsForClass(Class aClass) {
        List results = oc.query(aClass);
        return results.size();
    }*/

    public int getNumberOfObjectsForClass(String aClass) {
    	return inside.getNumberOfObjectsForClass(aClass);
    }

    public long getSpaceFree() {
        try {
            return 0;
            // todo: return oc.ext().systemInfo().freespaceSize();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return 0;
    }

    public long getSpaceLost() {
        return 0;
    }

    public int getNumberOfIndexes() {
        int counter = 0;
        List classes = getClassesStored();
        for (int i = 0; i < classes.size(); i++) {
            StoredClass storedClass = (StoredClass) classes.get(i);
            StoredField[] storedFields = storedClass.getStoredFields();
            for (int j = 0; j < storedFields.length; j++) {
                StoredField storedField = storedFields[j];
                if(storedField.hasIndex()){
                    counter++;
                }
            }
        }
        return counter;
    }

    public List getIndexStats() {
        // Waiting for YapField.getIndex() to have an Index interface so it doesn't return BTree class
        return null;
    }

    public List getReplicationRecords() {
        return null;
    }

    public long getSize() {
        try {
            return oc.ext().systemInfo().totalSize();
        } catch (Exception e) {
			System.err.println("Could not get database size!");
			//e.printStackTrace();
        }
        return 0;
    }

    public long getSpaceUsedByClass(String className) {
        return 0;
    }

    public long getSpaceUsedByClassIndexes(String className) {
        return 0;
    }
}
