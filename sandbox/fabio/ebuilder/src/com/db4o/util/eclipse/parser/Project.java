package com.db4o.util.eclipse.parser;

import com.db4o.util.file.*;


public interface Project {

	String name();

	void accept(ProjectVisitor visitor);

	String getRelativePathToRoot(IFile file);

	IFile root();

}
