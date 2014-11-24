/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Internal.Query
{
	public class NQOptimizationInfo
	{
		private Db4objects.Db4o.Query.Predicate _predicate;

		private string _message;

		private object _optimized;

		public NQOptimizationInfo(Db4objects.Db4o.Query.Predicate predicate, string message
			, object optimized)
		{
			this._predicate = predicate;
			this._message = message;
			this._optimized = optimized;
		}

		public virtual string Message()
		{
			return _message;
		}

		public virtual object Optimized()
		{
			return _optimized;
		}

		public virtual Db4objects.Db4o.Query.Predicate Predicate()
		{
			return _predicate;
		}

		public override string ToString()
		{
			return Message() + "/" + Optimized();
		}
	}
}
