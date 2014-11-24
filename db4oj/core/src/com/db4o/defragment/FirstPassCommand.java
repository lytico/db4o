/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.defragment;

import com.db4o.*;
import com.db4o.internal.*;
import com.db4o.internal.btree.*;
import com.db4o.internal.metadata.*;
		
/**
 * First step in the defragmenting process: Allocates pointer slots in the target file for
 * each ID (but doesn't fill them in, yet) and registers the mapping from source pointer address
 * to target pointer address.
 * 
 * @exclude
 */
public final class FirstPassCommand implements PassCommand {
	
	private IDMappingCollector _collector = new IDMappingCollector();
	
	public void processClass(final DefragmentServicesImpl context, ClassMetadata classMetadata,int id,int classIndexID) {
		_collector.createIDMapping(context,id, true);
		
        classMetadata.traverseAllAspects(new TraverseFieldCommand() {
    		
			@Override
			protected void process(FieldMetadata field) {
                if(!field.isVirtual()&&field.hasIndex()) {
                    processBTree(context,field.getIndex(context.systemTrans()));
                }
			}
		});

	}

	public void processObjectSlot(DefragmentServicesImpl context, ClassMetadata classMetadata, int sourceID) {
		_collector.createIDMapping(context,sourceID, false);
	}

	public void processClassCollection(DefragmentServicesImpl context) throws CorruptionException {
		_collector.createIDMapping(context,context.sourceClassCollectionID(), false);
	}

	public void processBTree(final DefragmentServicesImpl context, final BTree btree) {
		context.registerBTreeIDs(btree, _collector);
	}

	public void flush(DefragmentServicesImpl context) {
		_collector.flush(context);
	}

}