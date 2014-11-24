/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Diagnostics;
using Sharpen.Util;

namespace Db4objects.Db4o.Tests.Common.Diagnostics
{
	public class MissingClassDiagnosticsTestCase : ITestCase, ITestLifeCycle, IOptOutMultiSession
	{
		private static readonly string DbUri = "test_db";

		private const int Port = unchecked((int)(0xdb40));

		private static readonly string User = "user";

		private static readonly string Password = "password";

		[System.NonSerialized]
		private MemoryStorage _storage = new MemoryStorage();

		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(MissingClassDiagnosticsTestCase)).Run();
		}

		[System.Serializable]
		public class AcceptAllPredicate : Predicate
		{
			public virtual bool Match(object candidate)
			{
				return true;
			}
		}

		public class Pilot
		{
			public string name;

			public IList cars = new ArrayList();

			public Pilot(string name) : base()
			{
				this.name = name;
			}

			public virtual IList GetCars()
			{
				return cars;
			}

			public virtual string GetName()
			{
				return name;
			}

			public override string ToString()
			{
				return "Pilot[" + name + "]";
			}
		}

		public class Car
		{
			public string model;

			public Car(string model)
			{
				this.model = model;
			}

			public virtual string GetModel()
			{
				return model;
			}

			public override string ToString()
			{
				return "Car[" + model + "]";
			}
		}

		private void PrepareHost(IFileConfiguration fileConfig, ICommonConfiguration commonConfig
			, IList classesNotFound)
		{
			fileConfig.Storage = _storage;
			PrepareCommon(commonConfig, classesNotFound);
		}

		private void PrepareCommon(ICommonConfiguration commonConfig, IList classesNotFound
			)
		{
			commonConfig.ReflectWith(Platform4.ReflectorForType(typeof(MissingClassDiagnosticsTestCase.Pilot
				)));
			PrepareDiagnostic(commonConfig, classesNotFound);
		}

		private void PrepareDiagnostic(ICommonConfiguration common, IList classesNotFound
			)
		{
			common.Diagnostic.AddListener(new _IDiagnosticListener_94(classesNotFound));
		}

		private sealed class _IDiagnosticListener_94 : IDiagnosticListener
		{
			public _IDiagnosticListener_94(IList classesNotFound)
			{
				this.classesNotFound = classesNotFound;
			}

			public void OnDiagnostic(IDiagnostic d)
			{
				if (d is MissingClass)
				{
					classesNotFound.Add(((MissingClass)d).Reason());
				}
			}

			private readonly IList classesNotFound;
		}

		public virtual void TestEmbedded()
		{
			IList missingClasses = new ArrayList();
			IEmbeddedConfiguration excludingConfig = Db4oEmbedded.NewConfiguration();
			PrepareHost(excludingConfig.File, excludingConfig.Common, missingClasses);
			ExcludeClasses(excludingConfig.Common, new Type[] { typeof(MissingClassDiagnosticsTestCase.Pilot
				), typeof(MissingClassDiagnosticsTestCase.Car) });
			IEmbeddedObjectContainer excludingContainer = Db4oEmbedded.OpenFile(excludingConfig
				, DbUri);
			try
			{
				excludingContainer.Query(new MissingClassDiagnosticsTestCase.AcceptAllPredicate()
					);
			}
			finally
			{
				excludingContainer.Close();
			}
			AssertPilotAndCarMissing(missingClasses);
		}

		private void AssertPilotAndCarMissing(IList classesNotFound)
		{
			IList excluded = Arrays.AsList(new string[] { ReflectPlatform.FullyQualifiedName(
				typeof(MissingClassDiagnosticsTestCase.Pilot)), ReflectPlatform.FullyQualifiedName
				(typeof(MissingClassDiagnosticsTestCase.Car)) });
			Assert.AreEqual(excluded.Count, classesNotFound.Count);
			for (IEnumerator candidateIter = excluded.GetEnumerator(); candidateIter.MoveNext
				(); )
			{
				string candidate = ((string)candidateIter.Current);
				Assert.IsTrue(classesNotFound.Contains(candidate));
			}
		}

		public virtual void TestMissingClassesInServer()
		{
			IList serverMissedClasses = new ArrayList();
			IList clientMissedClasses = new ArrayList();
			IServerConfiguration serverConfig = Db4oClientServer.NewServerConfiguration();
			PrepareHost(serverConfig.File, serverConfig.Common, serverMissedClasses);
			ExcludeClasses(serverConfig.Common, new Type[] { typeof(MissingClassDiagnosticsTestCase.Pilot
				), typeof(MissingClassDiagnosticsTestCase.Car) });
			IObjectServer server = Db4oClientServer.OpenServer(serverConfig, DbUri, Port);
			server.GrantAccess(User, Password);
			try
			{
				IClientConfiguration clientConfig = Db4oClientServer.NewClientConfiguration();
				PrepareCommon(clientConfig.Common, clientMissedClasses);
				IObjectContainer client = Db4oClientServer.OpenClient(clientConfig, "localhost", 
					Port, User, Password);
				client.Query(new MissingClassDiagnosticsTestCase.AcceptAllPredicate());
				client.Close();
			}
			finally
			{
				server.Close();
			}
			AssertPilotAndCarMissing(serverMissedClasses);
			Assert.AreEqual(0, clientMissedClasses.Count);
		}

		public virtual void TestMissingClassesInClient()
		{
			IList serverMissedClasses = new ArrayList();
			IList clientMissedClasses = new ArrayList();
			IServerConfiguration serverConfig = Db4oClientServer.NewServerConfiguration();
			PrepareHost(serverConfig.File, serverConfig.Common, serverMissedClasses);
			IObjectServer server = Db4oClientServer.OpenServer(serverConfig, DbUri, Port);
			server.GrantAccess(User, Password);
			try
			{
				IClientConfiguration clientConfig = Db4oClientServer.NewClientConfiguration();
				PrepareCommon(clientConfig.Common, clientMissedClasses);
				ExcludeClasses(clientConfig.Common, new Type[] { typeof(MissingClassDiagnosticsTestCase.Pilot
					), typeof(MissingClassDiagnosticsTestCase.Car) });
				IObjectContainer client = Db4oClientServer.OpenClient(clientConfig, "localhost", 
					Port, User, Password);
				IObjectSet result = client.Query(new MissingClassDiagnosticsTestCase.AcceptAllPredicate
					());
				IterateOver(result);
				client.Close();
			}
			finally
			{
				server.Close();
			}
			Assert.AreEqual(0, serverMissedClasses.Count);
			AssertPilotAndCarMissing(clientMissedClasses);
		}

		private void IterateOver(IObjectSet result)
		{
			while (result.HasNext())
			{
				result.Next();
			}
		}

		private void ExcludeClasses(ICommonConfiguration commonConfiguration, Type[] classes
			)
		{
			commonConfiguration.ReflectWith(new ExcludingReflector(ByRef.NewInstance(typeof(MissingClassDiagnosticsTestCase.Pilot
				)), classes));
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void TestClassesFound()
		{
			IList missingClasses = new ArrayList();
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			PrepareHost(config.File, config.Common, missingClasses);
			PopulateContainer(config);
			Assert.AreEqual(0, missingClasses.Count);
		}

		private void PopulateContainer(IEmbeddedConfiguration config)
		{
			config.File.Storage = _storage;
			IObjectContainer container = Db4oEmbedded.OpenFile(config, DbUri);
			try
			{
				MissingClassDiagnosticsTestCase.Pilot pilot = new MissingClassDiagnosticsTestCase.Pilot
					("Barrichello");
				pilot.GetCars().Add(new MissingClassDiagnosticsTestCase.Car("BMW"));
				container.Store(pilot);
			}
			finally
			{
				container.Close();
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			PopulateContainer(Db4oEmbedded.NewConfiguration());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
		}
	}
}
#endif // !SILVERLIGHT
