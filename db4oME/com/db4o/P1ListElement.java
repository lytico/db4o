/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * element of linked lists 
 * @exclude 
 * @persistent
 */
public class P1ListElement extends P1Object{
    
    public P1ListElement i_next;
    public Object i_object;
    
    public P1ListElement(){
    }
    
    public P1ListElement(Transaction a_trans, P1ListElement a_next, Object a_object){
        super(a_trans);
        i_next = a_next;
        i_object = a_object;
    }
    
    public int adjustReadDepth(int a_depth) {
        if(a_depth >= 1){
            return 1;
        }
        return 0;
    }
    
    Object activatedObject(int a_depth){
        
        // TODO: It may be possible to optimise away the following call
        checkActive();
        
        
        activate(i_object, a_depth);
        return i_object;
    }

    public Object createDefault(Transaction a_trans) {
        P1ListElement elem4 = new P1ListElement();
        elem4.setTrans(a_trans);
        return elem4;
    }
    
    void delete(boolean a_deleteRemoved){
        if(a_deleteRemoved){
            delete(i_object);
        }
        delete();
    }
    
    
}
