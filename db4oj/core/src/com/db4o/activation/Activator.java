/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.activation;

/**
 * Activator interface.<br>
 * <br><br>{@link com.db4o.ta.Activatable} objects need to have a reference to 
 * an Activator implementation, which is called
 * by Transparent Activation, when a request is received to 
 * activate the host object.
 * @see <a href="http://developer.db4o.com/resources/view.aspx/reference/Object_Lifecycle/Activation/Transparent_Activation_Framework">Transparent Activation framework.</a> 
 */
public interface Activator {
	
	/**
	 * Method to be called to activate the host object.
	 * 
	 * @param purpose for which purpose is the object being activated? {@link ActivationPurpose#WRITE} will cause the object
	 * to be saved on the next {@link com.db4o.ObjectContainer#commit} operation.
	 */
	void activate(ActivationPurpose purpose);
}
