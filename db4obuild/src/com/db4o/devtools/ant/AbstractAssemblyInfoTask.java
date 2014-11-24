/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.devtools.ant;

import java.io.File;
import java.util.regex.*;

import org.apache.tools.ant.types.FileSet;


public abstract class AbstractAssemblyInfoTask extends AbstractMultiFileSetTask {
	
	protected File _currentFile;

	@Override
	protected void workOn(File file) throws Exception {
		_currentFile = file;
		try {
			IO.writeAll(file, updateAttributes(IO.readAll(file)));
		} finally {
			_currentFile = null;
		}
	}

	protected abstract String updateAttributes(String contents);
	
	public FileSet createFileSet() {
		return newFileSet();
	}

	protected String updateAttribute(String contents, String attributeName, String value) {
		Pattern pattern = Pattern.compile(attributeName + "\\s*\\((.+)\\)");
		Matcher matcher = pattern.matcher(contents);
		return matcher.replaceFirst(attributeName + "(\"" + escape(value) + "\")");
	}

	private String escape(String value) {
		return Matcher.quoteReplacement(value);
	}	

}
