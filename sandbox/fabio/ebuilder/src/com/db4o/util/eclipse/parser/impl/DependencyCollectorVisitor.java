package com.db4o.util.eclipse.parser.impl;

import java.util.*;

import com.db4o.builder.*;
import com.db4o.util.eclipse.parser.*;

public class DependencyCollectorVisitor extends ProjectClasspathVisitorAdapter {
	private Project project;
	private final Collection<Project> list;

	public DependencyCollectorVisitor(Collection<Project> list) {
		this.list = list;
	}

	@Override
	public void visit(Project project, String name) {
		this.project = project;
	}

	@Override
	public void visitExternalProject(Project project) {
		if (!list.contains(project)) {
			project.accept(new DependencyCollectorVisitor(list));
		}
	}

	@Override
	public void visitEnd() {
		list.add(project);
	}
}
