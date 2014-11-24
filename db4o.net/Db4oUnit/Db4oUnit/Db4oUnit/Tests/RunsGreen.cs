/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Tests
{
	internal class RunsGreen : ITest
	{
		public virtual string Label()
		{
			return "RunsGreen";
		}

		public virtual void Run()
		{
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
