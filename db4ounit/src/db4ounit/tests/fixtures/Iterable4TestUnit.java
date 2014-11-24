/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.tests.fixtures;

import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.fixtures.*;

public class Iterable4TestUnit implements TestCase {
	
	private final Iterable4 subject = (Iterable4)SubjectFixtureProvider.value();
	private final Object[] data = MultiValueFixtureProvider.value();
	
	public void testElements() {
		
		final Iterator4 elements = subject.iterator();
		for (int i=0; i<data.length; ++i) {
			Assert.isTrue(elements.moveNext());
			Assert.areEqual(data[i], elements.current());
		}
		Assert.isFalse(elements.moveNext());
	}
}
