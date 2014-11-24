/* Copyright (C) 2004 - 2005  Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.test.legacy.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class AllTestsJdk1_2Legacy extends AllTestsLegacy {
	
    public static void main(String[] args) {
        new AllTestsJdk1_2Legacy(new String[]{}).runWithException();
    }
    
    public AllTestsJdk1_2Legacy(String[] testcasenames) {
    	super(testcasenames);
    }
    
    protected void addTestSuites(TestSuite suites) {
    	super.addTestSuites(suites);
    	suites.add(new TestSuite() {
			public Class[] tests() {
				return new Class[]{
					ArrayListInHashMap.class,
					CascadeToHashMap.class,
				   
				    ExtendsHashMap.class,
				    ExternalBlobs.class,
		            KeepCollectionContent.class,

		            TransientClone.class,
				    TreeSetCustomComparable.class,
				};
			}
    	});
	}

}
