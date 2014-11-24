/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Tests;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Tests
{
	public class ReflectionTestSuiteBuilderTestCase : ITestCase
	{
		private sealed class ExcludingReflectionTestSuiteBuilder : ReflectionTestSuiteBuilder
		{
			public ExcludingReflectionTestSuiteBuilder(Type[] classes) : base(classes)
			{
			}

			protected override bool IsApplicable(Type clazz)
			{
				return clazz != typeof(ReflectionTestSuiteBuilderTestCase.NotAccepted);
			}
		}

		public class NonTestFixture
		{
		}

		public virtual void TestUnmarkedTestFixture()
		{
			ReflectionTestSuiteBuilder builder = new ReflectionTestSuiteBuilder(typeof(ReflectionTestSuiteBuilderTestCase.NonTestFixture
				));
			AssertFailingTestCase(typeof(ArgumentException), builder);
		}

		public class Accepted : ITestCase
		{
			public virtual void Test()
			{
			}
		}

		public class NotAccepted : ITestCase
		{
			public virtual void Test()
			{
			}
		}

		public virtual void TestNotAcceptedFixture()
		{
			ReflectionTestSuiteBuilder builder = new ReflectionTestSuiteBuilderTestCase.ExcludingReflectionTestSuiteBuilder
				(new Type[] { typeof(ReflectionTestSuiteBuilderTestCase.Accepted), typeof(ReflectionTestSuiteBuilderTestCase.NotAccepted
				) });
			Assert.AreEqual(1, Iterators.Size(builder.GetEnumerator()));
		}

		public class ConstructorThrows : ITestCase
		{
			public static readonly Exception Error = new Exception("no way");

			public ConstructorThrows()
			{
				throw Error;
			}

			public virtual void Test1()
			{
			}

			public virtual void Test2()
			{
			}
		}

		public virtual void TestConstructorFailuresAppearAsFailedTestCases()
		{
			ReflectionTestSuiteBuilder builder = new ReflectionTestSuiteBuilder(typeof(ReflectionTestSuiteBuilderTestCase.ConstructorThrows
				));
			Assert.AreEqual(2, Iterators.ToArray(builder.GetEnumerator()).Length);
		}

		private Exception AssertFailingTestCase(Type expectedError, ReflectionTestSuiteBuilder
			 builder)
		{
			IEnumerator tests = builder.GetEnumerator();
			FailingTest test = (FailingTest)Iterators.Next(tests);
			Assert.AreSame(expectedError, test.Error().GetType());
			return test.Error();
		}
	}
}
