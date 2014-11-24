package com.db4o.util.file;

import java.io.*;

public interface RandomAccessBuffer extends DataOutput, DataInput, Flushable, Closeable {

	int position() throws IOException;
	void position(int newPosition) throws IOException;

	int length() throws IOException;
	boolean isEmpty();

}
