/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.wrapper.typed;

import com.db4o.*;
import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STCharWT implements STClass{
	
	final static String DESCENDANT = "i_char";
	
	public static transient SodaTest st;
	
	Character i_char;
	
	
	public STCharWT(){
	}
	
	private STCharWT(char a_char){
		i_char = new Character(a_char);
	}
	
	public Object[] store() {
		return new Object[]{
			new STCharWT((char)0),
			new STCharWT((char)1),
			new STCharWT((char)99),
			new STCharWT((char)909)
		};
	}
	
	public void testEquals(){
		Query q = st.query();
		q.constrain(new STCharWT((char)0));  
		
		// Primitive default values are ignored, so we need an 
		// additional constraint:
		q.descend(DESCENDANT).constrain(new Character((char)0));
		st.expectOne(q, store()[0]);
	}
	
	public void testNotEquals(){
		Query q = st.query();
		Object[] r = store();
		Constraint c = q.constrain(r[0]);
		q.descend(DESCENDANT).constraints().not();
		st.expect(q, new Object[] {r[1], r[2], r[3]});
	}
	
	public void testGreater(){
		Query q = st.query();
		Constraint c = q.constrain(new STCharWT((char)9));
		q.descend(DESCENDANT).constraints().greater();
		Object[] r = store();
		st.expect(q, new Object[] {r[2], r[3]});
	}
	
	public void testSmaller(){
		Query q = st.query();
		Constraint c = q.constrain(new STCharWT((char)1));
		q.descend(DESCENDANT).constraints().smaller();
		st.expectOne(q, store()[0]);
	}
	
	public void testIdentity(){
		Query q = st.query();
		Constraint c = q.constrain(new STCharWT((char)1));
		ObjectSet set = q.execute();
		STCharWT identityConstraint = (STCharWT)set.next();
		identityConstraint.i_char = new Character((char)9999);
		q = st.query();
		q.constrain(identityConstraint).identity();
		identityConstraint.i_char = new Character((char)1);
		st.expectOne(q,store()[1]);
	}
	
	public void testNotIdentity(){
		Query q = st.query();
		Constraint c = q.constrain(new STCharWT((char)1));
		ObjectSet set = q.execute();
		STCharWT identityConstraint = (STCharWT)set.next();
		identityConstraint.i_char = new Character((char)9080);
		q = st.query();
		q.constrain(identityConstraint).identity().not();
		identityConstraint.i_char = new Character((char)1);
		Object[] r = store();
		st.expect(q, new Object[] {r[0], r[2], r[3]});
	}
	
	public void testConstraints(){
		Query q = st.query();
		q.constrain(new STCharWT((char)1));
		q.constrain(new STCharWT((char)0));
		Constraints cs = q.constraints();
		Constraint[] csa = cs.toArray();
		if(csa.length != 2){
			st.error("Constraints not returned");
		}
	}
	
	public void testEvaluation(){
		Query q = st.query();
		q.constrain(new STCharWT());
		q.constrain(new Evaluation() {
			public void evaluate(Candidate candidate) {
				STCharWT sts = (STCharWT)candidate.getObject();
				candidate.include((sts.i_char.charValue() + 2) > 100);
			}
		});
		Object[] r = store();
		st.expect(q, new Object[] {r[2], r[3]});
	}
	
}

