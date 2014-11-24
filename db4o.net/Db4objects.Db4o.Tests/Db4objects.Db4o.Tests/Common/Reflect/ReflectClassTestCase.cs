/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using Db4objects.Db4o.Tests.Common.Reflect;

namespace Db4objects.Db4o.Tests.Common.Reflect
{
	public class ReflectClassTestCase : ITestCase
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(ReflectClassTestCase)).Run();
		}

		public virtual void TestNameIsFullyQualified()
		{
			AssertFullyQualifiedName(GetType());
			AssertFullyQualifiedName(typeof(GenericArrayClass));
			AssertFullyQualifiedName(typeof(int[]));
		}

		private void AssertFullyQualifiedName(Type clazz)
		{
			IReflectClass reflectClass = Platform4.ReflectorForType(clazz).ForClass(clazz);
			Assert.AreEqual(ReflectPlatform.FullyQualifiedName(clazz), reflectClass.GetName()
				);
		}
	}
}
