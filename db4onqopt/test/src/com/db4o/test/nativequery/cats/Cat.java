/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test.nativequery.cats;


public class Cat extends Animal{
    
    public String _firstName;
    
    public String _lastName;
    
    public int _age;
    
    public Cat _father;
    
    public Cat _mother;
    
    public String getFirstName(){
        return _firstName;
    }
    
    public int getAge(){
        return _age;
    }
    
    public String getFullName(){
        return _lastName==null ? null : _firstName + " " + _lastName;
        
    }

    public Cat getFather() {
    	return _father;
    }
    

}
