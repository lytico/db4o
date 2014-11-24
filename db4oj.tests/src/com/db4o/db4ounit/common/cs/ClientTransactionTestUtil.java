/* Copyright (C) 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.db4ounit.common.cs;

import com.db4o.foundation.io.*;

final class ClientTransactionTestUtil {
	
		static final String FILENAME_A = Path4.getTempFileName();
		static final String FILENAME_B = Path4.getTempFileName();
		public static final String MAINFILE_NAME = Path4.getTempFileName();
	
		private ClientTransactionTestUtil() {
		}
	
		static void deleteFiles() {
			File4.delete(MAINFILE_NAME);
			File4.delete(FILENAME_A);
			File4.delete(FILENAME_B);
		}			
}