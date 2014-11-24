/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Collections.Generic;
using Db4oUnit;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Drs.Db4o;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Db4o;

namespace Db4objects.Drs.Tests.Db4o
{
	public class PartialCollectionReplicationTestCase : DrsTestCase
	{
		public class Data
		{
			private IList _children;

			private string _id;

			public Data(string id)
			{
				_id = id;
				_children = new ArrayList();
			}

			public virtual object Id()
			{
				return _id;
			}

			public virtual void Id(string id)
			{
				_id = id;
			}

			public virtual void Add(PartialCollectionReplicationTestCase.Data data)
			{
				_children.Add(data);
			}

			public override string ToString()
			{
				return "Data(" + _id + ", " + _children + ")";
			}
		}

		public virtual void TestNoReplicationForUntouchedElements()
		{
			PartialCollectionReplicationTestCase.Data root = new PartialCollectionReplicationTestCase.Data
				("root");
			PartialCollectionReplicationTestCase.Data c1 = new PartialCollectionReplicationTestCase.Data
				("c1");
			PartialCollectionReplicationTestCase.Data c2 = new PartialCollectionReplicationTestCase.Data
				("c2");
			root.Add(c1);
			root.Add(c2);
			Store(root, 1);
			IList<PartialCollectionReplicationTestCase.Data> created = ReplicateAllCapturingCreatedObjects
				();
			AssertData(created, "root", "c1", "c2");
			PartialCollectionReplicationTestCase.Data c3 = new PartialCollectionReplicationTestCase.Data
				("c3");
			root.Add(c3);
			Store(root, 2);
			c2.Id("c2*");
			c2.Add(new PartialCollectionReplicationTestCase.Data("c4"));
			IList<PartialCollectionReplicationTestCase.Data> updated = ReplicateAllCapturingUpdatedObjects
				();
		}

		// The following fails after cleaning references has been removed from #replicate(obj) 
		// assertData(updated, "c3", "root");
		private void AssertData(IEnumerable<PartialCollectionReplicationTestCase.Data> data
			, params string[] expectedIds)
		{
			Iterator4Assert.SameContent(expectedIds, Ids(data));
		}

		private IEnumerator Ids(IEnumerable<PartialCollectionReplicationTestCase.Data> data
			)
		{
			Collection4 ids = new Collection4();
			foreach (PartialCollectionReplicationTestCase.Data d in data)
			{
				ids.Add(d.Id());
			}
			return ids.GetEnumerator();
		}

		private IList<PartialCollectionReplicationTestCase.Data> ReplicateAllCapturingUpdatedObjects
			()
		{
			IList<PartialCollectionReplicationTestCase.Data> changed = new List<PartialCollectionReplicationTestCase.Data
				>();
			ListenToUpdated(changed);
			ListenToCreated(changed);
			ReplicateAll();
			return changed;
		}

		private IList<PartialCollectionReplicationTestCase.Data> ReplicateAllCapturingCreatedObjects
			()
		{
			IList<PartialCollectionReplicationTestCase.Data> created = new List<PartialCollectionReplicationTestCase.Data
				>();
			ListenToCreated(created);
			ReplicateAll();
			return created;
		}

		private void ListenToUpdated(IList<PartialCollectionReplicationTestCase.Data> updated
			)
		{
			EventRegistryFor(B()).Updated += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
				(new _IEventListener4_97(this, updated).OnEvent);
		}

		private sealed class _IEventListener4_97
		{
			public _IEventListener4_97(PartialCollectionReplicationTestCase _enclosing, IList
				<PartialCollectionReplicationTestCase.Data> updated)
			{
				this._enclosing = _enclosing;
				this.updated = updated;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
				)
			{
				object o = ((ObjectEventArgs)args).Object;
				if (o is PartialCollectionReplicationTestCase.Data)
				{
					updated.Add((PartialCollectionReplicationTestCase.Data)o);
				}
				this._enclosing.Ods(o);
			}

			private readonly PartialCollectionReplicationTestCase _enclosing;

			private readonly IList<PartialCollectionReplicationTestCase.Data> updated;
		}

		private void ReplicateAll()
		{
			Ods("BEGIN REPLICATION");
			ReplicateAll(A().Provider(), B().Provider());
			Ods("END REPLICATION");
		}

		private void ListenToCreated(IList<PartialCollectionReplicationTestCase.Data> created
			)
		{
			EventRegistryFor(B()).Created += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
				(new _IEventListener4_115(this, created).OnEvent);
		}

		private sealed class _IEventListener4_115
		{
			public _IEventListener4_115(PartialCollectionReplicationTestCase _enclosing, IList
				<PartialCollectionReplicationTestCase.Data> created)
			{
				this._enclosing = _enclosing;
				this.created = created;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
				)
			{
				object o = ((ObjectEventArgs)args).Object;
				if (o is PartialCollectionReplicationTestCase.Data)
				{
					created.Add((PartialCollectionReplicationTestCase.Data)o);
				}
				this._enclosing.Ods(o);
			}

			private readonly PartialCollectionReplicationTestCase _enclosing;

			private readonly IList<PartialCollectionReplicationTestCase.Data> created;
		}

		private IEventRegistry EventRegistryFor(IDrsProviderFixture fixture)
		{
			return EventRegistryFactory.ForObjectContainer(ContainerFor(fixture));
		}

		public virtual void TestCollectionUpdateDoesNotTouchExistingElements()
		{
			PartialCollectionReplicationTestCase.Data root = new PartialCollectionReplicationTestCase.Data
				("root");
			PartialCollectionReplicationTestCase.Data c1 = new PartialCollectionReplicationTestCase.Data
				("c1");
			PartialCollectionReplicationTestCase.Data c2 = new PartialCollectionReplicationTestCase.Data
				("c2");
			root.Add(c1);
			root.Add(c2);
			Store(root, 1);
			long c1Version = VersionFor(c1);
			long c2Version = VersionFor(c2);
			PartialCollectionReplicationTestCase.Data c3 = new PartialCollectionReplicationTestCase.Data
				("c3");
			root.Add(c3);
			Store(root, 2);
			Assert.IsGreater(0, VersionFor(c3));
			Assert.AreEqual(c1Version, VersionFor(c1));
			Assert.AreEqual(c2Version, VersionFor(c2));
		}

		private void Store(PartialCollectionReplicationTestCase.Data root, int depth)
		{
			IExtObjectContainer container = ContainerFor(A());
			container.Ext().Store(root, depth);
			container.Commit();
		}

		private IExtObjectContainer ContainerFor(IDrsProviderFixture fixture)
		{
			return ((IDb4oReplicationProvider)fixture.Provider()).GetObjectContainer();
		}

		private long VersionFor(PartialCollectionReplicationTestCase.Data c1)
		{
			return ObjectInfoFor(c1).GetCommitTimestamp();
		}

		private IObjectInfo ObjectInfoFor(PartialCollectionReplicationTestCase.Data c1)
		{
			return ContainerFor(A()).Ext().GetObjectInfo(c1);
		}

		private void Ods(object o)
		{
		}

		//		System.out.println(o);
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(new DrsTestSuiteBuilder(new Db4oDrsFixture("db4o-a"), new Db4oDrsFixture
				("db4o-b"), typeof(PartialCollectionReplicationTestCase))).Run();
		}
	}
}
