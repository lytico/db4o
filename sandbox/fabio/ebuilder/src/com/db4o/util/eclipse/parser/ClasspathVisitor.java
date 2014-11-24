package com.db4o.util.eclipse.parser;

import com.db4o.util.file.*;

public interface ClasspathVisitor {
	
	void visitArchive(IFile jar);

	void visitSourceFolder(IFile dir);

	void visitOutputFolder(IFile dir);

	void visitExternalProject(Project project);
	
	void visitUnresolvedDependency(String dependency);

}
