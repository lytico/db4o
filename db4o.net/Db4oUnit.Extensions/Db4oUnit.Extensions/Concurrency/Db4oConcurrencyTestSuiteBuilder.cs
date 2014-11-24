/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Reflection;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Concurrency;
using Db4objects.Db4o.Ext;

namespace Db4oUnit.Extensions.Concurrency
{
	public class Db4oConcurrencyTestSuiteBuilder : Db4oTestSuiteBuilder
	{
		public Db4oConcurrencyTestSuiteBuilder(IDb4oFixture fixture, Type clazz) : base(fixture
			, clazz)
		{
		}

		public Db4oConcurrencyTestSuiteBuilder(IDb4oFixture fixture, Type[] classes) : base
			(fixture, classes)
		{
		}

		protected override ITest CreateTest(object instance, MethodInfo method)
		{
			return new ConcurrencyTestMethod(instance, method);
		}

		protected override bool IsTestMethod(MethodInfo method)
		{
			string name = method.Name;
			return StartsWithIgnoreCase(name, ConcurrencyConventions.TestPrefix()) && TestPlatform
				.IsPublic(method) && !TestPlatform.IsStatic(method) && HasValidParameter(method);
		}

		internal static bool HasValidParameter(MethodInfo method)
		{
			Type[] parameters = Sharpen.Runtime.GetParameterTypes(method);
			if (parameters.Length == 1 && parameters[0] == typeof(IExtObjectContainer))
			{
				return true;
			}
			if (parameters.Length == 2 && parameters[0] == typeof(IExtObjectContainer) && parameters
				[1] == typeof(int))
			{
				return true;
			}
			return false;
		}
	}
}
