/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.freespace;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.slots.*;

/**
 * @exclude
 */
public interface FreespaceManager {
	
	public void beginCommit();

	public void endCommit();
	
	public int slotCount();

	public void free(Slot slot);
	
    public void freeSelf();

	public int totalFreespace();
	
	public Slot allocateTransactionLogSlot(int length);

	public Slot allocateSlot(int length);

	public void migrateTo(FreespaceManager fm);

	public void read(LocalObjectContainer container, Slot slot);

	public void start(int id);

	public byte systemType();
	
	public void traverse(Visitor4<Slot> visitor);

	public void write(LocalObjectContainer container);

	public void commit();

	public Slot allocateSafeSlot(int length);

	public void freeSafeSlot(Slot slot);
	
	public void listener(FreespaceListener listener);
	
	public void slotFreed(Slot slot);

	public boolean isStarted();
	
}