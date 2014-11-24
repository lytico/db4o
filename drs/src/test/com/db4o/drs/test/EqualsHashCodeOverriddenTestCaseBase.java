/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.drs.test;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.drs.*;
import com.db4o.drs.db4o.*;
import com.db4o.io.*;

import db4ounit.*;

public class EqualsHashCodeOverriddenTestCaseBase implements TestCase {

	public static class Item {
		
		public String _name;
		
		public Item(String name) {
			_name = name;
		}
		
		@Override
		public boolean equals(Object obj) {
			if(obj == this) {
				return true;
			}
			if(obj == null || getClass() != obj.getClass()) {
				return false;
			}
			return _name.equals(((Item)obj)._name);
		}
		
		@Override
		public int hashCode() {
			return _name.hashCode();
		}
	}

	private Storage _storage = new MemoryStorage();

	public EqualsHashCodeOverriddenTestCaseBase() {
		super();
	}

	protected void assertReplicates(Object holder) {
		EmbeddedObjectContainer sourceDb = openContainer("source");
		sourceDb.store(holder);
		sourceDb.commit();
		EmbeddedObjectContainer targetDb = openContainer("target");
		try {
			Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(sourceDb);
			Db4oEmbeddedReplicationProvider providerB = new Db4oEmbeddedReplicationProvider(targetDb);
			final ReplicationSession replication = Replication.begin(providerA, providerB);
			final ObjectSet changed = replication.providerA().objectsChangedSinceLastReplication();
			while (changed.hasNext()) {
				final Object o = changed.next();
				if (holder.getClass() == o.getClass()) {
					replication.replicate(o);
					break;
				}
			}
			replication.commit();
		}
		finally {
			sourceDb.close();
			targetDb.close();
		}
	
	}

	private EmbeddedObjectContainer openContainer(String filePath) {
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.file().storage(_storage);
		config.file().generateUUIDs(ConfigScope.GLOBALLY);
		config.file().generateCommitTimestamps(true);
		return Db4oEmbedded.openFile(config, filePath);
	}

}