package com.db4o.container.tests;

public interface ComplexService {

	SingletonService singletonDependency();
	
	SimpleService simpleDependency();
}
