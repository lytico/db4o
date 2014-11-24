package com.db4o.util;
/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */


/**
 * This file was taken from the db4oj.tests project from
 * the com.db4o.db4ounit.util package. 
 * TODO: move to own project and supply as separate Jar.
 */


import java.io.*;

public class File4 {
	
	public static byte[] readAllBytes(final String fname) 
		throws FileNotFoundException, IOException {
			
		return readAllBytes(new File(fname));
	}

	public static byte[] readAllBytes(final File file) throws FileNotFoundException, IOException {
		byte[] bytes = new byte[(int)file.length()];
		final FileInputStream fis = new FileInputStream(file);
		try {
			fis.read(bytes);
		} finally {
			fis.close();
		}
		return bytes;
	}

	public static void rename(String oldPath,String newPath) throws IOException {
		if(!new File(oldPath).renameTo(new File(newPath))) {
			throw new IOException("Could not rename '"+oldPath+"' to '"+newPath+"'.");
		}
	}
	
    public static void copy(String source, String target) throws IOException {
		java.io.File sourceFile = new java.io.File(source);

		java.io.File targetFile = new java.io.File(target);
		targetFile.mkdirs();
		File4.delete(target);

		if (sourceFile.isDirectory()) {
			copyDirectory(sourceFile, targetFile);
		} else {
			copyFile(sourceFile, targetFile);
		}
	}

	public static void copyFile(File source, File target) throws IOException {
		final int bufferSize = 64000;

		RandomAccessFile rafIn = new RandomAccessFile(source.getAbsolutePath(), "r");
		RandomAccessFile rafOut = new RandomAccessFile(target.getAbsolutePath(), "rw");
		final long len = rafIn.length();
		final byte[] bytes = new byte[bufferSize];

		long totalRead=0;
		while (totalRead<len) {
		    int numRead=rafIn.read(bytes,0,bufferSize);
		    rafOut.write(bytes,0,numRead);
		    totalRead+=numRead;
		}

		rafIn.close();
		rafOut.close();
	}

	private static void copyDirectory(File source, File target) throws IOException {
		String[] files = source.list();
		if (files != null) {
		    for (int i = 0; i < files.length; i++) {
		        copy(Path4.combine(source.getAbsolutePath(), files[i]),
		        	Path4.combine(target.getAbsolutePath(), files[i]));
		    }
		}
	}
	
    public static void delete(String fname) {
		final File file = new File(fname);
		if (!file.exists()) {
			return;
		}
		translateDeleteFailureToException(file);
	}

	static void translateDeleteFailureToException(final File file) {
		if (!file.delete()) {
			throw new RuntimeException("Could not delete '" + file.getName() + "'.");
		}
	}
    
    public static boolean exists(String fname){
        return new File(fname).exists();
    }

    public static void mkdirs(String path) {
		new File(path).mkdirs();
	}

	public static long size(final String filePath) {
		File f = new File(filePath);
		return f.length();
	}
}
