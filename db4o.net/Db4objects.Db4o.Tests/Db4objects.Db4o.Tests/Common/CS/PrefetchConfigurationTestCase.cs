/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.CS;
using Sharpen.Util;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class PrefetchConfigurationTestCase : ClientServerTestCaseBase, IOptOutAllButNetworkingCS
	{
		/// <exception cref="System.Exception"></exception>
		protected override void Db4oSetupBeforeStore()
		{
			EnsureQueryGraphClassMetadataHasBeenExchanged();
		}

		public virtual void TestDefaultPrefetchDepth()
		{
			Assert.AreEqual(0, Client().Config().PrefetchDepth());
		}

		public virtual void TestPrefetchingBehaviorForClassOnlyQuery()
		{
			IQuery query = Client().Query();
			query.Constrain(typeof(PrefetchConfigurationTestCase.Item));
			AssertPrefetchingBehaviorFor(query, Msg.GetInternalIds);
		}

		public virtual void TestPrefetchingBehaviorForConstrainedQuery()
		{
			IQuery query = Client().Query();
			query.Constrain(typeof(PrefetchConfigurationTestCase.Item));
			query.Descend("child").Constrain(null);
			AssertPrefetchingBehaviorFor(query, Msg.QueryExecute);
		}

		public virtual void TestRefreshIsUnaffectedByPrefetchingBehavior()
		{
			IExtObjectContainer oc1 = Db();
			IExtObjectContainer oc2 = OpenNewSession();
			oc1.Configure().ClientServer().PrefetchDepth(1);
			oc2.Configure().ClientServer().PrefetchDepth(1);
			try
			{
				PrefetchConfigurationTestCase.Item itemFromClient1 = new PrefetchConfigurationTestCase.RootItem
					(new PrefetchConfigurationTestCase.Item());
				oc1.Store(itemFromClient1);
				oc1.Commit();
				itemFromClient1.child = null;
				oc1.Store(itemFromClient1);
				PrefetchConfigurationTestCase.Item itemFromClient2 = ((PrefetchConfigurationTestCase.RootItem
					)RetrieveOnlyInstance(oc2, typeof(PrefetchConfigurationTestCase.RootItem)));
				Assert.IsNotNull(itemFromClient2.child);
				oc1.Rollback();
				itemFromClient2 = ((PrefetchConfigurationTestCase.RootItem)RetrieveOnlyInstance(oc2
					, typeof(PrefetchConfigurationTestCase.RootItem)));
				oc2.Refresh(itemFromClient2, int.MaxValue);
				Assert.IsNotNull(itemFromClient2.child);
				oc1.Commit();
				itemFromClient2 = ((PrefetchConfigurationTestCase.RootItem)RetrieveOnlyInstance(oc2
					, typeof(PrefetchConfigurationTestCase.RootItem)));
				Assert.IsNotNull(itemFromClient2.child);
				oc1.Store(itemFromClient1);
				oc1.Commit();
				oc2.Refresh(itemFromClient2, int.MaxValue);
				itemFromClient2 = ((PrefetchConfigurationTestCase.RootItem)RetrieveOnlyInstance(oc2
					, typeof(PrefetchConfigurationTestCase.RootItem)));
				Assert.IsNull(itemFromClient2.child);
			}
			finally
			{
				oc2.Close();
			}
		}

		public virtual void TestMaxPrefetchingDepthBehavior()
		{
			StoreAllAndPurge(new PrefetchConfigurationTestCase.Item[] { new PrefetchConfigurationTestCase.Item
				(new PrefetchConfigurationTestCase.Item(new PrefetchConfigurationTestCase.Item()
				)), new PrefetchConfigurationTestCase.Item(new PrefetchConfigurationTestCase.Item
				(new PrefetchConfigurationTestCase.Item())), new PrefetchConfigurationTestCase.Item
				(new PrefetchConfigurationTestCase.Item(new PrefetchConfigurationTestCase.Item()
				)) });
			Client().Config().PrefetchObjectCount(2);
			Client().Config().PrefetchDepth(int.MaxValue);
			IQuery query = Client().Query();
			query.Constrain(typeof(PrefetchConfigurationTestCase.Item));
			query.Descend("child").Descend("child").Constrain(null).Not();
			AssertQueryIterationProtocol(query, Msg.QueryExecute, new PrefetchConfigurationTestCase.Stimulus
				[] { new PrefetchConfigurationTestCase.Depth2Stimulus(this, new MsgD[] {  }), new 
				PrefetchConfigurationTestCase.Depth2Stimulus(this, new MsgD[] {  }), new PrefetchConfigurationTestCase.Depth2Stimulus
				(this, new MsgD[] { Msg.ReadMultipleObjects }) });
		}

		public virtual void TestPrefetchingWithCyclesAscending()
		{
			PrefetchConfigurationTestCase.Item a = new PrefetchConfigurationTestCase.Item(1);
			PrefetchConfigurationTestCase.Item b = new PrefetchConfigurationTestCase.Item(2);
			PrefetchConfigurationTestCase.Item c = new PrefetchConfigurationTestCase.Item(3);
			a.child = b;
			b.child = a;
			c.child = b;
			StoreAllAndPurge(new PrefetchConfigurationTestCase.Item[] { a, b, c });
			Client().Config().PrefetchObjectCount(2);
			Client().Config().PrefetchDepth(2);
			IQuery query = QueryForItemsWithChild();
			query.Descend("order").OrderAscending();
			AssertQueryIterationProtocol(query, Msg.QueryExecute, new PrefetchConfigurationTestCase.Stimulus
				[] { new PrefetchConfigurationTestCase.Depth2Stimulus(this, new MsgD[] {  }), new 
				PrefetchConfigurationTestCase.Depth2Stimulus(this, new MsgD[] {  }), new PrefetchConfigurationTestCase.Depth2Stimulus
				(this, new MsgD[] { Msg.ReadMultipleObjects }) });
		}

		public virtual void TestPrefetchingWithCyclesDescending()
		{
			PrefetchConfigurationTestCase.Item a = new PrefetchConfigurationTestCase.Item(1);
			PrefetchConfigurationTestCase.Item b = new PrefetchConfigurationTestCase.Item(2);
			PrefetchConfigurationTestCase.Item c = new PrefetchConfigurationTestCase.Item(3);
			a.child = b;
			b.child = a;
			c.child = b;
			StoreAllAndPurge(new PrefetchConfigurationTestCase.Item[] { a, b, c });
			Client().Config().PrefetchObjectCount(2);
			Client().Config().PrefetchDepth(2);
			IQuery query = QueryForItemsWithChild();
			query.Descend("order").OrderDescending();
			AssertQueryIterationProtocol(query, Msg.QueryExecute, new PrefetchConfigurationTestCase.Stimulus
				[] { new PrefetchConfigurationTestCase.Depth2Stimulus(this, new MsgD[] {  }), new 
				PrefetchConfigurationTestCase.Depth2Stimulus(this, new MsgD[] {  }), new PrefetchConfigurationTestCase.Depth2Stimulus
				(this, new MsgD[] {  }) });
		}

		public virtual void TestPrefetchingDepth2Behavior()
		{
			StoreDepth2Graph();
			Client().Config().PrefetchObjectCount(2);
			Client().Config().PrefetchDepth(2);
			IQuery query = QueryForItemsWithChild();
			AssertQueryIterationProtocol(query, Msg.QueryExecute, new PrefetchConfigurationTestCase.Stimulus
				[] { new PrefetchConfigurationTestCase.Depth2Stimulus(this, new MsgD[] {  }), new 
				PrefetchConfigurationTestCase.Depth2Stimulus(this, new MsgD[] {  }), new PrefetchConfigurationTestCase.Depth2Stimulus
				(this, new MsgD[] { Msg.ReadMultipleObjects }) });
		}

		public virtual void TestGraphOfDepth2WithPrefetchDepth1()
		{
			StoreDepth2Graph();
			Client().Config().PrefetchObjectCount(2);
			Client().Config().PrefetchDepth(1);
			IQuery query = QueryForItemsWithChild();
			AssertQueryIterationProtocol(query, Msg.QueryExecute, new PrefetchConfigurationTestCase.Stimulus
				[] { new PrefetchConfigurationTestCase.Depth2Stimulus(this, new MsgD[] { Msg.ReadReaderById
				 }), new PrefetchConfigurationTestCase.Depth2Stimulus(this, new MsgD[] { Msg.ReadReaderById
				 }), new PrefetchConfigurationTestCase.Depth2Stimulus(this, new MsgD[] { Msg.ReadMultipleObjects
				, Msg.ReadReaderById }) });
		}

		public virtual void TestPrefetchCount1()
		{
			StoreAllAndPurge(new PrefetchConfigurationTestCase.Item[] { new PrefetchConfigurationTestCase.Item
				(), new PrefetchConfigurationTestCase.Item(), new PrefetchConfigurationTestCase.Item
				() });
			Client().Config().PrefetchObjectCount(1);
			Client().Config().PrefetchDepth(1);
			IQuery query = QueryForItemsWithoutChildren();
			AssertQueryIterationProtocol(query, Msg.QueryExecute, new PrefetchConfigurationTestCase.Stimulus
				[] { new PrefetchConfigurationTestCase.Stimulus(new MsgD[] {  }), new PrefetchConfigurationTestCase.Stimulus
				(new MsgD[] { Msg.ReadMultipleObjects }), new PrefetchConfigurationTestCase.Stimulus
				(new MsgD[] { Msg.ReadMultipleObjects }) });
		}

		public virtual void TestPrefetchingAfterDeleteFromOtherClient()
		{
			StoreAllAndPurge(new PrefetchConfigurationTestCase.Item[] { new PrefetchConfigurationTestCase.Item
				(), new PrefetchConfigurationTestCase.Item(), new PrefetchConfigurationTestCase.Item
				() });
			Client().Config().PrefetchObjectCount(1);
			Client().Config().PrefetchDepth(1);
			IQuery query = QueryForItemsWithoutChildren();
			IObjectSet result = query.Execute();
			DeleteAllItemsFromSecondClient();
			Assert.IsNotNull(((PrefetchConfigurationTestCase.Item)result.Next()));
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_211(result));
		}

		private sealed class _ICodeBlock_211 : ICodeBlock
		{
			public _ICodeBlock_211(IObjectSet result)
			{
				this.result = result;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				result.Next();
			}

			private readonly IObjectSet result;
		}

		private IQuery QueryForItemsWithoutChildren()
		{
			IQuery query = NewQuery(typeof(PrefetchConfigurationTestCase.Item));
			query.Descend("child").Constrain(null);
			return query;
		}

		private void DeleteAllItemsFromSecondClient()
		{
			IExtObjectContainer client = OpenNewSession();
			try
			{
				DeleteAll(client, typeof(PrefetchConfigurationTestCase.Item));
				client.Commit();
			}
			finally
			{
				client.Close();
			}
		}

		private IQuery QueryForItemsWithChild()
		{
			IQuery query = Client().Query();
			query.Constrain(typeof(PrefetchConfigurationTestCase.Item));
			query.Descend("child").Constrain(null).Not();
			return query;
		}

		private void StoreDepth2Graph()
		{
			StoreAllAndPurge(new PrefetchConfigurationTestCase.Item[] { new PrefetchConfigurationTestCase.Item
				(new PrefetchConfigurationTestCase.Item()), new PrefetchConfigurationTestCase.Item
				(new PrefetchConfigurationTestCase.Item()), new PrefetchConfigurationTestCase.Item
				(new PrefetchConfigurationTestCase.Item()) });
		}

		private void AssertPrefetchingBehaviorFor(IQuery query, MsgD expectedFirstMessage
			)
		{
			StoreFlatItemGraph();
			Client().Config().PrefetchObjectCount(2);
			Client().Config().PrefetchDepth(1);
			AssertQueryIterationProtocol(query, expectedFirstMessage, new PrefetchConfigurationTestCase.Stimulus
				[] { new PrefetchConfigurationTestCase.Stimulus(new MsgD[] {  }), new PrefetchConfigurationTestCase.Stimulus
				(new MsgD[] {  }), new PrefetchConfigurationTestCase.Stimulus(new MsgD[] { Msg.ReadMultipleObjects
				 }), new PrefetchConfigurationTestCase.Stimulus(new MsgD[] {  }), new PrefetchConfigurationTestCase.Stimulus
				(new MsgD[] { Msg.ReadMultipleObjects }) });
		}

		private void AssertQueryIterationProtocol(IQuery query, MsgD expectedResultMessage
			, PrefetchConfigurationTestCase.Stimulus[] stimuli)
		{
			IList messages = MessageCollector.ForServerDispatcher(ServerDispatcher());
			IObjectSet result = query.Execute();
			AssertMessages(messages, new IMessage[] { expectedResultMessage });
			messages.Clear();
			for (int stimulusIndex = 0; stimulusIndex < stimuli.Length; ++stimulusIndex)
			{
				PrefetchConfigurationTestCase.Stimulus stimulus = stimuli[stimulusIndex];
				stimulus.ActUpon(result);
				AssertMessages(messages, stimulus.expectedMessagesAfter);
				messages.Clear();
			}
			if (result.HasNext())
			{
				Assert.Fail("Unexpected item: " + ((PrefetchConfigurationTestCase.Item)result.Next
					()));
			}
			AssertMessages(messages, new IMessage[] {  });
		}

		private class Depth2Stimulus : PrefetchConfigurationTestCase.Stimulus
		{
			public Depth2Stimulus(PrefetchConfigurationTestCase _enclosing, MsgD[] expectedMessagesAfter
				) : base(expectedMessagesAfter)
			{
				this._enclosing = _enclosing;
			}

			public override void ActUpon(IObjectSet result)
			{
				this.ActUpon(((PrefetchConfigurationTestCase.Item)result.Next()));
			}

			protected virtual void ActUpon(PrefetchConfigurationTestCase.Item item)
			{
				Assert.IsNotNull(item.child);
				this._enclosing.Db().Activate(item.child, 1);
			}

			private readonly PrefetchConfigurationTestCase _enclosing;
			// ensure no further messages are exchange
		}

		public class Stimulus
		{
			public readonly MsgD[] expectedMessagesAfter;

			public Stimulus(MsgD[] expectedMessagesAfter)
			{
				this.expectedMessagesAfter = expectedMessagesAfter;
			}

			public virtual void ActUpon(IObjectSet result)
			{
				Assert.IsNotNull(((PrefetchConfigurationTestCase.Item)result.Next()));
			}
		}

		private void AssertMessages(IList actualMessages, IMessage[] expectedMessages)
		{
			Iterator4Assert.AreEqual(expectedMessages, Iterators.Iterator(actualMessages));
		}

		private void EnsureQueryGraphClassMetadataHasBeenExchanged()
		{
			Container().ProduceClassMetadata(ReflectClass(typeof(PrefetchConfigurationTestCase.Item
				)));
			// ensures classmetadata exists for query objects
			IQuery query = Client().Query();
			query.Constrain(typeof(PrefetchConfigurationTestCase.Item));
			query.Descend("child").Descend("child").Constrain(null).Not();
			query.Descend("order").OrderAscending();
			Assert.AreEqual(0, query.Execute().Count);
		}

		private void StoreFlatItemGraph()
		{
			StoreAllAndPurge(new PrefetchConfigurationTestCase.Item[] { new PrefetchConfigurationTestCase.Item
				(), new PrefetchConfigurationTestCase.Item(), new PrefetchConfigurationTestCase.Item
				(), new PrefetchConfigurationTestCase.Item(), new PrefetchConfigurationTestCase.Item
				() });
		}

		private void StoreAllAndPurge(PrefetchConfigurationTestCase.Item[] items)
		{
			StoreAll(items);
			PurgeAll(items);
			Client().Commit();
		}

		private void StoreAll(PrefetchConfigurationTestCase.Item[] items)
		{
			for (int itemIndex = 0; itemIndex < items.Length; ++itemIndex)
			{
				PrefetchConfigurationTestCase.Item item = items[itemIndex];
				Client().Store(item);
			}
		}

		private void PurgeAll(PrefetchConfigurationTestCase.Item[] items)
		{
			HashSet purged = new HashSet();
			for (int itemIndex = 0; itemIndex < items.Length; ++itemIndex)
			{
				PrefetchConfigurationTestCase.Item item = items[itemIndex];
				Purge(purged, item);
			}
		}

		private void Purge(Sharpen.Util.ISet purged, PrefetchConfigurationTestCase.Item item
			)
		{
			if (purged.Contains(item))
			{
				return;
			}
			purged.Add(item);
			Client().Purge(item);
			PrefetchConfigurationTestCase.Item child = item.child;
			if (null != child)
			{
				Purge(purged, child);
			}
		}

		public class Item
		{
			public Item(PrefetchConfigurationTestCase.Item child)
			{
				this.child = child;
			}

			public Item()
			{
			}

			public Item(int order)
			{
				this.order = order;
			}

			public PrefetchConfigurationTestCase.Item child;

			public int order;
		}

		public class RootItem : PrefetchConfigurationTestCase.Item
		{
			public RootItem() : base()
			{
			}

			public RootItem(PrefetchConfigurationTestCase.Item child) : base(child)
			{
			}
		}
	}
}
#endif // !SILVERLIGHT
