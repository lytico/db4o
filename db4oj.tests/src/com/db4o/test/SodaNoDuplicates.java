/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;

public class SodaNoDuplicates {
	
	public Atom atom;
	
	public void store(){
		Test.deleteAllInstances(this);
		Test.deleteAllInstances(new Atom());
		Atom m1 = new Atom("One");
		Atom m2 = new Atom("Two");
		SodaNoDuplicates  snd = new SodaNoDuplicates();
		snd.atom = m1;
		Test.store(snd);
		snd = new SodaNoDuplicates();
		snd.atom = m1;
		Test.store(snd);
		snd = new SodaNoDuplicates();
		snd.atom = m2;
		Test.store(snd);
		snd = new SodaNoDuplicates();
		snd.atom = m2;
		Test.store(snd);
	}
	
	public void test(){
		Query q = Test.query();
		q.constrain(SodaNoDuplicates.class);
		Query qAtoms = q.descend("atom");
		ObjectSet set = qAtoms.execute();
		Test.ensure(set.size() == 2);
	}
	

}
