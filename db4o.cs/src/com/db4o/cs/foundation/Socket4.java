/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.foundation;

import java.io.*;

public interface Socket4 {

	void close() throws IOException;

	void flush() throws IOException;
    
	void setSoTimeout(int timeout);
	
    boolean isConnected();

  	int read(byte[] buffer, int offset, int count) throws IOException;
  	
  	void write(byte[] bytes, int offset, int count) throws IOException;
  	
	Socket4 openParallelSocket() throws IOException;

}