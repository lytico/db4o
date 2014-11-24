package com.db4o.util.eclipse.parser;

public interface ClasspathEntry {

	void accept(ProjectVisitor visitor);

}
