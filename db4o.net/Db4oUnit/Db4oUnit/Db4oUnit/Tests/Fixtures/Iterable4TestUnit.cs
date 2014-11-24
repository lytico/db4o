/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4oUnit.Fixtures;

namespace Db4oUnit.Tests.Fixtures
{
	public class Iterable4TestUnit : ITestCase
	{
		private readonly IEnumerable subject = (IEnumerable)SubjectFixtureProvider.Value(
			);

		private readonly object[] data = MultiValueFixtureProvider.Value();

		public virtual void TestElements()
		{
			IEnumerator elements = subject.GetEnumerator();
			for (int i = 0; i < data.Length; ++i)
			{
				Assert.IsTrue(elements.MoveNext());
				Assert.AreEqual(data[i], elements.Current);
			}
			Assert.IsFalse(elements.MoveNext());
		}
	}
}
