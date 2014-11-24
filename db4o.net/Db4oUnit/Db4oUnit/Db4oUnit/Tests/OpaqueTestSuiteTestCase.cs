/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Tests;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Tests
{
	public class OpaqueTestSuiteTestCase : ITestCase
	{
		private const int NumTests = 3;

		public virtual void TestAllSucceed()
		{
			AssertTestRuns(new int[] {  });
		}

		public virtual void TestSingleFailure()
		{
			AssertTestRuns(new int[] { NumTests / 2 });
		}

		public virtual void TestAllFail()
		{
			int[] failingIndices = new int[NumTests];
			for (int i = 0; i < NumTests; i++)
			{
				failingIndices[i] = i;
			}
			AssertTestRuns(failingIndices);
		}

		private void AssertTestRuns(int[] failingIndices)
		{
			IntByRef counter = new IntByRef();
			TestResult result = new _TestResult_29();
			new TestRunner(Iterators.Iterable(new OpaqueTestSuiteTestCase.SimpleTestSuite[] { 
				new OpaqueTestSuiteTestCase.SimpleTestSuite(counter, NumTests, failingIndices) }
				)).Run(result);
			Assert.AreEqual(NumTests, result.TestCount);
			Assert.AreEqual(failingIndices.Length, result.Failures.Count);
			Assert.AreEqual(NumTests + 2, counter.value);
		}

		private sealed class _TestResult_29 : TestResult
		{
			public _TestResult_29()
			{
			}

			public override void TestStarted(ITest test)
			{
				base.TestStarted(test);
				Assert.IsInstanceOf(typeof(OpaqueTestSuiteTestCase.CountingTest), test);
			}
		}

		public class SimpleTestSuite : OpaqueTestSuiteBase
		{
			private IntByRef _counter;

			private int _numTests;

			public SimpleTestSuite(IntByRef counter, int numTests, int[] failingIndices) : this
				(counter, numTests, new _IClosure4_47(counter, numTests, failingIndices))
			{
			}

			private sealed class _IClosure4_47 : IClosure4
			{
				public _IClosure4_47(IntByRef counter, int numTests, int[] failingIndices)
				{
					this.counter = counter;
					this.numTests = numTests;
					this.failingIndices = failingIndices;
				}

				public object Run()
				{
					return Iterators.Iterate(OpaqueTestSuiteTestCase.SimpleTestSuite.Tests(counter, numTests
						, failingIndices));
				}

				private readonly IntByRef counter;

				private readonly int numTests;

				private readonly int[] failingIndices;
			}

			private SimpleTestSuite(IntByRef counter, int numTests, IClosure4 tests) : base(tests
				)
			{
				_counter = counter;
				_numTests = numTests;
			}

			/// <exception cref="System.Exception"></exception>
			protected override void SuiteSetUp()
			{
				Assert.AreEqual(0, _counter.value);
				_counter.value++;
			}

			/// <exception cref="System.Exception"></exception>
			protected override void SuiteTearDown()
			{
				Assert.AreEqual(_numTests + 1, _counter.value);
				_counter.value++;
			}

			public override string Label()
			{
				return GetType().FullName;
			}

			private static ITest[] Tests(IntByRef counter, int numTests, int[] failingIndices
				)
			{
				ITest[] tests = new ITest[numTests];
				for (int i = 0; i < numTests; i++)
				{
					tests[i] = new OpaqueTestSuiteTestCase.CountingTest(counter, i + 1, Arrays4.IndexOf
						(failingIndices, i) >= 0);
				}
				return tests;
			}

			protected override OpaqueTestSuiteBase Transmogrified(IClosure4 tests)
			{
				return new OpaqueTestSuiteTestCase.SimpleTestSuite(_counter, _numTests, tests);
			}
		}

		public class CountingTest : ITest
		{
			private IntByRef _counter;

			private int _idx;

			private bool _fail;

			public CountingTest(IntByRef counter, int idx, bool fail)
			{
				_counter = counter;
				_idx = idx;
				_fail = fail;
			}

			public virtual bool IsLeafTest()
			{
				return true;
			}

			public virtual string Label()
			{
				return GetType().FullName;
			}

			public virtual ITest Transmogrify(IFunction4 fun)
			{
				return ((ITest)fun.Apply(this));
			}

			public virtual void Run()
			{
				Assert.AreEqual(_idx, _counter.value);
				_counter.value++;
				if (_fail)
				{
					Assert.Fail();
				}
			}
		}
	}
}
