/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Reflect;
using Db4objects.Drs;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class R0to4Runner : DrsTestCase
	{
		private const int Linkers = 4;

		public R0to4Runner() : base()
		{
		}

		//	 ------------------------------ FIELDS ------------------------------
		// --------------------------- CONSTRUCTORS ---------------------------
		protected override void Clean()
		{
			Delete(A().Provider());
			Delete(B().Provider());
		}

		protected virtual void Delete(ITestableReplicationProviderInside provider)
		{
			ArrayList toDelete = new ArrayList();
			IEnumerator rr = provider.GetStoredObjects(typeof(R0)).GetEnumerator();
			while (rr.MoveNext())
			{
				object o = rr.Current;
				IReflectClass claxx = ReplicationReflector().ForObject(o);
				SetFieldsToNull(o, claxx);
				toDelete.Add(o);
			}
			object commitObject = null;
			for (IEnumerator iterator = toDelete.GetEnumerator(); iterator.MoveNext(); )
			{
				object o = iterator.Current;
				//System.out.println("o = " + o);
				provider.Delete(o);
				commitObject = o;
			}
			if (commitObject != null)
			{
				provider.Commit();
			}
			else
			{
				provider.Commit();
			}
		}

		private void CompareR4(ITestableReplicationProviderInside a, ITestableReplicationProviderInside
			 b, bool isSameExpected)
		{
			IEnumerator it = a.GetStoredObjects(typeof(R4)).GetEnumerator();
			while (it.MoveNext())
			{
				string name = ((R4)it.Current).GetName();
				IEnumerator it2 = b.GetStoredObjects(typeof(R4)).GetEnumerator();
				bool found = false;
				while (it2.MoveNext())
				{
					string name2 = ((R4)it2.Current).GetName();
					if (name.Equals(name2))
					{
						found = true;
					}
				}
				Assert.IsTrue(found == isSameExpected);
			}
		}

		private void ReplicateAllToB(ITestableReplicationProviderInside peerA, ITestableReplicationProviderInside
			 peerB)
		{
			Assert.AreEqual(Linkers * 5, ReplicateAll(peerA, peerB, false));
		}

		private void EnsureCount(ITestableReplicationProviderInside provider, int linkers
			)
		{
			EnsureCount(provider, typeof(R0), linkers * 5);
			EnsureCount(provider, typeof(R1), linkers * 4);
			EnsureCount(provider, typeof(R2), linkers * 3);
			EnsureCount(provider, typeof(R3), linkers * 2);
			EnsureCount(provider, typeof(R4), linkers * 1);
		}

		private void EnsureCount(ITestableReplicationProviderInside provider, Type clazz, 
			int count)
		{
			IEnumerator instances = provider.GetStoredObjects(clazz).GetEnumerator();
			int i = count;
			while (instances.MoveNext())
			{
				object o = instances.Current;
				i--;
			}
			Assert.AreEqual(0, i);
		}

		private void EnsureR4Different(ITestableReplicationProviderInside peerA, ITestableReplicationProviderInside
			 peerB)
		{
			CompareR4(peerB, peerA, false);
		}

		private void EnsureR4Same(ITestableReplicationProviderInside peerA, ITestableReplicationProviderInside
			 peerB)
		{
			CompareR4(peerB, peerA, true);
			CompareR4(peerA, peerB, true);
		}

		private void Init(ITestableReplicationProviderInside peerA)
		{
			R0Linker lCircles = new R0Linker();
			lCircles.SetNames("circles");
			lCircles.LinkCircles();
			lCircles.Store(peerA);
			R0Linker lList = new R0Linker();
			lList.SetNames("list");
			lList.LinkList();
			lList.Store(peerA);
			R0Linker lThis = new R0Linker();
			lThis.SetNames("this");
			lThis.LinkThis();
			lThis.Store(peerA);
			R0Linker lBack = new R0Linker();
			lBack.SetNames("back");
			lBack.LinkBack();
			lBack.Store(peerA);
			peerA.Commit();
		}

		private void ModifyR4(ITestableReplicationProviderInside provider)
		{
			object commitObject = null;
			IEnumerator it = provider.GetStoredObjects(typeof(R4)).GetEnumerator();
			while (it.MoveNext())
			{
				R4 r4 = (R4)it.Current;
				r4.SetName(r4.GetName() + "_");
				provider.Update(r4);
				commitObject = r4;
			}
			provider.Commit();
		}

		private int Replicate(ITestableReplicationProviderInside peerA, ITestableReplicationProviderInside
			 peerB)
		{
			return ReplicateAll(peerA, peerB, true);
		}

		private int ReplicateAll(ITestableReplicationProviderInside peerA, ITestableReplicationProviderInside
			 peerB, bool modifiedOnly)
		{
			IReplicationSession replication = Replication.Begin(peerA, peerB, null, _fixtures
				.reflector);
			IEnumerator it = modifiedOnly ? peerA.ObjectsChangedSinceLastReplication(typeof(R0
				)).GetEnumerator() : peerA.GetStoredObjects(typeof(R0)).GetEnumerator();
			int replicated = 0;
			while (it.MoveNext())
			{
				R0 r0 = (R0)it.Current;
				replication.Replicate(r0);
				replicated++;
			}
			replication.Commit();
			EnsureCount(peerA, Linkers);
			EnsureCount(peerB, Linkers);
			return replicated;
		}

		private void ReplicateNoneModified(ITestableReplicationProviderInside peerA, ITestableReplicationProviderInside
			 peerB)
		{
			Assert.IsTrue(Replicate(peerA, peerB) == 0);
		}

		private void ReplicateR4(ITestableReplicationProviderInside peerA, ITestableReplicationProviderInside
			 peerB)
		{
			int replicatedObjectsCount = ReplicateAll(peerA, peerB, true);
			Assert.AreEqual(Linkers, replicatedObjectsCount);
		}

		private void SetFieldsToNull(object @object, IReflectClass claxx)
		{
			IReflectField[] fields;
			fields = claxx.GetDeclaredFields();
			for (int i = 0; i < fields.Length; i++)
			{
				IReflectField field = fields[i];
				if (field.IsStatic())
				{
					continue;
				}
				if (field.IsTransient())
				{
					continue;
				}
				field.Set(@object, null);
			}
			IReflectClass superclass = claxx.GetSuperclass();
			if (superclass == null)
			{
				return;
			}
			SetFieldsToNull(@object, superclass);
		}

		public virtual void Test()
		{
			Init(A().Provider());
			EnsureCount(A().Provider(), Linkers);
			ReplicateAllToB(A().Provider(), B().Provider());
			ReplicateNoneModified(A().Provider(), B().Provider());
			ModifyR4(A().Provider());
			EnsureR4Different(A().Provider(), B().Provider());
			ReplicateR4(A().Provider(), B().Provider());
			EnsureR4Same(A().Provider(), B().Provider());
		}
	}
}
