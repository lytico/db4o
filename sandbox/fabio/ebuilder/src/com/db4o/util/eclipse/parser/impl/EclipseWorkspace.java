package com.db4o.util.eclipse.parser.impl;

import java.util.*;

import org.w3c.dom.*;

import com.db4o.builder.*;
import com.db4o.util.eclipse.parser.*;
import com.db4o.util.file.*;

public final class EclipseWorkspace implements Workspace {

	private final IFile root;
	private Map<String, ProjectImpl> projects = new HashMap<String, ProjectImpl>();
	private boolean projectsResolved = false;
	private Map<String, UserLibraryImpl> userLibraries = new HashMap<String, UserLibraryImpl>();
	private Map<String, IFile> variables = new HashMap<String, IFile>();

	public EclipseWorkspace(IFile root) {
		this.root = root;
	}

	public IFile root() {
		return root;
	}

	private ProjectImpl project(IFile path) {
		if (path.name().indexOf('/') != -1) {
			throw new IllegalArgumentException("Invalid file name '"+path.name()+"'");
		}
		final ProjectImpl project = new ProjectImpl(this, path);
		project.accept(new ProjectVisitorAdapter() {
			@Override
			public void visit(Project p, String name) {
				projects.put(name, project);
			}
		});
		return project;
	}

	@Override
	public ProjectImpl project(String name) {
		ProjectImpl p = tryToResolveProject(name);
//		if (p == null) {
//			throw new IllegalArgumentException("Project '"+name+"' not found");
//		}
		return p;
	}

	private ProjectImpl tryToResolveProject(String name) {
		ProjectImpl p = projects.get(name);
		if (!projectsResolved && p == null) {
			resolveProjects();
			p = projects.get(name);
		}
		return p;
	}
	
	protected void resolveProjects() {
		projectsResolved = true;
		accept(new WorkspaceVisitor() {
			@Override
			public void visitProject(Project project) {
				// dont need to do anything coz we already cache the project in #accept()
			}
		});
	
	}

	@Override
	public void accept(final WorkspaceVisitor visitor) {
		root.accept(new FileVisitor() {
			@Override
			public void visit(IFile child) {
				if (child.exists(".project")) {
					visitor.visitProject(project(child));
				} else if (!".metadata".equals(child.name())) {
					child.accept(this);
				}
			}
		}, FileVisitor.DIRECTORY);
	}

	public UserLibrary userLibrary(String userLibrary) {
		return userLibraries.get(userLibrary);
	}

	@Override
	public void importUserLibrary(IFile xmlFile) {
		NodeList list = xmlFile.xml().root().getElementsByTagName("library");
		for (int i=0;i<list.getLength();i++) {
			Element lib = (Element) list.item(i);
			String name = lib.getAttribute("name");
			UserLibraryImpl ul = new UserLibraryImpl(this, name);
			NodeList paths = lib.getElementsByTagName("archive");
			for(int j=0;j<paths.getLength();j++) {
				ul.addArchive(((Element)paths.item(j)).getAttribute("path"));
			}
			userLibraries.put(name, ul);
		}
	}

	@Override
	public IFile file(String path) {
		
		int secondSlash = path.indexOf("/", 1);
		String projectName = path.substring(1, secondSlash);
		
		ProjectImpl p = tryToResolveProject(projectName);
		if (p != null) {
			return p.root().file(path.substring(secondSlash+1));
		}
		
		return root().file(path);
		
	}

	@Override
	public void addProjectRoot(IFile file) {
		file.accept(new FileVisitor() {
			@Override
			public void visit(IFile child) {
				if (child.exists(".project")) {
					project(child);
				} else {
					child.accept(this);
				}
			}
		}, FileVisitor.DIRECTORY);
	}

	@Override
	public void addVariable(String var, IFile path) {
		variables.put(var, path);
	}
	
	@Override
	public IFile variable(String var) {
		return variables.get(var);
	}
}