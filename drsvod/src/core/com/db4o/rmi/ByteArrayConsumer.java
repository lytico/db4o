package com.db4o.rmi;

import java.io.*;

public interface ByteArrayConsumer {

	void consume(byte[] buffer, int offset, int length) throws IOException;

}
