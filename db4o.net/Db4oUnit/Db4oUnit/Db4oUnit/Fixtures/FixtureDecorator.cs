/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Fixtures
{
	internal sealed class FixtureDecorator : ITestDecorator
	{
		private readonly object _fixture;

		private readonly FixtureVariable _provider;

		private readonly int _fixtureIndex;

		internal FixtureDecorator(FixtureVariable provider, object fixture, int fixtureIndex
			)
		{
			_fixture = fixture;
			_provider = provider;
			_fixtureIndex = fixtureIndex;
		}

		public ITest Decorate(ITest test)
		{
			string label = Label();
			return test.Transmogrify(new _IFunction4_22(this, label));
		}

		private sealed class _IFunction4_22 : IFunction4
		{
			public _IFunction4_22(FixtureDecorator _enclosing, string label)
			{
				this._enclosing = _enclosing;
				this.label = label;
			}

			public object Apply(object innerTest)
			{
				return new TestWithFixture(((ITest)innerTest), label, this._enclosing._provider, 
					this._enclosing._fixture);
			}

			private readonly FixtureDecorator _enclosing;

			private readonly string label;
		}

		private string Label()
		{
			string label = _provider.Label + "[" + _fixtureIndex + "]";
			if (_fixture is ILabeled)
			{
				label += ":" + ((ILabeled)_fixture).Label();
			}
			return label;
		}
	}
}
