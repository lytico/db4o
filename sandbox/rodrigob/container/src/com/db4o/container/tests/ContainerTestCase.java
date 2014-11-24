package com.db4o.container.tests;

import com.db4o.container.*;
import com.db4o.container.tests.internal.*;

import db4ounit.*;

public class ContainerTestCase implements TestCase {
	
	public static void main(String[] args) {
		new ConsoleTestRunner(ContainerTestCase.class).run();
	}
	
	final Container container = ContainerFactory.newContainer();
	
	public void testConventionBasedInstantiation() {

		final SimpleService service = container.produce(SimpleService.class);
		Assert.areSame(SimpleServiceImpl.class, service.getClass());
	}
	
	public void testDefaultLifetime() {
		
		Assert.areNotSame(container.produce(SimpleService.class), container.produce(SimpleService.class));
	}
	
	public void testSingletonLifetime() {
		Assert.isNotNull(container.produce(SingletonService.class));
		Assert.areSame(container.produce(SingletonService.class), container.produce(SingletonService.class));
	}
	
	public void testDependencyResolution() {
		
		final ComplexService service = container.produce(ComplexService.class);
		Assert.areSame(container.produce(SingletonService.class), service.singletonDependency());
	}
	
	public void testContainerDependencyResolvesToSelf() {
		final ContainerDependentService service = container.produce(ContainerDependentService.class);
		Assert.areSame(container, service.container());
	}
	
	public void testCustomBindingReplacesDefaultBinding() {
		final SingletonServiceImpl singleton = new SingletonServiceImpl();
		final Container container = ContainerFactory.newContainer(singleton);
		Assert.areSame(singleton, container.produce(SingletonService.class));
		Assert.areSame(singleton, container.produce(ComplexService.class).singletonDependency());
	}
	
	interface SingletonSimpleMix extends SingletonService, SimpleService {
	}
	
	public void testCustomBindingCanProvideMultipleServices() {
		
		SingletonSimpleMix customBinding = new SingletonSimpleMix() {};
		final Container container = ContainerFactory.newContainer(customBinding);
		Assert.areSame(customBinding, container.produce(SingletonService.class));
		Assert.areSame(customBinding, container.produce(SimpleService.class));
		
		final ComplexService service = container.produce(ComplexService.class);
		Assert.areSame(customBinding, service.singletonDependency());
		Assert.areSame(customBinding, service.simpleDependency());
	}
}
