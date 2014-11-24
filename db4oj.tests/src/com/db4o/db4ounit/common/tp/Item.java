/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.tp;

import com.db4o.activation.*;
import com.db4o.db4ounit.common.ta.*;


public class Item extends ActivatableImpl {
	
	public String name;
	
	public Item() {
	}
	
	public Item(String initialName) {
		name = initialName;
	}
	
	public String getName() {
		activate(ActivationPurpose.READ);
		return name;
	}
	
	public void setName(String newName) {
		activate(ActivationPurpose.WRITE);
		name = newName;
	}

	public String toString() {
		return "Item(" + getName() + ")";
	}
}