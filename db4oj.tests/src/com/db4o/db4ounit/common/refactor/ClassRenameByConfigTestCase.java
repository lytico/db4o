/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.refactor;

import com.db4o.config.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;
import db4ounit.extensions.util.*;


public class ClassRenameByConfigTestCase extends AbstractDb4oTestCase implements OptOutDefragSolo {

	public static void main(String[] args) {
		new ClassRenameByConfigTestCase().runNetworking();
	}
	
	
    public static class Original {
    	
        public String originalName;

        public Original() {

        }

        public Original(String name) {
            originalName = name;
        }
    }

    public static class Changed {

        public String changedName;

    }

	
	public void test() throws Exception{
		
		store(new Original("original"));
		
		db().commit();
		
		Assert.areEqual(1, countOccurences(Original.class));
		
        // Rename messages are visible at level 1
        // fixture().config().messageLevel(1);
		
        ObjectClass oc = fixture().config().objectClass(Original.class);

        // allways rename fields first
        oc.objectField("originalName").rename("changedName");
        // we must use ReflectPlatform here as the string must include
        // the assembly name in .net
        oc.rename(CrossPlatformServices.fullyQualifiedName(Changed.class));

        reopen();
        
        Assert.areEqual(0, countOccurences(Original.class));
        Assert.areEqual(1, countOccurences(Changed.class));
        
        Changed changed = (Changed) retrieveOnlyInstance(Changed.class);
        
        Assert.areEqual("original", changed.changedName);

	}

}
