/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.config.*;
import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class CascadeOnUpdateTestCase extends AbstractDb4oTestCase {
    
    public static class Holder {
    	public Object child;
    	
    	public Holder(Object child) {
    		this.child = child;
    	}
    }
    
	public static class Atom {
		
		public Atom child;
		public String name;
		
		public Atom(){
		}
		
		public Atom(Atom child){
			this.child = child;
		}
		
		public Atom(String name){
			this.name = name;
		}
		
		public Atom(Atom child, String name){
			this(child);
			this.name = name;
		}
	}

	public Object child;

	protected void configure(Configuration conf) {
		conf.objectClass(Holder.class).cascadeOnUpdate(true);
	}

	protected void store() {
		Holder cou = new Holder(new Atom(new Atom("storedChild"), "stored"));
		db().store(cou);
	}

	public void test() throws Exception {
		foreach(getClass(), new Visitor4() {
			public void visit(Object obj) {
				Holder cou = (Holder) obj;
				((Atom)cou.child).name = "updated";
				((Atom)cou.child).child.name = "updated";
				db().store(cou);
			}
		});
		
		reopen();
		
		foreach(getClass(), new Visitor4() {
			public void visit(Object obj) {
				Holder cou = (Holder) obj;
				Atom atom = (Atom)cou.child;
				Assert.areEqual("updated", atom.name);
				Assert.areNotEqual("updated", atom.child.name);
			}
		});
	}
}
