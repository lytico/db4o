/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test.nativequery;

import java.util.*;

import com.db4o.*;
import com.db4o.query.*;
import com.db4o.test.*;



/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class Cat {
    
    public String name;
    
    public Cat(){
        
    }
    
    public Cat(String name){
        this.name = name;
    }
    
    public void store(){
        Test.store(new Cat("Fritz"));
        Test.store(new Cat("Garfield"));
        Test.store(new Cat("Tom"));
        Test.store(new Cat("Occam"));
        Test.store(new Cat("Zora"));
    }
    
    public void test(){
        ObjectContainer objectContainer = Test.objectContainer();
        List<Cat> list = objectContainer.query(new Predicate <Cat> () {
            public boolean match(Cat cat){
                return cat.name.equals("Occam") || cat.name.equals("Zora"); 
            }
        });
        Test.ensure(list.size() == 2);
        String[] lookingFor = new String[] {"Occam" , "Zora"};
        boolean[] found = new boolean[2];
        for (Cat cat : list){
            for (int i = 0; i < lookingFor.length; i++) {
                if(cat.name.equals(lookingFor[i])){
                    found[i] = true;
                }
            }
        }
        for (int i = 0; i < found.length; i++) {
            Test.ensure(found[i]);
        }
    }
    

}
