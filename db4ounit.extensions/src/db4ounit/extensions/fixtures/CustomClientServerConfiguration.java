package db4ounit.extensions.fixtures;

import com.db4o.config.*;

import db4ounit.extensions.*;

public interface CustomClientServerConfiguration extends Db4oTestCase {

	void configureServer(Configuration config) throws Exception;

	void configureClient(Configuration config) throws Exception;

}
