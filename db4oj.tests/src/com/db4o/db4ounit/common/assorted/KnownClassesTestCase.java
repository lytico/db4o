/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.reflect.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class KnownClassesTestCase extends AbstractDb4oTestCase {
	
	public static void main(String[] args) {
	    new KnownClassesTestCase().runAll();
    }
    
    public static final Class[] INTERNAL_CLASSES = new Class[] {
        Db4oDatabase.class,
        StaticClass.class,
    };
    
    public static class Item {
    }
    
    @Override
    protected void store() {
    	Assert.isFalse(isKnownClass(Item.class));
    	store(new Item());
    	Assert.isTrue(isKnownClass(Item.class));
    }
    
    public void testNoPrimitives() {
    	for (ReflectClass knownClass : container().knownClasses()) {
			Assert.isFalse(knownClass.isPrimitive(), knownClass.getName());
    	}
    }

    /**
     * @sharpen.remove.first
     */
    @decaf.Ignore(decaf.Platform.JDK11)
    public void testValueTypes() {
    	if (Platform4.jdk().ver() == 2)
    		return;
    	
    	container().reflector().forName(typeName());
    	boolean found = false;
    	for (ReflectClass knownClass : container().knownClasses()) {
    		if (knownClass.getName().equals(typeName())){
    			found = true;
    		}
    	}
    	
    	Assert.isTrue(found);
    }
    
    /**
     * @sharpen.remove "System.Guid, mscorlib"
     */
	private String typeName() {
		return java.math.BigDecimal.class.getName();		
	}

	public void testInternalClassesAreNotVisible() {
    	for (ReflectClass knownClass : container().knownClasses()) {
	        assertIsNotInternal(knownClass.getName());
		}
    }
	
	public void testNewClassIsFound() {
		Assert.isTrue(isKnownClass(Item.class));
	}

	private boolean isKnownClass(final Class<?> klass) {
	    return isKnownClass(ReflectPlatform.fullyQualifiedName(klass));
    }

	private boolean isKnownClass(final String expected) {
		for (ReflectClass knownClass : container().knownClasses()) {
	        final String className = knownClass.getName();
			if(className.equals(expected)){
                return true;
            }
		}
		return false;
    }

	private void assertIsNotInternal(final String className) {
	    for (int j = 0; j < INTERNAL_CLASSES.length; j++) {
	        Assert.areNotEqual(INTERNAL_CLASSES[j].getName(), className);
	    }
    }

}
