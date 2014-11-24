/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Drs;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class ReplicationEventTest : DrsTestCase
	{
		private static readonly string InA = "in A";

		private static readonly string ModifiedInA = "modified in A";

		private static readonly string ModifiedInB = "modified in B";

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
			Assert.AreEqual(parent.GetName(), parentName);
			Assert.AreEqual(childName, parent.GetChild().GetName());
		}

		private void EnsureNotExist(ITestableReplicationProviderInside provider, Type type
			)
		{
			Assert.IsTrue(!provider.GetStoredObjects(type).GetEnumerator().MoveNext());
		}

		private void EnsureOneInstanceOfParentAndChild(IDrsProviderFixture fixture)
		{
			EnsureOneInstance(fixture, typeof(SPCParent));
			EnsureOneInstance(fixture, typeof(SPCChild));
		}

		private void ModifyInProviderA()
		{
			SPCParent parent = (SPCParent)GetOneInstance(A(), typeof(SPCParent));
			parent.SetName(ModifiedInA);
			SPCChild child = parent.GetChild();
			child.SetName(ModifiedInA);
			A().Provider().Update(parent);
			A().Provider().Update(child);
			A().Provider().Commit();
			EnsureNames(A(), ModifiedInA, ModifiedInA);
		}

		private void ModifyInProviderB()
		{
			SPCParent parent = (SPCParent)GetOneInstance(B(), typeof(SPCParent));
			parent.SetName(ModifiedInB);
			SPCChild child = parent.GetChild();
			child.SetName(ModifiedInB);
			B().Provider().Update(parent);
			B().Provider().Update(child);
			B().Provider().Commit();
			EnsureNames(B(), ModifiedInB, ModifiedInB);
		}

		private void ReplicateAllToProviderBFirstTime()
		{
			ReplicateAll(A().Provider(), B().Provider());
			EnsureNames(A(), InA, InA);
			EnsureNames(B(), InA, InA);
		}

		private void StoreParentAndChildToProviderA()
		{
			SPCChild child = new SPCChild(InA);
			SPCParent parent = new SPCParent(child, InA);
			A().Provider().StoreNew(parent);
			A().Provider().Commit();
			EnsureNames(A(), InA, InA);
		}

		public virtual void TestNewObject()
		{
			StoreParentAndChildToProviderA();
			ReplicationEventTest.BooleanClosure invoked = new ReplicationEventTest.BooleanClosure
				(false);
			IReplicationEventListener listener = new _IReplicationEventListener_203(invoked);
			ReplicateAll(A().Provider(), B().Provider(), listener);
			Assert.IsTrue(invoked.GetValue());
			EnsureNames(A(), InA, InA);
			EnsureNotExist(B().Provider(), typeof(SPCParent));
			EnsureNotExist(B().Provider(), typeof(SPCChild));
		}

		private sealed class _IReplicationEventListener_203 : IReplicationEventListener
		{
			public _IReplicationEventListener_203(ReplicationEventTest.BooleanClosure invoked
				)
			{
				this.invoked = invoked;
			}

			public void OnReplicate(IReplicationEvent @event)
			{
				invoked.SetValue(true);
				IObjectState stateA = @event.StateInProviderA();
				IObjectState stateB = @event.StateInProviderB();
				Assert.IsTrue(stateA.IsNew());
				Assert.IsTrue(!stateB.IsNew());
				Assert.IsNotNull(stateA.GetObject());
				Assert.IsNull(stateB.GetObject());
				@event.OverrideWith(null);
			}

			private readonly ReplicationEventTest.BooleanClosure invoked;
		}

		public virtual void TestNoAction()
		{
			StoreParentAndChildToProviderA();
			ReplicateAllToProviderBFirstTime();
			ModifyInProviderB();
			IReplicationEventListener listener = new _IReplicationEventListener_234();
			//do nothing
			ReplicateAll(B().Provider(), A().Provider(), listener);
			EnsureNames(A(), ModifiedInB, ModifiedInB);
			EnsureNames(B(), ModifiedInB, ModifiedInB);
		}

		private sealed class _IReplicationEventListener_234 : IReplicationEventListener
		{
			public _IReplicationEventListener_234()
			{
			}

			public void OnReplicate(IReplicationEvent @event)
			{
			}
		}

		public virtual void TestOverrideWhenConflicts()
		{
			StoreParentAndChildToProviderA();
			ReplicateAllToProviderBFirstTime();
			//introduce conflicts
			ModifyInProviderA();
			ModifyInProviderB();
			IReplicationEventListener listener = new _IReplicationEventListener_254();
			ReplicateAll(A().Provider(), B().Provider(), listener);
			EnsureNames(A(), ModifiedInB, ModifiedInB);
			EnsureNames(B(), ModifiedInB, ModifiedInB);
		}

		private sealed class _IReplicationEventListener_254 : IReplicationEventListener
		{
			public _IReplicationEventListener_254()
			{
			}

			public void OnReplicate(IReplicationEvent @event)
			{
				Assert.IsTrue(@event.IsConflict());
				if (@event.IsConflict())
				{
					@event.OverrideWith(@event.StateInProviderB());
				}
			}
		}

		public virtual void TestOverrideWhenNoConflicts()
		{
			StoreParentAndChildToProviderA();
			ReplicateAllToProviderBFirstTime();
			ModifyInProviderB();
			IReplicationEventListener listener = new _IReplicationEventListener_274();
			ReplicateAll(B().Provider(), A().Provider(), listener);
			EnsureNames(A(), InA, InA);
			EnsureNames(B(), InA, InA);
		}

		private sealed class _IReplicationEventListener_274 : IReplicationEventListener
		{
			public _IReplicationEventListener_274()
			{
			}

			public void OnReplicate(IReplicationEvent @event)
			{
				Assert.IsTrue(!@event.IsConflict());
				@event.OverrideWith(@event.StateInProviderB());
			}
		}

		public virtual void TestStopTraversal()
		{
			StoreParentAndChildToProviderA();
			ReplicateAllToProviderBFirstTime();
			//introduce conflicts
			ModifyInProviderA();
			ModifyInProviderB();
			IReplicationEventListener listener = new _IReplicationEventListener_295();
			ReplicateAll(A().Provider(), B().Provider(), listener);
			EnsureNames(A(), ModifiedInA, ModifiedInA);
			EnsureNames(B(), ModifiedInB, ModifiedInB);
		}

		private sealed class _IReplicationEventListener_295 : IReplicationEventListener
		{
			public _IReplicationEventListener_295()
			{
			}

			public void OnReplicate(IReplicationEvent @event)
			{
				Assert.IsTrue(@event.IsConflict());
				@event.OverrideWith(null);
			}
		}

		internal class BooleanClosure
		{
			private bool value;

			public BooleanClosure(bool value)
			{
				this.value = value;
			}

			internal virtual void SetValue(bool v)
			{
				value = v;
			}

			public virtual bool GetValue()
			{
				return value;
			}
		}
	}
}
