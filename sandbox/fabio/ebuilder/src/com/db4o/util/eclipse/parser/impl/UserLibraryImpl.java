package com.db4o.util.eclipse.parser.impl;

import java.util.*;

import com.db4o.util.eclipse.parser.*;

public class UserLibraryImpl implements UserLibrary {

	private final String name;
	private List<String> archives = new ArrayList<String>();
	private final Workspace workspace;

	public UserLibraryImpl(Workspace workspace, String name) {
		this.workspace = workspace;
		this.name = name;
	}

	public void addArchive(String path) {
		archives.add(path);
	}

	@Override
	public void accept(UserLibraryVisitor visitor) {
		visitor.visitStart(name);
		for(String arch : archives) {
			visitor.visitArchive(workspace().file(arch));
		}
		visitor.visitEnd();
	}

	private Workspace workspace() {
		return workspace;
	}

}
