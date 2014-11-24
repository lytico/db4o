package com.db4o.util.eclipse.parser.impl;

import com.db4o.util.eclipse.parser.*;
import com.db4o.util.file.*;

public class FolderImpl implements Folder {

	private final IFile root;

	public FolderImpl(IFile root) {
		this.root = root;
	}

	@Override
	public IFile path() {
		return root;
	}

}
