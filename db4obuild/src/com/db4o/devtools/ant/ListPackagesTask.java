package com.db4o.devtools.ant;

import java.io.*;
import java.util.*;

import org.apache.tools.ant.*;

public class ListPackagesTask extends Task {

	private String _version;
	private String _property;
	private List<PackagesBaseDir> _baseDirs = new ArrayList<PackagesBaseDir>();
	private String _extension;
	
	public void setProperty(String property) {
		_property = property;
	}

	public void setVersion(String version) {
		_version = version;
	}
	
	public void setExtension(String extension) {
		_extension = extension;
	}
	
	public void addDir(PackagesBaseDir dir) {
		_baseDirs.add(dir);
	}
	
	@Override
	public void execute() throws BuildException {
		try {
			List<String> packages = new ArrayList<String>();
			for (PackagesBaseDir curDir : _baseDirs) {
				collectPackages(curDir.getPath(), _extension, packages);
			}
			getProject().setProperty(_property, joinPackages(packages, _version));
		} 
		catch (IOException exc) {
			throw new BuildException(exc);
		}
	}

	private static void collectPackages(String baseDirName, final String extension, List<String> packages) throws IOException {
		File baseDir = new File(baseDirName);
		String baseDirPath = baseDir.getCanonicalPath();
		LinkedList<File> pendingDirs = new LinkedList<File>();
		pendingDirs.add(baseDir);
		while(!pendingDirs.isEmpty()) {
			File curDir = pendingDirs.removeFirst();
			File[] subDirs = curDir.listFiles(new FileFilter(){
				public boolean accept(File file) {
					return file.isDirectory();
				}
			});
			pendingDirs.addAll(Arrays.asList(subDirs));
			File[] classFiles = curDir.listFiles(new FileFilter() {
				public boolean accept(File path) {
					return path.getName().endsWith(extension);
				}
			});
			if(classFiles.length > 0 && !baseDir.equals(curDir)) {
				packages.add(packageName(baseDirPath, curDir));
			}
		}
	}

	private static String joinPackages(List<String> packages, String version) {
		StringBuilder builder = new StringBuilder();
		boolean isFirst = true;
		for (String pkg : packages) {
			if(!isFirst) {
				builder.append(",\n");
			}
			builder.append(' ').append(pkg);
			if(version != null) {
				builder.append("; version=\"" + version +"\"");
			}
			isFirst = false;
		}
		return builder.toString();
	}

	private static String packageName(String baseDirPath, File curDir) throws IOException {
		String curDirPath = curDir.getCanonicalPath();
		String packageSegment = curDirPath.substring(baseDirPath.length() + 1);
		return packageSegment.replace(File.separatorChar, '.');
	}

	public static class PackagesBaseDir {
		private String _path;
		
		public void setPath(String path) {
			_path = path;
		}
		
		public String getPath() {
			return _path;
		}
	}
}
