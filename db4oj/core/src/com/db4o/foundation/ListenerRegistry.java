/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;


/**
 * @exclude
 */
public class ListenerRegistry <E>{
	
	public static <E> ListenerRegistry<E> newInstance() {
		return new ListenerRegistry<E>();
	}

	private IdentitySet4 _listeners;
	
	public void register(Listener4<E> listener){
		if(_listeners == null){
			_listeners = new IdentitySet4();
		}
		_listeners.add(listener);
	}
	
	public void notifyListeners(E event){
		if(_listeners == null){
			return;
		}
		Iterator4 i = _listeners.iterator();
		while(i.moveNext()){
			((Listener4)i.current()).onEvent(event);
		}
	}

	public void remove(Listener4<E> listener) {
		if (_listeners == null) {
			return;
		}
		
		_listeners.remove(listener);			
	}
}
