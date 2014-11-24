/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class SimpleArrayTest : DrsTestCase
	{
		public virtual void Test()
		{
			StoreListToProviderA();
			ReplicateAllToProviderBFirstTime();
			ModifyInProviderB();
			ReplicateAllStep2();
			AddElementInProviderA();
			ReplicateHolderStep3();
		}

		protected override void Clean()
		{
			Delete(new Type[] { typeof(SimpleArrayHolder), typeof(SimpleArrayContent) });
		}

		private void StoreListToProviderA()
		{
			SimpleArrayHolder sah = new SimpleArrayHolder("h1");
			SimpleArrayContent sac1 = new SimpleArrayContent("c1");
			SimpleArrayContent sac2 = new SimpleArrayContent("c2");
			sah.Add(sac1);
			sah.Add(sac2);
			A().Provider().StoreNew(sah);
			A().Provider().Commit();
			EnsureContent(A(), new string[] { "h1" }, new string[] { "c1", "c2" });
		}

		private void ReplicateAllToProviderBFirstTime()
		{
			ReplicateAll(A().Provider(), B().Provider());
			EnsureContent(A(), new string[] { "h1" }, new string[] { "c1", "c2" });
			EnsureContent(B(), new string[] { "h1" }, new string[] { "c1", "c2" });
		}

		private void ModifyInProviderB()
		{
			SimpleArrayHolder sah = (SimpleArrayHolder)GetOneInstance(B(), typeof(SimpleArrayHolder
				));
			sah.SetName("h2");
			SimpleArrayContent sac1 = sah.GetArr()[0];
			SimpleArrayContent sac2 = sah.GetArr()[1];
			sac1.SetName("co1");
			sac2.SetName("co2");
			B().Provider().Update(sac1);
			B().Provider().Update(sac2);
			B().Provider().Update(sah);
			B().Provider().Commit();
			EnsureContent(B(), new string[] { "h2" }, new string[] { "co1", "co2" });
		}

		private void ReplicateAllStep2()
		{
			ReplicateAll(B().Provider(), A().Provider());
			EnsureContent(B(), new string[] { "h2" }, new string[] { "co1", "co2" });
			EnsureContent(A(), new string[] { "h2" }, new string[] { "co1", "co2" });
		}

		private void AddElementInProviderA()
		{
			SimpleArrayHolder sah = (SimpleArrayHolder)GetOneInstance(A(), typeof(SimpleArrayHolder
				));
			sah.SetName("h3");
			SimpleArrayContent lc3 = new SimpleArrayContent("co3");
			A().Provider().StoreNew(lc3);
			sah.Add(lc3);
			A().Provider().Update(sah);
			A().Provider().Commit();
			EnsureContent(A(), new string[] { "h3" }, new string[] { "co1", "co2", "co3" });
		}

		private void ReplicateHolderStep3()
		{
			ReplicateClass(A().Provider(), B().Provider(), typeof(SimpleArrayHolder));
			EnsureContent(A(), new string[] { "h3" }, new string[] { "co1", "co2", "co3" });
			EnsureContent(B(), new string[] { "h3" }, new string[] { "co1", "co2", "co3" });
		}

		private void EnsureContent(IDrsProviderFixture fixture, string[] holderNames, string
			[] contentNames)
		{
			int holderCount = holderNames.Length;
			int contentCount = contentNames.Length;
			EnsureInstanceCount(fixture, typeof(SimpleArrayHolder), holderCount);
			EnsureInstanceCount(fixture, typeof(SimpleArrayContent), contentCount);
			int i = 0;
			IObjectSet objectSet = fixture.Provider().GetStoredObjects(typeof(SimpleArrayHolder
				));
			IEnumerator iterator = objectSet.GetEnumerator();
			while (iterator.MoveNext())
			{
				SimpleArrayHolder lh = (SimpleArrayHolder)iterator.Current;
				Assert.AreEqual(holderNames[i], lh.GetName());
				SimpleArrayContent[] sacs = lh.GetArr();
				for (int j = 0; j < contentNames.Length; j++)
				{
					Assert.AreEqual(contentNames[j], sacs[j].GetName());
				}
			}
		}
	}
}
