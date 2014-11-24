/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class IncompatibleFileFormatExceptionTestCase : ITestLifeCycle
	{
		private static readonly string DbPath = "inmemory.db4o";

		private IStorage storage;

		/// <exception cref="System.Exception"></exception>
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(IncompatibleFileFormatExceptionTestCase)).Run();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			storage = new MemoryStorage();
			IBin bin = storage.Open(new BinConfiguration(DbPath, false, 0, false));
			bin.Write(0, new byte[] { 1, 2, 3 }, 3);
			bin.Close();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
		}

		public virtual void Test()
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.File.Storage = storage;
			Assert.Expect(typeof(IncompatibleFileFormatException), new _ICodeBlock_36(config)
				);
		}

		private sealed class _ICodeBlock_36 : ICodeBlock
		{
			public _ICodeBlock_36(IEmbeddedConfiguration config)
			{
				this.config = config;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Db4oEmbedded.OpenFile(config, IncompatibleFileFormatExceptionTestCase.DbPath);
			}

			private readonly IEmbeddedConfiguration config;
		}
	}
}
#endif // !SILVERLIGHT
