/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class NoDuplicatesQueueTestCase : ITestLifeCycle
	{
		private IQueue4 _queue;

		public virtual void Test()
		{
			_queue.Add("A");
			_queue.Add("B");
			_queue.Add("B");
			_queue.Add("A");
			Assert.AreEqual("A", _queue.Next());
			Assert.AreEqual("B", _queue.Next());
			Assert.IsFalse(_queue.HasNext());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			_queue = new NoDuplicatesQueue(new NonblockingQueue());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
			_queue = null;
		}
	}
}
