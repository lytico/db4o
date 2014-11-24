package com.db4o.util.eclipse.parser;

import com.db4o.builder.*;
import com.db4o.util.file.*;

public interface Workspace {

	Project project(String name);

	void accept(WorkspaceVisitor visitor);

	void importUserLibrary(IFile xmlFile);

	IFile file(String path);

	void addProjectRoot(IFile file);

	void addVariable(String var, IFile path);

	IFile variable(String var);

}
