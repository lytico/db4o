/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.io.*;

import com.db4o.*;
import com.db4o.query.*;

/**
 * demonstrates a case-insensitive query using an Evaluation
 */
public class CaseInsensitive implements Serializable {
    
    public String name;
    
    public CaseInsensitive() {
    }
    
    public CaseInsensitive(String name) {
        this.name = name;
    }
    
    public void store(){
        Test.deleteAllInstances(this);
        Test.store(new CaseInsensitive("HelloWorld"));
    }
    
    public void test(){
        Test.ensure(queryingCaseInsensitiveResults("heLLOworld") == 1);
    }
    
    private int queryingCaseInsensitiveResults(final String name){
        ObjectContainer objectContainer = Test.objectContainer();
        Query q = objectContainer.query();
        q.constrain(CaseInsensitive.class);
        q.constrain(new Evaluation() {
            public void evaluate(Candidate candidate) {
                CaseInsensitive ci = (CaseInsensitive)candidate.getObject();
                candidate.include(ci.name.toLowerCase().equals(name.toLowerCase()));
            }
        });
        return q.execute().size();
    }
}
