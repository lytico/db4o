package de.sangamon.freespace.managers;

import de.sangamon.freespace.core.*;

public class SynchronizedFreespaceManagerWrapper implements FreespaceManager {

	private final Object _lock = new Object();
	private final FreespaceManager _delegate;

	public SynchronizedFreespaceManagerWrapper(FreespaceManager delegate) {
		_delegate = delegate;
	}

	@Override
	public Freespace acquire() {
		synchronized(_lock) {
			return _delegate.acquire();
		}
	}

	@Override
	public void free(Freespace freed) {
		synchronized(_lock) {
			_delegate.free(freed);
		}
	}

	@Override
	public void shutdown() {
		_delegate.shutdown();
	}

}
