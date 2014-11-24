/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public class PausableBlockingQueue : BlockingQueue, IPausableBlockingQueue4
	{
		private volatile bool _paused = false;

		public virtual bool Pause()
		{
			if (_paused)
			{
				return false;
			}
			_paused = true;
			return true;
		}

		public virtual bool Resume()
		{
			return (((bool)_lock.Run(new _IClosure4_17(this))));
		}

		private sealed class _IClosure4_17 : IClosure4
		{
			public _IClosure4_17(PausableBlockingQueue _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Run()
			{
				if (!this._enclosing._paused)
				{
					return false;
				}
				this._enclosing._paused = false;
				this._enclosing._lock.Awake();
				return true;
			}

			private readonly PausableBlockingQueue _enclosing;
		}

		public virtual bool IsPaused()
		{
			return _paused;
		}

		/// <exception cref="Db4objects.Db4o.Foundation.BlockingQueueStoppedException"></exception>
		protected override bool UnsafeWaitForNext(long timeout)
		{
			bool hasNext = base.UnsafeWaitForNext(timeout);
			while (_paused && !_stopped)
			{
				_lock.Snooze(timeout);
			}
			if (_stopped)
			{
				throw new BlockingQueueStoppedException();
			}
			return hasNext;
		}

		public virtual object TryNext()
		{
			return _lock.Run(new _IClosure4_46(this));
		}

		private sealed class _IClosure4_46 : IClosure4
		{
			public _IClosure4_46(PausableBlockingQueue _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Run()
			{
				return this._enclosing.IsPaused() ? null : this._enclosing.HasNext() ? this._enclosing
					.Next() : null;
			}

			private readonly PausableBlockingQueue _enclosing;
		}
	}
}
