package db4ounit;

import com.db4o.foundation.*;

/**
 * A test that always fails with a specific exception.
 */
public class FailingTest implements Test {

	private final Throwable _error;
	private final String _label;

	public FailingTest(String label, Throwable error) {
		_label = label;
		_error = error;
	}

	public String label() {
		return _label;
	}
	
	public Throwable error() {
		return _error;
	}

	public void run() {
		throw new TestException(_error);
	}

	public boolean isLeafTest() {
		return true;
	}
	
	public Test transmogrify(Function4<Test, Test> fun) {
		return fun.apply(this);
	}

}