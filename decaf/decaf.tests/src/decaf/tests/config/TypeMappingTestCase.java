package decaf.tests.config;

import java.util.HashMap;
import java.util.Map;

import decaf.config.*;
import decaf.core.TargetPlatform;
import decaf.tests.DecafTestCaseBase;

public class TypeMappingTestCase extends DecafTestCaseBase {
	public void testTypeMapping() throws Exception {
		runResourceTestCaseWithConfig("TypeMapping", TargetPlatform.NONE, mappingTestConfig());
	}
	
	@Override
	protected String packagePath() {
		return "config";
	}

	private DecafConfiguration mappingTestConfig() {
		Map<String, String> mapping = new HashMap<String, String>();
		mapping.put("decaf.config.SourceMappedType", "decaf.config.TargetMappedType");
		mapping.put("decaf.config.SourceGenericType", "decaf.config.TargetMappedType");
		
		return DecafConfiguration.create(mapping);
	}
}
