package decaf.tests.application;

import org.junit.*;
import decaf.application.DecafCommandLine;
import decaf.application.DecafCommandLineParser;
import decaf.core.TargetPlatform;

public class DecafCommandLineTestCase extends Assert {
	
	@Test
	public void testProjectReference() {
		DecafCommandLine cmdLine = DecafCommandLineParser.parse("db4o.tests", "-projectReference", "db4o");
		assertEquals("db4o.tests", cmdLine.project);
		assertEquals(1, cmdLine.projectReferences.size());
		assertEquals("db4o", cmdLine.projectReferences.get(0));
		assertTrue(cmdLine.targetPlatforms.isEmpty());
	}
	
	@Test
	public void testTargetPlatform() {
		DecafCommandLine cmdLine = DecafCommandLineParser.parse("db4o.tests", "-targetPlatform", "jdk12", "-targetPlatform", "jdk11");
		
		assertEquals(2, cmdLine.targetPlatforms.size());
		assertSame(TargetPlatform.JDK12, cmdLine.targetPlatforms.get(0));
		assertSame(TargetPlatform.JDK11, cmdLine.targetPlatforms.get(1));
		
	}
	
	@Test
	public void testSourceFolders() {
		final DecafCommandLine cmdLine = DecafCommandLineParser.parse("db4o.cs", "-srcFolder", "core/src", "-srcFolder", "cs/src");
		assertArrayEquals(new String[] { "core/src", "cs/src" }, cmdLine.srcFolders.toArray(new String[0]));
	}

}
