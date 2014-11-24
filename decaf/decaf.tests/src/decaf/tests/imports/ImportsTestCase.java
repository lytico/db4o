package decaf.tests.imports;

import decaf.core.*;
import decaf.tests.*;

public class ImportsTestCase extends DecafTestCaseBase {
	
	public void testStaticImport() throws Exception {
		runResourceTestCase("StaticImport", TargetPlatform.NONE, TargetPlatform.ANDROID);
	}

	@Override
	protected String packagePath() {
		return "imports";
	}
}
