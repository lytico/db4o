package com.db4o.devtools.ant;

import java.io.File;

import org.apache.tools.ant.types.*;

public final class UpdateAssemblyKeyTask extends AbstractMultiFileSetTask {

	private File _keyFile;
	
	public File getKeyFile() {
		return _keyFile;
	}
	
	public void setKeyFile(File keyFile) {
		this._keyFile = keyFile;
	}
	
	public FileSet createProjectFiles() {
		return newFileSet();
	}

	@Override
	protected void workOn(File file) throws Exception {
		log("Updated keyfile on '" + file + "' to '" + _keyFile + "'.");
		new UpdateAssemblyKey(_keyFile).update(file);
	}
	
}
