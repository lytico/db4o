/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.ta.nested;

import com.db4o.activation.*;
import com.db4o.db4ounit.common.ta.*;

/**
 * 	@sharpen.partial
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class OuterClass extends ActivatableImpl {
	
	public int _foo;
	
	public int foo() {
		// TA BEGIN
		activate(ActivationPurpose.READ);
		// TA END
		return _foo;
	}
	
	public InnerClass createInnerObject() {
		return new InnerClass();
	}
	
	/**
	 * 	@sharpen.partial
 	 */
	@decaf.Ignore(decaf.Platform.JDK11)
    public class InnerClass extends ActivatableImpl {	
	
		public OuterClass getOuterObject(){
			// TA BEGIN
			activate(ActivationPurpose.READ);
			// TA END
			return OuterClass.this;
		}

		public OuterClass getOuterObjectWithoutActivation() {
			return OuterClass.this;
		}
	} 

}
