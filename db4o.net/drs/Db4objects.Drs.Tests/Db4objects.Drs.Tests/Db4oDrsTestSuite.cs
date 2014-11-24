/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Db4o;
using Db4objects.Drs.Tests.Dotnet;

namespace Db4objects.Drs.Tests
{
	public partial class Db4oDrsTestSuite : VersantDrsTestSuite, IDb4oTestCase
	{
		private Type[] SpecificTestCases()
		{
			return new Type[] { typeof(StructTestCase), typeof(UntypedFieldTestCase), typeof(
				PartialCollectionReplicationTestCase), typeof(TheSimplestWithCallConstructors) };
		}
	}
}
