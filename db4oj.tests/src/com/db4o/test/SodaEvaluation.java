/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class SodaEvaluation {
	
	public SodaEvaluation child;
	public String name;
	
	public void store(){
		Test.deleteAllInstances(this);
		name = "one";
		Test.store(this);
		SodaEvaluation se = new SodaEvaluation();
		se.child= new SodaEvaluation();
		se.child.name = "three";
		se.name = "two";
		Test.store(se);
		
	}
	
	public void test(){
		final String nameConstraint = "three"; 
		Query q = Test.query();
		Query cq = q;
		q.constrain(this.getClass());
		cq = cq.descend("child");
		cq.constrain(new Evaluation() {
            public void evaluate(Candidate candidate) {
            	candidate.include(((SodaEvaluation)candidate.getObject()).name.equals(nameConstraint));
            }
        });
        ObjectSet os = q.execute();
        Test.ensure(os.size() == 1);
        SodaEvaluation se = (SodaEvaluation)os.next();
        Test.ensure(se.name.equals("two"));
		
	}

}
