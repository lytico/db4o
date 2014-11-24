package com.db4o.container;

import com.db4o.container.internal.*;

public class ContainerFactory {

	public static Container newContainer() {
	    return new ContainerImpl();
    }
	
	public static Container newContainer(Object... bindings) {
		return new BoundContainerImpl(bindings);
	}

}
