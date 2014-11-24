/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Foundation;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class PausableBlockingQueueTestCase : Queue4TestCaseBase
	{
		public virtual void TestTimeoutNext()
		{
			IPausableBlockingQueue4 queue = new PausableBlockingQueue();
			Assert.IsFalse(queue.IsPaused());
			queue.Pause();
			Assert.IsTrue(queue.IsPaused());
			object obj = new object();
			queue.Add(obj);
			Assert.IsTrue(queue.HasNext());
			Assert.IsNull(queue.TryNext());
			queue.Resume();
			Assert.AreSame(obj, queue.Next(50));
		}

		public virtual void TestStop()
		{
			IPausableBlockingQueue4 queue = new PausableBlockingQueue();
			queue.Pause();
			ExecuteAfter("Pausable queue stopper", 200, new _IRunnable_40(queue));
			Assert.Expect(typeof(BlockingQueueStoppedException), new _ICodeBlock_46(queue));
		}

		private sealed class _IRunnable_40 : IRunnable
		{
			public _IRunnable_40(IPausableBlockingQueue4 queue)
			{
				this.queue = queue;
			}

			public void Run()
			{
				queue.Stop();
			}

			private readonly IPausableBlockingQueue4 queue;
		}

		private sealed class _ICodeBlock_46 : ICodeBlock
		{
			public _ICodeBlock_46(IPausableBlockingQueue4 queue)
			{
				this.queue = queue;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				queue.Next();
			}

			private readonly IPausableBlockingQueue4 queue;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDrainTo()
		{
			IPausableBlockingQueue4 queue = new PausableBlockingQueue();
			queue.Add(new object());
			queue.Add(new object());
			queue.Pause();
			Collection4 list = new Collection4();
			Thread t = ExecuteAfter("Pausable queue drainer", 0, new _IRunnable_66(queue, list
				));
			Runtime4.SleepThrowsOnInterrupt(200);
			lock (list)
			{
				Assert.AreEqual(0, list.Size());
			}
			Assert.IsTrue(queue.HasNext());
			queue.Resume();
			t.Join();
			lock (list)
			{
				Assert.AreEqual(2, list.Size());
			}
			Assert.IsFalse(queue.HasNext());
		}

		private sealed class _IRunnable_66 : IRunnable
		{
			public _IRunnable_66(IPausableBlockingQueue4 queue, Collection4 list)
			{
				this.queue = queue;
				this.list = list;
			}

			public void Run()
			{
				Collection4 l = new Collection4();
				queue.DrainTo(l);
				lock (list)
				{
					list.AddAll(l);
				}
			}

			private readonly IPausableBlockingQueue4 queue;

			private readonly Collection4 list;
		}

		public static Thread ExecuteAfter(string threadName, long timeInMillis, IRunnable
			 runnable)
		{
			Thread t = new _Thread_96(timeInMillis, runnable);
			t.SetName(threadName);
			t.SetDaemon(true);
			t.Start();
			return t;
		}

		private sealed class _Thread_96 : Thread
		{
			public _Thread_96(long timeInMillis, IRunnable runnable)
			{
				this.timeInMillis = timeInMillis;
				this.runnable = runnable;
			}

			public override void Run()
			{
				if (timeInMillis > 0)
				{
					try
					{
						Thread.Sleep(timeInMillis);
					}
					catch (Exception)
					{
						return;
					}
				}
				runnable.Run();
			}

			private readonly long timeInMillis;

			private readonly IRunnable runnable;
		}
	}
}
