/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class ServerObjectContainerIsolationTestCase : EmbeddedAndNetworkingClientTestCaseBase
	{
		public class Item
		{
			public Item(string name)
			{
				_name = name;
			}

			public string _name;
		}

		public virtual void TestStoringNewItem()
		{
			ServerObjectContainer().Store(new ServerObjectContainerIsolationTestCase.Item("original"
				));
			Assert.AreEqual(0, NetworkingClient().Query(typeof(ServerObjectContainerIsolationTestCase.Item
				)).Count);
			Assert.AreEqual(1, ServerObjectContainer().Query(typeof(ServerObjectContainerIsolationTestCase.Item
				)).Count);
		}
	}
}
#endif // !SILVERLIGHT
