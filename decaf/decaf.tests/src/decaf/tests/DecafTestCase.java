package decaf.tests;

import decaf.core.*;

public class DecafTestCase extends DecafTestCaseBase {

	public void testForEachArray() throws Exception {
		runResourceTestCase("ForEachArray", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}

	public void testForEachArrayMethod() throws Exception {
		runResourceTestCase("ForEachArrayMethod", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testNestedForEach() throws Exception {
		runResourceTestCase("NestedForEach", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testDeepVarArgs() throws Exception {
		runResourceTestCase("DeepVarArgs", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}

	public void testVarArgsMethod() throws Exception {
		runResourceTestCase("VarArgsMethod", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}

	public void testVarArgsGenericMethod() throws Exception {
		runResourceTestCase("VarArgsGenericMethod", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testVarArgsConstructor() throws Exception {
		runResourceTestCase("VarArgsConstructor", TargetPlatform.NONE);
	}
	
	public void testPackageDeclaration() throws Exception {
		runResourceTestCase("PackageDeclaration");
	}

	public void testSameSignatureConstructor() throws Exception {
		runResourceTestCase("SameSignatureConstructor", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testGenericForEach() throws Exception {
		runResourceTestCase("GenericForEach", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}

	public void _testCovariance() throws Exception {
		runResourceTestCase("Covariance", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testOverride() throws Exception {
		runResourceTestCase("Override", TargetPlatform.JDK12, TargetPlatform.JDK15);
	}
}
