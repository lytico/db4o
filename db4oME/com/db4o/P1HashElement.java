/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude 
 * @persistent
 */
public class P1HashElement extends P1ListElement {
    
    public Object i_key;
    public int i_hashCode;
    public int i_position;
    
    public P1HashElement(){
    }
    
    public P1HashElement(Transaction a_trans, P1ListElement a_next, Object a_key, int a_hashCode, Object a_object){
        super(a_trans, a_next, a_object);
        i_hashCode = a_hashCode;
        i_key = a_key;
    }
    
    public int adjustReadDepth(int a_depth) {
        return 1;
    }
    
    Object activatedKey(int a_depth){
        
        // TODO: It may be possible to optimise away the following call.
        checkActive();

        
        // The pathologic case here:
        // No activation depth for the map.
        // Global activation depth of 0 during defragment
        // The key can't activate at all.
        
        // Let's make sure it has a depth of 1 at least, but of course that
        // may not be sufficient for more complex #hashCode calls.
        if(a_depth < 0){
            Transaction trans = getTrans();
            if(trans != null){
                if(trans.i_stream.i_config.activationDepth() < 1){
                    a_depth = 1;
                }
            }
        }
        
        activate(i_key, a_depth);
        return i_key;
    }
    
    void delete(boolean a_deleteRemoved){
        if(a_deleteRemoved){
            delete(i_key);
        }
        super.delete(a_deleteRemoved);
    }
}
