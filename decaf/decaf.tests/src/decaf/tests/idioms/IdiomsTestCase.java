package decaf.tests.idioms;

import decaf.core.*;
import decaf.tests.*;

public class IdiomsTestCase extends DecafTestCaseBase {
	
	public void testThreadLocalIdiom() throws Exception {
		runPlatformTestCase("ThreadLocalIdiom");
	}

	public void testStringIdioms() throws Exception {
		runResourceTestCase("StringIdioms", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testClassCast() throws Exception {
		runResourceTestCase("ClassCast", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}

	@Override
	protected String packagePath() {
		return "idioms";
	}

}
