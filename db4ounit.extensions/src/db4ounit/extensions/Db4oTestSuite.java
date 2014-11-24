package db4ounit.extensions;

import com.db4o.foundation.*;

import db4ounit.*;

/**
 * Base class for composable db4o test suites (AllTests classes inside each package, for instance).
 */
public abstract class Db4oTestSuite extends AbstractDb4oTestCase implements TestSuiteBuilder {

	public Iterator4 iterator() {
		return new Db4oTestSuiteBuilder(fixture(), testCases()).iterator();
	}

	protected abstract Class[] testCases();
}
