/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class ObjectCanActiviateExceptionTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new ObjectCanActiviateExceptionTestCase().RunSoloAndClientServer();
		}

		public class Item
		{
			public virtual bool ObjectCanActivate(IObjectContainer container)
			{
				throw new ItemException();
			}
		}

		public virtual void Test()
		{
			Store(new ObjectCanActiviateExceptionTestCase.Item());
			Assert.Expect(typeof(ReflectException), typeof(ItemException), new _ICodeBlock_25
				(this));
		}

		private sealed class _ICodeBlock_25 : ICodeBlock
		{
			public _ICodeBlock_25(ObjectCanActiviateExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				IObjectSet os = this._enclosing.Db().QueryByExample(null);
				os.Next();
			}

			private readonly ObjectCanActiviateExceptionTestCase _enclosing;
		}
	}
}
