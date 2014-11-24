/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.tools;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;

/**
 * prints statistics about a database file to System.out.
 * <br><br>Pass the database file path as an argument.
 * <br><br><b>This class is not part of db4o.jar!</b><br>
 * It is delivered as sourcecode in the
 * path ../com/db4o/tools/<br><br>
 */
public class Statistics {

	/**
	 * the main method that runs the statistics.
	 * @param args a String array of length 1, with the name of the database
	 * file as element 0.
	 */
	public static void main(String[] args) {
		if (args == null || args.length != 1) {
			System.out.println("Usage: java com.db4o.tools.Statistics <database filename>");
		} else {
			new Statistics().run(args[0]);
		}
	}

	public void run(String filename) {
		if (new java.io.File(filename).exists()) {
			ObjectContainer con = null;
			try {
				Configuration config = Db4o.newConfiguration();
				config.messageLevel(-1);
				con = Db4o.openFile(config, filename);
				printHeader("STATISTICS");
				System.out.println("File: " + filename);
				printStats(con, filename);
				con.close();
			} catch (Exception e) {
				System.out.println("Statistics failed for file: '" + filename + "'");
				System.out.println(e.getMessage());
				e.printStackTrace();
			}
		} else {
			System.out.println("File not found: '" + filename + "'");
		}
	}
	
	private static boolean canCallConstructor(String className) {
		return ReflectPlatform.createInstance(className) != null;
	}

	private void printStats(ObjectContainer con, String filename) {

		Tree unavailable = new TreeString(REMOVE);
		Tree noConstructor = new TreeString(REMOVE);

		// one element too many, substract one in the end
		StoredClass[] internalClasses = con.ext().storedClasses();
		for (int i = 0; i < internalClasses.length; i++) {
			String internalClassName = internalClasses[i].getName();
			Class clazz = ReflectPlatform.forName(internalClassName);
			if(clazz == null) {
				unavailable = unavailable.add(new TreeString(internalClassName));
			} else {
				if(!canCallConstructor(internalClassName)) {
					noConstructor =	noConstructor.add(new TreeString(internalClassName));
				}
			}
		}
		unavailable = unavailable.removeLike(new TreeString(REMOVE));
		noConstructor = noConstructor.removeLike(new TreeString(REMOVE));
		if (unavailable != null) {
			printHeader("UNAVAILABLE");
			unavailable.traverse(new Visitor4() {
				public void visit(Object obj) {
					System.out.println(((TreeString) obj)._key);
				}
			});
		}
		if (noConstructor != null) {
			printHeader("NO PUBLIC CONSTRUCTOR");
			noConstructor.traverse(new Visitor4() {
				public void visit(Object obj) {
					System.out.println(((TreeString) obj)._key);
				}
			});
		}

		printHeader("CLASSES");
		System.out.println("Number of objects per class:");
		
		final ByRef<Tree<Integer>> ids = ByRef.<Tree<Integer>>newInstance(new TreeInt(0));

		if (internalClasses.length > 0) {
			Tree all = new TreeStringObject(internalClasses[0].getName(), internalClasses[0]);
			for (int i = 1; i < internalClasses.length; i++) {
				all =
					all.add(
						new TreeStringObject(internalClasses[i].getName(), internalClasses[i]));
			}
			all.traverse(new Visitor4() {
				public void visit(Object obj) {
					TreeStringObject node = (TreeStringObject) obj;
					long[] newIDs = ((StoredClass)node._value).getIDs();
					for (int j = 0; j < newIDs.length; j++) {
						if (ids.value.find(new TreeInt((int) newIDs[j])) == null) {
							ids.value = ids.value.add(new TreeInt((int) newIDs[j]));
						}
					}
					System.out.println(node._key + ": " + newIDs.length);
				}
			});

		}

		printHeader("SUMMARY");
		System.out.println("File: " + filename);
		System.out.println("Stored classes: " + internalClasses.length);
		if (unavailable != null) {
			System.out.println("Unavailable classes: " + unavailable.size());
		}
		if (noConstructor != null) {
			System.out.println("Classes without public constructors: " + noConstructor.size());
		}
		System.out.println("Total number of objects: " + (ids.value.size() - 1));
	}
	
	private void printHeader(String str){
		int stars = (39 - str.length()) / 2;
		System.out.println("\n");
		for (int i = 0; i < stars; i++) {
			System.out.print("*");
		}
		System.out.print(" " + str + " ");
		for (int i = 0; i < stars; i++) {
			System.out.print("*");
		}
		System.out.println();
	}

	private static final String REMOVE = "XXxxREMOVExxXX";

}
