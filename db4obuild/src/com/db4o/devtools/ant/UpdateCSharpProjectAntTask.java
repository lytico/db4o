package com.db4o.devtools.ant;

import java.io.File;
import java.io.IOException;
import java.net.URI;
import java.util.ArrayList;
import java.util.List;

import org.apache.tools.ant.BuildException;
import org.apache.tools.ant.DirectoryScanner;
import org.apache.tools.ant.Task;
import org.apache.tools.ant.types.FileSet;

public class UpdateCSharpProjectAntTask extends Task {

	private final List<FileSet> _sources = new ArrayList<FileSet>();
	private final List<FileSet> _resources = new ArrayList<FileSet>();
	private File _projectFile;
	private URI _baseDir;
	private boolean _ignoreNonExisting;
	
	public UpdateCSharpProjectAntTask() {
	}
	
	public FileSet createSources() {
		FileSet set = new FileSet();
		_sources.add(set);
		return set;
	}
	
	public FileSet createResources() {
		FileSet set = new FileSet();
		_resources.add(set);
		return set;
	}
	
	public void setProjectFile(File srcFile) throws IOException {
		_projectFile = srcFile;
		_baseDir = srcFile.getParentFile().toURI();
	}
	
	public void setIgnoreNonExisting(boolean ignore) {
		_ignoreNonExisting = ignore;
	}
	
	@Override
	public void execute() throws BuildException {
		try {
			log("loading '" + _projectFile + "'");
			if (_ignoreNonExisting && !_projectFile.exists()) {
				log("ignoring non existing project '" + _projectFile + "'");
				return;
			}
			
			CSharpProject project = CSharpProject.load(_projectFile);
			
			for (FileSet fs : _sources) {
				project.addFiles(includedFilesBy(fs));
			}
			for (FileSet fs : _resources) {
				project.addResources(includedFilesBy(fs));
			}
			
			log("writing '" + _projectFile + "'");
			log("base source dir is '" + _baseDir + "'");
			project.writeToFile(_projectFile);
			
		} catch (Exception x) {
			throw new BuildException(x, getLocation());
		}
	}

	private String[] includedFilesBy(FileSet fs) {
	    final DirectoryScanner scanner = fs.getDirectoryScanner(this.getProject());
	    final String[] files = scanner.getIncludedFiles();
	    return files;
    }
}
