package db4ounit.fixtures;

import com.db4o.foundation.*;

import db4ounit.*;

public final class TestWithFixture implements Test {
	private Test _test;
	private final FixtureVariable _variable;
	private final Object _value;
	private final String _fixtureLabel;
	
	public TestWithFixture(Test test, FixtureVariable fixtureVariable, Object fixtureValue) {
		this(test, null, fixtureVariable, fixtureValue);
	}

	public TestWithFixture(Test test, String fixtureLabel, FixtureVariable fixtureVariable, Object fixtureValue) {
		_test = test;
		_fixtureLabel = fixtureLabel;
		_variable = fixtureVariable;
		_value = fixtureValue;
	}
	
	public String label() {
		final ObjectByRef label = new ObjectByRef(); 
		runDecorated(new Runnable() {
			public void run() {
				label.value = "(" + fixtureLabel() + ") " + _test.label();
			}
		});
		return (String)label.value;
	}

	public void run() {
		runDecorated(_test);
	}
	
	public Test test() {
		return _test;
	}

	private void runDecorated(final Runnable block) {
		_variable.with(value(), block);
	}

	private Object value() {
		return _value instanceof Deferred4
			? ((Deferred4)_value).value()
			: _value;
	}

	private Object fixtureLabel() {
		return (_fixtureLabel == null ? _value : _fixtureLabel);
	}

	public boolean isLeafTest() {
		final BooleanByRef isLeaf = new BooleanByRef();
		runDecorated(new Runnable() {
			public void run() {
				isLeaf.value = _test.isLeafTest();
			}
		});
		return isLeaf.value;
	}

	public Test transmogrify(Function4<Test, Test> fun) {
		return fun.apply(this);
	}
}