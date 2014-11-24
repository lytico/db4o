package com.db4o.filestats;

import com.db4o.internal.*;
import com.db4o.internal.btree.*;

/**
 * @exclude
 */
@decaf.Ignore(decaf.Platform.JDK11) 
class BigSetMiscCollector implements MiscCollector {
	public long collectFor(LocalObjectContainer db, int id, SlotMap slotMap) {
		Object bigSet = db.getByID(id);
		db.activate(bigSet, 1);
		BTree btree = (BTree) Reflection4.getFieldValue(bigSet, "_bTree");
		return FileUsageStatsCollector.bTreeUsage(db, btree, slotMap);
	}
}