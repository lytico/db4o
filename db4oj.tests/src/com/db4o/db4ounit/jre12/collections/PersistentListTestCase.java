/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections;

import java.util.*;

import db4ounit.*;
import db4ounit.extensions.*;


/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class PersistentListTestCase extends AbstractDb4oTestCase {
    
    public static void main(String[] args) {
        new PersistentListTestCase().runSolo();
    }
    
    private final List[] lists = new List[]{
        new MockPersistentList(),
    };
    
    private Vector testData(){
        Vector vector = new Vector();
        vector.add("zero");
        vector.add("one");
        vector.add("two");
        return vector;
    }
    
    public void testAdd(){
        for (int i = 0; i < lists.length; i++) {
            List list = lists[i];
            Vector data = testData();
            addData(list, data);
            assertAreEqual(data, list);
            data.add("one");
            list.add("one");
            assertAreEqual(data, list);
        }
    }

    public void testClear(){
        for (int i = 0; i < lists.length; i++) {
            List list = lists[i];
            list.clear();
            Assert.areEqual(0, list.size());
            addData(list, testData());
            list.clear();
            Assert.areEqual(0, list.size());
        }
    }
    
    public void testRemove(){
        for (int i = 0; i < lists.length; i++) {
            List list = lists[i];
            Vector data = testData();
            addData(list, data);
            
            Object item = data.get(1);
            list.remove(item);
            data.remove(item);
            assertAreEqual(data, list);
            
            for (int j = 0; j < data.size(); j++) {
                item = data.get(0);
                list.remove(item);
                data.remove(item);
            }
            assertAreEqual(data, list);
        }
    }
    
    public void testSet(){
        for (int i = 0; i < lists.length; i++) {
            List list = lists[i];
            Vector data = testData();
            addData(list, data);
            
            int size = data.size();
            for (int j = 0; j < size; j++) {
                data.set(j, new Integer(j));
                list.set(j, new Integer(j));
            }
            
            assertAreEqual(data, list);
        }
    }
    
    public void testAddAtIndex(){
        for (int i = 0; i < lists.length; i++) {
            List list = lists[i];
            Vector data = testData();
            addData(list, data);
            
            int size = data.size();
            for (int j = 0; j < size; j++) {
                data.add(j + 2, new Integer(j));
                list.add(j + 2, new Integer(j));
            }
            
            assertAreEqual(data, list);
        }
    }
    
    public void testRemoveAtIndex(){
        for (int i = 0; i < lists.length; i++) {
            List list = lists[i];
            
            Vector data = testData();
            addData(list, data);
            
            int size = data.size();
            for (int j = 0; j < size; j++) {
                data.remove(0);
                list.remove(0);
            }
            assertAreEqual(data, list);
            
            
            data = testData();
            addData(list, data);
            
            for (int j = 0; j < size; j++) {
                int pos = data.size() - 1;
                data.remove(pos);
                list.remove(pos);
            }
            assertAreEqual(data, list);

            data = testData();
            addData(list, data);
            
            for (int j = 0; j < size - 2; j++) {
                data.remove(1);
                list.remove(1);
            }
            assertAreEqual(data, list);
            
        }
    }
    
    private void addData(List list, Vector data) {
        for (int j = 0; j < data.size(); j++) {
            list.add(data.get(j));
        }
    }
    
    private void assertAreEqual(Vector vector, List list){
        Assert.areEqual(vector.size(), list.size());
        ArrayAssert.areEqual(vector.toArray(), list.toArray());
    }
    
}
