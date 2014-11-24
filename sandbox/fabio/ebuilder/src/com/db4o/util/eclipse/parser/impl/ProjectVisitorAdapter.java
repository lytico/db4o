package com.db4o.util.eclipse.parser.impl;

import com.db4o.util.eclipse.parser.*;

public class ProjectVisitorAdapter implements ProjectVisitor {

	@Override
	public void visit(Project project, String name) {
	}

	@Override
	public void visitEnd() {
	}

	@Override
	public ClasspathVisitor visitClasspath() {
		return null;
	}

}
