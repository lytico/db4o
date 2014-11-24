/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class Queue4TestCaseBase : ITestCase
	{
		public Queue4TestCaseBase() : base()
		{
		}

		protected virtual void AssertIterator(IQueue4 queue, string[] data, int size)
		{
			IEnumerator iter = queue.Iterator();
			for (int idx = 0; idx < size; idx++)
			{
				Assert.IsTrue(iter.MoveNext(), "should be able to move in iteration #" + idx + " of "
					 + size);
				Assert.AreEqual(data[idx], iter.Current);
			}
			Assert.IsFalse(iter.MoveNext());
		}
	}
}
