/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class ServerClosedTestCase : Db4oClientServerTestCase, IOptOutAllButNetworkingCS
	{
		public static void Main(string[] args)
		{
			new ServerClosedTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			IExtObjectContainer db = Fixture().Db();
			ObjectServerImpl serverImpl = (ObjectServerImpl)ClientServerFixture().Server();
			try
			{
				IEnumerator iter = serverImpl.IterateDispatchers();
				iter.MoveNext();
				ServerMessageDispatcherImpl serverDispatcher = (ServerMessageDispatcherImpl)iter.
					Current;
				serverDispatcher.Socket().Close();
				Runtime4.Sleep(1000);
				Assert.Expect(typeof(DatabaseClosedException), new _ICodeBlock_30(db));
				Assert.IsTrue(db.IsClosed());
			}
			finally
			{
				serverImpl.Close();
			}
		}

		private sealed class _ICodeBlock_30 : ICodeBlock
		{
			public _ICodeBlock_30(IExtObjectContainer db)
			{
				this.db = db;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				db.QueryByExample(null);
			}

			private readonly IExtObjectContainer db;
		}
	}
}
#endif // !SILVERLIGHT
