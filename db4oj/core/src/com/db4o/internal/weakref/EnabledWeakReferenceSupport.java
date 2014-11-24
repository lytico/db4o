/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.weakref;

import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;

class EnabledWeakReferenceSupport implements WeakReferenceSupport {
   
	private final Object _queue;
    private final ObjectContainerBase _container;
    private SimpleTimer _timer;
    
    EnabledWeakReferenceSupport(ObjectContainerBase container) {
        _container = container;
        _queue = Platform4.createReferenceQueue();
    }

    public Object newWeakReference(ObjectReference referent, Object obj) {
        return Platform4.createActiveObjectReference(_queue, referent, obj);
    }

    public void purge() {
        Platform4.pollReferenceQueue(_container, _queue);
    }
    
    public void start() {
    	if (_timer != null) {
    		return;
    	}
    	
        if(! _container.configImpl().weakReferences()){
            return;
        }
    	
        if (_container.configImpl().weakReferenceCollectionInterval() <= 0) {
        	return;
        }
        
        _timer = new SimpleTimer(new Collector(), _container.configImpl().weakReferenceCollectionInterval());
        _container.threadPool().start("db4o WeakReference collector", _timer);
    }

    /* (non-Javadoc)
	 * @see com.db4o.internal.WeakReferenceSupport#stopTimer()
	 */
    public void stop() {
    	if (_timer == null){
            return;
        }
        _timer.stop();
        _timer = null;
    }
    
    private final class Collector implements Runnable {
		public void run() {
			try {
				purge();
			} catch (DatabaseClosedException dce) {
				// can happen, no stack trace
			} catch (Exception e) {
				// don't bring down the thread
				e.printStackTrace();
			}
		}
	}

    
}