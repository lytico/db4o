/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.mixed;

import com.db4o.activation.*;
import com.db4o.db4ounit.common.ta.*;

/**
 * @exclude
 */
public class TNTItem extends ActivatableImpl {

	public NTItem ntItem;

	public TNTItem() {

	}

	public TNTItem(int v) {
		ntItem = new NTItem(v);
	}

	public NTItem value() {
		activate(ActivationPurpose.READ);
		return ntItem;
	}
}
