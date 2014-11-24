/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.convert.conversions;

import com.db4o.internal.*;
import com.db4o.internal.convert.*;
import com.db4o.internal.convert.ConversionStage.*;


/**
 * @exclude
 */
public class FieldIndexesToBTrees_5_7 extends Conversion{
    
    public static final int VERSION = 6;

	public void convert(SystemUpStage stage) {
        stage.file().classCollection().writeAllClasses();        
		rebuildUUIDIndex(stage.file());
    	freeOldUUIDMetaIndex(stage.file());
    }
	
    private void rebuildUUIDIndex(LocalObjectContainer file) {
		final UUIDFieldMetadata uuid = file.uUIDIndex();
		final ClassMetadataIterator i = file.classCollection().iterator();
		while (i.moveNext()) {
			final ClassMetadata clazz = i.currentClass();
			if (clazz.generateUUIDs()) {
				uuid.rebuildIndexForClass(file, clazz);
			}
		}
	}

    /** @param file */
    private void freeOldUUIDMetaIndex(LocalObjectContainer file) {
		// updating removed here to allow removing MetaIndex class
	}
}
