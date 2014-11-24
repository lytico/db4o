/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.ta;

import com.db4o.activation.*;
import com.db4o.db4ounit.common.ta.*;


public class TAIntItem extends ActivatableImpl {
		public int value;
		
		public Object obj;
		
		public Integer i;

		public TAIntItem() {

		}
		
		public int value() {
			activate(ActivationPurpose.READ);
			return value;
		}
		
		public Integer integerValue() {
			activate(ActivationPurpose.READ);
			return i;
		}
		
		public Object object() {
			activate(ActivationPurpose.READ);
			return obj;
		}
	
	}