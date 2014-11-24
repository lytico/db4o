/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	/// <exclude></exclude>
	public class IntMatcherTestCase : ITestCase
	{
		public virtual void Test()
		{
			Assert.IsTrue(IntMatcher.Zero.Match(0));
			Assert.IsFalse(IntMatcher.Zero.Match(-1));
			Assert.IsFalse(IntMatcher.Zero.Match(1));
			Assert.IsFalse(IntMatcher.Zero.Match(int.MinValue));
			Assert.IsFalse(IntMatcher.Zero.Match(int.MaxValue));
			Assert.IsFalse(IntMatcher.Positive.Match(0));
			Assert.IsFalse(IntMatcher.Positive.Match(-1));
			Assert.IsTrue(IntMatcher.Positive.Match(1));
			Assert.IsFalse(IntMatcher.Positive.Match(int.MinValue));
			Assert.IsTrue(IntMatcher.Positive.Match(int.MaxValue));
			Assert.IsFalse(IntMatcher.Negative.Match(0));
			Assert.IsTrue(IntMatcher.Negative.Match(-1));
			Assert.IsFalse(IntMatcher.Negative.Match(1));
			Assert.IsTrue(IntMatcher.Negative.Match(int.MinValue));
			Assert.IsFalse(IntMatcher.Negative.Match(int.MaxValue));
		}
	}
}
