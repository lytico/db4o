/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test.constraints;

import com.db4o.test.*;

public class ConstraintsTestSuite extends TestSuite{
    
    public Class[] tests(){
        return new Class[] {
            UniqueField.class
        };
    }

}
