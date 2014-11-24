/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;

public class CallbackHelper {

		public String name;
		public CallbacksTestCase parent;
		
		public void objectOnActivate(ObjectContainer container){
		    CallbacksTestCase.called[CallbacksTestCase.ACTIVATE] = true;
		    container.activate(parent, 3);
		}
		
		public void objectOnDeactivate(ObjectContainer container){
			container.deactivate(parent, 3);
		}
		
		public void objectOnDelete(ObjectContainer container){
			container.delete(parent);
		}
		
		public void objectOnNew(ObjectContainer container){
			
			// New logic: Updating a touched object inside a callback
			//            is not allowes. The following is no longer
			//            a legal call:
			
			// container.store(parent);
		}
		
		public void objectOnUpdate(ObjectContainer container){
		    
		    // circular sets are necessary in many cases
		    // Don' stop them!
		    
		    // Accordingly the following will produce an endless loop
			// container.set(parent);
		}
}
