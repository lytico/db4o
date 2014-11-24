/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.io.*;

import com.db4o.*;
import com.db4o.query.*;

/**
 * 
 */
public class ParameterizedEvaluation implements Serializable {
    
    public String str;
    
    public void store(){
        Test.deleteAllInstances(this);
        store("one");
        store("fun");
        store("ton");
        store("sun");
    }
    
    private void store(String str){
        ParameterizedEvaluation pe = new ParameterizedEvaluation();
        pe.str = str;
        Test.store(pe);
    }
    
    public void test(){
        Test.ensure(queryContains("un").size() == 2);
    }
    
    private ObjectSet queryContains(final String str){
        Query q = Test.query();
        q.constrain(ParameterizedEvaluation.class);
        q.constrain(new Evaluation() {
            public void evaluate(Candidate candidate) {
                ParameterizedEvaluation pe = (ParameterizedEvaluation)candidate.getObject();
                boolean inc = pe.str.indexOf(str) != -1;
                candidate.include(inc);
            }
        });
        
        return q.execute();
    }

}
