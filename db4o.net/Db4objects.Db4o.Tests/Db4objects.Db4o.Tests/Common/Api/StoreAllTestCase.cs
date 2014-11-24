/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Api;

namespace Db4objects.Db4o.Tests.Common.Api
{
	public class StoreAllTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new StoreAllTestCase().RunAll();
		}

		public class Item
		{
			public int _id;

			public Item(int id)
			{
				_id = id;
			}

			public override bool Equals(object obj)
			{
				StoreAllTestCase.Item other = (StoreAllTestCase.Item)obj;
				return _id != other._id;
			}
		}

		internal StoreAllTestCase.Item item1 = new StoreAllTestCase.Item(1);

		internal StoreAllTestCase.Item item2 = new StoreAllTestCase.Item(2);

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ClientServer().BatchMessages(false);
		}

		public virtual void Test()
		{
			StoreAll(Container());
			ObjectSetAssert.SameContent(QueryAllItems(), new object[] { item1, item2 });
		}

		private void StoreAll(IInternalObjectContainer internalObjectContainer)
		{
			internalObjectContainer.StoreAll(Trans(), Iterators.Iterate(new StoreAllTestCase.Item
				[] { item1, item2 }));
		}

		public virtual void TestClientSendsSingleMessage()
		{
			if (!(Container() is ClientObjectContainer))
			{
				return;
			}
			ClientObjectContainer clientObjectContainer = (ClientObjectContainer)Container();
			ArrayList messages = new ArrayList();
			ClientObjectContainer.IMessageListener listener = new _IMessageListener_65(messages
				);
			Db().Store(new StoreAllTestCase.Item(0));
			// class creation
			clientObjectContainer.MessageListener(listener);
			StoreAll(clientObjectContainer);
			clientObjectContainer.Commit();
			Assert.AreEqual(1, messages.Count);
		}

		private sealed class _IMessageListener_65 : ClientObjectContainer.IMessageListener
		{
			public _IMessageListener_65(ArrayList messages)
			{
				this.messages = messages;
			}

			public void OnMessage(Msg msg)
			{
				messages.Add(msg);
			}

			private readonly ArrayList messages;
		}

		private IObjectSet QueryAllItems()
		{
			IQuery q = Db().Query();
			q.Constrain(typeof(StoreAllTestCase.Item));
			return q.Execute();
		}
	}
}
#endif // !SILVERLIGHT
