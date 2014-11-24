/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import java.util.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.db4ounit.common.api.*;
import com.db4o.db4ounit.common.assorted.AliasesQueryingTestCase.*;

import db4ounit.*;
import db4ounit.extensions.fixtures.*;
import db4ounit.extensions.util.*;

public class RenamingClassAfterQueryingTestCase extends TestWithTempFile implements OptOutSilverlight{
	

    public void testNoQueryBeforeRenaming()
    {
        createData();
        queryDb(renameConfig(), A.class, 0);
        queryDb(renameConfig(), B.class, 1);

    }

    public void testQueryBeforeRenaming()
    {
        createData();
        queryDb(Db4oEmbedded.newConfiguration(), A.class, 1);
        queryDb(Db4oEmbedded.newConfiguration(), B.class, 0);

        queryDb(renameConfig(),A.class,  0);
        queryDb(renameConfig(), B.class, 1);
     
    }
  
    private void createData() {
    	EmbeddedObjectContainer database = Db4oEmbedded.openFile(tempFile());
        database.store(new A("Item1"));
        database.commit();
        database.close();
    }


    public <T> void  queryDb(EmbeddedConfiguration config, Class<T> clazz, int count) {
    	EmbeddedObjectContainer database = Db4oEmbedded.openFile(config, tempFile());
    	try{
	        List<T> list = database.query(clazz);
	        Assert.areEqual(count, list.size());
    	} finally {
    		database.close();
    	}
   }
   

    private EmbeddedConfiguration renameConfig(){
       EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
       configuration.common().objectClass(A.class).rename(CrossPlatformServices.fullyQualifiedName(B.class));
       return configuration;
   }


    public static class A
    {
        private String _name;

        public A(String name)
        {
            _name = name;
        }

        public String getName()
        {
            return _name;
        }
        
        public void setName(String name){
        	_name = name;
        }

        public String toString()
        {
            return "Name: " + _name + " Type: " + getClass().getName();
        }
    }

    public static class B
    {
        private String _name;

        public String getName(){
            return _name;
        }
        
        public void setName(String name){
        	_name = name;
        }

        public String toString()
        {
            return "Name: " + _name + " Type: " + getClass().getName();
        }
    }

}
