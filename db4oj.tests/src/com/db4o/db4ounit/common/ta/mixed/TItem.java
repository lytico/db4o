/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.mixed;

import com.db4o.activation.*;
import com.db4o.db4ounit.common.ta.*;

/**
 * @exclude
 */
public class TItem extends ActivatableImpl {
	
	public int value;
	
	public TItem() {
		
	}
	
	public TItem(int v) {
		value = v;
	}

	public int value() {
		activate(ActivationPurpose.READ);
		return value;
	}
}
