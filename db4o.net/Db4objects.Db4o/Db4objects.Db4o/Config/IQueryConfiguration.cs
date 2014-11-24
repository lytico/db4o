/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Config;

namespace Db4objects.Db4o.Config
{
	/// <summary>interface to configure the querying settings to be used by the query processor.
	/// 	</summary>
	/// <remarks>
	/// interface to configure the querying settings to be used by the query processor.
	/// <br /><br />All settings can be configured after opening an ObjectContainer.
	/// In a Client/Server setup the client-side configuration will be used.
	/// </remarks>
	public interface IQueryConfiguration
	{
		/// <summary>configures the query processor evaluation mode.</summary>
		/// <remarks>
		/// configures the query processor evaluation mode.
		/// <br /><br />The db4o query processor can run in three modes:<br />
		/// - <b>Immediate</b> mode<br />
		/// - <b>Snapshot</b> mode<br />
		/// - <b>Lazy</b> mode<br /><br />
		/// In <b>Immediate</b> mode, a query will be fully evaluated when
		/// <see cref="Db4objects.Db4o.Query.IQuery.Execute()">Db4objects.Db4o.Query.IQuery.Execute()
		/// 	</see>
		/// 
		/// is called. The complete
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// of all matching IDs is
		/// generated immediately.<br /><br />
		/// In <b>Snapshot</b> mode, the
		/// <see cref="Db4objects.Db4o.Query.IQuery.Execute()">Db4objects.Db4o.Query.IQuery.Execute()
		/// 	</see>
		/// call will trigger all index
		/// processing immediately. A snapshot of the current state of all relevant indexes
		/// is taken for further processing by the SODA query processor. All non-indexed
		/// constraints and all evaluations will be run when the user application iterates
		/// through the resulting
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// .<br /><br />
		/// In <b>Lazy</b> mode, the
		/// <see cref="Db4objects.Db4o.Query.IQuery.Execute()">Db4objects.Db4o.Query.IQuery.Execute()
		/// 	</see>
		/// call will only create an Iterator
		/// against the best index found. Further query processing (including all index
		/// processing) will happen when the user application iterates through the resulting
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// .<br /><br />
		/// Advantages and disadvantages of the individual modes:<br /><br />
		/// <b>Immediate</b> mode<br />
		/// <b>+</b> If the query is intended to iterate through the entire resulting
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// ,
		/// this mode will be slightly faster than the others.<br />
		/// <b>+</b> The query will process without intermediate side effects from changed
		/// objects (by the caller or by other transactions).<br />
		/// <b>-</b> Query processing can block the server for a long time.<br />
		/// <b>-</b> In comparison to the other modes it will take longest until the first results
		/// are returned.<br />
		/// <b>-</b> The ObjectSet will require a considerate amount of memory to hold the IDs of
		/// all found objects.<br /><br />
		/// <b>Snapshot</b> mode<br />
		/// <b>+</b> Index processing will happen without possible side effects from changes made
		/// by the caller or by other transaction.<br />
		/// <b>+</b> Since index processing is fast, a server will not be blocked for a long time.<br />
		/// <b>-</b> The entire candidate index will be loaded into memory. It will stay there
		/// until the query ObjectSet is garbage collected. In a C/S setup, the memory will
		/// be used on the server side.<br /><br />
		/// <b>Lazy</b> mode<br />
		/// <b>+</b> The call to
		/// <see cref="Db4objects.Db4o.Query.IQuery.Execute()">Db4objects.Db4o.Query.IQuery.Execute()
		/// 	</see>
		/// will return very fast. First results can be
		/// made available to the application before the query is fully processed.<br />
		/// <b>+</b> A query will consume hardly any memory at all because no intermediate ID
		/// representation is ever created.<br />
		/// <b>-</b> Lazy queries check candidates when iterating through the resulting
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// .
		/// In doing so the query processor takes changes into account that may have happened
		/// since the Query#execute()call: committed changes from other transactions, <b>and
		/// uncommitted changes from the calling transaction</b>. There is a wide range
		/// of possible side effects. The underlying index may have changed. Objects themselves
		/// may have changed in the meanwhile. There even is the chance of creating an endless
		/// loop, if the caller of the iterates through the
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// and changes each
		/// object in a way that it is placed at the end of the index: The same objects can be
		/// revisited over and over. <b>In lazy mode it can make sense to work in a way one would
		/// work with collections to avoid concurrent modification exceptions.</b> For instance one
		/// could iterate through the
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// first and store all objects to a temporary
		/// other collection representation before changing objects and storing them back to db4o.<br /><br />
		/// Note: Some method calls against a lazy
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// will require the query
		/// processor to create a snapshot or to evaluate the query fully. An example of such
		/// a call is
		/// <see cref="Db4objects.Db4o.IObjectSet.Count()">Db4objects.Db4o.IObjectSet.Count()
		/// 	</see>
		/// .
		/// <br /><br />
		/// The default query evaluation mode is <b>Immediate</b> mode.
		/// <br /><br />
		/// Recommendations:<br />
		/// - <b>Lazy</b> mode can be an excellent choice for single transaction read use,
		/// to keep memory consumption as low as possible.<br />
		/// - Client/Server applications with the risk of concurrent modifications should prefer
		/// <b>Snapshot</b> mode to avoid side effects from other transactions.
		/// <br /><br />
		/// To change the evaluationMode, pass any of the three static
		/// <see cref="QueryEvaluationMode">QueryEvaluationMode</see>
		/// constants from the
		/// <see cref="QueryEvaluationMode">QueryEvaluationMode</see>
		/// class to this method:<br />
		/// -
		/// <see cref="QueryEvaluationMode.Immediate">QueryEvaluationMode.Immediate</see>
		/// <br />
		/// -
		/// <see cref="QueryEvaluationMode.Snapshot">QueryEvaluationMode.Snapshot</see>
		/// <br />
		/// -
		/// <see cref="QueryEvaluationMode.Lazy">QueryEvaluationMode.Lazy</see>
		/// <br /><br />
		/// This setting must be issued from the client side.
		/// </remarks>
		void EvaluationMode(QueryEvaluationMode mode);

		/// <seealso cref="EvaluationMode(QueryEvaluationMode)">EvaluationMode(QueryEvaluationMode)
		/// 	</seealso>
		/// <returns>the currently configured query evaluation mode</returns>
		QueryEvaluationMode EvaluationMode();
	}
}
