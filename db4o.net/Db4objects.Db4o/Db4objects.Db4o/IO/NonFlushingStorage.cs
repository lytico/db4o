/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.IO;
using Sharpen.Lang;

namespace Db4objects.Db4o.IO
{
	/// <summary>
	/// Storage adapter that does not pass flush calls
	/// on to its delegate.
	/// </summary>
	/// <remarks>
	/// Storage adapter that does not pass flush calls
	/// on to its delegate.
	/// You can use this
	/// <see cref="IStorage">IStorage</see>
	/// for improved db4o
	/// speed at the risk of corrupted database files in
	/// case of system failure.
	/// </remarks>
	public class NonFlushingStorage : StorageDecorator
	{
		public NonFlushingStorage(IStorage storage) : base(storage)
		{
		}

		protected override IBin Decorate(BinConfiguration config, IBin storage)
		{
			return new NonFlushingStorage.NonFlushingBin(storage);
		}

		private class NonFlushingBin : BinDecorator
		{
			public NonFlushingBin(IBin storage) : base(storage)
			{
			}

			public override void Sync()
			{
			}

			public override void Sync(IRunnable runnable)
			{
				runnable.Run();
			}
		}
	}
}
