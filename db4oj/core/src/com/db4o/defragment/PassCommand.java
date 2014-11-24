/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.defragment;

import java.io.*;

import com.db4o.*;
import com.db4o.internal.*;
import com.db4o.internal.btree.*;

/**
 * Implements one step in the defragmenting process.
 * 
 * @exclude
 */
interface PassCommand {
	void processObjectSlot(DefragmentServicesImpl context,ClassMetadata classMetadata,int id) throws CorruptionException, IOException;
	void processClass(DefragmentServicesImpl context,ClassMetadata classMetadata,int id,int classIndexID) throws CorruptionException, IOException;
	void processClassCollection(DefragmentServicesImpl context) throws CorruptionException, IOException;
	void processBTree(DefragmentServicesImpl context, BTree btree) throws CorruptionException, IOException;
	void flush(DefragmentServicesImpl context);
}
