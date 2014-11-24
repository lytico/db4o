/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections;

import java.io.*;
import java.util.*;

import com.db4o.filestats.*;
import com.db4o.internal.freespace.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class HashMapUpdateFileSizeTestCase extends AbstractDb4oTestCase implements OptOutMultiSession, OptOutDefragSolo{

	public static void main(String[] args) {
		new HashMapUpdateFileSizeTestCase().runAll();
	}

	protected void store() throws Exception {
		HashMap map = new HashMap();
		fillMap(map);
		store(map);
	}

	private void fillMap(HashMap map) {
		map.put(new Integer(1), "string 1");
		map.put(new Integer(2), "String 2");
	}

	public void testFileSize() throws Exception {
		warmUp();
		assertFileSizeConstant();
	}

	private void assertFileSizeConstant() throws Exception {
		defragment();
		long beforeUpdate = dbSize();
		for (int i = 0; i < 15; ++i) {
			updateMap();
		}
		defragment();
		long afterUpdate = dbSize();
		Assert.isSmallerOrEqual(beforeUpdate + AbstractFreespaceManager.REMAINDER_SIZE_LIMIT, afterUpdate);
	}

	private void warmUp() throws Exception, IOException {
		for (int i = 0; i < 3; ++i) {
			updateMap();
		}
	}

	private void updateMap() throws Exception, IOException {
		HashMap map = (HashMap) retrieveOnlyInstance(HashMap.class);
		fillMap(map);
		store(map);
		db().commit();
	}
	
	private long dbSize() {
		FileUsageStats stats = new FileUsageStatsCollector(db(), false).collectStats();
		return stats.totalUsage()-stats.freespace();
	}

}
