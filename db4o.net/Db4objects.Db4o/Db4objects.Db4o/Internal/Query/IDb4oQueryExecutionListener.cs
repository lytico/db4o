/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Query;

namespace Db4objects.Db4o.Internal.Query
{
	public interface IDb4oQueryExecutionListener
	{
		void NotifyQueryExecuted(NQOptimizationInfo info);
	}
}
