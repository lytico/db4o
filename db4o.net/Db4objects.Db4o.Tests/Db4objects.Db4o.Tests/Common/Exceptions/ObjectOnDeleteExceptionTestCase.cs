/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class ObjectOnDeleteExceptionTestCase : AbstractDb4oTestCase, IOptOutMultiSession
	{
		public static void Main(string[] args)
		{
			new ObjectOnDeleteExceptionTestCase().RunSolo();
		}

		public class Item
		{
			public virtual bool ObjectOnDelete(IObjectContainer container)
			{
				throw new ItemException();
			}
		}

		public virtual void Test()
		{
			ObjectOnDeleteExceptionTestCase.Item item = new ObjectOnDeleteExceptionTestCase.Item
				();
			Store(item);
			Assert.Expect(typeof(ReflectException), typeof(ItemException), new _ICodeBlock_27
				(this, item));
		}

		private sealed class _ICodeBlock_27 : ICodeBlock
		{
			public _ICodeBlock_27(ObjectOnDeleteExceptionTestCase _enclosing, ObjectOnDeleteExceptionTestCase.Item
				 item)
			{
				this._enclosing = _enclosing;
				this.item = item;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Db().Delete(item);
				this._enclosing.Db().Commit();
			}

			private readonly ObjectOnDeleteExceptionTestCase _enclosing;

			private readonly ObjectOnDeleteExceptionTestCase.Item item;
		}
	}
}
