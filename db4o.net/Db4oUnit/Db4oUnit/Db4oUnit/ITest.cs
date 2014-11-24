/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4oUnit
{
	public interface ITest : IRunnable
	{
		string Label();

		bool IsLeafTest();

		ITest Transmogrify(IFunction4 fun);
	}
}
