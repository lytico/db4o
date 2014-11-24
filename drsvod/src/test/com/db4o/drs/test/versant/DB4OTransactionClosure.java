/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

/**
 * 
 */
package com.db4o.drs.test.versant;

import com.db4o.*;

public interface DB4OTransactionClosure {
    void invoke(ObjectContainer transaction);
}