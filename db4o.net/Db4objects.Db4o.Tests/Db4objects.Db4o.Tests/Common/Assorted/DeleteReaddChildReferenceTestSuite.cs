/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	/// <summary>COR-1539  Readding a deleted object from a different client changes database ID in embedded mode
	/// 	</summary>
	public class DeleteReaddChildReferenceTestSuite : FixtureTestSuiteDescription, IDb4oTestCase
	{
		public class DeleteReaddChildReferenceTestUnit : Db4oClientServerTestCase
		{
			private static readonly string ItemName = "child";

			private IExtObjectContainer client1;

			private IExtObjectContainer client2;

			public class ItemParent
			{
				public DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.Item 
					child;
			}

			public class Item
			{
				public string name;

				public Item(string name_)
				{
					name = name_;
				}
			}

			/// <exception cref="System.Exception"></exception>
			protected override void Configure(IConfiguration config)
			{
				if (!(UseIndices()))
				{
					return;
				}
				IndexField(config, typeof(DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent
					), ItemName);
				IndexField(config, typeof(DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.Item
					), "name");
			}

			private bool UseIndices()
			{
				return ((bool)SubjectFixtureProvider.Value());
			}

			/// <exception cref="System.Exception"></exception>
			protected override void Store()
			{
				DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.Item child = 
					new DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.Item(ItemName
					);
				DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent parent
					 = new DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent
					();
				parent.child = child;
				Store(parent);
			}

			public virtual void TestDeleteReaddFromOtherClient()
			{
				if (!PrepareTest())
				{
					return;
				}
				DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent parent1
					 = ((DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent
					)RetrieveOnlyInstance(client1, typeof(DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent
					)));
				DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent parent2
					 = ((DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent
					)RetrieveOnlyInstance(client2, typeof(DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent
					)));
				client1.Delete(parent1.child);
				AssertQueries(0, 1);
				client1.Commit();
				AssertQueries(0, 0);
				client2.Store(parent2.child);
				AssertQueries(0, 1);
				client2.Commit();
				AssertQueries(1, 1);
				client2.Close();
				AssertRestoredState();
			}

			public virtual void TestDeleteReaddTwiceFromOtherClient()
			{
				if (!PrepareTest())
				{
					return;
				}
				DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent parent1
					 = ((DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent
					)RetrieveOnlyInstance(client1, typeof(DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent
					)));
				DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent parent2
					 = ((DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent
					)RetrieveOnlyInstance(client2, typeof(DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent
					)));
				client1.Delete(parent1.child);
				AssertQueries(0, 1);
				client1.Commit();
				AssertQueries(0, 0);
				client2.Store(parent2.child);
				AssertQueries(0, 1);
				client2.Store(parent2.child);
				AssertQueries(0, 1);
				client2.Commit();
				AssertQueries(1, 1);
				client2.Close();
				AssertRestoredState();
			}

			public virtual void TestDeleteReaddFromBoth()
			{
				if (!PrepareTest())
				{
					return;
				}
				DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent parent1
					 = ((DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent
					)RetrieveOnlyInstance(client1, typeof(DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent
					)));
				DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent parent2
					 = ((DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent
					)RetrieveOnlyInstance(client2, typeof(DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent
					)));
				client1.Delete(parent1.child);
				AssertQueries(0, 1);
				client2.Delete(parent2.child);
				AssertQueries(0, 0);
				client1.Store(parent1.child);
				AssertQueries(1, 0);
				client2.Store(parent2.child);
				AssertQueries(1, 1);
				client1.Commit();
				AssertQueries(1, 1);
				client2.Commit();
				AssertQueries(1, 1);
				client2.Close();
				AssertRestoredState();
			}

			private void AssertRestoredState()
			{
				DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent parent3
					 = ((DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent
					)RetrieveOnlyInstance(client1, typeof(DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent
					)));
				Db().Refresh(parent3, int.MaxValue);
				Assert.IsNotNull(parent3);
				Assert.IsNotNull(parent3.child);
			}

			private void AssertQueries(int exp1, int exp2)
			{
				AssertQuery(exp1, client1);
				AssertQuery(exp2, client2);
			}

			private bool PrepareTest()
			{
				if (!IsMultiSession())
				{
					return false;
				}
				client1 = Db();
				client2 = OpenNewSession();
				return true;
			}

			private void AssertQuery(int expectedCount, IExtObjectContainer queryClient)
			{
				AssertChildClassOnlyQuery(expectedCount, queryClient);
				AssertParentChildQuery(expectedCount, queryClient);
				AssertChildQuery(expectedCount, queryClient);
			}

			private void AssertParentChildQuery(int expectedCount, IExtObjectContainer queryClient
				)
			{
				IQuery query = queryClient.Query();
				query.Constrain(typeof(DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.ItemParent
					));
				query.Descend("child").Descend("name").Constrain(ItemName);
				Assert.AreEqual(expectedCount, query.Execute().Count);
			}

			private void AssertChildQuery(int expectedCount, IExtObjectContainer queryClient)
			{
				IQuery query = queryClient.Query();
				query.Constrain(typeof(DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.Item
					));
				query.Descend("name").Constrain(ItemName);
				Assert.AreEqual(expectedCount, query.Execute().Count);
			}

			private void AssertChildClassOnlyQuery(int expectedCount, IExtObjectContainer queryClient
				)
			{
				IObjectSet result = queryClient.Query(typeof(DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit.Item
					));
				Assert.AreEqual(expectedCount, result.Count);
			}

			public static void Main(string[] arguments)
			{
				new DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit().RunAll
					();
			}
		}

		public DeleteReaddChildReferenceTestSuite()
		{
			{
				FixtureProviders(new IFixtureProvider[] { new SubjectFixtureProvider(new bool[] { 
					true, false }), new Db4oFixtureProvider() });
				TestUnits(new Type[] { typeof(DeleteReaddChildReferenceTestSuite.DeleteReaddChildReferenceTestUnit
					) });
			}
		}
	}
}
