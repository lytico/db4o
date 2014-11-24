/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.ext.*;

public class StoredClassInformation {
	
	static final int COUNT = 10;
	
	public String name;
	
	public void test(){

		Test.deleteAllInstances(this);
		name = "hi";
		Test.store(this);
		for(int i = 0; i < COUNT; i ++){
			Test.store(new StoredClassInformation());
		}
		
		StoredClass[] storedClasses = Test.objectContainer().ext().storedClasses();
		StoredClass myClass = Test.objectContainer().ext().storedClass(this);
		
		boolean found = false;
		for (int i = 0; i < storedClasses.length; i++) {
            if(storedClasses[i].getName().equals(myClass.getName())){
            	found = true;
            	break;
            }
        }
        
        Test.ensure(found);
        
        long id = Test.objectContainer().getID(this);
        
        long ids[] = myClass.getIDs();
        Test.ensure(ids.length == COUNT + 1);
        
        found = false;
        for (int i = 0; i < ids.length; i++) {
            if (ids[i] == id){
            	found = true;
            	break;
            }
        }
        
        Test.ensure(found);
        
	}
	
}
