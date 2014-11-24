package com.db4o.devtools.ant.tests;

import java.io.*;
import java.util.*;

import com.db4o.devtools.ant.*;

import db4ounit.*;

public class FolderDiffTestCase implements TestCase, TestLifeCycle {

	private String folder1Path; 
	private String folder2Path; 
	private String folder3Path;
	
	public void setUp() throws Exception {
		
		folder1Path = IO.createFolderStructure(
						"Folder1",
						"A/f1.txt(file1 contents)",
						"A/f2.txt(file2 contents)",
						"A/B/f3.txt(file3 contents)",
						"A/B/C/f4.txt(file4 contents)");
		
		folder2Path = IO.createFolderStructure(
						"Folder2",
						
						"A/f1_new.txt(file1 new contents)", 
						"A/f2.txt(file2 contents)", 
						"A/B/f3.txt(file3 changed contents)", 
						"A/B/C/f4.txt(file4 contents)", 
						"D/f5.txt()");
		
		folder3Path = IO.createFolderStructure(
						"Folder3", 
						
						"A/f2.txt(contents file2)",
						"B/f3.txt(file3 contents)",
						"A/B/C/f1.txt(file1 contents)");
		
	}
	
	public void testCommonCases() throws Throwable {
		assertFolder(
				FolderDiff.diff(folder1Path, folder2Path),
				"-/A/f1.txt",
				"+/A/f1_new.txt", 
				"+/D/f5.txt",
				"C/A/B/f3.txt");                           
		
		assertFolder(
				FolderDiff.diff(folder2Path, folder1Path),
				"-/A/f1_new.txt", 
		        "-/D/f5.txt",                           
				"+/A/f1.txt",
				"C/A/B/f3.txt");
		
		assertFolder(
				FolderDiff.diff(folder1Path, folder3Path),
				"-/A/f1.txt",
				"-/A/B/f3.txt",
				"-/A/B/C/f4.txt",
				"+/B/f3.txt",
		        "+/A/B/C/f1.txt");			
	}
	
	public void testBoundaryConditions() throws Throwable {
		final String emptyFolderPath = IO.createFolderStructure("emptyFolder");
		assertFolder(FolderDiff.diff(emptyFolderPath, emptyFolderPath));
		assertFolder(FolderDiff.diff(folder1Path, folder1Path));
		assertFolder(
				FolderDiff.diff(emptyFolderPath, folder1Path),
				"+/A/f1.txt",
				"+/A/f2.txt",
				"+/A/B/f3.txt",
				"+/A/B/C/f4.txt");
		
		assertFolder(
				FolderDiff.diff(folder1Path, emptyFolderPath),
				"-/A/f1.txt",
				"-/A/f2.txt",
				"-/A/B/f3.txt",
				"-/A/B/C/f4.txt");
	}
	
	public void testFolderFilter() throws Throwable {
		
		final String folderWithIgnoredSubFolders = IO.createFolderStructure(
				"FolderWithIgnoredSubFolders", 
				
				"A/.svn/f1.txt(file in svn folder!)",
				"A/.svn/f5.txt(file in svn folder!)",
				"A/.svn/f6.txt(file in svn folder!)",
				"A/.svn/f7.txt(file in svn folder!)",
				"A/f1.txt(Fiona)",
				"A/f2.txt(file2 contents)",
				"A/B/f3.txt(contents file3)",
				"A/f4.txt(new file1)",
				"A/f4.txt.new(new file 2)");

		assertFolder(
				FolderDiff.diff(folder1Path, folderWithIgnoredSubFolders, new FilterFoldersInList(new String[] {".svn"}) ),
				"-/A/B/C/f4.txt",
				"+/A/f4.txt",
				"+/A/f4.txt.new");
	}
	
	private void assertFolder(FolderDiff diff, String ... expectedDiffs) {
		Assert.isNotNull(diff);
		
		if (expectedDiffs.length == 0) {
			assertEmpty(diff.changedFiles());
			assertEmpty(diff.deletedFiles());
			assertEmpty(diff.newFiles());
			return;
		}
		
		for (String diffItem : expectedDiffs) {
			switch (diffItem.charAt(0)) {
				case '+':
					assertFileAdded(diff, normalizePath(diffItem.substring(1)));
					break;
					
				case '-':
					assertFileRemoved(diff, normalizePath(diffItem.substring(1)));
					break;
					
				case 'C':
					assertFileChanged(diff, normalizePath(diffItem.substring(1)));
			}
		}
	}

	private void assertEmpty(final Set<String> files) {
		Assert.isTrue(files.isEmpty());
	}

	private String normalizePath(String substring) {
		return substring.replace('/', File.separatorChar);
	}
	
	private void assertFileChanged(FolderDiff diff, String file) {
		assertContains(file, diff.changedFiles(), "C");
	}

	private void assertFileRemoved(FolderDiff diff, String file) {
		assertContains(file, diff.deletedFiles(), "-");
	}

	private void assertFileAdded(FolderDiff diff, String file) {
		assertContains(file, diff.newFiles(), "+");
	}

	private void assertContains(String expectedFile, final Set<String> files, final String operation) {
		Assert.isTrue(files.contains(expectedFile), "Expecting '" + operation + expectedFile + "'. Found '" + files + "'");
	}
	
	public void tearDown() throws Exception {
	}
}
