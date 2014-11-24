package com.db4o.container.internal;

public class BoundContainerImpl extends ContainerImpl {
	
	private final Object[] _singletons;

	public BoundContainerImpl(Object[] singletons) {
		_singletons = singletons;
    }

	@Override
	protected Binding resolve(Class serviceType) throws ClassNotFoundException {
		for (Object singleton : _singletons) {
	        if (serviceType.isInstance(singleton)) {
	        	return new SingletonBinding(singleton);
	        }
        }
		return super.resolve(serviceType);
	}
}
