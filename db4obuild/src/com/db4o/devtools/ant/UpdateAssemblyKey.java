/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.devtools.ant;

import java.io.*;
import java.net.*;

import javax.xml.xpath.*;

import org.w3c.dom.*;

public class UpdateAssemblyKey {

	private static final String ASSEMBLY_ORIGINATOR_KEY_FILE = "AssemblyOriginatorKeyFile";
	private static final String SIGN_ASSEMBLY = "SignAssembly";
	
	private final File _keyFile;

	public UpdateAssemblyKey(File keyFile) {
		_keyFile = keyFile;
	}

	public void update(File projectFile) throws Exception {
		CSharpProject project = CSharpProject.load(projectFile);
		updatePropertyGroupElement(project, SIGN_ASSEMBLY, "true");
		updatePropertyGroupElement(project, ASSEMBLY_ORIGINATOR_KEY_FILE, relativeKeyfilePath(projectFile));
		project.writeToFile(projectFile);
	}

	private void updatePropertyGroupElement(CSharpProject project,
			final String tagName, final String value)
			throws XPathExpressionException {
		final Element group = propertyGroup(project);
		removeAll(group, tagName);
		group.appendChild(project.createElement(tagName, value));
	}

	private void removeAll(final Element group, final String tagName) {
		final NodeList nodes = group.getElementsByTagName(tagName);
		for (int i=0; i<nodes.getLength(); ++i) {
			group.removeChild(nodes.item(i));
		}
	}

	private Element propertyGroup(CSharpProject project)
			throws XPathExpressionException {
		return project.selectElement("Project/PropertyGroup");
	}

	private String relativeKeyfilePath(File projectFile) {
		final String relativeProjectFile = relativePath(projectFile);
		final int folderCount = folderCount(relativeProjectFile);
		StringBuilder path = new StringBuilder();
		for (int i = 0; i < folderCount; i++) {
			path.append("../");
		}
		return path.append(_keyFile.getName()).toString();
	}

	private int folderCount(final String relativeProjectFile) {
		return relativeProjectFile.split("/").length - 1;
	}

	private String relativePath(File projectFile) {
		final URI parentUri = projectFile.toURI().normalize();
		final URI keyUri = _keyFile.getParentFile().toURI().normalize();
		final String relativeProjectFile = keyUri.relativize(parentUri).toString();
		return relativeProjectFile;
	}
}
