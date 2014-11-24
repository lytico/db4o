/* Copyright (C) 2004 - 2006 db4objects Inc. http://www.db4o.com */

package com.db4o.devtools.ant.tests;

import java.io.*;

import javax.xml.xpath.*;

import org.w3c.dom.*;

import com.db4o.devtools.ant.*;

import db4ounit.*;


public class UpdateAssemblyKeyTestCase implements TestLifeCycle {
	
	private String _root;

	public void setUp() throws Exception {
		_root = IO.createFolderStructure("db4o.net", "db4objects.snk");
		IO.createFolderStructure("db4o.net/Db4objects.Db4o", "resource:Db4objects.Db4o-2005.csproj");
		IO.createFolderStructure("db4o.net/Db4oTool/Db4oTool", "resource:Db4oTool-2005.csproj");
	}
	
	public void testOneLevelDeep() throws Exception {
		updateAndAssert("../db4objects.snk", "Db4objects.Db4o/Db4objects.Db4o-2005.csproj");
	}
	
	public void testTwoLevelDeep() throws Exception {
		updateAndAssert("../../db4objects.snk", "Db4oTool/Db4oTool/Db4oTool-2005.csproj");
	}
	
	public void testUpdateSecondTime() throws Exception {
		testOneLevelDeep();
		testOneLevelDeep();
	}

	private void updateAndAssert(final String expectedKeyFile, final String projectFilePath) throws Exception {
		updateProjectFile(projectFilePath);		
		assertKey(expectedKeyFile, projectFilePath);
	}

	private void assertKey(final String expectedKeyFile, final String projectFilePath) throws Exception {
		final CSharpProject project = CSharpProject.load(file(projectFilePath));
		final String signAssemblyXPath = "/Project/PropertyGroup/SignAssembly";
		assertSingleElement(project, signAssemblyXPath);
		assertElementContent("true", project, signAssemblyXPath);
		final String keyFileXPath = "/Project/PropertyGroup/AssemblyOriginatorKeyFile";
		assertSingleElement(project, keyFileXPath);
		assertElementContent(expectedKeyFile, project, keyFileXPath);
	}

	private void assertSingleElement(CSharpProject project, String xpath) throws Exception {
		Assert.areEqual(1, project.selectNodes(xpath).getLength(), xpath);
	}

	private void updateProjectFile(final String projectFilePath)
			throws Exception {
		final UpdateAssemblyKey worker = new UpdateAssemblyKey(file("db4objects.snk"));
		final File projectFile = file(projectFilePath);
		worker.update(projectFile);
	}

	private File file(final String path) {
		return new File(_root, path);
	}

	private void assertElementContent(final String expected, final CSharpProject project, final String xpath)
			throws XPathExpressionException {
		final Element signAssemblyElement = project.selectElement(xpath);
		Assert.isNotNull(signAssemblyElement, xpath);
		Assert.areEqual(expected, signAssemblyElement.getTextContent(), xpath);
	}
	
	public void tearDown() throws Exception {
	}

}
