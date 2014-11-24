package com.db4o.rmi;

import java.io.*;

public interface Serializer<T> {

	void serialize(DataOutput out, T value) throws IOException;
	
	T deserialize(DataInput in) throws IOException;
	
}
