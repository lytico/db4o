/* Copyright (C) 2004 - 2006 db4objects Inc. http://www.db4o.com */

package com.db4o.devtools.ant.tests;

import java.io.*;
import java.util.regex.*;

public class IO {

	private static final String RESOURCE_PREFIX = "resource:";

	public static String getTempPath() throws IOException {
		return new File(System.getProperty("java.io.tmpdir"))
				.getCanonicalPath();
	}

	public static void createFolder(String path) {
		File targetFolder = new File(path);
		if (!targetFolder.exists()) {
			targetFolder.mkdirs();
		}
	}

	static final Pattern FILE_CONTENTS_REGEX = Pattern
			.compile("(.*)\\((.*)\\)");

	public static void createFile(String filePath, String fileContents)
			throws IOException {
		ensureParent(filePath);
		writeFile(filePath, fileContents);
	}

	private static void writeFile(String filePath, String fileContents)
			throws IOException {
		FileWriter writer = new FileWriter(filePath);
		try {
			writer.write(fileContents);
		} finally {
			writer.close();
		}
	}

	private static void ensureParent(String filePath) {
		createFolder(new File(filePath).getParent());
	}

	public static String createFileContents(String parent, String fileReference) throws IOException {

		if (fileReference.startsWith(IO.RESOURCE_PREFIX)) {
			return createFileFromResource(parent, fileReference.substring(IO.RESOURCE_PREFIX.length()));
		}

		Matcher m = FILE_CONTENTS_REGEX.matcher(fileReference);
		if (m.matches()) {
			String filePath = combine(parent, m.group(1));
			String fileContents = m.group(2);
			createFile(filePath, fileContents);
			
			return filePath;
		} else {
			createFolder(parent);
		}
		
		return parent;
	}

	private static String combine(final String parent, final String fname) {
		return parent + "/" + fname;
	}

	private static String createFileFromResource(String parent, String resource)
			throws IOException {
		final String contents = readResource(IO.class, resource);
		String filePath = combine(parent, resource);
		
		createFile(filePath, contents);
		
		return filePath;
	}

	public static String createFolderStructure(String folderName,
			String... files) throws IOException {
		String tempPath = getTempPath();
		String fullFolderPath = combine(tempPath, folderName);

		for (int i = 0; i < files.length; i++) {
			createFileContents(fullFolderPath, files[i]);
		}

		return fullFolderPath;
	}
	
	public static String readResource(final Class anchor, String resourceName) throws IOException {
		InputStream stream = anchor.getResourceAsStream(resourceName);
		if (null == stream) {
			throw new IllegalArgumentException("Resource '" + resourceName + "' not found");
		}
		try {
			return IO.readString(stream);			
		} finally {
			stream.close();
		}
	}

	public static String readString(java.io.InputStream stream)
			throws IOException {
		return readString(new InputStreamReader(stream));
	}

	public static String readString(InputStream stream, String charset)
			throws IOException {
		return readString(new InputStreamReader(stream, charset));
	}

	public static String readString(InputStreamReader reader)
			throws IOException {

		final BufferedReader bufferedReader = new BufferedReader(reader);
		final StringWriter writer = new StringWriter();
		String line = null;
		while (null != (line = bufferedReader.readLine())) {
			writer.write(line);
			writer.write("\n");
		}
		return writer.toString();
	}

}
