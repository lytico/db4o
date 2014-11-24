package db4ounit;

import com.db4o.foundation.*;
import com.db4o.internal.*;

public class ClassLevelFixtureTestSuite extends OpaqueTestSuiteBase {

	public static final String TEARDOWN_METHOD_NAME = "classTearDown";
	public static final String SETUP_METHOD_NAME = "classSetUp";
	
	private final Class<?> _clazz;
	
	public ClassLevelFixtureTestSuite(Class<?> clazz, Closure4<Iterator4<Test>> tests) {
		super(tests);
		_clazz = clazz;
	}

	@Override
	protected void suiteSetUp() throws Exception {
		Reflection4.invokeStatic(_clazz, SETUP_METHOD_NAME);
	}

	@Override
	protected void suiteTearDown() throws Exception {
		Reflection4.invokeStatic(_clazz, TEARDOWN_METHOD_NAME);
	}

	public String label() {
		return _clazz.getName();
	}
	
	protected OpaqueTestSuiteBase transmogrified(Closure4<Iterator4<Test>> tests) {
		return new ClassLevelFixtureTestSuite(_clazz, tests);
	}
}
