/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.internal;

import java.util.*;

import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.delete.*;
import com.db4o.marshall.*;
import com.db4o.typehandlers.*;

import db4ounit.*;
import db4ounit.extensions.*;


public class ClassMetadataTypeHandlerIntegrationTestCase extends AbstractDb4oTestCase {
    
    public static class Item {
    }
    
    public static class MyReferenceType {
    }

    public static class MyReferenceTypeHandler implements ReferenceTypeHandler {

		public void activate(ReferenceActivationContext context) {
		}

		public void defragment(DefragmentContext context) {
		}

		public void delete(DeleteContext context) throws Db4oIOException {
		}

		public void write(WriteContext context, Object obj) {
		}
    }
    
    protected void configure(Configuration config) throws Exception {
        config.registerTypeHandler(
            new SingleClassTypeHandlerPredicate(MyReferenceType.class), 
            new MyReferenceTypeHandler());
    }
    
    protected void store() throws Exception {
        store(new Item());
        store(new MyReferenceType());
    }
    
    public void testIsValueType(){
        for (Pair<Object, Boolean> typeDescriptor : typeDescriptors()) { 
            ClassMetadata classMetadata = container().classMetadataForObject(typeDescriptor.first);
            Assert.areEqual(typeDescriptor.second.booleanValue(), classMetadata.isValueType(), classMetadata.toString());
        }
    }

	private Pair<Object, Boolean>[] typeDescriptors() {
		 return new Pair[] {
			 pair(new Integer(1), true),
			 pair(new Date(), true),
			 pair("astring", true),
			 pair(new Item(), false),
			 pair((new int[] {1}), false),
			 pair((new Date[] {new Date()}), false),
			 pair((new Item[] {new Item()}), false),
			 pair(new MyReferenceType(), false),
		 };
	}
	
	private <TFirst, TSecond> Pair<TFirst, TSecond> pair(TFirst first, TSecond second) {
		return new Pair<TFirst, TSecond>(first, second);
	}

}
