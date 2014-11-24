/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Drs;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class UnqualifiedNamedTestCase : DrsTestCase
	{
		public virtual void Test()
		{
			StoreInA();
			Replicate();
			ModifyInB();
			Replicate2();
			ModifyInA();
			Replicate3();
		}

		private void StoreInA()
		{
			UnqualifiedNamed unqualifiedNamed = new UnqualifiedNamed("storedInA");
			A().Provider().StoreNew(unqualifiedNamed);
			A().Provider().Commit();
			EnsureData(A(), "storedInA");
		}

		private void Replicate()
		{
			ReplicateAll(A().Provider(), B().Provider());
			EnsureData(A(), "storedInA");
			EnsureData(B(), "storedInA");
		}

		private void ModifyInB()
		{
			UnqualifiedNamed unqualifiedNamed = (UnqualifiedNamed)GetOneInstance(B(), typeof(
				UnqualifiedNamed));
			unqualifiedNamed.SetData("modifiedInB");
			B().Provider().Update(unqualifiedNamed);
			B().Provider().Commit();
			EnsureData(B(), "modifiedInB");
		}

		private void Replicate2()
		{
			ReplicateAll(B().Provider(), A().Provider());
			EnsureData(A(), "modifiedInB");
			EnsureData(B(), "modifiedInB");
		}

		private void ModifyInA()
		{
			UnqualifiedNamed unqualifiedNamed = (UnqualifiedNamed)GetOneInstance(A(), typeof(
				UnqualifiedNamed));
			unqualifiedNamed.SetData("modifiedInA");
			A().Provider().Update(unqualifiedNamed);
			A().Provider().Commit();
			EnsureData(A(), "modifiedInA");
		}

		private void Replicate3()
		{
			ReplicateClass(A().Provider(), B().Provider(), typeof(UnqualifiedNamed));
			EnsureData(A(), "modifiedInA");
			EnsureData(B(), "modifiedInA");
		}

		private void EnsureData(IDrsProviderFixture fixture, object data)
		{
			EnsureOneInstance(fixture, typeof(UnqualifiedNamed));
			UnqualifiedNamed unqualifiedNamed = (UnqualifiedNamed)GetOneInstance(fixture, typeof(
				UnqualifiedNamed));
			Assert.AreEqual(data, unqualifiedNamed.GetData());
		}

		protected override void ReplicateClass(ITestableReplicationProviderInside providerA
			, ITestableReplicationProviderInside providerB, Type clazz)
		{
			IReplicationSession replication = Replication.Begin(providerA, providerB, _fixtures
				.reflector);
			IEnumerator allObjects = providerA.ObjectsChangedSinceLastReplication(clazz).GetEnumerator
				();
			while (allObjects.MoveNext())
			{
				object obj = allObjects.Current;
				//System.out.println("obj = " + obj);
				replication.Replicate(obj);
			}
			replication.Commit();
		}
	}
}
