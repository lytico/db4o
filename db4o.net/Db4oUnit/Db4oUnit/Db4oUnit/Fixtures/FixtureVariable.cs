/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4oUnit.Fixtures
{
	public class FixtureVariable
	{
		public static Db4oUnit.Fixtures.FixtureVariable NewInstance(string label)
		{
			return new Db4oUnit.Fixtures.FixtureVariable(label);
		}

		private readonly string _label;

		public FixtureVariable() : this(string.Empty)
		{
		}

		public FixtureVariable(string label)
		{
			_label = label;
		}

		public virtual string Label
		{
			get
			{
				return _label;
			}
		}

		public override string ToString()
		{
			return _label;
		}

		public virtual object With(object value, IClosure4 closure)
		{
			return Inject(value).Run(closure);
		}

		public virtual void With(object value, IRunnable runnable)
		{
			Inject(value).Run(runnable);
		}

		private FixtureContext Inject(object value)
		{
			return CurrentContext().Add(this, value);
		}

		public virtual object Value
		{
			get
			{
				FixtureContext.Found found = CurrentContext().Get(this);
				if (null == found)
				{
					throw new InvalidOperationException();
				}
				return (object)found.value;
			}
		}

		private FixtureContext CurrentContext()
		{
			return FixtureContext.Current;
		}
	}
}
