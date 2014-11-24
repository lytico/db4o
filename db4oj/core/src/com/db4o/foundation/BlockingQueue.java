/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.foundation;


/**
 * @exclude
 */
public class BlockingQueue<T> implements BlockingQueue4<T> {
    
	protected NonblockingQueue<T> _queue = new NonblockingQueue<T>();

	protected Lock4 _lock = new Lock4();
	
	protected boolean _stopped;

	public void add(final T obj) {
		if(obj == null){
			throw new IllegalArgumentException();
		}
		_lock.run(new Closure4<Void>() {
			public Void run() {
				_queue.add(obj);
				_lock.awake();
				return null;
			}
		});
	}

	public boolean hasNext() {
		return _lock.run(new Closure4<Boolean>() {
			public Boolean run() {
				return _queue.hasNext();
			}
		});
	}

	public Iterator4<T> iterator() {
		return _lock.run(new Closure4<Iterator4<T>>() {
			public Iterator4<T> run() {
				return _queue.iterator();
			}
		});
	}

	public T next(final long timeout) throws BlockingQueueStoppedException {
		return (T) _lock.run(new Closure4<T>() {
			public T run() {
				return unsafeWaitForNext(timeout) ? unsafeNext() : null;
			}
		});
	}
	
	public int drainTo(final Collection4<T> target) {
		return _lock.run(new Closure4<Integer>() {
			public Integer run() {
				unsafeWaitForNext();
				int i = 0;
				while(hasNext()) {
					i++;
					target.add(unsafeNext());
				}
				return i;
			}
		});
	}

	public boolean waitForNext(final long timeout) throws BlockingQueueStoppedException {
		 return _lock.run(new Closure4<Boolean>() {
			public Boolean run() {
				return unsafeWaitForNext(timeout);
			}
		});
	}

	public T next() throws BlockingQueueStoppedException {
		return (T) _lock.run(new Closure4<T>() {
			public T run() {
				unsafeWaitForNext();
				return unsafeNext();
			}
		});
	}
	
	public void stop(){
		_lock.run(new Closure4<Void>() {
			public Void run() {
				_stopped = true;
				_lock.awake();
				return null;
			}
		});
	}

	public T nextMatching(final Predicate4<T> condition) {
		return _lock.run(new Closure4<T>() {
			public T run() {
				return _queue.nextMatching(condition);
			}
		});
	}
	
	public void waitForNext() throws BlockingQueueStoppedException {
		_lock.run(new Closure4<Boolean>() {
			public Boolean run() {
				unsafeWaitForNext();
				return null;
			}
		});
	}

	protected void unsafeWaitForNext() throws BlockingQueueStoppedException {
		unsafeWaitForNext(Long.MAX_VALUE);
	}

	protected boolean unsafeWaitForNext(final long timeout) throws BlockingQueueStoppedException {
		long timeLeft = timeout;
		long now = System.currentTimeMillis();
		while (timeLeft > 0) {
			if (_queue.hasNext()) {
				return true;
			}
			if(_stopped) {
				throw new BlockingQueueStoppedException();
			}
			_lock.snooze(timeLeft);
			long l = now;
			now = System.currentTimeMillis();
			timeLeft -= now-l;
		}
		return false;
	}

	private T unsafeNext() {
		return _queue.next();
	}
}
