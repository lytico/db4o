/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class SimpleParentChild : DrsTestCase
	{
		public virtual void Test()
		{
			StoreParentAndChildToProviderA();
			ReplicateAllToProviderBFirstTime();
			ModifyParentInProviderB();
			ReplicateAllStep2();
			ModifyParentAndChildInProviderA();
			ReplicateParentClassStep3();
		}

		private void EnsureNames(IDrsProviderFixture fixture, string parentName, string childName
			)
		{
			EnsureOneInstanceOfParentAndChild(fixture);
			SPCParent parent = (SPCParent)GetOneInstance(fixture, typeof(SPCParent));
			if (!parent.GetName().Equals(parentName))
			{
				Sharpen.Runtime.Out.WriteLine("expected = " + parentName);
				Sharpen.Runtime.Out.WriteLine("actual = " + parent.GetName());
			}
			Assert.AreEqual(parentName, parent.GetName());
			Assert.AreEqual(childName, parent.GetChild().GetName());
		}

		private void EnsureOneInstanceOfParentAndChild(IDrsProviderFixture fixture)
		{
			EnsureOneInstance(fixture, typeof(SPCParent));
			EnsureOneInstance(fixture, typeof(SPCChild));
		}

		private void ModifyParentAndChildInProviderA()
		{
			SPCParent parent = (SPCParent)GetOneInstance(A(), typeof(SPCParent));
			parent.SetName("p3");
			SPCChild child = parent.GetChild();
			child.SetName("c3");
			A().Provider().Update(parent);
			A().Provider().Update(child);
			A().Provider().Commit();
			EnsureNames(A(), "p3", "c3");
		}

		private void ModifyParentInProviderB()
		{
			SPCParent parent = (SPCParent)GetOneInstance(B(), typeof(SPCParent));
			parent.SetName("p2");
			B().Provider().Update(parent);
			B().Provider().Commit();
			EnsureNames(B(), "p2", "c1");
		}

		private void ReplicateAllStep2()
		{
			ReplicateAll(B().Provider(), A().Provider());
			EnsureNames(A(), "p2", "c1");
			EnsureNames(B(), "p2", "c1");
		}

		private void ReplicateAllToProviderBFirstTime()
		{
			ReplicateAll(A().Provider(), B().Provider());
			EnsureNames(A(), "p1", "c1");
			EnsureNames(B(), "p1", "c1");
		}

		private void ReplicateParentClassStep3()
		{
			ReplicateClass(A().Provider(), B().Provider(), typeof(SPCParent));
			EnsureNames(A(), "p3", "c3");
			EnsureNames(B(), "p3", "c3");
		}

		private void StoreParentAndChildToProviderA()
		{
			SPCChild child = new SPCChild("c1");
			SPCParent parent = new SPCParent(child, "p1");
			A().Provider().StoreNew(parent);
			A().Provider().Commit();
			EnsureNames(A(), "p1", "c1");
		}
	}
}
