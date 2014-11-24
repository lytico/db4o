/**
 * 
 */
package com.db4o.internal;

public final class InterfaceTypeHandler extends OpenTypeHandler {
	
	public InterfaceTypeHandler(ObjectContainerBase container) {
		super(container);
	}
	
	@Override
	public boolean equals(Object obj) {
		return obj instanceof InterfaceTypeHandler;
	}
}