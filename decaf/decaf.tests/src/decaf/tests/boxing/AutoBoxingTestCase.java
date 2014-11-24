package decaf.tests.boxing;

import decaf.core.*;
import decaf.tests.*;

public class AutoBoxingTestCase extends DecafTestCaseBase {
	
	public void testAutoUnboxingInIf() throws Exception {
		runResourceTestCase("AutoUnboxingInIf", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testAutoUnboxing() throws Exception {
		runResourceTestCase("AutoUnboxing", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testAutoBoxing() throws Exception {
		runResourceTestCase("AutoBoxing", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}

	public void testMethodArgBoxing() throws Exception {
		runResourceTestCase("MethodArgBoxing", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}

	public void testTernaryOpBoxing() throws Exception {
		runResourceTestCase("TernaryOpBoxing", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}

	@Override
	protected String packagePath() {
		return "boxing";
	}

}
