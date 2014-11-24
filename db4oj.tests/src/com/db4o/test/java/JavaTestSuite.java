/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.java;

import com.db4o.test.*;


public class JavaTestSuite extends TestSuite{
    
    public Class[] tests(){
        return new Class[] {
            PrimitiveWrappers.class
        };
    }

}
