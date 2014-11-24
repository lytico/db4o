/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.ids.profile;

import java.io.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.ids.*;
import com.db4o.internal.slots.*;
import com.db4o.io.*;

public class IdSysProfileUtil {

	public final static int NUM_IDS = 500000;
	
	private final static String DB_PATH = "idsysprofile.db4o";
	
	public static void clearDatabase() {
		new File(DB_PATH).delete();
	}
	
	public static void withIdSystem(Procedure4<IdSystem> block) {
		LocalObjectContainer db = (LocalObjectContainer) Db4oEmbedded.openFile(config(), DB_PATH);
		try {
			block.apply(db.idSystem());
		}
		finally {
			db.close();
		}
	}
	
	public static void generateIDs(final int count) {
		Procedure4<IdSystem> block = new Procedure4<IdSystem>() {
			public void apply(IdSystem idSystem) {
				final int[] ids = new int[count];
				for (int idIdx = 0; idIdx < count; idIdx++) {
					ids[idIdx] = idSystem.newId();
				}
				idSystem.commit(new Visitable<SlotChange>() {
					public void accept(Visitor4<SlotChange> visitor) {
						for (int idIdx = 0; idIdx < count; idIdx++) {
							SlotChange change = new SlotChange(ids[idIdx]);
							change.notifySlotCreated(new Slot(idIdx, 1));
							visitor.visit(change);
						}
					}
				}, FreespaceCommitter.DO_NOTHING);
			}
		};
		withIdSystem(block);
	}


	private static EmbeddedConfiguration config() {
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		// config.idSystem().useStackedBTreeSystem();
		config.idSystem().useSingleBTreeSystem();
		// config.file().storage(new FileStorage());
		return config;
	}
}
