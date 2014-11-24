package decaf.tests.enums;

import decaf.core.*;
import decaf.tests.DecafTestCaseBase;

public class EnumTestCase extends DecafTestCaseBase {
	public void testSimpleEnum() throws Exception {
		runResourceTestCase("SimpleEnum");
	}
	
	public void testEnumsWithConstructors() throws Exception {
		runResourceTestCase("EnumsWithConstructors", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testComplexEnum() throws Exception {
		runResourceTestCase("ComplexEnum", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}	

	public void testPublicEnum() throws Exception {
		runResourceTestCase("PublicEnum", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}	

	protected String packagePath() {
		return "enums";
	}
}
