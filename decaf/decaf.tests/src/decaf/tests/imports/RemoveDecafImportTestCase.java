/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
package decaf.tests.imports;

import decaf.tests.*;

public class RemoveDecafImportTestCase extends DecafTestCaseBase {
	
	public void testPublic() throws Exception {
		runResourceTestCase("DecafImportIsRemoved", "deeper/FooBar");
	}
	
	@Override
	protected String packagePath() {
		return "imports";
	}	
}
