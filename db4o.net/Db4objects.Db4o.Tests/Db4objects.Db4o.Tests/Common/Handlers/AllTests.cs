/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class AllTests : Db4oTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Handlers.AllTests().RunAll();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(Db4objects.Db4o.Tests.Common.Handlers.Framework.AllTests
				), typeof(ArrayHandlerTestCase), typeof(BooleanHandlerTestCase), typeof(ByteHandlerTestCase
				), typeof(CharHandlerTestCase), typeof(ClassHandlerTestCase), typeof(CustomTypeHandlerTestCase
				), typeof(DeleteStringInUntypedFieldTestCase), typeof(DoubleHandlerTestCase), typeof(
				FloatHandlerTestCase), typeof(IgnoreFieldsTypeHandlerTestCase), typeof(IntHandlerTestCase
				), typeof(LongHandlerTestCase), typeof(MultiDimensionalArrayHandlerTestCase), typeof(
				MultidimensionalArrayIterator4TestCase), typeof(StringBufferHandlerTestCase), typeof(
				StringHandlerTestCase), typeof(ShortHandlerTestCase), typeof(UntypedHandlerTestCase
				) };
		}
	}
}
