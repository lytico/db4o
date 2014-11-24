/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.foundation;

import java.io.*;

public abstract class LinePrinter {
	
	public static final LinePrinter NULL_PRINTER = new LinePrinter() {
		
		@Override
		public void println(String str) {
			// do nothing
		}
	}; 
	
	public abstract void println(String str);
	
	public static LinePrinter forPrintStream(final PrintStream ps){
		return new LinePrinter() {
			@Override
			public void println(String str) {
				ps.println(str);
			}
		};
	}

}
