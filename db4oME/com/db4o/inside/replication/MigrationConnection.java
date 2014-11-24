/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside.replication;

import com.db4o.*;
import com.db4o.foundation.Hashtable4;

/**
 * @exclude
 */
public class MigrationConnection {
    
    public final YapStream _peerA;
    public final YapStream _peerB;

    private final Hashtable4 _referenceMap;
    private final Hashtable4 _identityMap;

    public MigrationConnection(YapStream peerA, YapStream peerB) {
        _referenceMap = new Hashtable4(1);
        _identityMap = new Hashtable4(1);
        _peerA = peerA;
        _peerB = peerB;
    }

    public void mapReference(Object obj, YapObject ref) {
        
        // FIXME: Identityhashcode is not unique
        
        // ignored for now, since it is on most VMs.
        
        // This should be fixed by adding 
        // putIdentity and getIdentity methods to Hashtable4,
        // using the actual object as the parameter and 
        // checking for object identity in addition to the
        // hashcode
        
        _referenceMap.put(System.identityHashCode(obj), ref);
    }
    
    public void mapIdentity(Object obj, Object otherObj) {
        _identityMap.put(System.identityHashCode(obj), otherObj);
    }


    public YapObject referenceFor(Object obj) {
        int hcode = System.identityHashCode(obj);
        YapObject ref = (YapObject) _referenceMap.get(hcode);
        _referenceMap.remove(hcode);
        return ref;
    }
    
    public Object identityFor(Object obj) {
        int hcode = System.identityHashCode(obj);
        return _identityMap.get(hcode);
    }

    
    public void terminate(){
        _peerA.migrateFrom(null);
        _peerB.migrateFrom(null);
    }
    
    public YapStream peer(YapStream stream){
        if(_peerA == stream){
            return _peerB;
        }
        return _peerA;
    }
    
    

}
