/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

import com.db4o.foundation.*;


class ShutDownRunnable implements Runnable {
	
	private Collection4 _containers = new Collection4();
	
	public volatile boolean dontRemove = false;

	public void ensure(ObjectContainerBase container) {
		_containers.ensure(container);	
	}
	
	public void remove(ObjectContainerBase container) {
		_containers.remove(container);
	}

	public void run(){
		dontRemove = true;
		Collection4 copy= new Collection4(_containers);
		Iterator4 i = copy.iterator();
		while(i.moveNext()){
			((ObjectContainerBase)i.current()).shutdownHook();
		}
	}

	public int size() {
		return _containers.size();
	}
	
}

