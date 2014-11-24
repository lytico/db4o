/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Tests;
using Db4objects.Db4o.Reflect;

namespace Db4oUnit.Extensions.Tests
{
	public class ExcludingReflectorTestCase : ITestCase
	{
		public class Excluded
		{
		}

		public virtual void TestExcludedClasses()
		{
			AssertNotVisible(typeof(ExcludingReflectorTestCase.Excluded));
		}

		private void AssertNotVisible(Type type)
		{
			Assert.IsNull(ReflectClassForName(type.FullName));
		}

		private IReflectClass ReflectClassForName(string className)
		{
			return new ExcludingReflector(new Type[] { typeof(ExcludingReflectorTestCase.Excluded
				) }).ForName(className);
		}

		public virtual void TestVisibleClasses()
		{
			AssertVisible(GetType());
		}

		private void AssertVisible(Type type)
		{
			Assert.IsNotNull(ReflectClassForName(type.FullName));
		}
	}
}
