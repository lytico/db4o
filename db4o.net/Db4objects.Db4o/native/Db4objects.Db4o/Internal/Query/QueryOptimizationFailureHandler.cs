/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
namespace Db4objects.Db4o.Internal.Query
{
	public class QueryOptimizationFailureEventArgs : System.EventArgs
	{
		System.Exception _reason;

		public QueryOptimizationFailureEventArgs(System.Exception e)
		{
			_reason = e;
		}

		public System.Exception Reason
		{
			get { return _reason; }
		}
	}

	public delegate void QueryOptimizationFailureHandler(object sender, QueryOptimizationFailureEventArgs args);
}