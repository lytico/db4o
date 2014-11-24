/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit;

import java.io.*;

import javax.swing.JTable.*;

public class ConsoleListener implements TestListener {
	
	private final Writer _writer;
	
	public ConsoleListener(Writer writer) {
		_writer = writer;
	}

	public void runFinished() {
	}

	public void runStarted() {
	}

	public void testFailed(Test test, Throwable failure) {
		printFailure(failure);
	}

	public void testStarted(Test test) {
		print(test.label());
	}
	
	private void printFailure(Throwable failure) {
		if (failure == null) {
			print("\t!");
		} else {
			print("\t! " + failure);
		}
	}
	
	private void print(String message) {
		try {
			_writer.write(message + TestPlatform.NEW_LINE);
			_writer.flush();
		} catch (IOException x) {
			TestPlatform.printStackTrace(_writer, x);
		}
	}

	public void failure(String msg, Throwable failure) {
		print("\t ! " + msg);
		printFailure(failure);
	}
}