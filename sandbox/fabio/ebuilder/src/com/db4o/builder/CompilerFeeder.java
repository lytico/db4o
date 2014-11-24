package com.db4o.builder;

import java.util.*;

import com.db4o.util.eclipse.parser.*;
import com.db4o.util.file.*;

public class CompilerFeeder extends ProjectClasspathVisitorAdapter {
	private final Compiler ecj;
	
	private Set<String> knownProjects = new HashSet<String>();
	private Set<IFile> knownSources = new HashSet<IFile>();
	private Set<IFile> knownArchives = new HashSet<IFile>();

	public CompilerFeeder(Compiler ecj) {
		this.ecj = ecj;
	}
	
	@Override
	public void visit(Project project, String name) {
		System.out.println("adding project: " + name);
		knownProjects.add(name);
	}

	@Override
	public void visitArchive(IFile jar) {
		if (knownArchives.add(jar)) {
			System.out.println("jar: "+ jar.name());
			ecj.addClasspathEntry(jar);
		}
	}

	@Override
	public void visitSourceFolder(IFile sourceFolder) {
		sourceFolder.accept(new FileVisitor() {
			
			@Override
			public void visit(IFile child) {
				if (child.isFile() && child.name().toLowerCase().endsWith(".java")) {
					if (knownSources.add(child)) {
						ecj.addSourceFile(child);
					}
				} else if (child.isDirectory()) {
					child.accept(this);
				}
			}
		});
	}

	@Override
	public void visitExternalProject(Project project) {
		if (knownProjects.add(project.name())) {
			project.accept(this);
		}
	}

	@Override
	public void visitUnresolvedDependency(String token) {
		System.err.println("Unresolved dependency: " + token);
	}
}