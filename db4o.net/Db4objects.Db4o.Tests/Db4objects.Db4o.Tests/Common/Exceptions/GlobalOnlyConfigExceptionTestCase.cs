/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class GlobalOnlyConfigExceptionTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new GlobalOnlyConfigExceptionTestCase().RunAll();
		}

		public virtual void TestBlockSize()
		{
			Assert.Expect(typeof(ArgumentException), new _ICodeBlock_16(this));
			Assert.Expect(typeof(ArgumentException), new _ICodeBlock_22(this));
			Assert.Expect(typeof(GlobalOnlyConfigException), new _ICodeBlock_28(this));
		}

		private sealed class _ICodeBlock_16 : ICodeBlock
		{
			public _ICodeBlock_16(GlobalOnlyConfigExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Db().Configure().BlockSize(-1);
			}

			private readonly GlobalOnlyConfigExceptionTestCase _enclosing;
		}

		private sealed class _ICodeBlock_22 : ICodeBlock
		{
			public _ICodeBlock_22(GlobalOnlyConfigExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Db().Configure().BlockSize(128);
			}

			private readonly GlobalOnlyConfigExceptionTestCase _enclosing;
		}

		private sealed class _ICodeBlock_28 : ICodeBlock
		{
			public _ICodeBlock_28(GlobalOnlyConfigExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Db().Configure().BlockSize(12);
			}

			private readonly GlobalOnlyConfigExceptionTestCase _enclosing;
		}
	}
}
