package com.db4o.objectmanager.model;

import java.io.*;

public class Db4oFileConnectionSpec extends Db4oConnectionSpec {
	private String filePath;
	
	public Db4oFileConnectionSpec(String path,boolean readOnly) {
		super(readOnly);
		this.filePath=path;
	}

	public String getPath() {
		return filePath;
	}

	/*protected ObjectContainer connectInternal() {
		return Db4o.openFile(filePath);
	}
*/
	public boolean isRemote() {
		return false;
	}

	public String toString() {
        return getFullPath();
    }

    public String getFullPath() {
		return new File(filePath).getAbsolutePath();
	}
}
