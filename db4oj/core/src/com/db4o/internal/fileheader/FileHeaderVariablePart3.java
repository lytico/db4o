/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.fileheader;

import com.db4o.internal.*;


/**
 * @exclude
 */
public class FileHeaderVariablePart3 extends FileHeaderVariablePart2 {

	public FileHeaderVariablePart3(LocalObjectContainer container) {
		super(container);
	}
	
	@Override
	public int ownLength() {
		return super.ownLength() + Const4.INT_LENGTH * 2;
	}
	@Override
	protected void readBuffer(ByteArrayBuffer buffer, boolean versionsAreConsistent) {
		super.readBuffer(buffer, versionsAreConsistent);
		
		SystemData systemData = systemData();
		systemData.idToTimestampIndexId(buffer.readInt());
		systemData.timestampToIdIndexId(buffer.readInt());
	}
	
	@Override
	protected void writeBuffer(ByteArrayBuffer buffer, boolean shuttingDown) {
		super.writeBuffer(buffer, shuttingDown);
		
		SystemData systemData = systemData();
        buffer.writeInt(systemData.idToTimestampIndexId());
        buffer.writeInt(systemData.timestampToIdIndexId());
	}
	
	
}
