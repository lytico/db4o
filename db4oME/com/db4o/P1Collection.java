/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.types.*;

/**
 * base class for database aware collections
 * @exclude 
 * @persistent
 */
public abstract class P1Collection extends P1Object implements Db4oCollection{
    
    // This is an off-by-one variable. 
    // 0 means default, use standard activation depth
    // a value greater than 0 means (value - 1)
    private transient int i_activationDepth;
    
    transient boolean i_deleteRemoved;
    
    public void activationDepth(int a_depth){
        i_activationDepth = a_depth + 1;
    }
    
    public void deleteRemoved(boolean a_flag){
        i_deleteRemoved = a_flag;
    }
    
    int elementActivationDepth(){
        return i_activationDepth - 1;
    }

}
