/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.foundation;

import static com.db4o.drs.foundation.Logger4Support.*;
import db4ounit.*;

/**
 * @sharpen.remove
 */
public class Logger4TestCase implements TestCase{
	
	public void testLogIdentity(){
		logIdentity(new Object(), "message");
		logIdentity(new Object());
	}

}
