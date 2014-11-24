/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.objectexchange;

import com.db4o.cs.caching.*;
import com.db4o.cs.internal.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;

public interface ObjectExchangeStrategy {

	ByteArrayBuffer marshall(LocalTransaction transaction, IntIterator4 ids, int maxCount);

	FixedSizeIntIterator4 unmarshall(ClientTransaction transaction, ClientSlotCache slotCache, ByteArrayBuffer reader);

}
