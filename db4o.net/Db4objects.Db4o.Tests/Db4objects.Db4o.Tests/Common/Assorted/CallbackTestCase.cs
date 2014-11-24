/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	/// <summary>Regression test case for COR-1117</summary>
	public class CallbackTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new CallbackTestCase().RunAll();
		}

		public virtual void TestPublicCallback()
		{
			RunTest(new CallbackTestCase.PublicCallback());
		}

		#if !SILVERLIGHT
		public virtual void TestPrivateCallback()
		{
			RunTest(new CallbackTestCase.PrivateCallback());
		}
		#endif // !SILVERLIGHT

		#if !SILVERLIGHT
		public virtual void TestPackageCallback()
		{
			RunTest(new CallbackTestCase.PackageCallback());
		}
		#endif // !SILVERLIGHT

		public virtual void TestInheritedPublicCallback()
		{
			RunTest(new CallbackTestCase.InheritedPublicCallback());
		}

		#if !SILVERLIGHT
		/// <seealso>testPrivateCallback()</seealso>
		public virtual void TestInheritedPrivateCallback()
		{
			RunTest(new CallbackTestCase.InheritedPrivateCallback());
		}
		#endif // !SILVERLIGHT

		#if !SILVERLIGHT
		/// <seealso>testPackageCallback()</seealso>
		public virtual void TestInheritedPackageCallback()
		{
			RunTest(new CallbackTestCase.InheritedPackageCallback());
		}
		#endif // !SILVERLIGHT

		public virtual void TestThrowingCallback()
		{
			Assert.Expect(typeof(Exception), new _ICodeBlock_58(this));
		}

		private sealed class _ICodeBlock_58 : ICodeBlock
		{
			public _ICodeBlock_58(CallbackTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Store(new CallbackTestCase.ThrowingCallback());
			}

			private readonly CallbackTestCase _enclosing;
		}

		private void RunTest(CallbackTestCase.Item item)
		{
			Store(item);
			Db().Commit();
			Assert.IsTrue(item.IsStored());
			Assert.IsTrue(Db().Ext().IsStored(item));
		}

		public class Item
		{
			[System.NonSerialized]
			public IObjectContainer _objectContainer;

			public virtual bool IsStored()
			{
				return _objectContainer.Ext().IsStored(this);
			}
		}

		public class PackageCallback : CallbackTestCase.Item
		{
			internal virtual void ObjectOnNew(IObjectContainer container)
			{
				_objectContainer = container;
			}
		}

		public class ThrowingCallback : CallbackTestCase.Item
		{
			internal virtual void ObjectOnNew(IObjectContainer container)
			{
				throw new Exception();
			}
		}

		public class InheritedPackageCallback : CallbackTestCase.PackageCallback
		{
		}

		public class PrivateCallback : CallbackTestCase.Item
		{
			private void ObjectOnNew(IObjectContainer container)
			{
				_objectContainer = container;
			}
		}

		public class InheritedPrivateCallback : CallbackTestCase.PrivateCallback
		{
		}

		public class PublicCallback : CallbackTestCase.Item
		{
			public virtual void ObjectOnNew(IObjectContainer container)
			{
				_objectContainer = container;
			}
		}

		public class InheritedPublicCallback : CallbackTestCase.PublicCallback
		{
		}
	}
}
