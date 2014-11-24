/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
package com.db4o.foundation;

public class PausableBlockingQueue<T> extends BlockingQueue<T> implements PausableBlockingQueue4<T> {

	private volatile boolean _paused = false;

	public boolean pause() {
		if (_paused) {
			return false;
		}
		_paused = true;
		return true;
	}

	public boolean resume() {
		return _lock.run(new Closure4<Boolean>() {

			public Boolean run() {
				if (!_paused) {
					return false;
				}
				_paused = false;
				_lock.awake();
				return true;
			}
		});
	}
	
	public boolean isPaused() {
		return _paused;
	}
	
	@Override
	protected boolean unsafeWaitForNext(final long timeout) throws BlockingQueueStoppedException {
		boolean hasNext = super.unsafeWaitForNext(timeout);
		while (_paused && !_stopped) {
			_lock.snooze(timeout);
		}
		if (_stopped) {
			throw new BlockingQueueStoppedException();
		}
		return hasNext;
	}

	public T tryNext() {
		return _lock.run(new Closure4<T>() {
			public T run() {
				return isPaused() ? null : hasNext() ? next() : null;
			}
		});
	}

}
