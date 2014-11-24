/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.inside.replication.*;

/**
 * base class for all database aware objects
 * @exclude
 * @persistent
 */
public class P1Object implements Db4oTypeImpl{
    
    private transient Transaction i_trans;
    private transient YapObject i_yapObject;
    
    public P1Object(){
    }
    
    P1Object(Transaction a_trans){
        i_trans = a_trans;
    }
    
    public void activate (Object a_obj, int a_depth){
        if(i_trans != null){
            if(a_depth < 0){
                i_trans.i_stream.activate1(i_trans, a_obj);
            }else{
                i_trans.i_stream.activate1(i_trans, a_obj, a_depth);
            }
        }
    }
    
    public int activationDepth(){
        return 1;
    }
    
    public int adjustReadDepth(int a_depth) {
        return a_depth;
    }
    
    public boolean canBind() {
        return false;
    }
    
    void checkActive(){
        if(i_trans != null){
		    if(i_yapObject == null){
		        i_yapObject = i_trans.i_stream.getYapObject(this);
		        if(i_yapObject == null){
		            i_trans.i_stream.set(this);
		            i_yapObject = i_trans.i_stream.getYapObject(this);
		        }
		    }
		    if(validYapObject()){
		        i_yapObject.activate(i_trans, this, activationDepth(), false);
		    }
        }
    }

    public Object createDefault(Transaction a_trans) {
        throw YapConst.virtualException();
    }
    
    void deactivate(){
        if(validYapObject()){
            i_yapObject.deactivate(i_trans, activationDepth());
        }
    }
    
    void delete(){
        if(i_trans != null){
	        if(i_yapObject == null){
	            i_yapObject = i_trans.i_stream.getYapObject(this);
	        }
	        if(validYapObject()){
	            i_trans.i_stream.delete3(i_trans,i_yapObject,this, 0, false);
	        }
        }
    }
    
    protected void delete(Object a_obj){
        if(i_trans != null){
            i_trans.i_stream.delete(a_obj);
        }
    }
    
    protected long getIDOf(Object a_obj){
        if(i_trans == null){
            return 0;
        }
        return i_trans.i_stream.getID(a_obj);
    }
    
    protected Transaction getTrans(){
        return i_trans;
    }
    
    public boolean hasClassIndex() {
        return false;
    }
    
    public void preDeactivate(){
        // virtual, do nothing
    }
	
    protected Object replicate(Transaction fromTrans, Transaction toTrans) {
        
        YapStream fromStream = fromTrans.i_stream;
        YapStream toStream = toTrans.i_stream;
        
        MigrationConnection mgc = fromStream.i_handlers.i_migration;
        
        synchronized(fromStream.lock()){
            
    		int id = toStream.oldReplicationHandles(this);
            
            if(id == -1){
                // no action to be taken, already handled
                return this;
            }
            
    		if(id > 0) {
                // replication has taken care, we need that object
    			return toStream.getByID(id);
    		}
            
            if(mgc != null){
                Object otherObj = mgc.identityFor(this);
                if(otherObj != null){
                    return otherObj;
                }
            }
            
            P1Object replica = (P1Object)createDefault(toTrans);
            
            if(mgc != null){
                mgc.mapReference(replica, i_yapObject);
                mgc.mapIdentity(this, replica);
            }
			
            replica.store(0);
			
            return replica;
        }
	}
    
    public void replicateFrom(Object obj) {
        // do nothing
    }

    public void setTrans(Transaction a_trans){
        i_trans = a_trans;
    }

    public void setYapObject(YapObject a_yapObject) {
        i_yapObject = a_yapObject;
    }
    
    protected void store(Object a_obj){
        if(i_trans != null){
            i_trans.i_stream.setInternal(i_trans, a_obj, true);
        }
    }
    
    public Object storedTo(Transaction a_trans){
        i_trans = a_trans;
        return this;
    }
    
    Object streamLock(){
        if(i_trans != null){
	        i_trans.i_stream.checkClosed();
	        return i_trans.i_stream.lock();
        }
        return this;
    }
    
    public void store(int a_depth){
        if(i_trans != null){
            if(i_yapObject == null){
                i_yapObject = i_trans.i_stream.getYapObject(this);
                if(i_yapObject == null){
                    i_trans.i_stream.setInternal(i_trans, this, true);
                    i_yapObject = i_trans.i_stream.getYapObject(this);
                    return;
                }
            }
            update(a_depth);
        }
    }
    
    void update(){
        update(activationDepth());
    }
    
    void update(int depth){
        if(validYapObject()){
            i_trans.i_stream.beginEndSet(i_trans);
            i_yapObject.writeUpdate(i_trans, depth);
            i_trans.i_stream.checkStillToSet();
            i_trans.i_stream.beginEndSet(i_trans);
        }
    }
    
    void updateInternal(){
        updateInternal(activationDepth());
    }
    
    void updateInternal(int depth){
        if(validYapObject()){
            i_yapObject.writeUpdate(i_trans, depth);
            i_trans.i_stream.rememberJustSet(i_yapObject.getID());
            i_trans.i_stream.checkStillToSet();
        }
    }
    
    private boolean validYapObject(){
        return (i_trans != null) && (i_yapObject != null) && (i_yapObject.getID() > 0);
    }
    
}
