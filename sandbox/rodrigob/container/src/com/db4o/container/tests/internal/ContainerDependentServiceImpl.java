package com.db4o.container.tests.internal;

import com.db4o.container.*;
import com.db4o.container.tests.*;

public class ContainerDependentServiceImpl implements ContainerDependentService {

	private final Container _container;
	
	public ContainerDependentServiceImpl(Container container) {
		_container = container;
	}

	public Container container() {
		return _container;
	}
}
