package decaf.tests.integration;

import decaf.core.*;
import decaf.tests.*;

public class IntegrationTestCase extends DecafTestCaseBase {
	
	public void testIterableErasureInForEach() throws Exception {
		runPlatformTestCase("IterableErasureInForEach");
	}
	
	public void testUnboxingInVarArgs() throws Exception {
		runResourceTestCase("UnboxingInVarArgs", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testUnboxingInForEach() throws Exception {
		runResourceTestCase("UnboxingInForEach", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}

	public void testUnboxingForGenerics() throws Exception {
		runResourceTestCase("UnboxingForGenerics", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}

	public void testErasureInVarArgs() throws Exception {
		runResourceTestCase("ErasureInVarArgs", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testErasureInForEach() throws Exception {
		runResourceTestCase("ErasureInForEach", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testIgnoreMappedInterface() throws Exception {
		runPlatformTestCase("IgnoreMappedInterface");
	}	
	
	@Override
	protected String packagePath() {
		return "integration";
	}

}
