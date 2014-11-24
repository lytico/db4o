/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside.replication;


/**
 * @exclude
 */
public interface Db4oReplicationReferenceProvider {
    
    public Db4oReplicationReference referenceFor(Object obj);

}
