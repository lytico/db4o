/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.collections;

import com.db4o.activation.*;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Remove(decaf.Platform.JDK11)
public final class ActivatableSupport {

	public static void activate(Activator activator, ActivationPurpose purpose) {
		if(activator != null) {
			activator.activate(purpose);
		}
	}

	public static Activator validateForBind(Activator oldActivator, Activator newActivator) {
    	if (oldActivator != newActivator && oldActivator != null && newActivator != null) {
            throw new IllegalStateException();
        }
		return newActivator;
	}

	private ActivatableSupport() {
	}
}
