package com.db4o.foundation;


public class DelegatingBlockingQueue<T> implements BlockingQueue4<T> {
	
	private BlockingQueue4<T> queue;
	
	public T next(long timeout) throws BlockingQueueStoppedException {
		return queue.next(timeout);
	}

	public T next() {
		return queue.next();
	}

	public void add(T obj) {
		queue.add(obj);
	}

	public boolean hasNext() {
		return queue.hasNext();
	}

	public T nextMatching(Predicate4<T> condition) {
		return queue.nextMatching(condition);
	}

	public Iterator4 iterator() {
		return queue.iterator();
	}

	public DelegatingBlockingQueue(BlockingQueue4<T> queue) {
		this.queue = queue;
	}

	public void stop() {
		queue.stop();
	}

	public int drainTo(Collection4<T> list) {
		return queue.drainTo(list);
	}

}
