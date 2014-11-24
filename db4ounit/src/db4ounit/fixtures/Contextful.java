/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.fixtures;

import com.db4o.foundation.*;

public class Contextful {

	protected final FixtureContext _context;

	public Contextful() {
		_context = currentContext();
	}

	protected Object run(final Closure4 closure4) {
		return combinedContext().run(closure4);
	}
	
	protected void run(final Runnable runnable) {
		combinedContext().run(runnable);
	}

	private FixtureContext combinedContext() {
		return currentContext().combine(_context);
	}

	private FixtureContext currentContext() {
		return FixtureContext.current();
	}
}