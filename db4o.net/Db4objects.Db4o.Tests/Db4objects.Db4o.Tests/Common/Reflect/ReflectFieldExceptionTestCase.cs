/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Reflect;

namespace Db4objects.Db4o.Tests.Common.Reflect
{
	public class ReflectFieldExceptionTestCase : ITestCase
	{
		public class Item
		{
			public string _name;
		}

		public virtual void TestExceptionIsPropagated()
		{
			IReflector reflector = Platform4.ReflectorForType(typeof(ReflectFieldExceptionTestCase.Item
				));
			IReflectField field = reflector.ForClass(typeof(ReflectFieldExceptionTestCase.Item
				)).GetDeclaredField("_name");
			Assert.Expect(typeof(Db4oException), typeof(ArgumentException), new _ICodeBlock_18
				(field));
		}

		private sealed class _ICodeBlock_18 : ICodeBlock
		{
			public _ICodeBlock_18(IReflectField field)
			{
				this.field = field;
			}

			public void Run()
			{
				field.Set(new ReflectFieldExceptionTestCase.Item(), 42);
			}

			private readonly IReflectField field;
		}
	}
}
