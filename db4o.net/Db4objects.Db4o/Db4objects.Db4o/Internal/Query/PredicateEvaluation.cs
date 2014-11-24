/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Query
{
	/// <exclude></exclude>
	[System.Serializable]
	public class PredicateEvaluation : IEvaluation
	{
		public Predicate _predicate;

		public PredicateEvaluation()
		{
		}

		public PredicateEvaluation(Predicate predicate)
		{
			// CS
			_predicate = predicate;
		}

		public virtual void Evaluate(ICandidate candidate)
		{
			candidate.Include(_predicate.AppliesTo(candidate.GetObject()));
		}
	}
}
