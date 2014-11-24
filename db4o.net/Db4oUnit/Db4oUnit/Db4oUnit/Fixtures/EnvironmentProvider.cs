/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4oUnit.Fixtures
{
	public class EnvironmentProvider : IFixtureProvider
	{
		private sealed class _FixtureVariable_7 : FixtureVariable
		{
			public _FixtureVariable_7()
			{
			}

			public override void With(object value, IRunnable runnable)
			{
				base.With(value, new _IRunnable_9(value, runnable));
			}

			private sealed class _IRunnable_9 : IRunnable
			{
				public _IRunnable_9(object value, IRunnable runnable)
				{
					this.value = value;
					this.runnable = runnable;
				}

				public void Run()
				{
					Environments.RunWith((IEnvironment)value, runnable);
				}

				private readonly object value;

				private readonly IRunnable runnable;
			}
		}

		private readonly FixtureVariable _variable = new _FixtureVariable_7();

		public virtual FixtureVariable Variable()
		{
			return _variable;
		}

		public virtual IEnumerator GetEnumerator()
		{
			return Iterators.SingletonIterator(Environments.NewConventionBasedEnvironment());
		}
	}
}
