/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.query.processor;



/**
 * @exclude
 */
public class QEEqual extends QEAbstract
{
    public void indexBitMap(boolean[] bits){
        bits[QE.EQUAL] = true;
    }
}
