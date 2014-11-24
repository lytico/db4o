/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.ta;

import com.db4o.activation.*;
import com.db4o.db4ounit.common.ta.*;


public class TANArrayItem extends ActivatableImpl {
		
		public int[][] value;
		
		public Object obj;
		
		public LinkedList[][] lists;

		public Object listsObject;

		public TANArrayItem() {

		}
		
		public int[][] value() {
			activate(ActivationPurpose.READ);
			return value;
		}
		
		public Object object() {
			activate(ActivationPurpose.READ);
			return obj;
		}
		
		public LinkedList[][] lists() {
			activate(ActivationPurpose.READ);
			return lists;
		}
		
		public Object listsObject() {
			activate(ActivationPurpose.READ);
			return listsObject;
		}
	}