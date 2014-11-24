/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.defragment;

import java.io.*;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.btree.*;
import com.db4o.internal.mapping.*;
import com.db4o.internal.slots.*;

/**
 * Encapsulates services involving source and target database files during defragmenting.
 * 
 * @exclude
 */
public interface DefragmentServices extends IDMapping {
	
	ByteArrayBuffer sourceBufferByAddress(int address,int length) throws IOException;
	ByteArrayBuffer targetBufferByAddress(int address,int length) throws IOException;

	ByteArrayBuffer sourceBufferByID(int sourceID) ;

	Slot allocateTargetSlot(int targetLength);

	void targetWriteBytes(ByteArrayBuffer targetPointerReader, int targetAddress);

	Transaction systemTrans();

	void targetWriteBytes(DefragmentContextImpl context, int targetAddress);

	void traverseAllIndexSlots(BTree tree, Visitor4 visitor4);	
	
	void registerBTreeIDs(BTree tree, IDMappingCollector collector);
	
	ClassMetadata classMetadataForId(int id);

	int mappedID(int id);

	void registerUnindexed(int id);
	
	IdSource unindexedIDs();
	
	int sourceAddressByID(int sourceID);
	
	int targetAddressByID(int sourceID);
	
	int targetNewId();
	
	public IdMapping mapping();
	
	public void commitIds();
	
}