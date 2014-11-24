/* Copyright (C) 2004 - 2005  Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.test.nativequery.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class AllTestsJdk5 extends AllTestsJdk1_2{
	
    public static void main(String[] args) {
    	if(args!=null&&args.length==1&&args[0].equals("-withExceptions")) {
            new AllTestsJdk5().runWithException();
            return;
    	}
        new AllTestsJdk5().run();
    }
    
    @Override
    protected void addTestSuites(TestSuite suites) {
    	super.addTestSuites(suites);
    	suites.add(new Jdk5TestSuite());
        if(Db4oVersion.MAJOR >= 5){
            suites.add(new NativeQueryTestSuite());
        }
	}

}
