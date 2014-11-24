package decaf.tests.generics;

import decaf.core.*;
import decaf.tests.*;

public class GenericsTestCase extends DecafTestCaseBase {
	
	public void testSuperMethodErasure() throws Exception {
		runResourceTestCase("SuperMethodErasure", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testNestedTypeErasure() throws Exception {
		runResourceTestCase("NestedTypeErasure", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testCovarianceErasure() throws Exception {
		runResourceTestCase("CovarianceErasure", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testBoundedType() throws Exception {
		runResourceTestCase("BoundedType", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}

	// FIXME
	public void _testBoundedInheritance() throws Exception {
		runResourceTestCase("BoundedInheritance", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}

	public void testMultipleGenericParameters() throws Exception {
		runResourceTestCase("MultipleGenericParameters", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testNestedGenerics() throws Exception {
		runResourceTestCase("NestedGenerics", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testGenericMethods() throws Exception {
		runResourceTestCase("GenericMethods", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testDeclarationErasureNoBounds() throws Exception {
		runResourceTestCase("DeclarationErasureNoBounds", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testIntroduceCastsForFields() throws Exception {
		runResourceTestCase("IntroduceCastsForFields", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}
	
	public void testIntroduceCastsForMethods() throws Exception {
		runResourceTestCase("IntroduceCastsForMethods", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}

	public void testNQPredicate() throws Exception {
		addNQPredicateDefinition();
		runResourceTestCase("NQPredicate", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}

	@Override
	protected String packagePath() {
		return "generics";
	}

}
