/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.cluster;

import com.db4o.*;
import com.db4o.internal.cluster.*;
import com.db4o.query.*;

/**
 * allows running Queries against multiple ObjectContainers.
 * @exclude   
 */
public class Cluster {
    
    public final ObjectContainer[] _objectContainers;
    
    /**
     * use this constructor to create a Cluster and call
     * add() to add ObjectContainers
     */
    public Cluster(ObjectContainer[] objectContainers){
        if(objectContainers == null){
            throw new NullPointerException();
        }
        if(objectContainers.length < 1){
            throw new IllegalArgumentException();
        }
        for (int i = 0; i < objectContainers.length; i++) {
            if(objectContainers[i] == null){
                throw new IllegalArgumentException();
            }
        }
        _objectContainers = objectContainers;
    }
    
    /**
     * starts a query against all ObjectContainers in 
     * this Cluster.
     * @return the Query
     */
    public Query query(){
        synchronized(this){
            Query[] queries = new Query[_objectContainers.length];
            for (int i = 0; i < _objectContainers.length; i++) {
                queries[i] = _objectContainers[i].query(); 
            }
            return new ClusterQuery(this, queries);
        }
    }
    
    /**
     * returns the ObjectContainer in this cluster where the passed object
     * is stored or null, if the object is not stored to any ObjectContainer
     * in this cluster
     * @param obj the object
     * @return the ObjectContainer
     */
    public ObjectContainer objectContainerFor(Object obj){
        synchronized(this){
            for (int i = 0; i < _objectContainers.length; i++) {
                if(_objectContainers[i].ext().isStored(obj)){
                    return _objectContainers[i];
                }
            }
        }
        return null;
    }
    
}
