/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Reflection;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Extensions
{
	public class Db4oTestSuiteBuilder : ReflectionTestSuiteBuilder
	{
		private IDb4oFixture _fixture;

		public Db4oTestSuiteBuilder(IDb4oFixture fixture, Type clazz) : this(fixture, new 
			Type[] { clazz })
		{
		}

		public Db4oTestSuiteBuilder(IDb4oFixture fixture, Type[] classes) : base(classes)
		{
			Fixture(fixture);
		}

		private void Fixture(IDb4oFixture fixture)
		{
			if (null == fixture)
			{
				throw new ArgumentNullException("fixture");
			}
			_fixture = fixture;
		}

		protected override bool IsApplicable(Type clazz)
		{
			return _fixture.Accept(clazz);
		}

		protected override ITest CreateTest(object instance, MethodInfo method)
		{
			ITest test = base.CreateTest(instance, method);
			return new _TestDecorationAdapter_38(test, test);
		}

		private sealed class _TestDecorationAdapter_38 : TestDecorationAdapter
		{
			public _TestDecorationAdapter_38(ITest test, ITest baseArg1) : base(baseArg1)
			{
				this.test = test;
			}

			public override string Label()
			{
				return "(" + Db4oFixtureVariable.Fixture().Label() + ") " + test.Label();
			}

			private readonly ITest test;
		}

		protected override object WithContext(IClosure4 closure)
		{
			return Db4oFixtureVariable.FixtureVariable.With(_fixture, closure);
		}
	}
}
