/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Extensions.Tests;

namespace Db4oUnit.Extensions.Tests
{
	public class Db4oEmbeddedSessionFixtureTestCase : ITestCase
	{
		internal readonly Db4oEmbeddedSessionFixture subject = new Db4oEmbeddedSessionFixture
			();

		public virtual void TestDoesNotAcceptRegularTest()
		{
			Assert.IsFalse(subject.Accept(typeof(Db4oEmbeddedSessionFixtureTestCase.RegularTest
				)));
		}

		public virtual void TestAcceptsDb4oTest()
		{
			Assert.IsTrue(subject.Accept(typeof(Db4oEmbeddedSessionFixtureTestCase.Db4oTest))
				);
		}

		public virtual void TestDoesNotAcceptOptOutCS()
		{
			Assert.IsFalse(subject.Accept(typeof(Db4oEmbeddedSessionFixtureTestCase.OptOutTest
				)));
		}

		public virtual void TestDoesNotAcceptOptOutAllButNetworkingCS()
		{
			Assert.IsFalse(subject.Accept(typeof(Db4oEmbeddedSessionFixtureTestCase.OptOutAllButNetworkingCSTest
				)));
		}

		public virtual void TestAcceptsOptOutNetworking()
		{
			Assert.IsTrue(subject.Accept(typeof(Db4oEmbeddedSessionFixtureTestCase.OptOutNetworkingTest
				)));
		}

		internal class RegularTest : ITestCase
		{
		}

		internal class Db4oTest : IDb4oTestCase
		{
		}

		internal class OptOutTest : IOptOutMultiSession
		{
		}

		internal class OptOutNetworkingTest : IOptOutNetworkingCS
		{
		}

		internal class OptOutAllButNetworkingCSTest : IOptOutAllButNetworkingCS
		{
		}
	}
}
