/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Sharpen;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class BlockingQueue : IBlockingQueue4
	{
		protected NonblockingQueue _queue = new NonblockingQueue();

		protected Lock4 _lock = new Lock4();

		protected bool _stopped;

		public virtual void Add(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentException();
			}
			_lock.Run(new _IClosure4_20(this, obj));
		}

		private sealed class _IClosure4_20 : IClosure4
		{
			public _IClosure4_20(BlockingQueue _enclosing, object obj)
			{
				this._enclosing = _enclosing;
				this.obj = obj;
			}

			public object Run()
			{
				this._enclosing._queue.Add(obj);
				this._enclosing._lock.Awake();
				return null;
			}

			private readonly BlockingQueue _enclosing;

			private readonly object obj;
		}

		public virtual bool HasNext()
		{
			return (((bool)_lock.Run(new _IClosure4_30(this))));
		}

		private sealed class _IClosure4_30 : IClosure4
		{
			public _IClosure4_30(BlockingQueue _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Run()
			{
				return this._enclosing._queue.HasNext();
			}

			private readonly BlockingQueue _enclosing;
		}

		public virtual IEnumerator Iterator()
		{
			return ((IEnumerator)_lock.Run(new _IClosure4_38(this)));
		}

		private sealed class _IClosure4_38 : IClosure4
		{
			public _IClosure4_38(BlockingQueue _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Run()
			{
				return this._enclosing._queue.Iterator();
			}

			private readonly BlockingQueue _enclosing;
		}

		/// <exception cref="Db4objects.Db4o.Foundation.BlockingQueueStoppedException"></exception>
		public virtual object Next(long timeout)
		{
			return (object)_lock.Run(new _IClosure4_46(this, timeout));
		}

		private sealed class _IClosure4_46 : IClosure4
		{
			public _IClosure4_46(BlockingQueue _enclosing, long timeout)
			{
				this._enclosing = _enclosing;
				this.timeout = timeout;
			}

			public object Run()
			{
				return this._enclosing.UnsafeWaitForNext(timeout) ? this._enclosing.UnsafeNext() : 
					null;
			}

			private readonly BlockingQueue _enclosing;

			private readonly long timeout;
		}

		public virtual int DrainTo(Collection4 target)
		{
			return (((int)_lock.Run(new _IClosure4_54(this, target))));
		}

		private sealed class _IClosure4_54 : IClosure4
		{
			public _IClosure4_54(BlockingQueue _enclosing, Collection4 target)
			{
				this._enclosing = _enclosing;
				this.target = target;
			}

			public object Run()
			{
				this._enclosing.UnsafeWaitForNext();
				int i = 0;
				while (this._enclosing.HasNext())
				{
					i++;
					target.Add(this._enclosing.UnsafeNext());
				}
				return i;
			}

			private readonly BlockingQueue _enclosing;

			private readonly Collection4 target;
		}

		/// <exception cref="Db4objects.Db4o.Foundation.BlockingQueueStoppedException"></exception>
		public virtual bool WaitForNext(long timeout)
		{
			return (((bool)_lock.Run(new _IClosure4_68(this, timeout))));
		}

		private sealed class _IClosure4_68 : IClosure4
		{
			public _IClosure4_68(BlockingQueue _enclosing, long timeout)
			{
				this._enclosing = _enclosing;
				this.timeout = timeout;
			}

			public object Run()
			{
				return this._enclosing.UnsafeWaitForNext(timeout);
			}

			private readonly BlockingQueue _enclosing;

			private readonly long timeout;
		}

		/// <exception cref="Db4objects.Db4o.Foundation.BlockingQueueStoppedException"></exception>
		public virtual object Next()
		{
			return (object)_lock.Run(new _IClosure4_76(this));
		}

		private sealed class _IClosure4_76 : IClosure4
		{
			public _IClosure4_76(BlockingQueue _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Run()
			{
				this._enclosing.UnsafeWaitForNext();
				return this._enclosing.UnsafeNext();
			}

			private readonly BlockingQueue _enclosing;
		}

		public virtual void Stop()
		{
			_lock.Run(new _IClosure4_85(this));
		}

		private sealed class _IClosure4_85 : IClosure4
		{
			public _IClosure4_85(BlockingQueue _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Run()
			{
				this._enclosing._stopped = true;
				this._enclosing._lock.Awake();
				return null;
			}

			private readonly BlockingQueue _enclosing;
		}

		public virtual object NextMatching(IPredicate4 condition)
		{
			return _lock.Run(new _IClosure4_95(this, condition));
		}

		private sealed class _IClosure4_95 : IClosure4
		{
			public _IClosure4_95(BlockingQueue _enclosing, IPredicate4 condition)
			{
				this._enclosing = _enclosing;
				this.condition = condition;
			}

			public object Run()
			{
				return this._enclosing._queue.NextMatching(condition);
			}

			private readonly BlockingQueue _enclosing;

			private readonly IPredicate4 condition;
		}

		/// <exception cref="Db4objects.Db4o.Foundation.BlockingQueueStoppedException"></exception>
		public virtual void WaitForNext()
		{
			_lock.Run(new _IClosure4_103(this));
		}

		private sealed class _IClosure4_103 : IClosure4
		{
			public _IClosure4_103(BlockingQueue _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Run()
			{
				this._enclosing.UnsafeWaitForNext();
				return null;
			}

			private readonly BlockingQueue _enclosing;
		}

		/// <exception cref="Db4objects.Db4o.Foundation.BlockingQueueStoppedException"></exception>
		protected virtual void UnsafeWaitForNext()
		{
			UnsafeWaitForNext(long.MaxValue);
		}

		/// <exception cref="Db4objects.Db4o.Foundation.BlockingQueueStoppedException"></exception>
		protected virtual bool UnsafeWaitForNext(long timeout)
		{
			long timeLeft = timeout;
			long now = Runtime.CurrentTimeMillis();
			while (timeLeft > 0)
			{
				if (_queue.HasNext())
				{
					return true;
				}
				if (_stopped)
				{
					throw new BlockingQueueStoppedException();
				}
				_lock.Snooze(timeLeft);
				long l = now;
				now = Runtime.CurrentTimeMillis();
				timeLeft -= now - l;
			}
			return false;
		}

		private object UnsafeNext()
		{
			return _queue.Next();
		}
	}
}
