/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Drs;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;
using Sharpen.Lang;

namespace Db4objects.Drs.Tests
{
	public abstract class DrsTestCase : ITestCase, ITestLifeCycle
	{
		public static readonly Type[] mappings;

		public static readonly Type[] extraMappingsForCleaning = new Type[] { typeof(IList
			), typeof(IDictionary) };

		static DrsTestCase()
		{
			mappings = new Type[] { typeof(Car), typeof(CollectionHolder), typeof(ItemDates), 
				typeof(ItemWithCloneable), typeof(ItemWithUntypedField), typeof(ListContent), typeof(
				ListHolder), typeof(MapContent), typeof(NamedList), typeof(Pilot), typeof(R0), typeof(
				Replicated), typeof(SimpleArrayContent), typeof(SimpleArrayHolder), typeof(SimpleItem
				), typeof(SimpleListHolder), typeof(SPCChild), typeof(SPCParent), typeof(UnqualifiedNamed
				) };
		}

		protected readonly DrsFixture _fixtures = DrsFixtureVariable.Value();

		private Db4objects.Drs.Inside.ReplicationReflector _reflector;

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			CleanBoth();
			ConfigureBoth();
			OpenBoth();
			Store();
			Reopen();
		}

		private void CleanBoth()
		{
			A().Clean();
			B().Clean();
		}

		protected virtual void Clean()
		{
			for (int i = 0; i < mappings.Length; i++)
			{
				A().Provider().DeleteAllInstances(mappings[i]);
				B().Provider().DeleteAllInstances(mappings[i]);
			}
			for (int i = 0; i < extraMappingsForCleaning.Length; i++)
			{
				A().Provider().DeleteAllInstances(extraMappingsForCleaning[i]);
				B().Provider().DeleteAllInstances(extraMappingsForCleaning[i]);
			}
			A().Provider().Commit();
			B().Provider().Commit();
		}

		protected virtual void Store()
		{
		}

		private void ConfigureBoth()
		{
			ConfigureInitial(_fixtures.a);
			ConfigureInitial(_fixtures.b);
		}

		private void ConfigureInitial(IDrsProviderFixture fixture)
		{
			IConfiguration config = Db4oConfiguration(fixture);
			if (config == null)
			{
				return;
			}
			config.GenerateUUIDs(ConfigScope.Globally);
			config.GenerateCommitTimestamps(true);
			Configure(config);
		}

		private IConfiguration Db4oConfiguration(IDrsProviderFixture fixture)
		{
			if (!(fixture is Db4oDrsFixture))
			{
				return null;
			}
			return ((Db4oDrsFixture)fixture).Config();
		}

		protected virtual void Configure(IConfiguration config)
		{
		}

		/// <exception cref="System.Exception"></exception>
		protected virtual void Reopen()
		{
			CloseBoth();
			OpenBoth();
		}

		/// <exception cref="System.Exception"></exception>
		private void OpenBoth()
		{
			A().Open();
			B().Open();
			_reflector = new Db4objects.Drs.Inside.ReplicationReflector(A().Provider(), B().Provider
				(), _fixtures.reflector);
			A().Provider().ReplicationReflector(_reflector);
			B().Provider().ReplicationReflector(_reflector);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
			CloseBoth();
			CleanBoth();
		}

		/// <exception cref="System.Exception"></exception>
		private void CloseBoth()
		{
			A().Close();
			B().Close();
		}

		public virtual IDrsProviderFixture A()
		{
			return _fixtures.a;
		}

		public virtual IDrsProviderFixture B()
		{
			return _fixtures.b;
		}

		protected virtual void EnsureOneInstance(IDrsProviderFixture fixture, Type clazz)
		{
			EnsureInstanceCount(fixture, clazz, 1);
		}

		protected virtual void EnsureInstanceCount(IDrsProviderFixture fixture, Type clazz
			, int count)
		{
			IObjectSet objectSet = fixture.Provider().GetStoredObjects(clazz);
			Assert.AreEqual(count, objectSet.Count);
		}

		protected virtual object GetOneInstance(IDrsProviderFixture fixture, Type clazz)
		{
			IEnumerator objectSet = fixture.Provider().GetStoredObjects(clazz).GetEnumerator(
				);
			object candidate = null;
			if (objectSet.MoveNext())
			{
				candidate = objectSet.Current;
				if (objectSet.MoveNext())
				{
					throw new Exception("Found more than one instance of + " + clazz + " in provider = "
						 + fixture);
				}
			}
			return candidate;
		}

		protected virtual void ReplicateAll(ITestableReplicationProviderInside providerFrom
			, ITestableReplicationProviderInside providerTo)
		{
			IReplicationSession replication = Replication.Begin(providerFrom, providerTo, _fixtures
				.reflector);
			IObjectSet changedSet = providerFrom.ObjectsChangedSinceLastReplication();
			if (changedSet.Count == 0)
			{
				throw new Exception("Can't find any objects to replicate");
			}
			ReplicateAll(replication, changedSet.GetEnumerator());
		}

		protected virtual void ReplicateAll(IReplicationSession replication, IEnumerator 
			allObjects)
		{
			while (allObjects.MoveNext())
			{
				object changed = allObjects.Current;
				//			System.out.println("Replicating = " + changed);
				replication.Replicate(changed);
			}
			replication.Commit();
		}

		protected virtual void ReplicateAll(ITestableReplicationProviderInside from, ITestableReplicationProviderInside
			 to, IReplicationEventListener listener)
		{
			IReplicationSession replication = Replication.Begin(from, to, listener, _fixtures
				.reflector);
			ReplicateAll(replication, from.ObjectsChangedSinceLastReplication().GetEnumerator
				());
		}

		protected virtual void Delete(Type[] classes)
		{
			for (int i = 0; i < classes.Length; i++)
			{
				A().Provider().DeleteAllInstances(classes[i]);
				B().Provider().DeleteAllInstances(classes[i]);
			}
			A().Provider().Commit();
			B().Provider().Commit();
		}

		protected virtual void ReplicateClass(ITestableReplicationProviderInside providerA
			, ITestableReplicationProviderInside providerB, Type clazz)
		{
			IReplicationSession replication = Replication.Begin(providerA, providerB, null, _fixtures
				.reflector);
			IEnumerator allObjects = providerA.ObjectsChangedSinceLastReplication(clazz).GetEnumerator
				();
			ReplicateAll(replication, allObjects);
		}

		protected static void Sleep(int millis)
		{
			try
			{
				Thread.Sleep(millis);
			}
			catch (Exception e)
			{
				throw new Exception(e.ToString());
			}
		}

		protected virtual Db4objects.Drs.Inside.ReplicationReflector ReplicationReflector
			()
		{
			return _reflector;
		}
	}
}
