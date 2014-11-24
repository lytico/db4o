package com.db4o.internal.collections;

import com.db4o.marshall.*;

public interface BigSetPersistence {

	public void write(WriteContext context);

	public void read(ReadContext context);

	public void invalidate();

}