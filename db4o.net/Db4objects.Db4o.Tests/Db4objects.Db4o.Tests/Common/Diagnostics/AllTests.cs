/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Diagnostics;

namespace Db4objects.Db4o.Tests.Common.Diagnostics
{
	public class AllTests : ComposibleTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Diagnostics.AllTests().RunAll();
		}

		protected override Type[] TestCases()
		{
			return ComposeTests(new Type[] { typeof(ClassHasNoFieldsTestCase), typeof(DescendIntoTranslatorTestCase
				), typeof(DiagnosticsTestCase), typeof(IndexFieldDiagnosticTestCase) });
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(MissingClassDiagnosticsTestCase) };
		}
		#endif // !SILVERLIGHT
	}
}
