/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.*;

/**
 * @exclude
 */
public abstract class Identifiable {
	
    protected int _id;

    protected int _state = 2; // DIRTY and ACTIVE

    public final boolean beginProcessing() {
        if (bitIsTrue(Const4.PROCESSING)) {
            return false;
        }
        bitTrue(Const4.PROCESSING);
        return true;
    }

    final void bitFalse(int bitPos) {
        _state &= ~(1 << bitPos);
    }
    
    final boolean bitIsFalse(int bitPos) {
        return (_state | (1 << bitPos)) != _state;
    }

    final boolean bitIsTrue(int bitPos) {
        return (_state | (1 << bitPos)) == _state;
    }

    final void bitTrue(int bitPos) {
        _state |= (1 << bitPos);
    }

    public void endProcessing() {
        bitFalse(Const4.PROCESSING);
    }
    
    public int getID() {
        return _id;
    }

    public final boolean isActive() {
        return bitIsTrue(Const4.ACTIVE);
    }

    public boolean isDirty() {
        return bitIsTrue(Const4.ACTIVE) && (!bitIsTrue(Const4.CLEAN));
    }
    
    public final boolean isNew(){
        return getID() == 0;
    }

    public void setID(int id) {
    	if(DTrace.enabled){
    		DTrace.PERSISTENTBASE_SET_ID.log(id);
    	}
        _id = id;
    }

    public final void setStateClean() {
        bitTrue(Const4.ACTIVE);
        bitTrue(Const4.CLEAN);
    }

    public final void setStateDeactivated() {
        bitFalse(Const4.ACTIVE);
    }

    public void setStateDirty() {
        bitTrue(Const4.ACTIVE);
        bitFalse(Const4.CLEAN);
    }

    public int hashCode() {
    	if(isNew()){
    		throw new IllegalStateException();
    	}
    	return getID();
    }

}
