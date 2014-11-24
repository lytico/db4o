/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.foundation;

import java.util.*;

import com.db4o.foundation.*;

import db4ounit.*;

public class BlockingQueueTestCase extends Queue4TestCaseBase {
	public void testIterator() {
		Queue4 queue = new BlockingQueue();
		String[] data = { "a", "b", "c", "d" };
		for (int idx = 0; idx < data.length; idx++) {
			assertIterator(queue, data, idx);
			queue.add(data[idx]);
			assertIterator(queue, data, idx + 1);
		}
	}

	public void testNext() {
		Queue4 queue = new BlockingQueue();
		String[] data = { "a", "b", "c", "d" };
		queue.add(data[0]);
		Assert.areSame(data[0], queue.next());
		queue.add(data[1]);
		queue.add(data[2]);
		Assert.areSame(data[1], queue.next());
		Assert.areSame(data[2], queue.next());
	}
	
	public void testTimeoutNext() {
		final BlockingQueue<Object> queue = new BlockingQueue<Object>();

		Assert.isNull(assertTakeAtLeast(200, new Closure4<Object>() {

			public Object run() {
				return queue.next(200);
			}
		}));
		
		Object obj = new Object();
		
		queue.add(obj);
		
		Assert.areSame(obj, assertTakeLessThan(50, new Closure4<Object>() {

			public Object run() {
				return queue.next(200);
			}
		}));
		
		Assert.isNull(assertTakeAtLeast(200, new Closure4<Object>() {

			public Object run() {
				return queue.next(200);
			}
		}));
	}
	
	public void testDrainTo() {
		final BlockingQueue<Object> queue = new BlockingQueue<Object>();

		queue.add(new Object());
		queue.add(new Object());
		
		Collection4<Object> list = new Collection4<Object>();
		
		Assert.areEqual(2, queue.drainTo(list));
		Assert.areEqual(2, list.size());
		Assert.isFalse(queue.hasNext());
	}

	private <T> T assertTakeLessThan(long time, Closure4<T> runnable) {
		long before = System.currentTimeMillis();
		T ret = runnable.run();
		Assert.isSmallerOrEqual(time, System.currentTimeMillis()-before);
		return ret;
	}

	private <T> T assertTakeAtLeast(long time, Closure4<T> runnable) {
		long before = System.currentTimeMillis();
		T ret = runnable.run();
		Assert.isGreaterOrEqual(time, System.currentTimeMillis()-before);
		return ret;
	}

	public void testBlocking() {
		Queue4 queue = new BlockingQueue();
		String[] data = { "a", "b", "c", "d" };
		queue.add(data[0]);
		Assert.areSame(data[0], queue.next());

		NotifyThread notifyThread = new NotifyThread(queue, data[1]);
		notifyThread.start();
		long start = System.currentTimeMillis();
		Assert.areSame(data[1], queue.next());
		long end = System.currentTimeMillis();
		Assert.isGreater(500, end - start);
	}

	public void testStop() {
		final BlockingQueue queue = new BlockingQueue();
		String[] data = { "a", "b", "c", "d" };
		queue.add(data[0]);
		Assert.areSame(data[0], queue.next());

		StopThread notifyThread = new StopThread(queue);
		notifyThread.start();
		Assert.expect(BlockingQueueStoppedException.class, new CodeBlock() {
			public void run() throws Throwable {
				queue.next();
			}
		});		
	}

	private static class NotifyThread extends Thread {
		private Queue4 _queue;

		private Object _data;

		NotifyThread(Queue4 queue, Object data) {
			_queue = queue;
			_data = data;
		}

		public void run() {
			try {
				Thread.sleep(2000);
			} catch (Exception e) {
			
			}
			_queue.add(_data);
		}
	}
	
	private static class StopThread extends Thread {
		private BlockingQueue _queue;

		StopThread(BlockingQueue queue) {
			_queue = queue;
		}

		public void run() {
			try {
				Thread.sleep(2000);
			} catch (Exception e) {
			
			}
			_queue.stop();
		}
	}

}
