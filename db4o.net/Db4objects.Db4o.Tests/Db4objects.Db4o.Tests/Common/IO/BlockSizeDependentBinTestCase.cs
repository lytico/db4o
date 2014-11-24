/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.IO;

namespace Db4objects.Db4o.Tests.Common.IO
{
	/// <exclude></exclude>
	public class BlockSizeDependentBinTestCase : TestWithTempFile
	{
		public class BlockSizeDependentStorage : StorageDecorator
		{
			private readonly IntByRef _blockSize;

			public BlockSizeDependentStorage(IStorage storage, IntByRef blockSize) : base(storage
				)
			{
				_blockSize = blockSize;
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public override IBin Open(BinConfiguration config)
			{
				IBin bin = base.Open(config);
				((IBlockSize)Environments.My(typeof(IBlockSize))).Register((IListener4)bin);
				return bin;
			}

			protected override IBin Decorate(BinConfiguration config, IBin bin)
			{
				return new BlockSizeDependentBinTestCase.BlockSizeDependentStorage.BlockSizeDependentBin
					(bin, _blockSize);
			}

			private class BlockSizeDependentBin : BinDecorator, IListener4
			{
				private readonly IntByRef _blockSize;

				public BlockSizeDependentBin(IBin bin, IntByRef blockSize) : base(bin)
				{
					_blockSize = blockSize;
				}

				public virtual void OnEvent(object @event)
				{
					_blockSize.value = (((int)@event));
				}
			}
		}

		private IntByRef _blockSize = new IntByRef();

		public virtual void Test()
		{
			int configuredBlockSize = 13;
			IObjectContainer db = Db4oEmbedded.OpenFile(Configure(configuredBlockSize), TempFile
				());
			try
			{
				Assert.AreEqual(configuredBlockSize, _blockSize.value);
			}
			finally
			{
				db.Close();
			}
			db = Db4oEmbedded.OpenFile(Configure(14), TempFile());
			try
			{
				Assert.AreEqual(configuredBlockSize, _blockSize.value);
			}
			finally
			{
				db.Close();
			}
		}

		private IEmbeddedConfiguration Configure(int configuredBlockSize)
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.File.Storage = new BlockSizeDependentBinTestCase.BlockSizeDependentStorage
				(new FileStorage(), _blockSize);
			config.File.BlockSize = configuredBlockSize;
			return config;
		}
	}
}
#endif // !SILVERLIGHT
