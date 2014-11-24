/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	/// <summary>
	/// Options:
	/// 1) activate the objects on the server up to prefetchDepth and store them into
	/// the TransportObjectContainer 1.1) connect objects to the local client cache
	/// 2) activate the objects on the server up to prefetchDepth, collect all IDs
	/// and send the required slots to the client 2.1) connect the objects to the
	/// local client cache
	/// 2') don't activate the objects but traverse slots collecting IDs instead
	/// 3) Introduce slot cache in the client and prefetch slots every time objects
	/// are activated and the required slots (prefetchDepth) are not available
	/// </summary>
	public class BatchActivationTestCase : FixtureTestSuiteDescription, IOptOutAllButNetworkingCS
	{
		public class BatchActivationTestUnit : ClientServerTestCaseBase
		{
			// first - prefetchDepth
			// second - expected number of messages exchanged
			/// <exception cref="System.Exception"></exception>
			protected override void Configure(IConfiguration config)
			{
				config.ClientServer().PrefetchDepth(PrefetchDepth());
			}

			/// <exception cref="System.Exception"></exception>
			protected override void Store()
			{
				Store(new BatchActivationTestCase.BatchActivationTestUnit.Item("foo"));
			}

			public virtual void TestClassOnlyQuery()
			{
				IQuery query = NewQuery(typeof(BatchActivationTestCase.BatchActivationTestUnit.Item
					));
				AssertBatchBehaviorFor(query);
			}

			public virtual void TestConstrainedQuery()
			{
				IQuery query = NewConstrainedQuery();
				AssertBatchBehaviorFor(query);
			}

			public virtual void TestQueryPrefetchDepth0()
			{
				IQuery query = NewConstrainedQuery();
				Client().Config().ClientServer().PrefetchDepth(0);
				AssertBatchBehaviorFor(query, 2);
			}

			public virtual void TestQueryPrefetchDepth1()
			{
				IQuery query = NewConstrainedQuery();
				Client().Config().ClientServer().PrefetchDepth(1);
				AssertBatchBehaviorFor(query, 0);
			}

			public virtual void TestQueryPrefetchDepth0ForClassOnlyQuery()
			{
				IQuery query = NewQuery(typeof(BatchActivationTestCase.BatchActivationTestUnit.Item
					));
				Client().Config().ClientServer().PrefetchDepth(0);
				AssertBatchBehaviorFor(query, 2);
			}

			public virtual void TestQueryPrefetchDepth1ForClassOnlyQuery()
			{
				IQuery query = NewQuery(typeof(BatchActivationTestCase.BatchActivationTestUnit.Item
					));
				Client().Config().ClientServer().PrefetchDepth(1);
				AssertBatchBehaviorFor(query, 0);
			}

			private IQuery NewConstrainedQuery()
			{
				IQuery query = NewQuery(typeof(BatchActivationTestCase.BatchActivationTestUnit.Item
					));
				query.Descend("name").Constrain("foo");
				return query;
			}

			private void AssertBatchBehaviorFor(IQuery query)
			{
				AssertBatchBehaviorFor(query, ExpectedMessageCount());
			}

			private void AssertBatchBehaviorFor(IQuery query, int expectedMessageCount)
			{
				IObjectSet result = query.Execute();
				IList messages = MessageCollector.ForServerDispatcher(ServerDispatcher());
				Assert.AreEqual("foo", ((BatchActivationTestCase.BatchActivationTestUnit.Item)result
					.Next()).name);
				Assert.AreEqual(expectedMessageCount, messages.Count, messages.ToString());
			}

			private int PrefetchDepth()
			{
				return (((int)Subject().first));
			}

			private Pair Subject()
			{
				return ((Pair)SubjectFixtureProvider.Value());
			}

			private int ExpectedMessageCount()
			{
				return (((int)Subject().second));
			}

			public class Item
			{
				public string name;

				public Item(string name)
				{
					this.name = name;
				}
			}
		}

		public BatchActivationTestCase()
		{
			{
				TestUnits(new Type[] { typeof(BatchActivationTestCase.BatchActivationTestUnit) });
				FixtureProviders(new IFixtureProvider[] { new SubjectFixtureProvider(new Pair[] { 
					Pair.Of(0, 2), Pair.Of(1, 0) }) });
			}
		}
	}
}
#endif // !SILVERLIGHT
