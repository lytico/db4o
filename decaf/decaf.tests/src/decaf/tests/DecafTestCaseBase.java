package decaf.tests;

import java.io.*;

import org.eclipse.core.resources.*;
import org.eclipse.core.runtime.*;
import org.eclipse.jdt.core.*;
import org.eclipse.jdt.core.dom.rewrite.*;

import sharpen.core.*;
import decaf.*;
import decaf.builder.*;
import decaf.config.*;
import decaf.core.*;
import junit.framework.*;

public abstract class DecafTestCaseBase extends TestCase {

	private JavaProject _project;

	public DecafTestCaseBase() {
		super();
	}

	public DecafTestCaseBase(String name) {
		super(name);
	}
	
	protected JavaProject javaProject() {
		return _project;
	}

	@Override
	protected void setUp() throws Exception {
		_project = new JavaProject();
		_project.addClasspathEntry(Resources.decafAnnotationsJar());
	}

	@Override
	protected void tearDown() throws Exception {
		_project.dispose();
	}

	protected void runPlatformTestCaseWithConfig(String resourceName, DecafConfiguration config) throws Exception {
		for (TargetPlatform targetPlatform : TargetPlatform.values()) {
			if (targetPlatform == TargetPlatform.NONE) {
				continue;
			}
			runResourceTestCaseWithConfig(resourceName, targetPlatform, config);
		}
	}
	
	protected void runPlatformTestCase(String resourceName) throws Exception {
		for (TargetPlatform targetPlatform : TargetPlatform.values()) {
			if (targetPlatform == TargetPlatform.NONE) {
				continue;
			}
			runResourceTestCase(resourceName, targetPlatform);
		}
	}

	protected void runResourceTestCase(String resourceName) throws Exception {
		runResourceTestCase(resourceName, TargetPlatform.NONE);
	}
	
	protected void runResourceTestCase(String resourceName, String... supportingClasses) throws Exception {
		runResourceTestCase(TargetPlatform.NONE, resourceName, supportingClasses);
	}

	private void runResourceTestCase(final TargetPlatform platform, String resourceName, String... supportingClasses)
			throws CoreException, IOException, Exception {
		createCompilationUnits(platform, supportingClasses);
		runResourceTestCase(resourceName, platform);
	}

	private void createCompilationUnits(final TargetPlatform platform,
			String... supportingClasses) throws CoreException, IOException {
		for (String supportingClass : supportingClasses) {
			createCompilationUnit(testResourceFor(supportingClass, platform));
		}
	}
	
	protected void runResourceTestCase(String resourceName, TargetPlatform... targetPlatforms) throws Exception {
		for(TargetPlatform targetPlatform : targetPlatforms) {
			runResourceTestCaseWithConfig(resourceName, targetPlatform, targetPlatform.defaultConfig());
		}
	}

	protected void runResourceTestCaseWithConfig(String resourceName, TargetPlatform targetPlatform, DecafConfiguration config) throws Exception {
		DecafTestResource resource = testResourceFor(resourceName, targetPlatform);
		ICompilationUnit cu = createCompilationUnit(resource);
		try {
			IFile decafFile = decafFileFor(cu.getResource(), targetPlatform);
		
			final ASTRewrite rewrite = DecafRewriter.rewrite(cu, null, targetPlatform, config);
			if (rewrite != null) {
				FileRewriter.rewriteFile(rewrite, decafFile.getFullPath());
			} else {
				decafFile.delete(true, null);
			}
		
			resource.assertFile(decafFile);
		} finally {
			cu.getResource().delete(true, null);
		}
	}

	protected DecafTestResource testResourceFor(String name,
			TargetPlatform platform) {
		return new DecafTestResource(resourcePath(name), platform);
	}

	private String resourcePath(String resourceName) {
		StringBuilder path = new StringBuilder("decaf/");
		if(packagePath() != null) {
			path.append(packagePath()).append('/');
		}
		path.append(resourceName);
		return path.toString();
	}

	protected String packagePath() {
		return null;
	}
	
	private IFile decafFileFor(IResource originalFile, TargetPlatform targetPlatform) throws CoreException {
		final String decafFolderName = "decaf";
		createFolder(decafFolderName);
		IFolder targetFolder = createFolder(targetPlatform.appendPlatformId(decafFolderName, "/"));
		IFile actualFile = targetFolder.getFile("decaf.txt");
		originalFile.copy(actualFile.getFullPath(), true, null);
		return actualFile;
	}

	private IFolder createFolder(String folderPath) throws CoreException {
		IFolder targetFolder = _project.getProject().getFolder(folderPath);
		if(!targetFolder.exists()) {
			targetFolder = _project.createFolder(folderPath);
		}
		return targetFolder;
	}

	protected void addNQPredicateDefinition() throws CoreException {
		String content =
			"package com.db4o.query;\n\n" +
			"public abstract class Predicate<E> {\n" +
			"  public abstract boolean match(E candidate);\n" +
			"}\n";
		createCompilationUnit("com.db4o.query", "Predicate.java", content);
	}
	
	protected ICompilationUnit createCompilationUnit(DecafTestResource resource) throws CoreException, IOException {
		return createCompilationUnitIn(_project, resource);
	}

	protected ICompilationUnit createCompilationUnitIn(final JavaProject targetProject, DecafTestResource resource)
            throws CoreException, IOException {
	    return targetProject.createCompilationUnit(resource.packageName(), resource.javaFileName(), resource.actualStringContents());
    }
	
	protected ICompilationUnit createCompilationUnit(IPackageFragmentRoot sourceFolder, DecafTestResource resource) throws CoreException, IOException {
		return _project.createCompilationUnit(sourceFolder, resource.packageName(), resource.javaFileName(), resource.actualStringContents());
	}

	private ICompilationUnit createCompilationUnit(String packageName, String javaFileName, String contents) throws CoreException {
		return _project.createCompilationUnit(packageName, javaFileName, contents);
	}

}