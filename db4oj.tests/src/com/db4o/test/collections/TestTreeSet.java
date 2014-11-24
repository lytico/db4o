/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.collections;

import java.util.*;

import com.db4o.test.*;



/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class TestTreeSet {
    
    private static final String[] CONTENT = new String[]{
        "a","f","d","c","b"
    };
    
    SortedSet stringTreeSet;
    
    SortedSet objectTreeSet;
    
    
    public void storeOne(){
        stringTreeSet = new TreeSet();
        stringContentTo(stringTreeSet);
        
        objectTreeSet = new TreeSet();
        objectContentTo(objectTreeSet);
    }
    
    
    public void testOne(){
        
        TreeSet stringCompareTo = new TreeSet();
        stringContentTo(stringCompareTo);
        
        TreeSet objectCompareTo = new TreeSet();
        objectContentTo(objectCompareTo);
        
        Test.ensure(stringTreeSet instanceof TreeSet);
        Test.ensure(stringTreeSet.size() == stringCompareTo.size());
        
        Test.ensure(objectTreeSet instanceof TreeSet);
        Test.ensure(objectTreeSet.size() == objectCompareTo.size());
        
        Iterator i = stringTreeSet.iterator();
        Iterator j = stringCompareTo.iterator();
        while(i.hasNext()){
            Test.ensure(i.next().equals(j.next()));
        }
        i = objectTreeSet.iterator();
        j = objectCompareTo.iterator();
        while(i.hasNext()){
            Test.ensure(i.next().equals(j.next()));
        }
        
    }
    
    private void stringContentTo(SortedSet set){
        for (int i = 0; i < CONTENT.length; i++) {
            set.add(CONTENT[i]);
        }
    }
    
    private void objectContentTo(SortedSet set){
        for (int i = 0; i < CONTENT.length; i++) {
            set.add(new ComparableContent(CONTENT[i]));
        }
    }

    
    
    

}
