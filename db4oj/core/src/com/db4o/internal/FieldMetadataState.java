/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;


/**
 * @exclude
 */
class FieldMetadataState {
    
    private final String _info;
    
    private FieldMetadataState(String info) {
        _info = info;
    }

    static final FieldMetadataState NOT_LOADED = new FieldMetadataState("not loaded");
    
    static final FieldMetadataState UNAVAILABLE = new FieldMetadataState("unavailable");
    
    static final FieldMetadataState AVAILABLE = new FieldMetadataState("available");

    static final FieldMetadataState UPDATING = new FieldMetadataState("updating");

    public String toString() {
        return _info;
    }
    
}
