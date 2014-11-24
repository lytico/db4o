/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.internal.references;

import com.db4o.internal.*;

/**
 * @exclude
 */
public interface ReferenceSystemFactory {
	
	ReferenceSystem newReferenceSystem(InternalObjectContainer container);

}
