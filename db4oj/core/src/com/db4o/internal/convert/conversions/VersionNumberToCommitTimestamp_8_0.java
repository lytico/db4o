/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.convert.conversions;

import com.db4o.internal.*;
import com.db4o.internal.convert.*;
import com.db4o.internal.convert.ConversionStage.SystemUpStage;
import com.db4o.internal.handlers.*;
import com.db4o.internal.marshall.*;

/**
 * @exclude
 */
public class VersionNumberToCommitTimestamp_8_0 extends Conversion {

	public static final int VERSION = 12;
	private VersionFieldMetadata versionFieldMetadata;

	public void convert(SystemUpStage stage) {
		
		LocalObjectContainer container = stage.file();
        if (! container.config().generateCommitTimestamps().definiteYes()){
            return;
        }
		container.classCollection().writeAllClasses();
		buildCommitTimestampIndex(container);

		container.systemTransaction().commit();

	}

	private void buildCommitTimestampIndex(LocalObjectContainer container) {
		versionFieldMetadata = container.handlers().indexes()._version;
		final ClassMetadataIterator i = container.classCollection().iterator();
		while (i.moveNext()) {
			final ClassMetadata clazz = i.currentClass();
			if (clazz.hasVersionField() && ! clazz.isStruct()) {
				rebuildIndexForClass(container, clazz);
			}
		}
	}

	public boolean rebuildIndexForClass(LocalObjectContainer container, ClassMetadata classMetadata) {
		long[] ids = classMetadata.getIDs();
		for (int i = 0; i < ids.length; i++) {
			rebuildIndexForObject(container, (int) ids[i]);
		}
		return ids.length > 0;
	}

	protected void rebuildIndexForObject(LocalObjectContainer container, final int objectId) throws FieldIndexException {
		StatefulBuffer writer = container.readStatefulBufferById(container.systemTransaction(), objectId);
		if (writer != null) {
			rebuildIndexForWriter(container, writer, objectId);
		}
	}

	protected void rebuildIndexForWriter(LocalObjectContainer container, StatefulBuffer buffer, final int objectId) {
		ObjectHeader objectHeader = new ObjectHeader(container, buffer);
		ObjectIdContextImpl context = new ObjectIdContextImpl(container.systemTransaction(), buffer, objectHeader, objectId);
		ClassMetadata classMetadata = context.classMetadata();
		if(classMetadata.isStruct()){
			// We don't keep version information for structs.
			return;
		}
		if (classMetadata.seekToField(container.systemTransaction(), buffer, versionFieldMetadata) != HandlerVersion.INVALID) {
			long version = (Long) versionFieldMetadata.read(context);
			if (version != 0) {
				LocalTransaction t = (LocalTransaction) container.systemTransaction();
				t.commitTimestampSupport().put(container.systemTransaction(), objectId, version);
			}
		}
	}

}
