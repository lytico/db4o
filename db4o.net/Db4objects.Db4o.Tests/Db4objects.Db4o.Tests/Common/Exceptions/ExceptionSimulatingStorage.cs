/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.Exceptions;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class ExceptionSimulatingStorage : StorageDecorator
	{
		internal class ExceptionTriggerCondition
		{
			public bool _triggersException = false;

			public bool _isClosed = false;
		}

		private readonly IExceptionFactory _exceptionFactory;

		private readonly ExceptionSimulatingStorage.ExceptionTriggerCondition _triggerCondition
			 = new ExceptionSimulatingStorage.ExceptionTriggerCondition();

		public ExceptionSimulatingStorage(IStorage storage, IExceptionFactory exceptionFactory
			) : base(storage)
		{
			_exceptionFactory = exceptionFactory;
		}

		protected override IBin Decorate(BinConfiguration config, IBin bin)
		{
			ResetShutdownState();
			return new ExceptionSimulatingStorage.ExceptionSimulatingBin(bin, _exceptionFactory
				, _triggerCondition);
		}

		private void ResetShutdownState()
		{
			_triggerCondition._isClosed = false;
		}

		public virtual void TriggerException(bool exception)
		{
			ResetShutdownState();
			_triggerCondition._triggersException = exception;
		}

		public virtual bool TriggersException()
		{
			return this._triggerCondition._triggersException;
		}

		public virtual bool IsClosed()
		{
			return _triggerCondition._isClosed;
		}

		internal class ExceptionSimulatingBin : BinDecorator
		{
			private readonly IExceptionFactory _exceptionFactory;

			private readonly ExceptionSimulatingStorage.ExceptionTriggerCondition _triggerCondition;

			internal ExceptionSimulatingBin(IBin bin, IExceptionFactory exceptionFactory, ExceptionSimulatingStorage.ExceptionTriggerCondition
				 triggerCondition) : base(bin)
			{
				_exceptionFactory = exceptionFactory;
				_triggerCondition = triggerCondition;
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public override int Read(long pos, byte[] bytes, int length)
			{
				if (TriggersException())
				{
					_exceptionFactory.ThrowException();
				}
				return _bin.Read(pos, bytes, length);
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public override void Sync()
			{
				if (TriggersException())
				{
					_exceptionFactory.ThrowException();
				}
				_bin.Sync();
			}

			public override void Sync(IRunnable runnable)
			{
				if (TriggersException())
				{
					_exceptionFactory.ThrowException();
				}
				_bin.Sync(runnable);
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public override void Write(long pos, byte[] buffer, int length)
			{
				if (TriggersException())
				{
					_exceptionFactory.ThrowException();
				}
				_bin.Write(pos, buffer, length);
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public override void Close()
			{
				_triggerCondition._isClosed = true;
				_bin.Close();
				if (TriggersException())
				{
					_exceptionFactory.ThrowOnClose();
				}
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public override long Length()
			{
				if (TriggersException())
				{
					_exceptionFactory.ThrowException();
				}
				return _bin.Length();
			}

			private bool TriggersException()
			{
				return _triggerCondition._triggersException;
			}
		}
	}
}
