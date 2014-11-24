/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	public class AllTests : ComposibleTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Fieldindex.AllTests().RunSolo();
		}

		protected override Type[] TestCases()
		{
			return ComposeTests(new Type[] { typeof(IndexedNodeTestCase), typeof(FieldIndexTestCase
				), typeof(FieldIndexProcessorTestCase), typeof(StringFieldIndexTestCase), typeof(
				DoubleFieldIndexTestCase), typeof(RuntimeFieldIndexTestCase), typeof(SecondLevelIndexTestCase
				), typeof(StringFieldIndexDefragmentTestCase), typeof(StringIndexTestCase), typeof(
				StringIndexCorruptionTestCase), typeof(StringIndexWithSuperClassTestCase), typeof(
				UseSecondBestIndexTestCase) });
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(CommitAfterDroppedFieldIndexTestCase) };
		}
		#endif // !SILVERLIGHT
	}
}
