/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.fixtures;

import com.db4o.foundation.*;

public interface FixtureProvider extends Iterable4 {
	
	FixtureVariable variable();
	
}
