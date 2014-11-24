/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
namespace Db4objects.Db4o.Linq
{
	/// <summary>
	/// commonConfiguration.Environment.Add(new MyLinqQueryMonitor());
	/// </summary>
	public interface ILinqQueryMonitor
	{
		void OnOptimizedQuery();
		void OnUnoptimizedQuery();
	}
}
