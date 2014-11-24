/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

namespace Db4objects.Db4o.Internal.Query
{
	using System;

	public class NQOptimizerFactory
	{
		public static INQOptimizer CreateExpressionBuilder()
		{
			Type type = Type.GetType("Db4objects.Db4o.NativeQueries.NQOptimizer, Db4objects.Db4o.NativeQueries", true);
			return (INQOptimizer)Activator.CreateInstance(type);
		}
	}
}
