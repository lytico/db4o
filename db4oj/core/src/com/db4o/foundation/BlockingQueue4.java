/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
package com.db4o.foundation;


public interface BlockingQueue4<T> extends Queue4<T> {

	/**
	 * <p>
	 * Returns the next queued item or waits for it to be available for the
	 * maximum of <code>timeout</code> miliseconds.
	 * 
	 * @param timeout
	 *            maximum time to wait for the next avilable item in miliseconds
	 * @return the next item or <code>null</code> if <code>timeout</code> is
	 *         reached
	 * @throws BlockingQueueStoppedException
	 *             if the {@link BlockingQueue4#stop()} is called.
	 */
	T next(final long timeout) throws BlockingQueueStoppedException;

	void stop();

	/**
	 * <p>
	 * Removes all the available elements in the queue to the colletion passed
	 * as argument.
	 * <p>
	 * It will block until at least one element is available.
	 * 
	 * @param list
	 * @return the number of elements added to the list.
	 * @throws BlockingQueueStoppedException
	 *             if the {@link BlockingQueue4#stop()} is called.
	 */
	int drainTo(Collection4<T> list) throws BlockingQueueStoppedException;

}
