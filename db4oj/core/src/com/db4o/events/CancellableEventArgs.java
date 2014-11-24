/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.events;

/**
 * Argument for events related to cancellable actions.
 * 
 * @see EventRegistry
 */
public interface CancellableEventArgs  {
	
	/**
	 * Queries if the action was already cancelled by some event listener.
	 * 
	 * @sharpen.property
	 */
	public boolean isCancelled();

	/**
	 * Cancels the action related to this event.
	 * Although the related action will be cancelled all the registered
	 * listeners will still receive the event.
	 */
	public void cancel();
}
