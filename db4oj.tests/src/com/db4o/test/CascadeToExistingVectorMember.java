/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.util.*;

import com.db4o.*;
import com.db4o.foundation.*;

public class CascadeToExistingVectorMember {
	
	public Vector vec;
	
	public void configure(){
		Db4o.configure().objectClass(this).cascadeOnUpdate(true);
	}
	
	public void store(){
		Test.deleteAllInstances(new Atom());
		Test.deleteAllInstances(this);
		CascadeToExistingVectorMember cev = new CascadeToExistingVectorMember();
		cev.vec = new Vector();
		Atom atom = new Atom("one");
		Test.store(atom);
		cev.vec.addElement(atom);
		Test.store(cev);
	}
	
	public void test(){
		Test.forEach(new CascadeToExistingVectorMember(), new Visitor4() {
            public void visit(Object obj) {
            	CascadeToExistingVectorMember cev = (CascadeToExistingVectorMember)obj;
            	Atom atom = (Atom)cev.vec.elementAt(0);
            	atom.name = "two";
            	Test.store(cev);
            	atom.name = "three";
            	Test.store(cev);
            }
        });
        
        Test.reOpen();
        
        Test.forEach(new CascadeToExistingVectorMember(), new Visitor4() {
            public void visit(Object obj) {
            	CascadeToExistingVectorMember cev = (CascadeToExistingVectorMember)obj;
            	Atom atom = (Atom)cev.vec.elementAt(0);
            	Test.ensure(atom.name.equals("three"));
            	Test.ensureOccurrences(atom, 1);
            }
        });
        
        Test.forEach(new CascadeToExistingVectorMember(), new Visitor4() {
            public void visit(Object obj) {
            	CascadeToExistingVectorMember cev = (CascadeToExistingVectorMember)obj;
            	Atom atom = (Atom)cev.vec.elementAt(0);
            	atom.name = "four";
            	Test.store(cev);
            }
        });
        
        
        Test.reOpen();
        
        Test.forEach(new CascadeToExistingVectorMember(), new Visitor4() {
            public void visit(Object obj) {
            	CascadeToExistingVectorMember cev = (CascadeToExistingVectorMember)obj;
            	Atom atom = (Atom)cev.vec.elementAt(0);
            	Test.ensure(atom.name.equals("four"));
            	Test.ensureOccurrences(atom, 1);
            }
        });
        
        
	}
}
