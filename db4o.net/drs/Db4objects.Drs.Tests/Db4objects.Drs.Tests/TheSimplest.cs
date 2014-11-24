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
	public class TheSimplest : DrsTestCase
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
			string name = "c1";
			SPCChild child = CreateChildObject(name);
			A().Provider().StoreNew(child);
			A().Provider().Commit();
			EnsureNames(A(), "c1");
		}

		private void Replicate()
		{
			ReplicateAll(A().Provider(), B().Provider());
			EnsureNames(A(), "c1");
			EnsureNames(B(), "c1");
		}

		private void ModifyInB()
		{
			SPCChild child = GetTheObject(B());
			child.SetName("c2");
			B().Provider().Update(child);
			B().Provider().Commit();
			EnsureNames(B(), "c2");
		}

		private void Replicate2()
		{
			ReplicateAll(B().Provider(), A().Provider());
			EnsureNames(A(), "c2");
			EnsureNames(B(), "c2");
		}

		private void ModifyInA()
		{
			SPCChild child = GetTheObject(A());
			child.SetName("c3");
			A().Provider().Update(child);
			A().Provider().Commit();
			EnsureNames(A(), "c3");
		}

		private void Replicate3()
		{
			ReplicateClass(A().Provider(), B().Provider(), typeof(SPCChild));
			EnsureNames(A(), "c3");
			EnsureNames(B(), "c3");
		}

		protected virtual SPCChild CreateChildObject(string name)
		{
			return new SPCChild(name);
		}

		private void EnsureNames(IDrsProviderFixture fixture, string childName)
		{
			EnsureOneInstance(fixture, typeof(SPCChild));
			SPCChild child = GetTheObject(fixture);
			Assert.AreEqual(childName, child.GetName());
		}

		private SPCChild GetTheObject(IDrsProviderFixture fixture)
		{
			return (SPCChild)GetOneInstance(fixture, typeof(SPCChild));
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
