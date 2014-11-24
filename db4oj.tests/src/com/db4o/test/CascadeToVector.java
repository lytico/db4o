/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.util.*;

import com.db4o.*;
import com.db4o.foundation.*;

public class CascadeToVector {

	public Vector vec;

	public void configure() {
		Db4o.configure().objectClass(this).cascadeOnUpdate(true);
		Db4o.configure().objectClass(this).cascadeOnDelete(true);
	}

	public void store() {
		Test.deleteAllInstances(this);
		Test.deleteAllInstances(new Atom());
		CascadeToVector ctv = new CascadeToVector();
		ctv.vec = new Vector();
		ctv.vec.addElement(new Atom("stored1"));
		ctv.vec.addElement(new Atom(new Atom("storedChild1"), "stored2"));
		Test.store(ctv);
	}

	public void test() {

		Test.forEach(this, new Visitor4() {
			public void visit(Object obj) {
				CascadeToVector ctv = (CascadeToVector) obj;
                Enumeration i = ctv.vec.elements();
				while(i.hasMoreElements()){
					Atom atom = (Atom) i.nextElement();
					atom.name = "updated";
					if(atom.child != null){
						// This one should NOT cascade
						atom.child.name = "updated";
					}
				}
				Test.store(ctv);
			}
		});
		Test.reOpen();
		
		Test.forEach(this, new Visitor4() {
			public void visit(Object obj) {
				CascadeToVector ctv = (CascadeToVector) obj;
                Enumeration i = ctv.vec.elements();
                while(i.hasMoreElements()){
                    Atom atom = (Atom) i.nextElement();
					Test.ensure(atom.name.equals("updated"));
					if(atom.child != null){
						Test.ensure( ! atom.child.name.equals("updated"));
					}
				}
			}
		});
		
		
		// Cascade-On-Delete Test: We only want one atom to remain.
		
		Test.reOpen();
		Test.deleteAllInstances(this);
		Test.ensureOccurrences(new Atom(), 1);
	}
}
