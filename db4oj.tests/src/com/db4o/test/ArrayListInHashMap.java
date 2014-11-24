/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.util.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class ArrayListInHashMap {
    
    public HashMap hm;
    
    public void storeOne(){
        hm = new HashMap();
        ArrayList lOne = new ArrayList();
        lOne.add("OneOne");
        lOne.add("OneTwo");
        hm.put("One", lOne);
        ArrayList lTwo = new ArrayList();
        lTwo.add("TwoOne");
        lTwo.add("TwoTwo");
        lTwo.add("TwoThree");
        hm.put("Two", lTwo);
    }
    
    public void testOne(){
        ArrayList lOne = tContent();
        Test.objectContainer().deactivate(lOne, Integer.MAX_VALUE);
        Test.store(hm);
        Test.objectContainer().activate(this, Integer.MAX_VALUE);
        tContent();
    }
    
    private ArrayList tContent(){
        Test.ensure(hm.size() == 2);
        ArrayList lOne = (ArrayList)hm.get("One");
        Test.ensure(lOne.size() == 2);
        Test.ensure(lOne.get(0).equals("OneOne"));
        Test.ensure(lOne.get(1).equals("OneTwo"));
        ArrayList lTwo = (ArrayList)hm.get("Two");
        Test.ensure(lTwo.size() == 3);
        Test.ensure(lTwo.get(0).equals("TwoOne"));
        Test.ensure(lTwo.get(1).equals("TwoTwo"));
        Test.ensure(lTwo.get(2).equals("TwoThree"));
        return lOne;
    }
}


