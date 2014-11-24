/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.events;

/**
 * An event.
 * 
 * Holds a list of {@link EventListener4} objects 
 * which receive {@link EventListener4#onEvent(Event4, EventArgs)}
 * notifications whenever this event is triggered.
 * 
 * @sharpen.ignore
 */
public interface Event4<T extends EventArgs> {
	
	/**
	 * Adds a new listener to the notification list..
	 * 
	 * @sharpen.event.add
	 */
	public void addListener(EventListener4<T> listener);
	
	/**
	 * Removes a previously registered listener from the notification list.
	 */
	public void removeListener(EventListener4<T> listener);
}
