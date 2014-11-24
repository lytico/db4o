package com.db4o.util.eclipse.parser.impl;

import org.w3c.dom.*;

import com.db4o.util.eclipse.parser.*;
import com.db4o.util.file.*;

final class ClasspathImpl {
	private final ProjectImpl project;

	public ClasspathImpl(ProjectImpl project) {
		this.project = project;
	}

	public ProjectImpl project() {
		return project;
	}

	public void accept(final ClasspathVisitor visitor) {

		IFile cp = project().root().file(".classpath");
		
		if (!cp.exists()) {
			return;
		}

		Element root = cp.xml().root();
		
		NodeList list = root.getElementsByTagName("classpathentry");
		for(int i=0;i<list.getLength();i++) {
			Element entry = (Element) list.item(i);
			String kind = entry.getAttribute("kind");
			String path = entry.getAttribute("path");
			
			if ("src".equals(kind)) {
				if (path.startsWith("/")) {
					String projectName = path.substring(1);
					ProjectImpl p = project().workspace().project(projectName);
					if (p != null) {
						visitor.visitExternalProject(p);
					} else {
						visitor.visitUnresolvedDependency(projectName);
					}
				} else {
					visitor.visitSourceFolder(project().root().file(path));
				}
			} else if ("lib".equals(kind)) {
				visitor.visitArchive(file(path));
			} else if ("output".equals(kind)) {
				visitor.visitOutputFolder(project().root().file(path));
			} else if ("var".equals(kind)) {
				String var = path.substring(0, path.indexOf('/'));
				IFile value = project().workspace().variable(var);
				if (value != null) {
					IFile file = value.file(path.substring(path.indexOf('/')+1));
					if (file != null) {
						visitor.visitArchive(file);
						continue;
					}
				}
				visitor.visitUnresolvedDependency(path);
			} else if ("con".equals(kind)) {
				if (path.startsWith("org.eclipse.jdt.USER_LIBRARY")) {
					UserLibrary userLibrary = project().workspace().userLibrary(path.substring(path.indexOf('/')+1));
					if (userLibrary != null) {
						userLibrary.accept(new UserLibraryVisitorAdapter() {
							@Override
							public void visitArchive(IFile file) {
								visitor.visitArchive(file);
							}
						});
						continue;
					}
				} else if (path.startsWith("org.eclipse.jdt.launching.JRE_CONTAINER")) {
					// ignoring jre
					continue;
				}
				visitor.visitUnresolvedDependency(path);
			}
		}
	}
	
	private IFile file(String name) {
		if (name.startsWith("/")) {
			return project().workspace().root().file(name);
		} else {
			return project().root().file(name);
		}
	}
}