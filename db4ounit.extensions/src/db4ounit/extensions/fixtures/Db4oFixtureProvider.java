/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.extensions.fixtures;

import com.db4o.foundation.*;

import db4ounit.extensions.*;
import db4ounit.fixtures.*;

public class Db4oFixtureProvider implements FixtureProvider {

	public FixtureVariable variable() {
		return Db4oFixtureVariable.FIXTURE_VARIABLE;
	}

	public Iterator4 iterator() {
		return Iterators.singletonIterator(variable().value());
	}	
}
