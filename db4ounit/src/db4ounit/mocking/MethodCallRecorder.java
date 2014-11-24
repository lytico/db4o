package db4ounit.mocking;

import com.db4o.foundation.*;

import db4ounit.*;

public class MethodCallRecorder implements Iterable4 {
	
	private final Collection4 _calls = new Collection4();
	
	public Iterator4 iterator() {
		return _calls.iterator();
	}
	
	public void record(MethodCall call) {
		_calls.add(call);
	}
	
	public void reset() {
		_calls.clear();
	}
	
	/**
	 * Asserts that the method calls were the same as expectedCalls.
	 * 
	 * Unfortunately we cannot call this method 'assert' because
	 * it's a keyword starting with java 1.5.
	 * 
	 * @param expectedCalls
	 */
	public void verify(MethodCall... expectedCalls) {
		Iterator4Assert.areEqual(expectedCalls, iterator());
	}

	public void verifyUnordered(MethodCall... expectedCalls) {
		Iterator4Assert.sameContent(expectedCalls, iterator());
    }
}
