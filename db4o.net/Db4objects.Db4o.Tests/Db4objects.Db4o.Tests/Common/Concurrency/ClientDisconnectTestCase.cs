/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class ClientDisconnectTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] arguments)
		{
			new ClientDisconnectTestCase().RunConcurrency();
			new ClientDisconnectTestCase().RunConcurrency();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void _concDelete(IExtObjectContainer oc, int seq)
		{
			ClientObjectContainer client = (ClientObjectContainer)oc;
			try
			{
				if (seq % 2 == 0)
				{
					// ok to get something
					client.QueryByExample(null);
				}
				else
				{
					client.Socket().Close();
					Assert.IsFalse(oc.IsClosed());
					Assert.Expect(typeof(Db4oException), new _ICodeBlock_27(client));
				}
			}
			finally
			{
				oc.Close();
				Assert.IsTrue(oc.IsClosed());
			}
		}

		private sealed class _ICodeBlock_27 : ICodeBlock
		{
			public _ICodeBlock_27(ClientObjectContainer client)
			{
				this.client = client;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				client.QueryByExample(null);
			}

			private readonly ClientObjectContainer client;
		}
	}
}
#endif // !SILVERLIGHT
