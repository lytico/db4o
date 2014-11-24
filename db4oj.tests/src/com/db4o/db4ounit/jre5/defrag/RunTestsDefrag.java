/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre5.defrag;

import com.db4o.db4ounit.common.defragment.*;

import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class RunTestsDefrag extends AbstractDb4oDefragTestCase {
	
	@Override
	public Class testSuite() {
		return com.db4o.db4ounit.jre5.AllTestsDb4oUnitJdk5.class;
	}
	
	public static void main(String[] args) {
		Db4oTestSuite suite=new Db4oTestSuite() {
			protected Class[] testCases() {
				return new Class[] {
					RunTestsDefrag.class,
//					InvalidIDExceptionTestCase.class,
				};
			}

			protected Db4oTestSuiteBuilder soloSuite() {
		        return new Db4oTestSuiteBuilder(
	                new Db4oDefragSolo(), testCases());			}
			
		};
		
		System.exit(suite.runSolo());
	}

}
