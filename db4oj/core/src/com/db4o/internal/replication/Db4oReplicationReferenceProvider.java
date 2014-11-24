/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.replication;


/**
 * @exclude
 */
public interface Db4oReplicationReferenceProvider {
    
    public Db4oReplicationReference referenceFor(Object obj);

}
