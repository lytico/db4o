package com.db4o.db4ounit.common.reflect.custom;

public class PersistenceContext {

	private final String _url;
	private Object _providerContext;

	public PersistenceContext(String url) {
		_url = url;
	}

	public String url() {
		return _url;
	}

	public void setProviderContext(Object context) {
		_providerContext = context;
	}

	public Object getProviderContext() {
		return _providerContext;
	}

}
