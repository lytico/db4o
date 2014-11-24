/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;

namespace Db4oUnit.Tests
{
	public class AssertTestCase : ITestCase
	{
		public virtual void TestAreEqual()
		{
			Assert.AreEqual(true, true);
			Assert.AreEqual(42, 42);
			Assert.AreEqual(42, 42);
			Assert.AreEqual(null, null);
			ExpectFailure(new _ICodeBlock_14());
			ExpectFailure(new _ICodeBlock_19());
			ExpectFailure(new _ICodeBlock_24());
			ExpectFailure(new _ICodeBlock_29());
		}

		private sealed class _ICodeBlock_14 : ICodeBlock
		{
			public _ICodeBlock_14()
			{
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Assert.AreEqual(true, false);
			}
		}

		private sealed class _ICodeBlock_19 : ICodeBlock
		{
			public _ICodeBlock_19()
			{
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Assert.AreEqual(42, 43);
			}
		}

		private sealed class _ICodeBlock_24 : ICodeBlock
		{
			public _ICodeBlock_24()
			{
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Assert.AreEqual(new object(), new object());
			}
		}

		private sealed class _ICodeBlock_29 : ICodeBlock
		{
			public _ICodeBlock_29()
			{
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Assert.AreEqual(null, new object());
			}
		}

		public virtual void TestAreSame()
		{
			ExpectFailure(new _ICodeBlock_37());
			Assert.AreSame(this, this);
		}

		private sealed class _ICodeBlock_37 : ICodeBlock
		{
			public _ICodeBlock_37()
			{
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Assert.AreSame(new object(), new object());
			}
		}

		private void ExpectFailure(ICodeBlock block)
		{
			Assert.Expect(typeof(AssertionException), block);
		}
	}
}
