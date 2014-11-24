/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Foundation;
using Sharpen;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class BlockingQueueTestCase : Queue4TestCaseBase
	{
		public virtual void TestIterator()
		{
			IQueue4 queue = new BlockingQueue();
			string[] data = new string[] { "a", "b", "c", "d" };
			for (int idx = 0; idx < data.Length; idx++)
			{
				AssertIterator(queue, data, idx);
				queue.Add(data[idx]);
				AssertIterator(queue, data, idx + 1);
			}
		}

		public virtual void TestNext()
		{
			IQueue4 queue = new BlockingQueue();
			string[] data = new string[] { "a", "b", "c", "d" };
			queue.Add(data[0]);
			Assert.AreSame(data[0], queue.Next());
			queue.Add(data[1]);
			queue.Add(data[2]);
			Assert.AreSame(data[1], queue.Next());
			Assert.AreSame(data[2], queue.Next());
		}

		public virtual void TestTimeoutNext()
		{
			BlockingQueue queue = new BlockingQueue();
			Assert.IsNull(AssertTakeAtLeast(200, new _IClosure4_35(queue)));
			object obj = new object();
			queue.Add(obj);
			Assert.AreSame(obj, AssertTakeLessThan(50, new _IClosure4_46(queue)));
			Assert.IsNull(AssertTakeAtLeast(200, new _IClosure4_53(queue)));
		}

		private sealed class _IClosure4_35 : IClosure4
		{
			public _IClosure4_35(BlockingQueue queue)
			{
				this.queue = queue;
			}

			public object Run()
			{
				return queue.Next(200);
			}

			private readonly BlockingQueue queue;
		}

		private sealed class _IClosure4_46 : IClosure4
		{
			public _IClosure4_46(BlockingQueue queue)
			{
				this.queue = queue;
			}

			public object Run()
			{
				return queue.Next(200);
			}

			private readonly BlockingQueue queue;
		}

		private sealed class _IClosure4_53 : IClosure4
		{
			public _IClosure4_53(BlockingQueue queue)
			{
				this.queue = queue;
			}

			public object Run()
			{
				return queue.Next(200);
			}

			private readonly BlockingQueue queue;
		}

		public virtual void TestDrainTo()
		{
			BlockingQueue queue = new BlockingQueue();
			queue.Add(new object());
			queue.Add(new object());
			Collection4 list = new Collection4();
			Assert.AreEqual(2, queue.DrainTo(list));
			Assert.AreEqual(2, list.Size());
			Assert.IsFalse(queue.HasNext());
		}

		private object AssertTakeLessThan(long time, IClosure4 runnable)
		{
			long before = Runtime.CurrentTimeMillis();
			object ret = runnable.Run();
			Assert.IsSmallerOrEqual(time, Runtime.CurrentTimeMillis() - before);
			return ret;
		}

		private object AssertTakeAtLeast(long time, IClosure4 runnable)
		{
			long before = Runtime.CurrentTimeMillis();
			object ret = runnable.Run();
			Assert.IsGreaterOrEqual(time, Runtime.CurrentTimeMillis() - before);
			return ret;
		}

		public virtual void TestBlocking()
		{
			IQueue4 queue = new BlockingQueue();
			string[] data = new string[] { "a", "b", "c", "d" };
			queue.Add(data[0]);
			Assert.AreSame(data[0], queue.Next());
			BlockingQueueTestCase.NotifyThread notifyThread = new BlockingQueueTestCase.NotifyThread
				(queue, data[1]);
			notifyThread.Start();
			long start = Runtime.CurrentTimeMillis();
			Assert.AreSame(data[1], queue.Next());
			long end = Runtime.CurrentTimeMillis();
			Assert.IsGreater(500, end - start);
		}

		public virtual void TestStop()
		{
			BlockingQueue queue = new BlockingQueue();
			string[] data = new string[] { "a", "b", "c", "d" };
			queue.Add(data[0]);
			Assert.AreSame(data[0], queue.Next());
			BlockingQueueTestCase.StopThread notifyThread = new BlockingQueueTestCase.StopThread
				(queue);
			notifyThread.Start();
			Assert.Expect(typeof(BlockingQueueStoppedException), new _ICodeBlock_110(queue));
		}

		private sealed class _ICodeBlock_110 : ICodeBlock
		{
			public _ICodeBlock_110(BlockingQueue queue)
			{
				this.queue = queue;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				queue.Next();
			}

			private readonly BlockingQueue queue;
		}

		private class NotifyThread : Thread
		{
			private IQueue4 _queue;

			private object _data;

			internal NotifyThread(IQueue4 queue, object data)
			{
				_queue = queue;
				_data = data;
			}

			public override void Run()
			{
				try
				{
					Thread.Sleep(2000);
				}
				catch (Exception)
				{
				}
				_queue.Add(_data);
			}
		}

		private class StopThread : Thread
		{
			private BlockingQueue _queue;

			internal StopThread(BlockingQueue queue)
			{
				_queue = queue;
			}

			public override void Run()
			{
				try
				{
					Thread.Sleep(2000);
				}
				catch (Exception)
				{
				}
				_queue.Stop();
			}
		}
	}
}
