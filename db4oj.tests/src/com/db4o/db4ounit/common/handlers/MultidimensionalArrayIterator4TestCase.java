/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.handlers;

import com.db4o.foundation.*;
import com.db4o.internal.handlers.array.*;
import com.db4o.reflect.*;

import db4ounit.*;
import db4ounit.extensions.*;


/**
 * @exclude
 */
public class MultidimensionalArrayIterator4TestCase extends AbstractDb4oTestCase {
    
    public void testEmptyArray() {
        assertExhausted(iterate(new Object[0])); 
    }
    
    public void testStringArray() {
        Iterator4 i = iterate(new Object[] { new Object[]{"foo", "bar"}, new Object[] {"fly"} });
        Assert.isTrue(i.moveNext());
        Assert.areEqual("foo", i.current());
        
        Assert.isTrue(i.moveNext());
        Assert.areEqual("bar", i.current());
        
        Assert.isTrue(i.moveNext());
        Assert.areEqual("fly", i.current());
        
        assertExhausted(i);
    }
    
    public void testIntArray() {
        Iterator4 i = iterate(new int[][] { new int[]{1, 2}, new int[] {3} });
        Assert.isTrue(i.moveNext());
        Assert.areEqual(new Integer(1), i.current());
        
        Assert.isTrue(i.moveNext());
        Assert.areEqual(new Integer(2), i.current());
        
        Assert.isTrue(i.moveNext());
        Assert.areEqual(new Integer(3), i.current());
        
        assertExhausted(i);
    }

    
    private void assertExhausted(final Iterator4 i) {
        Assert.isFalse(i.moveNext());       
        Assert.expect(ArrayIndexOutOfBoundsException.class, new CodeBlock(){
            public void run() throws Throwable {
                System.out.println(i.current());
            }
        });
    }
    
    private Iterator4 iterate(Object[] array){
        return new MultidimensionalArrayIterator(reflectArray(), array);
    }
    
    private ReflectArray reflectArray(){
        return reflector().array();
    }


}
