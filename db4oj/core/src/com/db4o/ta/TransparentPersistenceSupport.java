/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.ta;

import com.db4o.config.*;
import com.db4o.internal.*;
import com.db4o.internal.activation.*;

/**
 * Enables Transparent Persistence and Transparent Activation behaviours for
 * the current session.
 * <br><br>
 * configuration.add(new TransparentPersistenceSupport());
 * @see TransparentActivationSupport
 */
public class TransparentPersistenceSupport extends TransparentActivationSupport {
	
	private final RollbackStrategy _rollbackStrategy;

	/**
	 * Creates a new instance of TransparentPersistenceSupport class
	 *  
	 * @param rollbackStrategy RollbackStrategy interface implementation, which
	 * defines the actions to be taken on the object when the transaction is rolled back.
	 */
	public TransparentPersistenceSupport(RollbackStrategy rollbackStrategy) {
		_rollbackStrategy = rollbackStrategy;
	}
	
	/**
	 * Creates a new instance of TransparentPersistenceSupport class 
	 * with no rollback strategies defined.
	 */
	public TransparentPersistenceSupport() {
		this(null);
	}

	/**
	 * Configures current ObjectContainer to support Transparent Activation and Transparent Persistence
	 * @see TransparentActivationSupport#apply(InternalObjectContainer) 
	 */
	@Override
	public void apply(InternalObjectContainer container) {
		super.apply(container);
		enableTransparentPersistenceFor(container);
	}

	private void enableTransparentPersistenceFor(InternalObjectContainer container) {
	    final TransparentActivationDepthProvider provider = (TransparentActivationDepthProvider) activationProvider(container);
		provider.enableTransparentPersistenceSupportFor(container, _rollbackStrategy);
    }
	
	@Override
	public void prepare(Configuration configuration) {
		super.prepare(configuration);
		((Config4Impl)configuration).updateDepthProvider(new TPUpdateDepthProvider());
	}
}
