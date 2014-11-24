/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.ext;

import com.db4o.*;
import com.db4o.query.*;
import com.db4o.types.*;


/**
 * Class to identify a database by it's signature.
 * <br><br>db4o UUID handling uses a reference to the Db4oDatabase object, that
 * represents the database an object was created on.
 * 
 * @persistent
 * @exclude
 */
public class Db4oDatabase implements Db4oType, Internal4{
    

    /**
     * Field is public for implementation reasons, DO NOT TOUCH!
     */
    public byte[] i_signature;
    
    /**
     * Field is public for implementation reasons, DO NOT TOUCH!
     * 
     * This field is badly named, it really is the creation time.
     */
    // TODO: change to _creationTime with PersistentFormatUpdater
    public long i_uuid;
    
    private static final String CREATIONTIME_FIELD = "i_uuid"; 

    
    /**
     * cached ObjectContainer for getting the own ID.
     */
    private transient YapStream i_stream;
    
    /**
     * cached ID, only valid in combination with i_objectContainer
     */
    private transient int i_id;
    
    /**
     * constructor for persistence
     */
    public Db4oDatabase(){
    }
    
    /**
     * constructor for comparison and to store new ones
     */
    public Db4oDatabase(byte[] signature, long creationTime){
        i_signature = signature;
        i_uuid = creationTime;
    }
    
    /**
     * generates a new Db4oDatabase object with a unique signature.
     */
    public static Db4oDatabase generate() {
        Db4oDatabase db = new Db4oDatabase();
        db.i_signature = Unobfuscated.generateSignature();
        db.i_uuid = System.currentTimeMillis();
        return db;
    }
    
    /**
     * comparison by signature.
     */
    public boolean equals(Object obj) {
    	if(obj==this) {
    		return true;
    	}
    	if(obj==null||this.getClass()!=obj.getClass()) {
    		return false;
    	}
        Db4oDatabase other = (Db4oDatabase)obj;
        // FIXME: Should never happen?
        if(other.i_signature==null||this.i_signature==null) {
        	return false;
        }
        if(other.i_signature.length != i_signature.length) {
        	return false;
        }
        for (int i = 0; i < i_signature.length; i++) {
			if (i_signature[i] != other.i_signature[i]) {
				return false;
			}
		}
		return true;
    }
    
    /**
	 * gets the db4o ID, and may cache it for performance reasons.
	 * 
	 * @return the db4o ID for the ObjectContainer
	 */
    public int getID(Transaction trans) {
        YapStream stream = trans.i_stream;
        if(stream != i_stream) {
            i_stream = stream;
            i_id = bind(trans);
        }
        return i_id;
    }
    
    public long getCreationTime(){
        return i_uuid;
    }
    
    /**
     * returns the unique signature 
     */
    public byte[] getSignature(){
        return i_signature;
    }
    
    public String toString(){
        return "db " + i_signature;
    }
    
    public boolean isOlderThan(Db4oDatabase peer){
		
		if(peer == this) 
			throw new IllegalArgumentException(); 
        
        if(i_uuid != peer.i_uuid){
            return i_uuid < peer.i_uuid;
        }
        
        // the above logic has failed, both are the same
        // age but we still want to distinguish in some 
        // way, to have an order in the ReplicationRecord
        
        // The following is arbitrary, it only needs to
        // be repeatable.
        
        // Let's distinguish by signature length 
        
        if(i_signature.length != peer.i_signature.length ){
            return i_signature.length < peer.i_signature.length;
        }
        
        for (int i = 0; i < i_signature.length; i++) {
            if(i_signature[i] != peer.i_signature[i]){
                return i_signature[i] < peer.i_signature[i];
            }
        }
        
        // This should never happen.
        
        // FIXME: Add a message and move to Messages.
        // 
        throw new RuntimeException();
    }
    
    /**
     * make sure this Db4oDatabase is stored. Return the ID.  
     */
    public int bind(Transaction trans){
        YapStream stream = trans.i_stream;
        Db4oDatabase stored = (Db4oDatabase)stream.db4oTypeStored(trans,this);
        if (stored == null) {
            stream.showInternalClasses(true);
            stream.set3(trans,this, 2, false);
            int newID = stream.getID1(trans, this);
            stream.showInternalClasses(false);
            return newID;
        }
        if(stored == this){
            return stream.getID1(trans, this);
        }
        if(i_uuid == 0){
            i_uuid = stored.i_uuid;
        }
        stream.showInternalClasses(true);
        int id = stream.getID1(trans, stored);
        stream.bind(this, id);
        stream.showInternalClasses(false);
        return id;
    }
    
    /**
     * find a Db4oDatabase with the same signature as this one
     */
    public Db4oDatabase query(Transaction trans){
        // showInternalClasses(true);  has to be set for this method to be successful
        if(i_uuid > 0){
            // try fast query over uuid (creation time) first
            Db4oDatabase res = query(trans, true);
            if(res != null){
                return res;
            }
        }
        // if not found, try to find with signature
        return query(trans, false);
    }
    
    private Db4oDatabase query(Transaction trans, boolean constrainByUUID){
        YapStream stream = trans.i_stream;
        Query q = stream.querySharpenBug(trans);
        q.constrain(getClass());
        if(constrainByUUID){
            q.descend(CREATIONTIME_FIELD).constrain(new Long(i_uuid));
        }
        ObjectSet objectSet = q.execute();
        while (objectSet.hasNext()) {
            Db4oDatabase storedDatabase = (Db4oDatabase) objectSet.next();
            stream.activate1(null, storedDatabase, 4);
            if (storedDatabase.equals(this)) {
                return storedDatabase;
            }
        }
        return null;
    }
    
    
}