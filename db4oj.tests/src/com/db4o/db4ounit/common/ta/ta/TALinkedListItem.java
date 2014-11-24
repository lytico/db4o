/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.ta;

import com.db4o.activation.*;
import com.db4o.db4ounit.common.ta.*;

public class TALinkedListItem extends ActivatableImpl {

	public TALinkedList list;

	public TALinkedListItem() {
	}

	public TALinkedList list() {
		activate(ActivationPurpose.READ);
		return list;
	}
}