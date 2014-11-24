/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Tests.Common.IO;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public partial class StorageTestSuite : FixtureTestSuiteDescription, IOptOutVerySlow
	{
		public override Type[] TestUnits()
		{
			return new Type[] { typeof(BinTest), typeof(ReadOnlyBinTest), typeof(StorageTest)
				 };
		}
		//	combinationToRun(2);
	}
}
