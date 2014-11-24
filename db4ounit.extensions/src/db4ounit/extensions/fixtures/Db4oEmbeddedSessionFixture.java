/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package db4ounit.extensions.fixtures;

import com.db4o.ext.*;

import db4ounit.extensions.*;

public class Db4oEmbeddedSessionFixture extends AbstractFileBasedDb4oFixture
		implements MultiSessionFixture {
	
	private static final String FILE = "db4oEmbeddedSession.db4o";
	private final String _label;
	private ExtObjectContainer _session;
	
	public Db4oEmbeddedSessionFixture(String label) {
		_label = label;
	}
	
	public Db4oEmbeddedSessionFixture() {
		this("E/S");
	}
	
	public String label() {
		return buildLabel(_label);
	}
	
	@Override
	public ExtObjectContainer db() {
		return _session;
	}

	@Override
	protected String fileName() {
		return FILE;
	}
	
	@Override
	public boolean accept(Class clazz) {
		if (!Db4oTestCase.class.isAssignableFrom(clazz)) {
			return false;
		}
		if (OptOutMultiSession.class.isAssignableFrom(clazz)) {
			return false;
		}
		if (OptOutAllButNetworkingCS.class.isAssignableFrom(clazz)) {
			return false;
		}
		return true;
	}
	
	@Override
	protected void postOpen(Db4oTestCase testInstance) {
		_session = openNewSession(testInstance);
	}
	
	@Override
	protected void preClose() {
		if (null != _session) {
			_session.close();
		}
	}

	public ExtObjectContainer openNewSession(Db4oTestCase testInstance) {
		return fileSession().openSession().ext();
	}
}
