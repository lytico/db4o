/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.IO;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Optional.Monitoring
{
	internal class CountingStorage : StorageDecorator
	{
		private int _numberOfSyncCalls;

		private int _numberOfBytesRead;

		private int _numberOfBytesWritten;

		private int _numberOfReadCalls;

		private int _numberOfWriteCalls;

		public CountingStorage(IStorage storage) : base(storage)
		{
		}

		public virtual int NumberOfSyncCalls()
		{
			return _numberOfSyncCalls;
		}

		public virtual int NumberOfBytesRead()
		{
			return _numberOfBytesRead;
		}

		public virtual int NumberOfBytesWritten()
		{
			return _numberOfBytesWritten;
		}

		public virtual int NumberOfReadCalls()
		{
			return _numberOfReadCalls;
		}

		public virtual int NumberOfWriteCalls()
		{
			return _numberOfWriteCalls;
		}

		protected override IBin Decorate(BinConfiguration config, IBin bin)
		{
			return new _BinDecorator_44(this, bin);
		}

		private sealed class _BinDecorator_44 : BinDecorator
		{
			public _BinDecorator_44(CountingStorage _enclosing, IBin baseArg1) : base(baseArg1
				)
			{
				this._enclosing = _enclosing;
			}

			public override void Sync()
			{
				++this._enclosing._numberOfSyncCalls;
				base.Sync();
			}

			public override void Sync(IRunnable runnable)
			{
				++this._enclosing._numberOfSyncCalls;
				base.Sync(runnable);
			}

			public override int Read(long position, byte[] bytes, int bytesToRead)
			{
				int bytesRead = base.Read(position, bytes, bytesToRead);
				this._enclosing._numberOfBytesRead += bytesRead;
				++this._enclosing._numberOfReadCalls;
				return bytesRead;
			}

			public override void Write(long position, byte[] bytes, int bytesToWrite)
			{
				this._enclosing._numberOfBytesWritten += bytesToWrite;
				++this._enclosing._numberOfWriteCalls;
				base.Write(position, bytes, bytesToWrite);
			}

			private readonly CountingStorage _enclosing;
		}
	}
}
