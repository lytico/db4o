package com.db4o.container;

public interface Container {
	
	<T> T produce(Class<T> serviceType) throws ContainerException;

}
