package de.sangamon.freespace.managers;

import de.sangamon.freespace.*;
import de.sangamon.freespace.core.*;

public class QueueingFreespaceManager implements FreespaceManager {
	
	private final FreespaceManager _delegate;
	private final FreeQueueService _freeService;
	private final Thread _freeServiceThread;

	public QueueingFreespaceManager(FreespaceManager delegate) {
		_delegate = delegate;
		_freeService = new FreeQueueService(delegate);
		_freeServiceThread = new Thread(_freeService);
		_freeServiceThread.setPriority(Thread.MAX_PRIORITY);
		_freeServiceThread.start();
		
	}

	@Override
	public Freespace acquire() {
		return _delegate.acquire();
	}

	@Override
	public void free(Freespace freed) {
		_freeService.free(freed);
	}

	@Override
	public void shutdown() {
		_freeService.stop();
		_freeServiceThread.interrupt();
		try {
			_freeServiceThread.join();
		} 
		catch (InterruptedException exc) {
			exc.printStackTrace();
		}
		_delegate.shutdown();
		//System.out.println("max queue size: " + _freeService.maxQueueSize());
	}

	
}
