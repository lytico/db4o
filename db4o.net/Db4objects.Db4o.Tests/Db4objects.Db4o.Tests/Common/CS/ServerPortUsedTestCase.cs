/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class ServerPortUsedTestCase : Db4oClientServerTestCase, IOptOutAllButNetworkingCS
	{
		private static readonly string DatabaseFile = "PortUsed.db";

		public static void Main(string[] args)
		{
			new ServerPortUsedTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oTearDownBeforeClean()
		{
			File4.Delete(DatabaseFile);
		}

		public virtual void Test()
		{
			int port = ClientServerFixture().ServerPort();
			Assert.Expect(typeof(Db4oIOException), new _ICodeBlock_28(port));
		}

		private sealed class _ICodeBlock_28 : ICodeBlock
		{
			public _ICodeBlock_28(int port)
			{
				this.port = port;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Db4oClientServer.OpenServer(ServerPortUsedTestCase.DatabaseFile, port);
			}

			private readonly int port;
		}
	}
}
#endif // !SILVERLIGHT
