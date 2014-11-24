/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class InvalidPasswordTestCase : Db4oClientServerTestCase, IOptOutAllButNetworkingCS
	{
		public virtual void TestInvalidPassword()
		{
			int port = ClientServerFixture().ServerPort();
			Assert.Expect(typeof(InvalidPasswordException), new _ICodeBlock_20(this, port));
		}

		private sealed class _ICodeBlock_20 : ICodeBlock
		{
			public _ICodeBlock_20(InvalidPasswordTestCase _enclosing, int port)
			{
				this._enclosing = _enclosing;
				this.port = port;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.OpenClient("127.0.0.1", port, "strangeusername", "invalidPassword"
					);
			}

			private readonly InvalidPasswordTestCase _enclosing;

			private readonly int port;
		}

		protected virtual IObjectContainer OpenClient(string host, int port, string user, 
			string password)
		{
			return Db4oClientServer.OpenClient(host, port, user, password);
		}

		public virtual void TestEmptyUserPassword()
		{
			int port = ClientServerFixture().ServerPort();
			Assert.Expect(typeof(InvalidPasswordException), new _ICodeBlock_35(this, port));
		}

		private sealed class _ICodeBlock_35 : ICodeBlock
		{
			public _ICodeBlock_35(InvalidPasswordTestCase _enclosing, int port)
			{
				this._enclosing = _enclosing;
				this.port = port;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.OpenClient("127.0.0.1", port, string.Empty, string.Empty);
			}

			private readonly InvalidPasswordTestCase _enclosing;

			private readonly int port;
		}

		public virtual void TestEmptyUserNullPassword()
		{
			int port = ClientServerFixture().ServerPort();
			Assert.Expect(typeof(InvalidPasswordException), new _ICodeBlock_44(this, port));
		}

		private sealed class _ICodeBlock_44 : ICodeBlock
		{
			public _ICodeBlock_44(InvalidPasswordTestCase _enclosing, int port)
			{
				this._enclosing = _enclosing;
				this.port = port;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.OpenClient("127.0.0.1", port, string.Empty, null);
			}

			private readonly InvalidPasswordTestCase _enclosing;

			private readonly int port;
		}

		public virtual void TestNullPassword()
		{
			int port = ClientServerFixture().ServerPort();
			Assert.Expect(typeof(InvalidPasswordException), new _ICodeBlock_53(this, port));
		}

		private sealed class _ICodeBlock_53 : ICodeBlock
		{
			public _ICodeBlock_53(InvalidPasswordTestCase _enclosing, int port)
			{
				this._enclosing = _enclosing;
				this.port = port;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.OpenClient("127.0.0.1", port, null, null);
			}

			private readonly InvalidPasswordTestCase _enclosing;

			private readonly int port;
		}
	}
}
#endif // !SILVERLIGHT
