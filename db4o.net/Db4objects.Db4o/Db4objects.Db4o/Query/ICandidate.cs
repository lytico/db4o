/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;

namespace Db4objects.Db4o.Query
{
	/// <summary>
	/// Candidate for
	/// <see cref="IEvaluation">IEvaluation</see>
	/// callbacks.
	/// <br /><br />
	/// During
	/// <see cref="IQuery.Execute()">query execution</see>
	/// all registered
	/// <see cref="IEvaluation">IEvaluation</see>
	/// callback
	/// handlers are called with
	/// <see cref="ICandidate">ICandidate</see>
	/// proxies that represent the persistent objects that
	/// meet all other
	/// <see cref="IQuery">IQuery</see>
	/// criteria.
	/// <br /><br />
	/// A
	/// <see cref="ICandidate">ICandidate</see>
	/// provides access to the query candidate object. It
	/// represents and allows to specify whether it is to be included in the query result
	/// </summary>
	public interface ICandidate
	{
		/// <summary>
		/// Returns the persistent object that is represented by this query
		/// <see cref="ICandidate">ICandidate</see>
		/// .
		/// </summary>
		/// <returns>Object the persistent object.</returns>
		object GetObject();

		/// <summary>
		/// Specify whether the Candidate is to be included in the result
		/// <br /><br />
		/// This method may be called multiple times.
		/// </summary>
		/// <remarks>
		/// Specify whether the Candidate is to be included in the result
		/// <br /><br />
		/// This method may be called multiple times. The last call prevails.
		/// </remarks>
		/// <param name="flag">true to include that object. False otherwise.</param>
		void Include(bool flag);

		/// <summary>Returns the object container the query is executed on</summary>
		/// <returns>
		/// the
		/// <see cref="Db4objects.Db4o.IObjectContainer">Db4objects.Db4o.IObjectContainer</see>
		/// </returns>
		IObjectContainer ObjectContainer();
	}
}
