package com.db4o.builder;

import java.io.*;

import org.eclipse.jdt.core.compiler.*;
import org.eclipse.jdt.core.compiler.batch.*;

import com.db4o.util.file.*;

public class EclipseCompiler implements Compiler {

	private StringBuilder classpath = new StringBuilder();
	private StringBuilder sources = new StringBuilder();
	private Version target = Version.Java16;
	private Version source = Version.Java16;
	private boolean debug = false;
	private String targetFolder = null;
	private PrintWriter out;
	private PrintWriter err;

	@Override
	public boolean compile() {
		
		if (sources.length() == 0) {
			return false;
		}

		CompilationProgress progress = new CompilationProgress() {

			@Override
			public void worked(int arg0, int arg1) {
			}

			@Override
			public void setTaskName(String arg0) {
			}

			@Override
			public boolean isCanceled() {
				return false;
			}

			@Override
			public void done() {
			}

			@Override
			public void begin(int arg0) {
			}
		};
		String commandLine = "-d " + targetFolder + " -warn:none " + (debug ? "-g" : "-g:none") + " -source " + source + " -target " + target
				+ (classpath.length() > 0 ? " -classpath " + classpath : "");
//		System.out.println("   ecj "+commandLine);
		return BatchCompiler.compile(commandLine + " " + sources, out, err, progress);
	}

	@Override
	public void sourceVersion(Version version) {
		source = version;
	}

	@Override
	public void targetVersion(Version version) {
		target = version;
	}

	@Override
	public void debugEnabled() {
		debug = true;
	}

	@Override
	public void targetFolder(String path) {
		targetFolder = path;
	}

	@Override
	public void addClasspathEntry(IFile jar) {
		if (classpath.length() > 0) {
			classpath.append(File.pathSeparatorChar);
		}
		classpath.append(jar.getAbsolutePath());
	}

	@Override
	public void addSourceFile(IFile source) {
		if (sources.length() > 0) {
			sources.append(' ');
		}
		sources.append(source.getAbsolutePath());
	}

	@Override
	public void outputWriter(PrintWriter out) {
		this.out = out;
	}

	@Override
	public void errorWriter(PrintWriter out) {
		err = out;
	}

}
