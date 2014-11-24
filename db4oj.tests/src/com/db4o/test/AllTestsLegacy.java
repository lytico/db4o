/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.test.legacy.*;
import com.db4o.test.legacy.soda.*;

/**
 * This suite contains all the old style regression tests that have been converted to
 * db4ounit. It should serve as a safety net against test bugs introduced during
 * conversion.
 */
public class AllTestsLegacy extends AllTests {

    public static void main(String[] args) {
        new AllTestsLegacy(new String[]{}).runWithException();
    }
    
    public AllTestsLegacy(String[] testcasenames) {
    	super(testcasenames);
    }
    protected void addTestSuites(TestSuite suites) {
        suites.add(this);
	}
    
    public Class[] tests(){
    	return new Class[] {
    		ArrayNOrder.class,
    		Book.class,
    		ByteArray.class,
    		CascadeToHashtable.class,
    		CreateIndex.class,
    		GetByUUID.class,
    		MultiDelete.class,
    		NestedArrays.class,
    		PersistStaticFieldValues.class,
    		SimpleTypeArrayInUntypedVariable.class,
	    	Soda.class,
	    	SodaNumberCoercion.class,
    		TypedArrayInObject.class,
    		TypedDerivedArray.class,
        };
    }


}