package com.db4o.devtools.ant;

import java.io.File;

import org.apache.tools.ant.types.FileSet;

public class UpdateAssemblyHintPath extends AbstractMultiFileSetTask {

	private String _to;

	public UpdateAssemblyHintPath() {
	}

	public FileSet createProjectFiles() {
		return newFileSet();
	}

	public void setTo(String to) {
		_to = to;
	}

	@Override
	protected void workOn(File file) throws Exception {
		updateProjectFile(file);
	}

	private void updateProjectFile(File file) throws Exception {
		log("Looking in '" + file + "' for references to '" + assemblyName() + "'");
		CSharpProject project = CSharpProject.load(file);
		String hintPath = project.getHintPathFor(assemblyName());
		if (hintPath == null) return;
		
		project.setHintPathFor(assemblyName(), _to);
		project.writeToFile(file);
	}

	private String assemblyName() {
		return new File(_to).getName();
	}

}
