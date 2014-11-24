/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * Tuning switches for customized versions.
 * 
 * @exclude
 */
public class Tuning {
    /**
     * @deprecated Use Db4o.configure().io(new com.db4o.io.SymbianIoAdapter()) instead
     */
    public static final boolean symbianSeek = false;
    static final boolean fieldIndices = true;
    static final boolean readableMessages = true;
}
