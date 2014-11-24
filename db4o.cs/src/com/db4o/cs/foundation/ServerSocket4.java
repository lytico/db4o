package com.db4o.cs.foundation;

import java.io.*;

public interface ServerSocket4 {

	void setSoTimeout(int timeout);

	int getLocalPort();

	Socket4 accept() throws IOException;

	void close() throws IOException;

}