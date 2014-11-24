/* Copyright (C) 2011   Versant Inc.   http://www.db4o.com */

package com.db4o.consistency;

import java.util.*;

import com.db4o.internal.*;
import com.db4o.internal.btree.*;
import com.db4o.internal.classindex.*;

/**
 * @exclude
 */
public final class ConsistencyCheckerUtil {

	public static Map<Integer, ClassMetadata> typesFor(LocalObjectContainer db, Set<Integer> ids) {
		Map<Integer, Set<ClassMetadata>> id2clazzes = new HashMap<Integer, Set<ClassMetadata>>();
		ClassMetadataIterator iter = db.classCollection().iterator();
		while(iter.moveNext()) {
			for(int id : ids) {
				ClassMetadata clazz = iter.currentClass();
				BTree btree = BTreeClassIndexStrategy.btree(clazz);
				if(btree.search(db.systemTransaction(), id) != null) {
					Set<ClassMetadata> clazzes = id2clazzes.get(id);
					if(clazzes == null) {
						clazzes = new HashSet<ClassMetadata>();
						id2clazzes.put(id, clazzes);
					}
					clazzes.add(clazz);
				}
			}
		}
		Map<Integer, ClassMetadata> id2clazz = new HashMap<Integer, ClassMetadata>();
		for(int id : id2clazzes.keySet()) {
			Set<ClassMetadata> clazzes = id2clazzes.get(id);
			ClassMetadata mostSpecific = null;
			OUTER:
			for(ClassMetadata curClazz : clazzes) {
				for(ClassMetadata cmpClazz : clazzes) {
					if(curClazz.equals(cmpClazz._ancestor)) {
						continue OUTER;
					}
				}
				mostSpecific = curClazz;
				break;
			}
			id2clazz.put(id, mostSpecific);
		}
		return id2clazz;
	}

	
	private ConsistencyCheckerUtil() {
	}
}
