/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.fixtures;

import com.db4o.foundation.*;

import db4ounit.*;

final class FixtureDecorator implements TestDecorator {
	private final Object _fixture;
	private final FixtureVariable _provider;
	private final int _fixtureIndex;

	FixtureDecorator(FixtureVariable provider, Object fixture, int fixtureIndex) {
		_fixture = fixture;
		_provider = provider;
		_fixtureIndex = fixtureIndex;
	}

	public Test decorate(final Test test) {
		final String label = label();
		return test.transmogrify(new Function4<Test, Test>() {
			public Test apply(Test innerTest) {
				return new TestWithFixture(innerTest, label, _provider, _fixture);
			}
		});
		
	}

	private String label() {
		String label = _provider.label() + "[" + _fixtureIndex + "]";
		if(_fixture instanceof Labeled) {
			label += ":" + ((Labeled)_fixture).label();
		}
		return label;
	}
}