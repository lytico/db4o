/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Fixtures;
using Db4oUnit.Mocking;
using Db4oUnit.Tests.Fixtures;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Tests.Fixtures
{
	public class FixtureBasedTestSuiteTestCase : ITestCase
	{
		internal static FixtureVariable RecorderFixture = FixtureVariable.NewInstance("recorder"
			);

		internal static FixtureVariable Fixture1 = new FixtureVariable("f1");

		internal static FixtureVariable Fixture2 = new FixtureVariable("f2");

		public sealed class TestUnit : ITestCase
		{
			private readonly object fixture1 = Fixture1.Value;

			private readonly object fixture2 = Fixture2.Value;

			public void TestFoo()
			{
				Record("testFoo");
			}

			public void TestBar()
			{
				Record("testBar");
			}

			private void Record(string test)
			{
				Recorder().Record(new MethodCall(test, new object[] { fixture1, fixture2 }));
			}

			private MethodCallRecorder Recorder()
			{
				return ((MethodCallRecorder)RecorderFixture.Value);
			}
		}

		public virtual void Test()
		{
			MethodCallRecorder recorder = new MethodCallRecorder();
			Run(new _FixtureBasedTestSuite_45(recorder));
			//		System.out.println(CodeGenerator.generateMethodCallArray(recorder));
			recorder.Verify(new MethodCall[] { new MethodCall("testFoo", new object[] { "f11"
				, "f21" }), new MethodCall("testFoo", new object[] { "f11", "f22" }), new MethodCall
				("testFoo", new object[] { "f12", "f21" }), new MethodCall("testFoo", new object
				[] { "f12", "f22" }), new MethodCall("testBar", new object[] { "f11", "f21" }), 
				new MethodCall("testBar", new object[] { "f11", "f22" }), new MethodCall("testBar"
				, new object[] { "f12", "f21" }), new MethodCall("testBar", new object[] { "f12"
				, "f22" }) });
		}

		private sealed class _FixtureBasedTestSuite_45 : FixtureBasedTestSuite
		{
			public _FixtureBasedTestSuite_45(MethodCallRecorder recorder)
			{
				this.recorder = recorder;
			}

			public override IFixtureProvider[] FixtureProviders()
			{
				return new IFixtureProvider[] { new SimpleFixtureProvider(FixtureBasedTestSuiteTestCase
					.RecorderFixture, new object[] { recorder }), new SimpleFixtureProvider(FixtureBasedTestSuiteTestCase
					.Fixture1, new object[] { "f11", "f12" }), new SimpleFixtureProvider(FixtureBasedTestSuiteTestCase
					.Fixture2, new object[] { "f21", "f22" }) };
			}

			public override Type[] TestUnits()
			{
				return new Type[] { typeof(FixtureBasedTestSuiteTestCase.TestUnit) };
			}

			private readonly MethodCallRecorder recorder;
		}

		public virtual void TestCombinationToRun()
		{
			MethodCallRecorder recorder = new MethodCallRecorder();
			Run(new _FixtureBasedTestSuite_78(recorder));
			//		System.out.println(CodeGenerator.generateMethodCallArray(recorder));
			recorder.Verify(new MethodCall[] { new MethodCall("testFoo", new object[] { "f11"
				, "f22" }), new MethodCall("testBar", new object[] { "f11", "f22" }) });
		}

		private sealed class _FixtureBasedTestSuite_78 : FixtureBasedTestSuite
		{
			public _FixtureBasedTestSuite_78(MethodCallRecorder recorder)
			{
				this.recorder = recorder;
			}

			public override IFixtureProvider[] FixtureProviders()
			{
				return new IFixtureProvider[] { new SimpleFixtureProvider(FixtureBasedTestSuiteTestCase
					.RecorderFixture, new object[] { recorder }), new SimpleFixtureProvider(FixtureBasedTestSuiteTestCase
					.Fixture1, new object[] { "f11", "f12" }), new SimpleFixtureProvider(FixtureBasedTestSuiteTestCase
					.Fixture2, new object[] { "f21", "f22" }) };
			}

			public override Type[] TestUnits()
			{
				return new Type[] { typeof(FixtureBasedTestSuiteTestCase.TestUnit) };
			}

			public override int[] CombinationToRun()
			{
				return new int[] { 0, 0, 1 };
			}

			private readonly MethodCallRecorder recorder;
		}

		public virtual void TestInvalidCombinationToRun()
		{
			Assert.Expect(typeof(AssertionException), new _ICodeBlock_107(this));
		}

		private sealed class _ICodeBlock_107 : ICodeBlock
		{
			public _ICodeBlock_107(FixtureBasedTestSuiteTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing.RunInvalidCombination();
			}

			private readonly FixtureBasedTestSuiteTestCase _enclosing;
		}

		private void RunInvalidCombination()
		{
			Run(new _FixtureBasedTestSuite_115());
		}

		private sealed class _FixtureBasedTestSuite_115 : FixtureBasedTestSuite
		{
			public _FixtureBasedTestSuite_115()
			{
			}

			public override IFixtureProvider[] FixtureProviders()
			{
				return new IFixtureProvider[] { new SimpleFixtureProvider(FixtureBasedTestSuiteTestCase
					.Fixture1, new object[] { "f11", "f12" }), new SimpleFixtureProvider(FixtureBasedTestSuiteTestCase
					.Fixture2, new object[] { "f21", "f22" }) };
			}

			public override Type[] TestUnits()
			{
				return new Type[] { typeof(FixtureBasedTestSuiteTestCase.TestUnit) };
			}

			public override int[] CombinationToRun()
			{
				return new int[] { 0 };
			}
		}

		private void Run(FixtureBasedTestSuite suite)
		{
			TestResult result = new TestResult();
			new TestRunner(suite).Run(result);
			if (result.Failures.Count > 0)
			{
				Assert.Fail(Iterators.ToString(result.Failures));
			}
		}

		public virtual void TestLabel()
		{
			FixtureBasedTestSuite suite = new _FixtureBasedTestSuite_142();
			IEnumerable labels = Iterators.Map(suite, new _IFunction4_154());
			Iterator4Assert.AreEqual(new object[] { TestLabel("testFoo", 0, 0), TestLabel("testFoo"
				, 1, 0), TestLabel("testFoo", 0, 1), TestLabel("testFoo", 1, 1), TestLabel("testBar"
				, 0, 0), TestLabel("testBar", 1, 0), TestLabel("testBar", 0, 1), TestLabel("testBar"
				, 1, 1) }, labels.GetEnumerator());
		}

		private sealed class _FixtureBasedTestSuite_142 : FixtureBasedTestSuite
		{
			public _FixtureBasedTestSuite_142()
			{
			}

			public override IFixtureProvider[] FixtureProviders()
			{
				return new IFixtureProvider[] { new SimpleFixtureProvider(FixtureBasedTestSuiteTestCase
					.Fixture1, new object[] { "f11", "f12" }), new SimpleFixtureProvider(FixtureBasedTestSuiteTestCase
					.Fixture2, new object[] { "f21", "f22" }) };
			}

			public override Type[] TestUnits()
			{
				return new Type[] { typeof(FixtureBasedTestSuiteTestCase.TestUnit) };
			}
		}

		private sealed class _IFunction4_154 : IFunction4
		{
			public _IFunction4_154()
			{
			}

			public object Apply(object arg)
			{
				return ((ITest)arg).Label();
			}
		}

		private string TestLabel(string testMethod, int fixture1Index, int fixture2Index)
		{
			string prefix = "(f2[" + fixture1Index + "]) (f1[" + fixture2Index + "]) ";
			return prefix + typeof(FixtureBasedTestSuiteTestCase.TestUnit).FullName + "." + testMethod;
		}
	}
}
