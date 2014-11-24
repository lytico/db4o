/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Tests;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Tests
{
	public class OpaqueTestSuiteFailureTestCase : ITestCase
	{
		public virtual void TestFailOnSetup()
		{
			BooleanByRef tearDownCalled = new BooleanByRef();
			TestResult result = new TestResult();
			new TestRunner(Iterators.Iterable(new OpaqueTestSuiteFailureTestCase.FailingTestSuite
				[] { new OpaqueTestSuiteFailureTestCase.FailingTestSuite(true, false, tearDownCalled
				) })).Run(result);
			Assert.AreEqual(0, result.TestCount);
			Assert.AreEqual(1, result.Failures.Count);
			Assert.IsFalse(tearDownCalled.value);
		}

		public virtual void TestFailOnTearDown()
		{
			BooleanByRef tearDownCalled = new BooleanByRef();
			TestResult result = new TestResult();
			new TestRunner(Iterators.Iterable(new OpaqueTestSuiteFailureTestCase.FailingTestSuite
				[] { new OpaqueTestSuiteFailureTestCase.FailingTestSuite(false, true, tearDownCalled
				) })).Run(result);
			Assert.AreEqual(1, result.TestCount);
			Assert.AreEqual(2, result.Failures.Count);
			Assert.IsTrue(tearDownCalled.value);
		}

		public class FailingTestSuite : OpaqueTestSuiteBase
		{
			private bool _failOnSetUp;

			private bool _failOnTeardown;

			private BooleanByRef _tearDownCalled;

			public FailingTestSuite(bool failOnSetup, bool failOnTeardown, BooleanByRef tearDownCalled
				) : this(failOnSetup, failOnTeardown, tearDownCalled, new _IClosure4_34())
			{
			}

			private sealed class _IClosure4_34 : IClosure4
			{
				public _IClosure4_34()
				{
				}

				public object Run()
				{
					return Iterators.Iterate(new FailingTest[] { new FailingTest("fail", new AssertionException
						("fail")) });
				}
			}

			private FailingTestSuite(bool failOnSetup, bool failOnTeardown, BooleanByRef tearDownCalled
				, IClosure4 tests) : base(tests)
			{
				_failOnSetUp = failOnSetup;
				_failOnTeardown = failOnTeardown;
				_tearDownCalled = tearDownCalled;
			}

			/// <exception cref="System.Exception"></exception>
			protected override void SuiteSetUp()
			{
				if (_failOnSetUp)
				{
					Assert.Fail();
				}
			}

			/// <exception cref="System.Exception"></exception>
			protected override void SuiteTearDown()
			{
				_tearDownCalled.value = true;
				if (_failOnTeardown)
				{
					Assert.Fail();
				}
			}

			protected override OpaqueTestSuiteBase Transmogrified(IClosure4 tests)
			{
				return new OpaqueTestSuiteFailureTestCase.FailingTestSuite(_failOnSetUp, _failOnTeardown
					, _tearDownCalled, tests);
			}

			public override string Label()
			{
				return GetType().FullName;
			}
		}
	}
}
