/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.types.arrays;

import db4ounit.*;
import db4ounit.extensions.*;


public class NestedArraysTestCase extends AbstractDb4oTestCase {

    private static final int DEPTH = 5;
    private static final int ELEMENTS = 3;

    public static class Data {
	    public Object _obj;
	    public Object[] _arr;

	    public Data(Object obj, Object[] arr) {
			this._obj = obj;
			_arr = arr;
		}
    }
    
    protected void store(){
        Object[] obj = new Object[ELEMENTS];
        fill(obj, DEPTH);
        Object[] arr = new Object[ELEMENTS];
        fill(arr, DEPTH);
        db().store(new Data(obj,arr));
    }
    
    private void fill(Object[] arr, int depth){
        if(depth <= 0){
            arr[0] = "somestring";
            arr[1] = new Integer(10);
            return;
        }
        
        depth --;
        
        for (int i = 0; i < ELEMENTS; i++) {
            arr[i] = new Object[ELEMENTS];
            fill((Object[])arr[i], depth );
        }
    }
    
    public void testOne(){
    	Data data=(Data)retrieveOnlyInstance(Data.class);
        db().activate(data, Integer.MAX_VALUE);
        check((Object[])data._obj, DEPTH);
        check(data._arr, DEPTH);
    }
    
    private void check(Object[] arr, int depth){
        if(depth <= 0){
            Assert.areEqual("somestring",arr[0]);
            Assert.areEqual(new Integer(10),arr[1]);
            return;
        }
        
        depth --;
        
        for (int i = 0; i < ELEMENTS; i++) {
            check((Object[])arr[i], depth );
        }
        
    }
    
}
