package com.db4o.devtools.ant;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import org.apache.tools.ant.BuildException;
import org.apache.tools.ant.DirectoryScanner;
import org.apache.tools.ant.Task;
import org.apache.tools.ant.types.FileSet;

public abstract class AbstractMultiFileSetTask extends Task {

	private List<FileSet> _fileSets = new ArrayList<FileSet>();	

	public AbstractMultiFileSetTask() {
		super();
	}
	
	protected abstract void workOn(File file) throws Exception;

	protected FileSet newFileSet() {
		FileSet set = new FileSet();
		_fileSets.add(set);
		return set;
	}

	@Override
	public void execute() throws BuildException {
		try {
			for (FileSet fs : _fileSets) {
				DirectoryScanner scanner = fs.getDirectoryScanner(this.getProject());
				for (String fname : scanner.getIncludedFiles()) {
					File file = new File(scanner.getBasedir(), fname);
					workOn(file);
				}
			}
		} catch (Exception x) {
			throw new BuildException(x, getLocation());
		}
	}

}