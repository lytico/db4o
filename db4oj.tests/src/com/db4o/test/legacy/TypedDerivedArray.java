/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy;

import com.db4o.foundation.*;
import com.db4o.test.*;

public class TypedDerivedArray {
	
	public Atom[] atoms;
	
	public void store(){
		Test.deleteAllInstances(this);
		TypedDerivedArray tda = new TypedDerivedArray();
		Molecule[] mols = new Molecule[1];
		mols[0] = new Molecule("TypedDerivedArray"); 
		tda.atoms = mols;
		Test.store(tda);
	}
	
	public void test(){
		Test.forEach(new TypedDerivedArray(), new Visitor4() {
            public void visit(Object obj) {
            	TypedDerivedArray tda = (TypedDerivedArray)obj;
            	Test.ensure(tda.atoms instanceof Molecule[]);
            }
        });
	}
}
