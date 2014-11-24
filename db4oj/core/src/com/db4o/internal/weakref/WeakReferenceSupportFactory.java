/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.internal.weakref;

import com.db4o.internal.*;

public class WeakReferenceSupportFactory {

	public static WeakReferenceSupport forObjectContainer(ObjectContainerBase container) {
		
		if (!Platform4.hasWeakReferences()) {
			return disabledWeakReferenceSupport();
		}
		
		if (!container.configImpl().weakReferences()) {
			return disabledWeakReferenceSupport();
		}
	        
		return new EnabledWeakReferenceSupport(container);
	}

	public static WeakReferenceSupport disabledWeakReferenceSupport() {
		return new WeakReferenceSupport() {
			public void stop() {
			}
			
			public void start() {
			}
			
			public void purge() {
			}
			
			public Object newWeakReference(ObjectReference referent, Object obj) {
				return obj;
			}
		};
	}

}
