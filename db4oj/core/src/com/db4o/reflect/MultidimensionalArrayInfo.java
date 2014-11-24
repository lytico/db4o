/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.reflect;



/**
 * @exclude
 */
public class MultidimensionalArrayInfo extends ArrayInfo {
    
    private int[] _dimensions;

    public void dimensions(int[] dim) {
        _dimensions = dim;
    }
    
    public int[] dimensions(){
        return _dimensions;
    }

}
