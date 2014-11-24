/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections.Generic;
using Db4objects.Db4o.Internal.Threading;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal.Threading
{
	public class ThreadPool4Impl : IThreadPool4
	{
		private System.EventHandler<UncaughtExceptionEventArgs> _uncaughtException;

		private readonly IList<Thread> _activeThreads = new List<Thread>();

		/// <exception cref="System.Exception"></exception>
		public virtual void Join(int timeoutMilliseconds)
		{
			foreach (Thread thread in ActiveThreads())
			{
				thread.Join(timeoutMilliseconds);
			}
		}

		public virtual void StartLowPriority(string taskName, IRunnable task)
		{
			Thread thread = ThreadFor(taskName, task);
			ActivateThread(thread);
		}

		public virtual void Start(string taskName, IRunnable task)
		{
			Thread thread = ThreadFor(taskName, task);
			ActivateThread(thread);
		}

		private Thread ThreadFor(string threadName, IRunnable task)
		{
			Thread thread = new Thread(new _IRunnable_41(this, task), threadName);
			thread.SetDaemon(true);
			return thread;
		}

		private sealed class _IRunnable_41 : IRunnable
		{
			public _IRunnable_41(ThreadPool4Impl _enclosing, IRunnable task)
			{
				this._enclosing = _enclosing;
				this.task = task;
			}

			public void Run()
			{
				try
				{
					task.Run();
				}
				catch (Exception e)
				{
					this._enclosing.TriggerUncaughtExceptionEvent(e);
				}
				finally
				{
					this._enclosing.Dispose(Thread.CurrentThread());
				}
			}

			private readonly ThreadPool4Impl _enclosing;

			private readonly IRunnable task;
		}

		private void ActivateThread(Thread thread)
		{
			AddActiveThread(thread);
			thread.Start();
		}

		private Thread[] ActiveThreads()
		{
			lock (_activeThreads)
			{
				return Sharpen.Collections.ToArray(_activeThreads, new Thread[_activeThreads.Count
					]);
			}
		}

		private void AddActiveThread(Thread thread)
		{
			lock (_activeThreads)
			{
				_activeThreads.Add(thread);
			}
		}

		protected virtual void Dispose(Thread thread)
		{
			lock (_activeThreads)
			{
				_activeThreads.Remove(thread);
			}
		}

		protected virtual void TriggerUncaughtExceptionEvent(Exception e)
		{
			if (null != _uncaughtException) _uncaughtException(null, new UncaughtExceptionEventArgs
				(e));
		}

		public virtual event System.EventHandler<UncaughtExceptionEventArgs> UncaughtException
		{
			add
			{
				_uncaughtException = (System.EventHandler<UncaughtExceptionEventArgs>)System.Delegate.Combine
					(_uncaughtException, value);
			}
			remove
			{
				_uncaughtException = (System.EventHandler<UncaughtExceptionEventArgs>)System.Delegate.Remove
					(_uncaughtException, value);
			}
		}
	}
}
