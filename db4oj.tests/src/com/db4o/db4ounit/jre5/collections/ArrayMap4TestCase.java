/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre5.collections;

import com.db4o.collections.*;
import com.db4o.db4ounit.common.ta.*;
import com.db4o.internal.*;

import db4ounit.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class ArrayMap4TestCase implements TestLifeCycle {

    private ArrayMap4<String, Integer> map;

    public static void main(String[] args) {
        new ConsoleTestRunner(ArrayMap4TestCase.class).run();
    }

    public void setUp() throws Exception {
        map = new ArrayMap4<String, Integer>();
        ArrayMap4Asserter.putData(map);
    }

    public void tearDown() throws Exception {
        map.clear();
    }
    
    public void testCloneWontCopyActivator() throws Exception {
    	map.bind(new MockActivator());
		
		final Object clone = map.clone();
		Assert.isNull(Reflection4.getFieldValue(clone, "_activator"));
    }

	// Function Test
    public void testConstructor() {
        ArrayMap4<String, Integer> m = new ArrayMap4<String, Integer>();
        ArrayMap4Asserter.assertInitalStatus(m);
    }

    public void testClear() {
        ArrayMap4Asserter.assertClear(map);
    }

    public void testClone() {
        ArrayMap4Asserter.assertClone(map);
    }

    public void testContainsKey() {
        ArrayMap4Asserter.assertContainsKey(map);
    }

    public void testContainsValue() {
        ArrayMap4Asserter.assertContainsValue(map);
    }

    public void testEntrySet() {
        ArrayMap4Asserter.assertEntrySet(map);
    }

    public void testGet() {
        ArrayMap4Asserter.assertGet(map);
    }

    public void testIsEmpty() {
        ArrayMap4Asserter.assertIsEmpty(map);
    }

    public void testKeySet() {
        ArrayMap4Asserter.assertKeySet(map);
    }

    public void testPut() {
        ArrayMap4Asserter.assertPut(map);
    }

    public void testPutAll() {
        ArrayMap4Asserter.assertPutAll(map);
    }

    public void testRemove() {
        ArrayMap4<String, String> m = new ArrayMap4<String, String>();
        m.put("one", "yi");
        m.put("two", "er");
        Assert.areEqual(2, m.size());
        String value = m.remove("one");
        Assert.areEqual("yi", value);
        Assert.areEqual(1, m.size());
        Assert.isNull(m.get("one"));

        value = m.remove("three");
        Assert.isNull(value);
        Assert.areEqual(1, m.size());

        value = m.remove("two");
        Assert.areEqual("er", value);
        Assert.areEqual(0, m.size());
        Assert.isNull(m.get("two"));

        m.put("three", "san");
        Assert.areEqual(1, m.size());

    }
    
    public void testRemove_FromHead() {
        ArrayMap4Asserter.assertRemove_FromHead(map);
    }

    public void testRemove_FromEnd() {
        ArrayMap4Asserter.assertRemove_FromEnd(map);
    }

    public void testRemove_FromMiddle() {
        ArrayMap4Asserter.assertRemove_FromMiddle(map);
    }

    public void testSize() {
        ArrayMap4Asserter.assertSize(map);
    }

    public void testValues() {
        ArrayMap4Asserter.assertValues(map);
    }

    public void testEquals() {
        ArrayMap4Asserter.assertEquals(map);
    }
    
    public void testIncreaseSize() {
        ArrayMap4Asserter.assertIncreaseSize(map);
    }
}
