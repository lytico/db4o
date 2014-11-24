package decaf.tests.core;

import java.util.*;

import org.eclipse.core.runtime.*;

import decaf.core.*;
import decaf.core.DecafProject.*;
import sharpen.core.*;
import junit.framework.*;

public class DecafProjectTestCase extends TestCase {

	private JavaProject _project;
	private DecafProject _decafProject;

	protected void setUp() throws Exception {
		super.setUp();
		
		_project = new JavaProject.Builder(null, "test").project;
		_decafProject = DecafProject.create(_project.getJavaProject());
		
	}	
	
	public void testSetTargetPlatforms() throws CoreException {
		
		_decafProject.setTargetPlatforms(TargetPlatform.JDK12);
		assertTargetPlatforms(TargetPlatform.JDK12);
		
		_decafProject.setTargetPlatforms(TargetPlatform.JDK12, TargetPlatform.JDK11);
		assertTargetPlatforms(TargetPlatform.JDK12, TargetPlatform.JDK11);
	}
	
	public void testDefaultTargetPlatformIsJdk11() throws CoreException {
	
		assertTargetPlatforms(TargetPlatform.JDK11);
	}

	private void assertTargetPlatforms(TargetPlatform... expected) throws CoreException {
		final List<OutputTarget> targets = _decafProject.targets();
		assertEquals(expected.length, targets.size());
		for (int i = 0; i < expected.length; i++) {
			final OutputTarget actual = targets.get(i);
			assertOutputTarget(expected[i], actual);
		}
	}

	private void assertOutputTarget(final TargetPlatform expected, final OutputTarget actual) {
		assertEquals(expected, actual.targetPlatform());
		assertEquals(expected.appendPlatformId(_project.getName(), ".decaf."), actual.targetProject().getElementName());
	}

	protected void tearDown() throws Exception {
		
		_project.dispose();
		
		super.tearDown();
	}

}
