/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;


/**
 * @exclude
 */
public class SharedIndexedFields {
    
	public final VersionFieldMetadata _version = new VersionFieldMetadata();
    
    public final UUIDFieldMetadata _uUID = new UUIDFieldMetadata();
    
    public final CommitTimestampFieldMetadata _commitTimestamp = new CommitTimestampFieldMetadata();

    
}
