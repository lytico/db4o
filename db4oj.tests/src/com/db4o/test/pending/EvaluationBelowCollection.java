/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.pending;

import com.db4o.*;
import com.db4o.query.*;
import com.db4o.test.*;


/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class EvaluationBelowCollection {	
    public void store(){
	    SimpleNode sub=new SimpleNode("sub",new SimpleNode[0]);
	    SimpleNode sup=new SimpleNode("sup",new SimpleNode[]{sub});
	    Test.store(sup);
	}

	public void test() {
		Query supq=Test.query();
		supq.constrain(SimpleNode.class);
		Query subq=supq.descend("children");
		//subq.constrain(SimpleNode.class);
		subq.descend("name").constrain(new Evaluation() {
            public void evaluate(Candidate candidate) {
                candidate.include(false);
            }		    
		});
		ObjectSet objectSet = supq.execute();
		Test.ensure(objectSet.size() == 0);
	}
}
