/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.ta;

import com.db4o.activation.*;
import com.db4o.db4ounit.common.ta.*;


public class TAStringItem extends ActivatableImpl {
		public String value;
		
		public Object obj;

		public TAStringItem() {

		}
		
		public String value() {
			activate(ActivationPurpose.READ);
			return value;
		}
		
		public Object object() {
			activate(ActivationPurpose.READ);
			return obj;
		}
		
	}