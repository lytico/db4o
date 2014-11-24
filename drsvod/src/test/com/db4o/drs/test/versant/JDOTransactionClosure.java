/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

/**
 * 
 */
package com.db4o.drs.test.versant;

import javax.jdo.*;

public interface JDOTransactionClosure {
    void invoke(PersistenceManager transaction);
}