/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4oUnit.Fixtures
{
	public sealed class TestWithFixture : ITest
	{
		private ITest _test;

		private readonly FixtureVariable _variable;

		private readonly object _value;

		private readonly string _fixtureLabel;

		public TestWithFixture(ITest test, FixtureVariable fixtureVariable, object fixtureValue
			) : this(test, null, fixtureVariable, fixtureValue)
		{
		}

		public TestWithFixture(ITest test, string fixtureLabel, FixtureVariable fixtureVariable
			, object fixtureValue)
		{
			_test = test;
			_fixtureLabel = fixtureLabel;
			_variable = fixtureVariable;
			_value = fixtureValue;
		}

		public string Label()
		{
			ObjectByRef label = new ObjectByRef();
			RunDecorated(new _IRunnable_26(this, label));
			return (string)label.value;
		}

		private sealed class _IRunnable_26 : IRunnable
		{
			public _IRunnable_26(TestWithFixture _enclosing, ObjectByRef label)
			{
				this._enclosing = _enclosing;
				this.label = label;
			}

			public void Run()
			{
				label.value = "(" + this._enclosing.FixtureLabel() + ") " + this._enclosing._test
					.Label();
			}

			private readonly TestWithFixture _enclosing;

			private readonly ObjectByRef label;
		}

		public void Run()
		{
			RunDecorated(_test);
		}

		public ITest Test()
		{
			return _test;
		}

		private void RunDecorated(IRunnable block)
		{
			_variable.With(Value(), block);
		}

		private object Value()
		{
			return _value is IDeferred4 ? ((IDeferred4)_value).Value() : _value;
		}

		private object FixtureLabel()
		{
			return (_fixtureLabel == null ? _value : _fixtureLabel);
		}

		public bool IsLeafTest()
		{
			BooleanByRef isLeaf = new BooleanByRef();
			RunDecorated(new _IRunnable_58(this, isLeaf));
			return isLeaf.value;
		}

		private sealed class _IRunnable_58 : IRunnable
		{
			public _IRunnable_58(TestWithFixture _enclosing, BooleanByRef isLeaf)
			{
				this._enclosing = _enclosing;
				this.isLeaf = isLeaf;
			}

			public void Run()
			{
				isLeaf.value = this._enclosing._test.IsLeafTest();
			}

			private readonly TestWithFixture _enclosing;

			private readonly BooleanByRef isLeaf;
		}

		public ITest Transmogrify(IFunction4 fun)
		{
			return ((ITest)fun.Apply(this));
		}
	}
}
