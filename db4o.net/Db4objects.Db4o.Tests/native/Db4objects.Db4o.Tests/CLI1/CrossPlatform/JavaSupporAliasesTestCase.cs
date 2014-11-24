/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

#if !SILVERLIGHT

using System;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Internal;
using Db4oUnit;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.CLI1.CrossPlatform
{
	public class JavaSupporAliasesTestCase : ITestCase
	{
		public void TestCSAliases()
		{
			Config4Impl config = new Config4Impl();
			JavaSupport javaSupport = new JavaSupport();

			javaSupport.Prepare(config);

			string resolvedName = config.ResolveAliasRuntimeName(UnversionedNameFor(typeof(ClassInfo)));
			Assert.AreEqual("com.db4o.cs.internal.ClassInfo", resolvedName);
		}

		private static string UnversionedNameFor(Type type)
		{
			return TypeReference.FromType(type).GetUnversionedName();
		}
	}
}

#endif