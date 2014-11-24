package db4ounit.extensions;

import com.db4o.*;

public interface ContainerBlock {
	void run(ObjectContainer client) throws Throwable;
}
