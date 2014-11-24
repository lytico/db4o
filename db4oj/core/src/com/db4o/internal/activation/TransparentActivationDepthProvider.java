/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.activation;

import com.db4o.internal.*;
import com.db4o.ta.*;

/**
 * @exclude
 */
public interface TransparentActivationDepthProvider extends ActivationDepthProvider{

	void enableTransparentPersistenceSupportFor(
			InternalObjectContainer container, RollbackStrategy withRollbackStrategy);

	void addModified(Object object, Transaction inTransaction);
	
	void removeModified(Object object, Transaction inTransaction);

}