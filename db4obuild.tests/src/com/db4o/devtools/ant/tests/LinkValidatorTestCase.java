/* Copyright (C) 2009   db4objects Inc.   http://www.db4o.com */
package com.db4o.devtools.ant.tests;

import java.io.*;

import org.apache.tools.ant.*;

import com.db4o.devtools.ant.*;
import com.db4o.foundation.io.*;

import db4ounit.*;

public class LinkValidatorTestCase implements TestLifeCycle {

	@Override
	public void setUp() throws Exception {
		
	}

	@Override
	public void tearDown() throws Exception {
		
	}
	
	public void testValidLinks() throws IOException {
		final String onlyValidLinksFile = "valid-links.html";
		final String fullFilePath = createHTMLTestFile(onlyValidLinksFile, 
															"http://www.db4o.com", 
															"http://msdn.microsoft.com/en-us/vstudio/default.aspx",
															"http://msdn2.microsoft.com/en-us/library/bb397926.aspx");
		
		assertLinks(fullFilePath);
		
	}
	
	public void testInvalidLinks() throws IOException {
		final String fullFilePath = createHTMLTestFile("invalid-links.html", "www.db4o.com/i-dont-believe-we-have-a-file-named-like-this.html");
		Assert.expect(BuildException.class, new CodeBlock() { public void run() throws Throwable {
			assertLinks(fullFilePath);
		}});				
	}

	private void assertLinks(String fullFilePath) {
		LinkCheckTask checker = new LinkCheckTask();
		
		final String logFilePath = Path4.getTempFileName();
		checker.setProtocolFilename(logFilePath);
		checker.setStartURI("file:/" + fullFilePath);
		checker.execute();
	}

	private String createHTMLTestFile(String fileName, String... links) throws IOException {
		final String fullFilePath = Path4.combine(Path4.getTempPath(), fileName);
		
		WriteHTMLFile(fullFilePath, links);
		return fullFilePath;
	}

	private void WriteHTMLFile(String filePath, String[] links) throws IOException {
		FileWriter htmlWriter = new FileWriter(filePath);
		htmlWriter.write("<html><body>");
		int index = 0;
		for (String anchor : links) {
			htmlWriter.write("<a href='" + anchor + "'>" + linkNameFor(index++) + "<a/>");
		}
		htmlWriter.write("</body></html>");
		htmlWriter.close();
	}

	private String linkNameFor(int i) {
		return "Link #" + i;
	}
}
