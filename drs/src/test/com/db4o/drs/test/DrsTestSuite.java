/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com

This file is part of the db4o open source object database.

db4o is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License as published
by the Free Software Foundation and as clarified by db4objects' GPL 
interpretation policy, available at
http://www.db4o.com/about/company/legalpolicies/gplinterpretation/
Alternatively you can write to db4objects, Inc., 1900 S Norfolk Street,
Suite 350, San Mateo, CA 94403, USA.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. */
package com.db4o.drs.test;

import com.db4o.drs.test.regression.*;
import com.db4o.foundation.*;

import db4ounit.*;

public abstract class DrsTestSuite extends ReflectionTestSuite {
	
	public static final boolean RUN_ONE_SINGLE_TEST = false;

	@SuppressWarnings("unchecked")
	protected Class[] testCases() {
		
		if(RUN_ONE_SINGLE_TEST){
			return new Class[]{
			        GenericEnumTestCase.class,
			};
		}
		
		return new Class[] {

		        TheSimplest.class, 
		        
		        GenericEnumTestCase.class,
				
				com.db4o.drs.test.DateReplicationTestCase.class,
				com.db4o.drs.test.foundation.AllTests.class,
				
				// Simple
				ReplicationEventTest.class,
				ReplicationProviderTest.class,
				ReplicationAfterDeletionTest.class,
				SimpleArrayTest.class,
				SimpleParentChild.class,
				ByteArrayTest.class,
				UnqualifiedNamedTestCase.class,
				
				// Collection
				ComplexListTestCase.class,
				ListTest.class, 

				// Complex
				R0to4Runner.class, 	
				ReplicationFeaturesMain.class,

				// General
				CollectionHandlerImplTest.class,
				BidirectionalReplicationTestCase.class,
				TimestampTestCase.class,
		
				MapTest.class,
				ArrayReplicationTest.class,
				SingleTypeCollectionReplicationTest.class,
				MixedTypesCollectionReplicationTest.class,
				TransparentActivationTestCase.class,
                
                //regression
                DRS42Test.class,
                
                SameHashCodeTestCase.class,
		};
	}
	
	@SuppressWarnings("unchecked")
	protected Class[] concat(Class[] x, Class[] y) {
		final Collection4 c = new Collection4(x);
		c.addAll(y);
		return (Class[]) c.toArray(new Class[c.size()]);
	}
}
