package com.db4o.marshall;

/**
 * this interface is passed to reference type handlers.
 */
public interface ReferenceActivationContext extends ReadContext {

	Object persistentObject();
}
