/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.freespace;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.slots.*;

/**
 * @exclude
 */
public class NullFreespaceManager implements FreespaceManager {
	
	public static final FreespaceManager INSTANCE = new NullFreespaceManager();
	
	private NullFreespaceManager(){
		
	}

	public Slot allocateSlot(int length) {
		return null;
	}

	public Slot allocateSafeSlot(int length) {
		return null;
	}

	public void beginCommit() {
		
	}

	public void commit() {
		
	}

	public void endCommit() {
		
	}

	public void free(Slot slot) {
		
	}

	public void freeSelf() {
		
	}

	public void freeSafeSlot(Slot slot) {
		
	}

	public void listener(FreespaceListener listener) {
		
	}

	public void migrateTo(FreespaceManager fm) {
		
	}

	public int slotCount() {
		return 0;
	}

	public void slotFreed(Slot slot) {
		
	}

	public void start(int id) {
		
	}

	public byte systemType() {
		return 0;
	}

	public int totalFreespace() {
		return 0;
	}

	public void traverse(Visitor4<Slot> visitor) {
		
	}

	public void write(LocalObjectContainer container) {
		
	}

	public boolean isStarted() {
		return false;
	}

	public Slot allocateTransactionLogSlot(int length) {
		return null;
	}

	public void read(LocalObjectContainer container, Slot slot) {
	}

}
