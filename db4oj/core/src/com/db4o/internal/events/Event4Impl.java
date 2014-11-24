/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.events;

import com.db4o.events.*;
import com.db4o.foundation.*;

/**
 * @exclude
 * @sharpen.ignore
 * 
 * @sharpen.macro System.EventHandler<$T>
 */
public class Event4Impl<T extends EventArgs> implements Event4<T> {
	
	/**
	 * @sharpen.remove null
	 */
	public static <T extends EventArgs> Event4Impl<T> newInstance() {
		return new Event4Impl();
	}
	
	private Collection4 _listeners;
	
	protected Event4Impl() {
	}
	
	public final void addListener(EventListener4<T> listener) {
		validateListener(listener);
		
		Collection4 listeners = new Collection4();
		listeners.add(listener);
		if (null != _listeners) {
			listeners.addAll(_listeners);
		}
		
		_listeners = listeners;
		
		onListenerAdded();
	}

	private Collection4 copyListeners() {
		return null != _listeners
			? new Collection4(_listeners)
			: new Collection4();
    }

	/**
	 * Might be overridden whenever specific events need
	 * to know when listeners subscribe to the event.
	 */
	protected void onListenerAdded() {
	}

	public final void removeListener(EventListener4<T> listener) {
		validateListener(listener);
		
		if (null == _listeners) {
			return;
		}
		
		Collection4 listeners = copyListeners();
		listeners.remove(listener);
		
		_listeners = listeners;
	}
	
	/**
	 * @sharpen.macro if (null != $expression) $expression(null, $arguments)
	 */
	public final void trigger(T args) {
		if (null == _listeners) {
			return;
		}
		Iterator4 iterator = _listeners.iterator();
		while (iterator.moveNext()) {
			EventListener4<T> listener = (EventListener4<T>)iterator.current();
			listener.onEvent(this, args);
		}
	}
	
	private void validateListener(EventListener4<T> listener) {
		if (null == listener) {
			throw new ArgumentNullException();
		}
	}

	/**
	 * @sharpen.macro ($expression != null)
	 */
	public boolean hasListeners() {
		return _listeners != null;
	}
}
