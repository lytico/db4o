package decaf.tests.collections;

import decaf.core.*;
import decaf.tests.*;

public class CollectionsTestCase extends DecafTestCaseBase {
	
	public void testHashSetUsage() throws Exception {
		runPlatformTestCase("HashSetUsage");
	}
	
	public void testExtendsList() throws Exception {
		runPlatformTestCase("ExtendsList");
	}

	public void testSimpleMapUsage() throws Exception {
		runPlatformTestCase("SimpleMapUsage");
	}

	public void testSimpleListUsage() throws Exception {
		runPlatformTestCase("SimpleListUsage");
	}

	public void testForEachCollection() throws Exception {
		runResourceTestCase("ForEachCollection", TargetPlatform.JDK12, TargetPlatform.ANDROID);
	}

	public void testForEachList() throws Exception {
		runPlatformTestCase("ForEachList");
	}
	
	public void testIterableMapping() throws Exception {
		runPlatformTestCase("IterableMapping");
	}

	// TODO jdk11 code doesn't compile
	public void testMapAPI() throws Exception {
		runPlatformTestCase("MapAPI");
	}
	
	@Override
	protected String packagePath() {
		return "collections";
	}

}
