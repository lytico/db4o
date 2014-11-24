package com.db4o.devtools.ant;

import java.io.*;

import org.apache.tools.ant.*;

public class DumpStderrTask extends Task {

	private File file;

	public File getFile() {
		return file;
	}

	public void setFile(File file) {
		this.file = file;
	}

	public void execute() throws BuildException {
		try {
			BufferedReader in = new BufferedReader(new FileReader(file));
			String line;
			while ((line = in.readLine()) != null) {
				System.err.println(line);
			}
			in.close();
		} catch (FileNotFoundException e) {
			throw new BuildException(e);
		} catch (IOException e) {
			throw new BuildException(e);
		}
	}

}
