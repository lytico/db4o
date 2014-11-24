/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.legacy;

import java.io.*;

import com.db4o.*;
import com.db4o.test.*;


public class NestedArrays {
    
    public Object _object;
    
    public Object[] _objectArray;
    
    private static final int DEPTH = 5;
    
    private static final int ELEMENTS = 3;
    
    private static final String FILE = "nestedArrays.db4o";
    
    
    public NestedArrays(){
        
    }
    
    
    public static void main(String[] arguments) {
        
        new File(FILE).delete();
        ObjectContainer oc = Db4o.openFile(FILE);
        NestedArrays nr = new NestedArrays();
        nr.storeOne();
        
        long storeStart = System.currentTimeMillis();
        oc.store(nr);
        long storeStop = System.currentTimeMillis();
        oc.commit();
        long commitStop = System.currentTimeMillis();
        
        oc.close();
        Db4o.configure().activationDepth(0);
        oc = Db4o.openFile(FILE);
        long loadStart = System.currentTimeMillis();
        nr = (NestedArrays)oc.queryByExample(new NestedArrays()).next();
        oc.activate(nr, Integer.MAX_VALUE);
        long loadStop = System.currentTimeMillis();
        
        oc.close();
        
        long store = storeStop - storeStart;
        long commit = commitStop - storeStop;
        long load = loadStop - loadStart;
        
        System.out.println(Db4o.version() +  " running com.db4o.test.NestedArrays");
        System.out.println("store: " + store + "ms");
        System.out.println("commit: " + commit + "ms");
        System.out.println("load: " + load + "ms");
        
    }
    
    
    public void storeOne(){
        
        _object = new Object[ELEMENTS];
        fill((Object[])_object, DEPTH);
        
        _objectArray = new Object[ELEMENTS];
        fill(_objectArray, DEPTH);
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
        Test.objectContainer().activate(this, Integer.MAX_VALUE);
        
        check((Object[])_object, DEPTH);
        
        check((Object[])_objectArray, DEPTH);
        
        
    }
    
    private void check(Object[] arr, int depth){
        if(depth <= 0){
            Test.ensure(arr[0].equals("somestring"));
            Test.ensure(arr[1].equals(new Integer(10)));
            return;
        }
        
        depth --;
        
        for (int i = 0; i < ELEMENTS; i++) {
            check((Object[])arr[i], depth );
        }
        
    }
    
}
