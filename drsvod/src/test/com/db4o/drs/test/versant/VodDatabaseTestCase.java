/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant;


import java.util.*;

import javax.jdo.*;

import com.db4o.drs.versant.*;
import com.versant.odbms.*;
import com.versant.odbms.model.*;

import db4ounit.*;

public class VodDatabaseTestCase extends VodDatabaseTestCaseBase {
	
	public void testCreateDatastoreManager(){
		DatastoreManager dm = _vod.createDatastoreManager();
		dm.close();
	}
	
	public void testPersistenceManagerFactory(){
		PersistenceManager pmf = _vod.persistenceManagerFactory().getPersistenceManager();
		Assert.isFalse(pmf.isClosed());
		pmf.close();
	}
	
	public void testSchema(){
		DatastoreManager dm = _vod.createDatastoreManager();
		DatastoreInfo info = dm.getPrimaryDatastoreInfo();
		SchemaEditor editor = dm.getSchemaEditor();
		long[] classLoids = dm.locateAllClasses(info, false);
		for (int i = 0; i < classLoids.length; i++) {
			DatastoreSchemaClass dc = editor.findClass(classLoids[i], info);
			System.out.println(dc.getName());
		}
		dm.close();
	}
	
    public void testNameFromPersistenceManagerFactory(){
        PersistenceManagerFactory persistenceManagerFactory = _vod.persistenceManagerFactory();
        VodDatabase database = new VodDatabase(persistenceManagerFactory);
        Assert.areEqual(_vod.name(),database.name());
    }

    public void testConfigurationWithPersisteneManagerFactoryConstructor(){
        PersistenceManagerFactory oldPersistence = _vod.persistenceManagerFactory();
        VodDatabase database = new VodDatabase(oldPersistence);
        PersistenceManagerFactory newPersistence = database.persistenceManagerFactory();
        assertSameProperties(oldPersistence, newPersistence);
    }

    private void assertSameProperties(PersistenceManagerFactory oldPersistence, PersistenceManagerFactory newPersistence) {
        Assert.areEqual(oldPersistence.getProperties().size(), newPersistence.getProperties().size());
        for (Map.Entry<Object, Object> configItem : oldPersistence.getProperties().entrySet()) {
            String propertyValue = newPersistence.getProperties().getProperty(configItem.getKey().toString());
            Assert.areEqual(configItem.getValue(),propertyValue);
        }
    }

    private Properties properties(){
        return new Properties();
    }


}
