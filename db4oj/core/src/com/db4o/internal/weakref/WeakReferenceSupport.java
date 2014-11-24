/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.internal.weakref;

import com.db4o.internal.*;

public interface WeakReferenceSupport {

	Object newWeakReference(ObjectReference referent, Object obj);

	void purge();

	void start();

	void stop();

}