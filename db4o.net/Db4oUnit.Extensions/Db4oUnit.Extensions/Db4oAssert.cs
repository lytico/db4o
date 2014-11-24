/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;

namespace Db4oUnit.Extensions
{
	public class Db4oAssert
	{
		public static void PersistedCount(int expected, Type extent)
		{
			Assert.AreEqual(expected, Db().Query(extent).Count);
		}

		private static IExtObjectContainer Db()
		{
			return Fixture().Db();
		}

		private static IDb4oFixture Fixture()
		{
			return Db4oFixtureVariable.Fixture();
		}
	}
}
