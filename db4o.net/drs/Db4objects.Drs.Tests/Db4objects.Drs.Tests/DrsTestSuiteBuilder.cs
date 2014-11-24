/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect;
using Db4objects.Drs.Tests;
using Sharpen;

namespace Db4objects.Drs.Tests
{
	public class DrsTestSuiteBuilder : ReflectionTestSuiteBuilder
	{
		private DrsFixture _fixtures;

		public DrsTestSuiteBuilder(IDrsProviderFixture a, IDrsProviderFixture b, Type clazz
			) : this(a, b, new Type[] { clazz }, null)
		{
		}

		public DrsTestSuiteBuilder(IDrsProviderFixture a, IDrsProviderFixture b, Type clazz
			, IReflector reflector) : this(a, b, new Type[] { clazz }, reflector)
		{
		}

		public DrsTestSuiteBuilder(IDrsProviderFixture a, IDrsProviderFixture b, Type[] classes
			, IReflector reflector) : base(AppendDestructor(classes))
		{
			_fixtures = new DrsFixture(a, b, reflector);
		}

		private static Type[] AppendDestructor(Type[] classes)
		{
			Type[] newClasses = new Type[classes.Length + 1];
			System.Array.Copy(classes, 0, newClasses, 0, classes.Length);
			newClasses[newClasses.Length - 1] = typeof(DrsTestSuiteBuilder.DrsFixtureDestructor
				);
			return newClasses;
		}

		public class DrsFixtureDestructor : ITestCase
		{
			public virtual void TestFixtureDestruction()
			{
				DrsFixture fixturePair = DrsFixtureVariable.Value();
				fixturePair.a.Destroy();
				fixturePair.b.Destroy();
			}
		}

		protected override object WithContext(IClosure4 closure)
		{
			return DrsFixtureVariable.With(_fixtures, closure);
		}
	}
}
