/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Collections.Generic;
using Db4oUnit;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;
using Db4objects.Drs.Db4o;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;
using Sharpen.Util;

namespace Db4objects.Drs.Tests
{
	public class SingleTypeCollectionReplicationTest : FixtureBasedTestSuite
	{
		private static readonly FixtureVariable TransparentActivationFixture = FixtureVariable
			.NewInstance("Transparent Activation");

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[] { new SubjectFixtureProvider(new SingleTypeCollectionReplicationTest.CollectionHolderFactory
				[] { Collection1(), Collection2(), Collection3() }), new SimpleFixtureProvider(TransparentActivationFixture
				, (object[])LabeledObject.ForObjects(new object[] { false, true })) };
		}

		public abstract class CollectionHolderFactory : ILabeled
		{
			public abstract CollectionHolder NewCollectionHolder();

			public abstract string Label();
		}

		private SingleTypeCollectionReplicationTest.CollectionHolderFactory Collection1()
		{
			return new _CollectionHolderFactory_56(this);
		}

		private sealed class _CollectionHolderFactory_56 : SingleTypeCollectionReplicationTest.CollectionHolderFactory
		{
			public _CollectionHolderFactory_56(SingleTypeCollectionReplicationTest _enclosing
				)
			{
				this._enclosing = _enclosing;
			}

			public override CollectionHolder NewCollectionHolder()
			{
				return this._enclosing.Initialize(new CollectionHolder("Hashtable", new Hashtable
					(), new HashSet(), new ArrayList()));
			}

			public override string Label()
			{
				return "Hashtable";
			}

			private readonly SingleTypeCollectionReplicationTest _enclosing;
		}

		private SingleTypeCollectionReplicationTest.CollectionHolderFactory Collection2()
		{
			return new _CollectionHolderFactory_74(this);
		}

		private sealed class _CollectionHolderFactory_74 : SingleTypeCollectionReplicationTest.CollectionHolderFactory
		{
			public _CollectionHolderFactory_74(SingleTypeCollectionReplicationTest _enclosing
				)
			{
				this._enclosing = _enclosing;
			}

			public override CollectionHolder NewCollectionHolder()
			{
				return this._enclosing.Initialize(new CollectionHolder("HashMap", new Dictionary<
					string, string>(), new HashSet(), new List<string>()));
			}

			public override string Label()
			{
				return "HashMap";
			}

			private readonly SingleTypeCollectionReplicationTest _enclosing;
		}

		private SingleTypeCollectionReplicationTest.CollectionHolderFactory Collection3()
		{
			return new _CollectionHolderFactory_92(this);
		}

		private sealed class _CollectionHolderFactory_92 : SingleTypeCollectionReplicationTest.CollectionHolderFactory
		{
			public _CollectionHolderFactory_92(SingleTypeCollectionReplicationTest _enclosing
				)
			{
				this._enclosing = _enclosing;
			}

			public override CollectionHolder NewCollectionHolder()
			{
				return this._enclosing.Initialize(new CollectionHolder("TreeMap", new SortedList<
					string, string>(), new HashSet(), new ArrayList()));
			}

			public override string Label()
			{
				return "TreeMap";
			}

			private readonly SingleTypeCollectionReplicationTest _enclosing;
		}

		private CollectionHolder Initialize(CollectionHolder h1)
		{
			h1.Map()["1"] = "one";
			h1.Map()["2"] = "two";
			h1.Set().Add("two");
			h1.List().Add("three");
			return h1;
		}

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(SingleTypeCollectionReplicationTest.TestUnit) };
		}

		public class TestUnit : DrsTestCase
		{
			protected override void Configure(IConfiguration config)
			{
				LabeledObject transparentActivation = (LabeledObject)TransparentActivationFixture
					.Value;
				if ((bool)transparentActivation.Value())
				{
					config.Add(new TransparentActivationSupport());
				}
			}

			public virtual void Test()
			{
				CollectionHolder h1 = Subject();
				StoreNewAndCommit(A().Provider(), h1);
				ReplicateAll(A().Provider(), B().Provider());
				IEnumerator it = B().Provider().GetStoredObjects(typeof(CollectionHolder)).GetEnumerator
					();
				Assert.IsTrue(it.MoveNext());
				CollectionHolder replica = (CollectionHolder)it.Current;
				B().Provider().Activate(replica);
				AssertSameClassIfDb4o(h1.Map(), replica.Map());
				foreach (object key in h1.Map().Keys)
				{
					B().Provider().Activate(replica.Map());
					Assert.AreEqual(h1.Map()[key], replica.Map()[key]);
				}
				AssertSameClassIfDb4o(h1.Set(), replica.Set());
				foreach (object element in h1.Set())
				{
					Assert.IsTrue(replica.Set().Contains(element));
				}
				AssertSameClassIfDb4o(h1.List(), replica.List());
				Assert.AreEqual(h1.List().Count, replica.List().Count);
				CollectionAssert.AreEqual(h1.List(), replica.List());
			}

			private CollectionHolder Subject()
			{
				SingleTypeCollectionReplicationTest.CollectionHolderFactory factory = (SingleTypeCollectionReplicationTest.CollectionHolderFactory
					)SubjectFixtureProvider.Value();
				return factory.NewCollectionHolder();
			}

			private void AssertSameClassIfDb4o(object expectedInstance, object actualInstance
				)
			{
				if (!IsDb4oProvider(A()))
				{
					return;
				}
				if (!IsDb4oProvider(B()))
				{
					return;
				}
				Assert.AreSame(expectedInstance.GetType(), actualInstance.GetType());
			}

			private bool IsDb4oProvider(IDrsProviderFixture fixture)
			{
				return fixture.Provider() is IDb4oReplicationProvider;
			}

			private void StoreNewAndCommit(ITestableReplicationProviderInside provider, CollectionHolder
				 h1)
			{
				provider.StoreNew(h1);
				provider.Activate(h1);
				provider.Commit();
			}
		}
	}
}
