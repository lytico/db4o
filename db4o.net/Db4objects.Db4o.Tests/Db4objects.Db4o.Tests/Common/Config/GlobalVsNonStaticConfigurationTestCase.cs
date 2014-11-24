/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.Config;

namespace Db4objects.Db4o.Tests.Common.Config
{
	public class GlobalVsNonStaticConfigurationTestCase : Db4oTestWithTempFile
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(GlobalVsNonStaticConfigurationTestCase)).Run();
		}

		public class Data
		{
			public int id;

			public Data(int id)
			{
				this.id = id;
			}
		}

		public virtual void TestOpenWithNonStaticConfiguration()
		{
			IEmbeddedConfiguration config1 = NewConfiguration();
			config1.File.ReadOnly = true;
			Assert.Expect(typeof(DatabaseReadOnlyException), new _ICodeBlock_30(this, config1
				));
			IEmbeddedConfiguration config2 = NewConfiguration();
			IObjectContainer db2 = Db4oEmbedded.OpenFile(config2, TempFile());
			try
			{
				db2.Store(new GlobalVsNonStaticConfigurationTestCase.Data(2));
				Assert.AreEqual(1, db2.Query(typeof(GlobalVsNonStaticConfigurationTestCase.Data))
					.Count);
			}
			finally
			{
				db2.Close();
			}
		}

		private sealed class _ICodeBlock_30 : ICodeBlock
		{
			public _ICodeBlock_30(GlobalVsNonStaticConfigurationTestCase _enclosing, IEmbeddedConfiguration
				 config1)
			{
				this._enclosing = _enclosing;
				this.config1 = config1;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Db4oEmbedded.OpenFile(config1, this._enclosing.TempFile());
			}

			private readonly GlobalVsNonStaticConfigurationTestCase _enclosing;

			private readonly IEmbeddedConfiguration config1;
		}

		#if !SILVERLIGHT
		[System.ObsoleteAttribute(@"using deprecated api")]
		public virtual void TestOpenWithStaticConfiguration()
		{
			Db4oFactory.Configure().ReadOnly(true);
			Assert.Expect(typeof(DatabaseReadOnlyException), new _ICodeBlock_53(this));
			Db4oFactory.Configure().ReadOnly(false);
			IObjectContainer db = Db4oFactory.OpenFile(TempFile());
			db.Store(new GlobalVsNonStaticConfigurationTestCase.Data(1));
			db.Close();
			db = Db4oFactory.OpenFile(TempFile());
			Assert.AreEqual(1, db.Query(typeof(GlobalVsNonStaticConfigurationTestCase.Data)).
				Count);
			db.Close();
		}
		#endif // !SILVERLIGHT

		private sealed class _ICodeBlock_53 : ICodeBlock
		{
			public _ICodeBlock_53(GlobalVsNonStaticConfigurationTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Db4oFactory.OpenFile(this._enclosing.TempFile());
			}

			private readonly GlobalVsNonStaticConfigurationTestCase _enclosing;
		}

		public virtual void TestIndependentObjectConfigs()
		{
			IEmbeddedConfiguration config = NewConfiguration();
			IObjectClass objectConfig = config.Common.ObjectClass(typeof(GlobalVsNonStaticConfigurationTestCase.Data
				));
			objectConfig.Translate(new TNull());
			IEmbeddedConfiguration otherConfig = NewConfiguration();
			Assert.AreNotSame(config, otherConfig);
			Config4Class otherObjectConfig = (Config4Class)otherConfig.Common.ObjectClass(typeof(
				GlobalVsNonStaticConfigurationTestCase.Data));
			Assert.AreNotSame(objectConfig, otherObjectConfig);
			Assert.IsNull(otherObjectConfig.GetTranslator());
		}
	}
}
