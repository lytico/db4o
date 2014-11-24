/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.caching;

import com.db4o.cs.caching.*;
import com.db4o.internal.*;

public class NullClientSlotCache implements ClientSlotCache{

	public void add(Transaction transaction, int id, ByteArrayBuffer slot) {
		// do nothing
	}

	public ByteArrayBuffer get(Transaction transaction, int id) {
		return null;
	}

}
