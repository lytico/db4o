/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;


/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class MultipleEvaluationGetObjectCalls {
    
	public String name;
	
	public void storeOne(){
	    name="hello";
	}
	
	public void test(){
		Query q = Test.query();
		q.constrain(getClass());
		q.descend("name").constrain(new Evaluation() {
            public void evaluate(Candidate candidate) {
            	boolean include = ((String)candidate.getObject()).startsWith("h")
            			&& ((String)candidate.getObject()).endsWith("o");
            	candidate.include(include);
            }
        });
		ObjectSet objectSet = q.execute();
		Test.ensure(objectSet.size() == 1);
	}
}
