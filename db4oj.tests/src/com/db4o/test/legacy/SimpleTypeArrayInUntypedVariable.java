/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test.legacy;

import com.db4o.test.*;


public class SimpleTypeArrayInUntypedVariable {
    
    public Object arr;
    
    public void storeOne(){
        arr = new int[]{1, 2, 3};
    }
    
    public void testOne(){
        Test.ensure(arr instanceof int[]);
        int[] arri = (int[]) arr;
        Test.ensure(arri[0] == 1);
        Test.ensure(arri[1] == 2);
        Test.ensure(arri[2] == 3);
        Test.ensure(arri.length == 3);
    }
}
