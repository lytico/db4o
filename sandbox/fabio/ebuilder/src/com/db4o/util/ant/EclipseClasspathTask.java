//package com.db4o.util.ant;
//
//import java.io.*;
//import java.util.*;
//
//import org.apache.tools.ant.*;
//
//import com.db4o.util.eclipse.parser.*;
//import com.db4o.util.eclipse.parser.Project;
//import com.db4o.util.file.*;
//
//public class EclipseClasspathTask extends Task {
//
//	private File workspaceRoot;
//	private String projectName;
//	private String outputProperty;
//
//	public File getWorkspaceRoot() {
//		return workspaceRoot;
//	}
//
//	public void setWorkspaceRoot(File file) {
//		this.workspaceRoot = file;
//	}
//
//	public void execute() throws BuildException {
//		
//		
//		Workspace w = EclipsePlatform.openWorkspace(new RealFile(getWorkspaceRoot()));
//
//		Project p = w.project(getProjectName());
//		
//		final Collection<IFile> classpath = new LinkedHashSet<IFile>();
//		
//		p.accept(new ProjectVisitor() {
//
//			public void visitLibrary(IFile classpathEntry) {
//				classpath.add(classpathEntry);
//			}
//			
//		});
//		
//		String path = "";
//		for (IFile file : classpath) {
//			if (path.length() > 0) {
//				path += File.pathSeparatorChar;
//			}
//			path += file.getAbsolutePath();
//		}
//		
//		this.getProject().setProperty(getOutputProperty(), path);
//		
//	}
//
//	public String getProjectName() {
//		return projectName;
//	}
//
//	public void setProjectName(String name) {
//		this.projectName = name;
//	}
//
//	public String getOutputProperty() {
//		return outputProperty;
//	}
//
//	public void setOutputProperty(String outputProperty) {
//		this.outputProperty = outputProperty;
//	}
//
//
//
//}
