/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import java.util.*;

import com.db4o.*;
import com.db4o.query.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class CallConstructors {
    
    static Hashtable constructorCalledByClass = new Hashtable();
    
    static void constructorCalled(Object obj){
        constructorCalledByClass.put(obj.getClass(), obj);
    }
    
    static Object[] cases = new Object[]{
        new CallGlobal(),
        new CallLocalYes(),
        new CallLocalNo()
    };
    
    public void configure(){
        Db4o.configure().callConstructors(false);
        Db4o.configure().objectClass(new CallLocalYes()).callConstructor(true);
        Db4o.configure().objectClass(new CallLocalNo()).callConstructor(false);
    }
    
    public void store(){
        for (int i = 0; i < cases.length; i++) {
            Test.store(cases[i]);
        }
    }
    
    public void test(){
        if(! Test.clientServer){
	        check(new CallLocalYes(), true);
	        check(new CallLocalNo(), false);
	        check(new CallGlobal(), false);
        }
        Db4o.configure().callConstructors(true);
        Test.reOpen();
        check(new CallLocalYes(), true);
        check(new CallLocalNo(), false);
        check(new CallGlobal(), true);
        Db4o.configure().callConstructors(false);
        Test.reOpen();
        check(new CallLocalYes(), true);
        check(new CallLocalNo(), false);
        check(new CallGlobal(), false);
        
    }
    
    private void check(Object obj, boolean expected){
        constructorCalledByClass.clear();
        Query q = Test.query();
        q.constrain(obj.getClass());
        ObjectSet os = q.execute();
        Test.ensure(os.hasNext());
        Test.ensureEquals(obj.getClass(), os.next().getClass());
        Test.ensure(!os.hasNext());
        boolean called = constructorCalledByClass.get(obj.getClass()) != null;
        Test.ensure(called == expected);
    }
    
    
    public static class CallCommonBase{
        public CallCommonBase(){
            constructorCalled(this);
        }
    }
    
    public static class CallGlobal extends CallCommonBase{
    }
    
    public static class CallLocalYes extends CallCommonBase{
    }
    
    public static class CallLocalNo extends CallCommonBase{
    }
}
