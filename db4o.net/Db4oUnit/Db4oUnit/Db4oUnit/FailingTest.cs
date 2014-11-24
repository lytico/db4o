/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit
{
	/// <summary>A test that always fails with a specific exception.</summary>
	/// <remarks>A test that always fails with a specific exception.</remarks>
	public class FailingTest : ITest
	{
		private readonly Exception _error;

		private readonly string _label;

		public FailingTest(string label, Exception error)
		{
			_label = label;
			_error = error;
		}

		public virtual string Label()
		{
			return _label;
		}

		public virtual Exception Error()
		{
			return _error;
		}

		public virtual void Run()
		{
			throw new TestException(_error);
		}

		public virtual bool IsLeafTest()
		{
			return true;
		}

		public virtual ITest Transmogrify(IFunction4 fun)
		{
			return ((ITest)fun.Apply(this));
		}
	}
}
