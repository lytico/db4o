package de.sangamon.freespace.managers;

import java.util.concurrent.*;

import de.sangamon.freespace.core.*;

class FreeQueueService implements Runnable {

	private final FreespaceManager _manager;
	private final BlockingQueue<Freespace> _freed = new LinkedBlockingQueue<Freespace>();
	private int _maxQueueSize = 0;
	
	private volatile boolean _stopped = false;
	
	public FreeQueueService(FreespaceManager manager) {
		_manager = manager;
	}

	@Override
	public void run() {
		while(!_stopped || !_freed.isEmpty()) {
			int queueSize = _freed.size();
			if(_maxQueueSize < queueSize) {
				_maxQueueSize = queueSize;
			}
			try {
				_manager.free(_freed.take());
			} 
			catch (InterruptedException exc) {
				// exc.printStackTrace();
			}
		}
	}

	public void free(Freespace freed) {
		_freed.add(freed);
	}

	public int maxQueueSize() {
		return _maxQueueSize;
	}
	
	public void stop() {
		_stopped = true;
	}
}
