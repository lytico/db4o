/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.mixed;

import com.db4o.activation.*;
import com.db4o.db4ounit.common.ta.*;

/**
 * @exclude
 */
public class TNItem extends ActivatableImpl {
	
	public LinkedList list;
	
	public TNItem() {
		
	}
	
	public TNItem(int v) {
		list = LinkedList.newList(v);
	}

	public LinkedList value() {
		activate(ActivationPurpose.READ);
		return list;
	}
}
