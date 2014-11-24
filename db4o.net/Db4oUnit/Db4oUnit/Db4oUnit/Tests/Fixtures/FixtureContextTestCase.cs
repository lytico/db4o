/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Fixtures;
using Db4oUnit.Tests.Fixtures;
using Sharpen.Lang;

namespace Db4oUnit.Tests.Fixtures
{
	public class FixtureContextTestCase : ITestCase
	{
		public sealed class ContextRef
		{
			public FixtureContext value;
		}

		public virtual void Test()
		{
			FixtureVariable f1 = new FixtureVariable();
			FixtureVariable f2 = new FixtureVariable();
			FixtureContextTestCase.ContextRef c1 = new FixtureContextTestCase.ContextRef();
			FixtureContextTestCase.ContextRef c2 = new FixtureContextTestCase.ContextRef();
			new FixtureContext().Run(new _IRunnable_19(this, f1, f2, c1, c2));
			AssertNoValue(f1);
			AssertNoValue(f2);
			c1.value.Run(new _IRunnable_41(this, f1, f2));
			c2.value.Run(new _IRunnable_48(this, f1, f2));
		}

		private sealed class _IRunnable_19 : IRunnable
		{
			public _IRunnable_19(FixtureContextTestCase _enclosing, FixtureVariable f1, FixtureVariable
				 f2, FixtureContextTestCase.ContextRef c1, FixtureContextTestCase.ContextRef c2)
			{
				this._enclosing = _enclosing;
				this.f1 = f1;
				this.f2 = f2;
				this.c1 = c1;
				this.c2 = c2;
			}

			public void Run()
			{
				f1.With("foo", new _IRunnable_21(this, f1, f2, c1, c2));
			}

			private sealed class _IRunnable_21 : IRunnable
			{
				public _IRunnable_21(_IRunnable_19 _enclosing, FixtureVariable f1, FixtureVariable
					 f2, FixtureContextTestCase.ContextRef c1, FixtureContextTestCase.ContextRef c2)
				{
					this._enclosing = _enclosing;
					this.f1 = f1;
					this.f2 = f2;
					this.c1 = c1;
					this.c2 = c2;
				}

				public void Run()
				{
					this._enclosing._enclosing.AssertValue("foo", f1);
					this._enclosing._enclosing.AssertNoValue(f2);
					c1.value = FixtureContext.Current;
					f2.With("bar", new _IRunnable_26(this, f1, f2, c2));
				}

				private sealed class _IRunnable_26 : IRunnable
				{
					public _IRunnable_26(_IRunnable_21 _enclosing, FixtureVariable f1, FixtureVariable
						 f2, FixtureContextTestCase.ContextRef c2)
					{
						this._enclosing = _enclosing;
						this.f1 = f1;
						this.f2 = f2;
						this.c2 = c2;
					}

					public void Run()
					{
						this._enclosing._enclosing._enclosing.AssertValue("foo", f1);
						this._enclosing._enclosing._enclosing.AssertValue("bar", f2);
						c2.value = FixtureContext.Current;
					}

					private readonly _IRunnable_21 _enclosing;

					private readonly FixtureVariable f1;

					private readonly FixtureVariable f2;

					private readonly FixtureContextTestCase.ContextRef c2;
				}

				private readonly _IRunnable_19 _enclosing;

				private readonly FixtureVariable f1;

				private readonly FixtureVariable f2;

				private readonly FixtureContextTestCase.ContextRef c1;

				private readonly FixtureContextTestCase.ContextRef c2;
			}

			private readonly FixtureContextTestCase _enclosing;

			private readonly FixtureVariable f1;

			private readonly FixtureVariable f2;

			private readonly FixtureContextTestCase.ContextRef c1;

			private readonly FixtureContextTestCase.ContextRef c2;
		}

		private sealed class _IRunnable_41 : IRunnable
		{
			public _IRunnable_41(FixtureContextTestCase _enclosing, FixtureVariable f1, FixtureVariable
				 f2)
			{
				this._enclosing = _enclosing;
				this.f1 = f1;
				this.f2 = f2;
			}

			public void Run()
			{
				this._enclosing.AssertValue("foo", f1);
				this._enclosing.AssertNoValue(f2);
			}

			private readonly FixtureContextTestCase _enclosing;

			private readonly FixtureVariable f1;

			private readonly FixtureVariable f2;
		}

		private sealed class _IRunnable_48 : IRunnable
		{
			public _IRunnable_48(FixtureContextTestCase _enclosing, FixtureVariable f1, FixtureVariable
				 f2)
			{
				this._enclosing = _enclosing;
				this.f1 = f1;
				this.f2 = f2;
			}

			public void Run()
			{
				this._enclosing.AssertValue("foo", f1);
				this._enclosing.AssertValue("bar", f2);
			}

			private readonly FixtureContextTestCase _enclosing;

			private readonly FixtureVariable f1;

			private readonly FixtureVariable f2;
		}

		private void AssertNoValue(FixtureVariable f1)
		{
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_57(f1));
		}

		private sealed class _ICodeBlock_57 : ICodeBlock
		{
			public _ICodeBlock_57(FixtureVariable f1)
			{
				this.f1 = f1;
			}

			public void Run()
			{
				this.Use(f1.Value);
			}

			private void Use(object value)
			{
			}

			private readonly FixtureVariable f1;
		}

		private void AssertValue(string expected, FixtureVariable fixture)
		{
			Assert.AreEqual(expected, fixture.Value);
		}
	}
}
