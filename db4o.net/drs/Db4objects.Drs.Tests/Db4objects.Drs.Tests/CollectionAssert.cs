/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4objects.Drs.Tests;

namespace Db4objects.Drs.Tests
{
	public class CollectionAssert
	{
		public static void AreEqual(IEnumerable expected, IEnumerable actual)
		{
			Iterator4Assert.AreEqual(CollectionAssert.Adapt(expected.GetEnumerator()), CollectionAssert
				.Adapt(actual.GetEnumerator()));
		}

		internal static IEnumerator Adapt(IEnumerator iterator)
		{
			return ReplicationTestPlatform.Adapt(iterator);
		}
	}
}
