/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.io.*;

import com.db4o.*;
import com.db4o.config.*;


/**
 * 
 */
public class SerializableTranslator implements Serializable{
    
    public ST1 st1;
    
    public void configure() {
        Db4o.configure().objectClass(ST1.class).translate(new TSerializable());
    }
    
    public void storeOne() {
        st1 = new ST1();
        st1.name = "foo";
    }
    
    public void testOne() {
        Test.ensure(st1.name.equals("foo"));
    }
    
    
    public static class ST1 implements Serializable{
        public String name;
    }
    

}
