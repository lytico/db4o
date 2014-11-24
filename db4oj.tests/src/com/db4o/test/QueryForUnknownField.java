/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import com.db4o.query.*;


public class QueryForUnknownField {
    
    public String _name;
    
    public QueryForUnknownField(){
    }
    
    public QueryForUnknownField(String name){
        _name = name;
    }
    
    public void storeOne(){
        _name = "name";
    }
    
    public void test(){
        Query q = Test.query();
        q.constrain(QueryForUnknownField.class);
        q.descend("_name").constrain("name");
        Test.ensure(q.execute().size() == 1);
        
        q = Test.query();
        q.constrain(QueryForUnknownField.class);
        q.descend("name").constrain("name");
        Test.ensure(q.execute().size() == 0);
        
        
    }

    
    

}
