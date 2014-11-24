package com.db4o.util.eclipse.parser;


public interface ProjectVisitor {
	
	void visit(Project project, String name);
	
	void visitEnd();
	
	ClasspathVisitor visitClasspath();

}
