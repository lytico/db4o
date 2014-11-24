/* Copyright (C) 2004 - 2007 Versant Inc. http://www.db4o.com */

package com.db4o.bench.util;

/**
 * 
 * @sharpen.ignore
 *
 */
public class IoBenchmarkArgumentParser {
	
	private String _resultsFile2;
	private String _resultsFile1;
	private int _objectCount;
	private boolean _delayed;

	public IoBenchmarkArgumentParser(String[] arguments) {
		validateArguments(arguments);
	}
		
	private void validateArguments(String[] arguments) {
		if (arguments.length != 1 && arguments.length != 3) {
			System.out.println("Usage: IoBenchmark <object-count> [<results-file-1> <results-file-2>]");
			throw new RuntimeException("Usage: IoBenchmark <object-count> [<results-file-1> <results-file-2>]");
		}
		_objectCount = java.lang.Integer.parseInt(arguments[0]);
		if (arguments.length > 1) {
			_resultsFile1 = arguments[1];
			_resultsFile2 = arguments[2];
			_delayed = true;
		}
	}


	public int objectCount() {
		return _objectCount;
	}
	
	public String resultsFile1() {
		return _resultsFile1;
	}
	
	public String resultsFile2() {
		return _resultsFile2;
	}
	
	public boolean delayed() {
		return _delayed;
	}
}
