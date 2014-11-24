/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class ClientDisconnectTestCase : Db4oClientServerTestCase, IOptOutAllButNetworkingCS
	{
		public static void Main(string[] arguments)
		{
			new ClientDisconnectTestCase().RunNetworking();
		}

		public virtual void TestDisconnect()
		{
			IExtObjectContainer oc1 = OpenNewSession();
			IExtObjectContainer oc2 = OpenNewSession();
			try
			{
				ClientObjectContainer client1 = (ClientObjectContainer)oc1;
				ClientObjectContainer client2 = (ClientObjectContainer)oc2;
				client1.Socket().Close();
				Assert.IsFalse(oc1.IsClosed());
				Assert.Expect(typeof(Db4oException), new _ICodeBlock_27(client1));
				// It's ok for client2 to get something.
				client2.QueryByExample(null);
			}
			finally
			{
				oc1.Close();
				oc2.Close();
				Assert.IsTrue(oc1.IsClosed());
				Assert.IsTrue(oc2.IsClosed());
			}
		}

		private sealed class _ICodeBlock_27 : ICodeBlock
		{
			public _ICodeBlock_27(ClientObjectContainer client1)
			{
				this.client1 = client1;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				client1.QueryByExample(null);
			}

			private readonly ClientObjectContainer client1;
		}
	}
}
#endif // !SILVERLIGHT
