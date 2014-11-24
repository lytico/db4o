/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Extensions.Util;
using Db4objects.Db4o;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Threading;

namespace Db4oUnit.Extensions.Fixtures
{
	public class Db4oNetworking : AbstractDb4oFixture, IDb4oClientServerFixture
	{
		private const int ThreadpoolTimeout = 3000;

		protected static readonly string File = "Db4oClientServer.db4o";

		public static readonly string Host = "127.0.0.1";

		public static readonly string Username = "db4o";

		public static readonly string Password = Username;

		private IObjectServer _server;

		private readonly Sharpen.IO.File _file;

		private IExtObjectContainer _objectContainer;

		private string _label;

		private int _port;

		private IConfiguration _serverConfig;

		private readonly IClientServerFactory _csFactory;

		public Db4oNetworking(IClientServerFactory csFactory, string label)
		{
			_csFactory = csFactory != null ? csFactory : DefaultClientServerFactory();
			_file = new Sharpen.IO.File(FilePath());
			_label = label;
		}

		private IClientServerFactory DefaultClientServerFactory()
		{
			return new StandardClientServerFactory();
		}

		public Db4oNetworking(string label) : this(null, label)
		{
		}

		public Db4oNetworking() : this("C/S")
		{
		}

		/// <exception cref="System.Exception"></exception>
		public override void Open(IDb4oTestCase testInstance)
		{
			OpenServerFor(testInstance);
			OpenClientFor(testInstance);
			ListenToUncaughtExceptions();
		}

		private void ListenToUncaughtExceptions()
		{
			ListenToUncaughtExceptions(ServerThreadPool());
			IThreadPool4 clientThreadPool = ClientThreadPool();
			if (null != clientThreadPool)
			{
				ListenToUncaughtExceptions(clientThreadPool);
			}
		}

		private IThreadPool4 ClientThreadPool()
		{
			return ThreadPoolFor(_objectContainer);
		}

		private IThreadPool4 ServerThreadPool()
		{
			return ThreadPoolFor(_server.Ext().ObjectContainer());
		}

		/// <exception cref="System.Exception"></exception>
		private void OpenClientFor(IDb4oTestCase testInstance)
		{
			IConfiguration config = ClientConfigFor(testInstance);
			_objectContainer = OpenClientWith(config);
		}

		/// <exception cref="System.Exception"></exception>
		private IConfiguration ClientConfigFor(IDb4oTestCase testInstance)
		{
			if (RequiresCustomConfiguration(testInstance))
			{
				IConfiguration customServerConfig = NewConfiguration();
				((ICustomClientServerConfiguration)testInstance).ConfigureClient(customServerConfig
					);
				return customServerConfig;
			}
			IConfiguration config = CloneConfiguration();
			ApplyFixtureConfiguration(testInstance, config);
			return config;
		}

		private IExtObjectContainer OpenSocketClient(IConfiguration config)
		{
			return _csFactory.OpenClient(AsClientConfiguration(config), Host, _port, Username
				, Password).Ext();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual IExtObjectContainer OpenNewSession(IDb4oTestCase testInstance)
		{
			IConfiguration config = ClientConfigFor(testInstance);
			return OpenClientWith(config);
		}

		private IExtObjectContainer OpenClientWith(IConfiguration config)
		{
			return OpenSocketClient(config);
		}

		/// <exception cref="System.Exception"></exception>
		private void OpenServerFor(IDb4oTestCase testInstance)
		{
			_serverConfig = ServerConfigFor(testInstance);
			_server = _csFactory.OpenServer(AsServerConfiguration(_serverConfig), _file.GetAbsolutePath
				(), -1);
			_port = _server.Ext().Port();
			_server.GrantAccess(Username, Password);
		}

		/// <exception cref="System.Exception"></exception>
		private IConfiguration ServerConfigFor(IDb4oTestCase testInstance)
		{
			if (RequiresCustomConfiguration(testInstance))
			{
				IConfiguration customServerConfig = NewConfiguration();
				((ICustomClientServerConfiguration)testInstance).ConfigureServer(customServerConfig
					);
				return customServerConfig;
			}
			return CloneConfiguration();
		}

		private bool RequiresCustomConfiguration(IDb4oTestCase testInstance)
		{
			if (testInstance is ICustomClientServerConfiguration)
			{
				return true;
			}
			return false;
		}

		/// <exception cref="System.Exception"></exception>
		public override void Close()
		{
			if (null != _objectContainer)
			{
				IThreadPool4 clientThreadPool = ClientThreadPool();
				_objectContainer.Close();
				_objectContainer = null;
				if (null != clientThreadPool)
				{
					clientThreadPool.Join(ThreadpoolTimeout);
				}
			}
			CloseServer();
		}

		/// <exception cref="System.Exception"></exception>
		private void CloseServer()
		{
			if (null != _server)
			{
				IThreadPool4 serverThreadPool = ServerThreadPool();
				_server.Close();
				_server = null;
				if (null != serverThreadPool)
				{
					serverThreadPool.Join(ThreadpoolTimeout);
				}
			}
		}

		public override IExtObjectContainer Db()
		{
			return _objectContainer;
		}

		protected override void DoClean()
		{
			_file.Delete();
		}

		public virtual IObjectServer Server()
		{
			return _server;
		}

		/// <summary>
		/// Does not accept a clazz which is assignable from OptOutCS, or not
		/// assignable from Db4oTestCase.
		/// </summary>
		/// <remarks>
		/// Does not accept a clazz which is assignable from OptOutCS, or not
		/// assignable from Db4oTestCase.
		/// </remarks>
		/// <returns>
		/// returns false if the clazz is assignable from OptOutCS, or not
		/// assignable from Db4oTestCase. Otherwise, returns true.
		/// </returns>
		public override bool Accept(Type clazz)
		{
			if (!typeof(IDb4oTestCase).IsAssignableFrom(clazz))
			{
				return false;
			}
			if (typeof(IOptOutMultiSession).IsAssignableFrom(clazz))
			{
				return false;
			}
			if (typeof(IOptOutNetworkingCS).IsAssignableFrom(clazz))
			{
				return false;
			}
			return true;
		}

		public override LocalObjectContainer FileSession()
		{
			return (LocalObjectContainer)_server.Ext().ObjectContainer();
		}

		/// <exception cref="System.Exception"></exception>
		public override void Defragment()
		{
			Defragment(FilePath());
		}

		public override string Label()
		{
			return BuildLabel(_label);
		}

		public virtual int ServerPort()
		{
			return _port;
		}

		private static string FilePath()
		{
			return CrossPlatformServices.DatabasePath(File);
		}

		public override void ConfigureAtRuntime(IRuntimeConfigureAction action)
		{
			action.Apply(Config());
			action.Apply(_serverConfig);
		}

		private IClientConfiguration AsClientConfiguration(IConfiguration serverConfig)
		{
			return Db4oClientServerLegacyConfigurationBridge.AsClientConfiguration(serverConfig
				);
		}

		private IServerConfiguration AsServerConfiguration(IConfiguration serverConfig)
		{
			return Db4oClientServerLegacyConfigurationBridge.AsServerConfiguration(serverConfig
				);
		}
	}
}
#endif // !SILVERLIGHT
