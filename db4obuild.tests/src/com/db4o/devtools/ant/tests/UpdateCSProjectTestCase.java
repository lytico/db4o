/* Copyright (C) 2011 Versant Inc. http://www.db4o.com */
package com.db4o.devtools.ant.tests;

import java.io.*;

import javax.xml.xpath.*;

import com.db4o.devtools.ant.*;
import com.db4o.foundation.io.*;

import db4ounit.*;

public class UpdateCSProjectTestCase implements TestCase {
	
	public void testNativeContentIsNotToutched() throws Exception {
		
		String filePath = IO.createFileContents(Path4.getTempPath() + "Tests", "resource:TestNativeFolders.csproj");		
		try {
			CSharpProject project = CSharpProject.load(new File(filePath));			
			
			int originalNativeCount = nativeFilesCountIn(project);			
			
			assertThereAreNativeFiles(originalNativeCount);			
			assertThereAreNonNativeFiles(project, originalNativeCount);
			
			project.addFile("foo.cs");
			Assert.areEqual(0, nativeFilesCountIn(project));
			
			project.addFile("native/bar.cs");
			Assert.areEqual(1, nativeFilesCountIn(project));
		} 
		finally {
			File4.delete(filePath);
		}		
		
	}

	private void assertThereAreNativeFiles(int originalNativeCount) {
		Assert.isGreater(0, originalNativeCount);
	}

	private void assertThereAreNonNativeFiles(CSharpProject project, int originalNativeCount) throws XPathExpressionException {
		Assert.isGreater(originalNativeCount, totalFileCount(project));
	}

	private int totalFileCount(CSharpProject project) throws XPathExpressionException {
		return project.selectNodes("//*").getLength();
	}

	private int nativeFilesCountIn(CSharpProject project) throws XPathExpressionException {
		return 	project.selectNodes("//*[starts-with(@Include, 'native\\')]").getLength();
	}
}
