/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package db4ounit.extensions.fixtures;

import com.db4o.*;

import db4ounit.extensions.*;

public interface Db4oClientServerFixture extends Db4oFixture, MultiSessionFixture {
	
	public ObjectServer server();
	
	public int serverPort();
}
