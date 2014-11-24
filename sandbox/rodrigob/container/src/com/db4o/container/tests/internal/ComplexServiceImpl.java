package com.db4o.container.tests.internal;

import com.db4o.container.tests.*;

public class ComplexServiceImpl implements ComplexService {
	
	private final SingletonService _singleton;
	private final SimpleService _simple;

	public ComplexServiceImpl(SingletonService singleton, SimpleService simple) {
		_singleton = singleton;
		_simple = simple;
	}
	
	public ComplexServiceImpl() {
		_singleton = null;
		_simple = null;
	}
	
	public SingletonService singletonDependency() {
		return _singleton;
	}

	public SimpleService simpleDependency() {
		return _simple;
	}
}
