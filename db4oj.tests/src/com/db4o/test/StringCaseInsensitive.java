/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class StringCaseInsensitive {
	
	public String name;
	
	public void storeOne(){
		name = "Hello";
	}
	
	public void test(){
		Query q = Test.query();
		q.constrain(getClass());
		q.descend("name").constrain(new Evaluation() {
            public void evaluate(Candidate candidate) {
            	boolean include = candidate
            		.getObject()
            		.toString()
            		.toLowerCase()
            		.startsWith("hello");
            	candidate.include(include);
            }
        });
		ObjectSet objectSet = q.execute();
		Test.ensure(objectSet.size() == 1);
	}
}
