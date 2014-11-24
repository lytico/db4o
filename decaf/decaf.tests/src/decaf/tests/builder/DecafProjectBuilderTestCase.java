package decaf.tests.builder;

import java.io.*;
import java.util.*;

import org.eclipse.core.resources.*;
import org.eclipse.core.runtime.*;
import org.eclipse.jdt.core.*;
import org.junit.*;
import static org.junit.Assert.*;

import sharpen.core.*;
import sharpen.core.framework.resources.*;
import decaf.*;
import decaf.core.*;
import decaf.tests.*;

public class DecafProjectBuilderTestCase extends DecafTestCaseBase {
	
	@Test
	public void testDecafProjectReferencesGetMapped() throws Exception {
		
		final DecafTestResource dependencyResource = testResourceFor("Dependency", TargetPlatform.NONE);
		createCompilationUnitIn(javaProject(), dependencyResource);
		
		runDecafBuild();
		
		final JavaProject dependent = new JavaProject("Dependent");
		try {
			dependent.addReferencedProject(javaProject().getProject(), null);
			dependent.addNature(DecafNature.NATURE_ID);
			final DecafTestResource dependentResource = testResourceFor("Dependent", TargetPlatform.NONE);
			createCompilationUnitIn(dependent, dependentResource);
			dependent.buildProject(null);
			
			final IProject decafDependent = targetDecafProjectFor(dependent, TargetPlatform.JDK11);
			final IProject decafMain = targetDecafProjectFor(javaProject(), TargetPlatform.JDK11);
			assertArrayEquals(new Object[] { decafMain }, decafDependent.getReferencedProjects());
		} finally {
			dependent.dispose();
		}
	}

	private void runDecafBuild(TargetPlatform... platforms) throws CoreException {
	    final JavaProject project = javaProject();

		if (platforms.length > 0) {
			final DecafProject decaf = DecafProject.create(project.getJavaProject());
			decaf.setTargetPlatforms(platforms);
		}
		
		runDecafBuild(project);
    }

	private void runDecafBuild(final JavaProject project) throws CoreException {
	    project.addNature(DecafNature.NATURE_ID);
		project.buildProject(null);
    }
	
	@Test
	public void testDecafProjectCompilerSettings() throws Exception {
		createCompilationUnit(testResourceFor("CompilerSettingsSubject", TargetPlatform.NONE));
		runDecafBuild(TargetPlatform.JDK11, TargetPlatform.JDK12, TargetPlatform.SHARPEN);
		
		final IJavaProject decaf11 = decafJavaProjectFor(TargetPlatform.JDK11);
		final Map options11 = decaf11.getOptions(false);
		assertEquals("1.3", options11.get(JavaCore.COMPILER_SOURCE));
		assertEquals("1.1", options11.get(JavaCore.COMPILER_CODEGEN_TARGET_PLATFORM));		
		assertNoDecafAnnotationsJar(decaf11.getRawClasspath());		
		
		final IJavaProject decaf12 = decafJavaProjectFor(TargetPlatform.JDK12);
		assertEquals(
				"org.eclipse.jdt.launching.JRE_CONTAINER/org.eclipse.jdt.internal.debug.ui.launcher.StandardVMType/J2SE-1.3",
				containerClasspathEntryFor(decaf12).getPath().toString());
		final Map options12 = decaf12.getOptions(false);
		assertEquals("1.3", options12.get(JavaCore.COMPILER_SOURCE));
		assertEquals("1.1", options12.get(JavaCore.COMPILER_CODEGEN_TARGET_PLATFORM));
		
		
		final IJavaProject sharpen = decafJavaProjectFor(TargetPlatform.SHARPEN);
		assertEquals(
				"org.eclipse.jdt.launching.JRE_CONTAINER/org.eclipse.jdt.internal.debug.ui.launcher.StandardVMType/J2SE-1.5",
				containerClasspathEntryFor(sharpen).getPath().toString());
		
		final Map optionsSharpen = sharpen.getOptions(false);
		assertEquals("1.5", optionsSharpen.get(JavaCore.COMPILER_SOURCE));
		assertEquals("1.5", optionsSharpen.get(JavaCore.COMPILER_CODEGEN_TARGET_PLATFORM));
	}

	private void assertNoDecafAnnotationsJar(IClasspathEntry[] rawClasspath) throws IOException {
		for (IClasspathEntry entry : rawClasspath) {
			if (entry.getEntryKind() == IClasspathEntry.CPE_LIBRARY) {
				String libraryName = entry.getPath().lastSegment();
				assertFalse("Unexpected classpath entry: " + libraryName, libraryName.equals(Resources.DECAF_ANNOTATIONS_JAR));
			}
		}
	}

	private IClasspathEntry containerClasspathEntryFor(IJavaProject p) throws CoreException {
		for (IClasspathEntry e : p.getRawClasspath())
			if (e.getEntryKind() == IClasspathEntry.CPE_CONTAINER)
				return e;
		return null;
    }

	private IJavaProject decafJavaProjectFor(final TargetPlatform platform) {
	    return JavaCore.create(targetDecafProjectFor(javaProject(), platform));
    }

	@Test
	public void testDecafOnlySourceFolders() throws Exception {
		final JavaProject project = javaProject();
		
		final IFolder resourcesFolder = project.createFolder("resources");
		final IFile ignoredFile = resourcesFolder.getFile("Resource.java");
		WorkspaceUtilities.writeText(ignoredFile, testResourceFor("Resource", TargetPlatform.NONE).actualStringContents());
		
		final DecafTestResource testResource1 = testResourceFor("Foo", TargetPlatform.NONE);
		createCompilationUnit(testResource1);
		
		final DecafTestResource testResource2 = testResourceFor("FooImpl", TargetPlatform.NONE);
		createCompilationUnit(project.addSourceFolder("src2"), testResource2);
		
		runDecafBuild();
		
		final IProject decaf = targetDecafProjectFor(project, TargetPlatform.JDK11);
		assertTrue(decaf.exists());
		
		final IFolder decafResources = decaf.getFolder("resources");
		assertFalse(decafResources.exists());
		
		testResource1.assertFile(decaf.getFile("src/decaf/builder/Foo.java"));
		testResource2.assertFile(decaf.getFile("src2/decaf/builder/FooImpl.java"));
	}


	private IProject targetDecafProjectFor(final JavaProject project, final TargetPlatform targetPlatform) {
	    return WorkspaceUtilities.getProject(targetPlatform.appendPlatformId(project.getName() + ".decaf", "."));
    }
	
	@Override
	protected String packagePath() {
		return "builder";
	}

}
