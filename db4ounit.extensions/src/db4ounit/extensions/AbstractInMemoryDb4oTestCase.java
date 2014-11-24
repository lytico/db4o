package db4ounit.extensions;

import com.db4o.config.*;
import com.db4o.io.*;

import db4ounit.extensions.fixtures.*;

public class AbstractInMemoryDb4oTestCase extends AbstractDb4oTestCase implements OptOutMultiSession {
	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.storage(new MemoryStorage());
	}

}
