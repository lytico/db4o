/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside.cluster;

import com.db4o.*;
import com.db4o.cluster.*;
import com.db4o.inside.*;
import com.db4o.inside.query.*;
import com.db4o.query.*;

/**
 * 
 * @exclude
 */
public class ClusterQueryResult implements QueryResult{
    
    private final Cluster _cluster;
    private final ObjectSet[] _objectSets;
    private int _current;
    private final int[] _sizes;
    private final int _size;
    
    public ClusterQueryResult(Cluster cluster, Query[] queries){
        _cluster = cluster;
        _objectSets = new ObjectSet[queries.length]; 
        _sizes = new int[queries.length];
        int size = 0;
        for (int i = 0; i < queries.length; i++) {
            _objectSets[i] = queries[i].execute();
            _sizes[i] = _objectSets[i].size();
            size += _sizes[i];
        }
        _size = size;
    }

    public boolean hasNext() {
        synchronized(_cluster){
            return hasNextNoSync();
        }
    }
    
    private ObjectSet current(){
        return _objectSets[_current];
    }
    
    private boolean hasNextNoSync(){
        if(current().hasNext()){
            return true;
        }
        if(_current >= _objectSets.length-1){
            return false;
        }
        _current ++;
        return hasNextNoSync();
    }

    public Object next() {
        synchronized(_cluster){
            if(hasNextNoSync()){
                return current().next();
            }
            return null;
        }
    }

    public void reset() {
        synchronized(_cluster){
            for (int i = 0; i < _objectSets.length; i++) {
               _objectSets[i].reset(); 
            }
            _current = 0;
        }
    }

    public int size() {
        return _size;
    }
    
    public Object get(int index) {
        synchronized(_cluster){
            if (index < 0 || index >= size()) {
                throw new IndexOutOfBoundsException();
            }
            int i = 0;
            while(index >= _sizes[i]){
                index -= _sizes[i];
                i++;
            }
            return ((ObjectSetFacade)_objectSets[i])._delegate.get(index); 
        }
    }

    public long[] getIDs() {
        Exceptions4.notSupported();
        return null;
    }

    public Object streamLock() {
        return _cluster;
    }

    public ObjectContainer objectContainer() {
        return _cluster._objectContainers[_current];
    }

    public int indexOf(int id) {
        Exceptions4.notSupported();
        return 0;
    }

	public void sort(QueryComparator cmp) {
        Exceptions4.notSupported();
	}
}

