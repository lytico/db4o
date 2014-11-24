/* Copyright (C) 2004 - 2005  Versant Inc.   http://www.db4o.com */

package com.db4o.test.nativequery;

import com.db4o.test.*;


/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class NativeQueryTestSuite extends TestSuite{
    
    public Class[] tests(){
        return new Class[] {
            Cat.class
        };
    }
    
}