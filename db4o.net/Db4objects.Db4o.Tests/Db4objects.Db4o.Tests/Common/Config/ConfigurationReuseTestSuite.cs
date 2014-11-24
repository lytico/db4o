/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using Db4oUnit;
using Db4oUnit.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.Config;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Config
{
	/// <summary>Tests all combinations of configuration use/reuse scenarios.</summary>
	/// <remarks>Tests all combinations of configuration use/reuse scenarios.</remarks>
	public class ConfigurationReuseTestSuite : FixtureTestSuiteDescription
	{
		internal static readonly FixtureVariable ConfigurationUseFunction = FixtureVariable
			.NewInstance("Successul configuration use");

		internal static readonly FixtureVariable ConfigurationReuseProcedure = FixtureVariable
			.NewInstance("Configuration reuse attempt");

		public class ConfigurationReuseTestUnit : ITestCase
		{
			// each function returns a block that disposes of any containers
			public virtual void Test()
			{
				IConfiguration config = NewInMemoryConfiguration();
				IRunnable tearDownBlock = ((IRunnable)((IFunction4)ConfigurationUseFunction.Value
					).Apply(config));
				try
				{
					Assert.Expect(typeof(ArgumentException), new _ICodeBlock_79(config));
				}
				finally
				{
					tearDownBlock.Run();
				}
			}

			private sealed class _ICodeBlock_79 : ICodeBlock
			{
				public _ICodeBlock_79(IConfiguration config)
				{
					this.config = config;
				}

				/// <exception cref="System.Exception"></exception>
				public void Run()
				{
					((IProcedure4)ConfigurationReuseTestSuite.ConfigurationReuseProcedure.Value).Apply
						(config);
				}

				private readonly IConfiguration config;
			}
		}

		internal static IConfiguration NewInMemoryConfiguration()
		{
			IConfiguration config = Db4oFactory.NewConfiguration();
			config.Storage = new MemoryStorage();
			return config;
		}

		protected virtual IObjectServer OpenServer(IConfiguration config, string databaseFileName
			, int port)
		{
			return Db4oClientServer.OpenServer(Db4oClientServerLegacyConfigurationBridge.AsServerConfiguration
				(config), databaseFileName, port);
		}

		protected virtual IObjectContainer OpenClient(IConfiguration config, string host, 
			int port, string user, string password)
		{
			return Db4oClientServer.OpenClient(Db4oClientServerLegacyConfigurationBridge.AsClientConfiguration
				(config), host, port, user, password);
		}

		public ConfigurationReuseTestSuite()
		{
			{
				FixtureProviders(new IFixtureProvider[] { new SimpleFixtureProvider(ConfigurationUseFunction
					, new object[] { new _IFunction4_26(), new _IFunction4_31(this), new _IFunction4_36
					(this) }), new SimpleFixtureProvider(ConfigurationReuseProcedure, new object[] { 
					new _IProcedure4_49(), new _IProcedure4_51(this), new _IProcedure4_53(this), new 
					_IProcedure4_61(this) }) });
				TestUnits(new Type[] { typeof(ConfigurationReuseTestSuite.ConfigurationReuseTestUnit
					) });
			}
		}

		private sealed class _IFunction4_26 : IFunction4
		{
			public _IFunction4_26()
			{
			}

			public object Apply(object config)
			{
				IObjectContainer container = Db4oFactory.OpenFile(((IConfiguration)config), ".");
				return new _IRunnable_28(container);
			}

			private sealed class _IRunnable_28 : IRunnable
			{
				public _IRunnable_28(IObjectContainer container)
				{
					this.container = container;
				}

				public void Run()
				{
					container.Close();
				}

				private readonly IObjectContainer container;
			}
		}

		private sealed class _IFunction4_31 : IFunction4
		{
			public _IFunction4_31(ConfigurationReuseTestSuite _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object config)
			{
				IObjectServer server = this._enclosing.OpenServer(((IConfiguration)config), ".", 
					0);
				return new _IRunnable_33(server);
			}

			private sealed class _IRunnable_33 : IRunnable
			{
				public _IRunnable_33(IObjectServer server)
				{
					this.server = server;
				}

				public void Run()
				{
					server.Close();
				}

				private readonly IObjectServer server;
			}

			private readonly ConfigurationReuseTestSuite _enclosing;
		}

		private sealed class _IFunction4_36 : IFunction4
		{
			public _IFunction4_36(ConfigurationReuseTestSuite _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object config)
			{
				IConfiguration serverConfig = Db4oFactory.NewConfiguration();
				serverConfig.Storage = new MemoryStorage();
				IObjectServer server = this._enclosing.OpenServer(serverConfig, ".", -1);
				server.GrantAccess("user", "password");
				IObjectContainer client = this._enclosing.OpenClient(((IConfiguration)config), "localhost"
					, server.Ext().Port(), "user", "password");
				return new _IRunnable_42(client, server);
			}

			private sealed class _IRunnable_42 : IRunnable
			{
				public _IRunnable_42(IObjectContainer client, IObjectServer server)
				{
					this.client = client;
					this.server = server;
				}

				public void Run()
				{
					client.Close();
					server.Close();
				}

				private readonly IObjectContainer client;

				private readonly IObjectServer server;
			}

			private readonly ConfigurationReuseTestSuite _enclosing;
		}

		private sealed class _IProcedure4_49 : IProcedure4
		{
			public _IProcedure4_49()
			{
			}

			public void Apply(object config)
			{
				Db4oFactory.OpenFile(((IConfiguration)config), "..");
			}
		}

		private sealed class _IProcedure4_51 : IProcedure4
		{
			public _IProcedure4_51(ConfigurationReuseTestSuite _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Apply(object config)
			{
				this._enclosing.OpenServer(((IConfiguration)config), "..", 0);
			}

			private readonly ConfigurationReuseTestSuite _enclosing;
		}

		private sealed class _IProcedure4_53 : IProcedure4
		{
			public _IProcedure4_53(ConfigurationReuseTestSuite _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Apply(object config)
			{
				IObjectServer server = this._enclosing.OpenServer(ConfigurationReuseTestSuite.NewInMemoryConfiguration
					(), "..", 0);
				try
				{
					this._enclosing.OpenClient(((IConfiguration)config), "localhost", server.Ext().Port
						(), "user", "password");
				}
				finally
				{
					server.Close();
				}
			}

			private readonly ConfigurationReuseTestSuite _enclosing;
		}

		private sealed class _IProcedure4_61 : IProcedure4
		{
			public _IProcedure4_61(ConfigurationReuseTestSuite _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Apply(object config)
			{
				this._enclosing.OpenClient(((IConfiguration)config), "localhost", unchecked((int)
					(0xdb40)), "user", "password");
			}

			private readonly ConfigurationReuseTestSuite _enclosing;
		}
	}
}
#endif // !SILVERLIGHT
