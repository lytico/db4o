package com.db4o.util.eclipse.parser.impl;

import org.w3c.dom.*;

import com.db4o.util.eclipse.parser.*;
import com.db4o.util.file.*;

final class ProjectImpl implements Project {
	private final EclipseWorkspace workspace;
	private final IFile root;
	private IFile outputDir;
	private ClasspathImpl classpath;
	private String name = null;

	public ProjectImpl(EclipseWorkspace workspace, IFile root) {
		this.workspace = workspace;
		this.root = root;
	}

	public EclipseWorkspace workspace() {
		return workspace;
	}

	public IFile getOutpuDir() {
		return outputDir;
	}

	public void setOutputDir(IFile outputDir) {
		this.outputDir = outputDir;
	}

	@Override
	public IFile root() {
		return root;
	}

	public ClasspathImpl classpath() {
		if (classpath == null) {
			classpath = new ClasspathImpl(this);
		}
		return classpath;
	}

	@Override
	public void accept(ProjectVisitor visitor) {

		if (name == null) {
			Element root = root().file(".project").xml().root();
			Node nameNode = root.getElementsByTagName("name").item(0);
			name = nameNode.getTextContent();
		}
		
		visitor.visit(this, name);
		
		ClasspathVisitor cpv = visitor.visitClasspath();
		if (cpv != null) {
			classpath().accept(cpv);
		}

		visitor.visitEnd();
	}

	@Override
	public String name() {
		if (name != null) {
			accept(new ProjectVisitorAdapter());
		}
		return name;
	}

	@Override
	public String getRelativePathToRoot(IFile file) {
		return file.getRelativePathTo(root);
	}
	
}
