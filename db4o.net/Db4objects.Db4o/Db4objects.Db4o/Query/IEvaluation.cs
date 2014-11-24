/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Query
{
	/// <summary>For implementation of callback evaluations.</summary>
	/// <remarks>
	/// For implementation of callback evaluations.
	/// <br /><br />
	/// This is for adding your own constrains to a given query.
	/// Note that evaluations force db4o to instantiate your objects in order
	/// to execute the query which slows down to query by an order of magnitude.
	/// Pass your implementation of this interface to
	/// <see cref="IQuery.Constrain(object)">IQuery.Constrain(object)</see>
	/// <br /><br />
	/// Evaluations are called as the last step during query execution,
	/// after all other constraints have been applied.
	/// <br /><br />Client-Server over a network only:<br />
	/// Avoid evaluations, because the evaluation object needs to be serialized, which is hard
	/// to manage correctly.
	/// </remarks>
	public interface IEvaluation
	{
		/// <summary>
		/// Callback method during
		/// <see cref="IQuery.Execute()">query execution</see>
		/// .
		/// </summary>
		/// <param name="candidate">reference to the candidate persistent object.</param>
		void Evaluate(ICandidate candidate);
	}
}
