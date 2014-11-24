/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Jre5.Annotation;

namespace Db4objects.Db4o.Tests.Jre5.Annotation
{
	public class AllTests : Db4oTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Jre5.Annotation.AllTests().RunAll();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(IndexedAnnotationTestCase) };
		}
	}
}
