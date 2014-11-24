package com.db4o.util.eclipse.parser.impl;

import com.db4o.util.file.*;

public interface UserLibraryVisitor {

	void visitStart(String name);

	void visitEnd();

	void visitArchive(IFile file);

}
