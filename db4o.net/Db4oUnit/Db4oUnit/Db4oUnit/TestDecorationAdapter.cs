/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit
{
	public class TestDecorationAdapter : ITest
	{
		private readonly ITest _test;

		public TestDecorationAdapter(ITest test)
		{
			_test = test;
		}

		public virtual string Label()
		{
			return _test.Label();
		}

		public virtual void Run()
		{
			_test.Run();
		}

		public virtual bool IsLeafTest()
		{
			return _test.IsLeafTest();
		}

		public virtual ITest Transmogrify(IFunction4 fun)
		{
			return ((ITest)fun.Apply(this));
		}
	}
}
