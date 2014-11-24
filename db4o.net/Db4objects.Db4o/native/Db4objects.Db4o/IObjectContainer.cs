/* Copyright (C) 2004 - 1010  Versant Inc.   http://www.db4o.com */

using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Qlin;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o
{
	/// <summary>The interface to a db4o database, stand-alone or client/server.</summary>
	/// <remarks>
	/// The interface to a db4o database, stand-alone or client/server.
	/// <br /><br />The IObjectContainer interface provides methods
	/// to store, query and delete objects and to commit and rollback
	/// transactions.<br /><br />
	/// An IObjectContainer can either represent a stand-alone database
	/// or a connection to a
	/// <see cref="Db4objects.Db4o.Db4o.OpenServer">db4o server</see>
	/// .
    /// <br/><br/>An object container also represents a transaction. All work
    /// with db4o always is transactional. Both <see cref="Commit"/> and
    /// <see cref="Rollback"/> start a new transaction immediately. For working 
    /// against the same database with multiple transactions, open a new object container
    /// with <see cref="Ext"/>.<see cref="IExtObjectContainer.OpenSession">OpenSession()</see></remarks>
	/// <seealso cref="Db4objects.Db4o.Ext.IExtObjectContainer">IExtObjectContainer for extended functionality.
	/// </seealso>
	public interface IObjectContainer : System.IDisposable, ISodaQueryFactory
	{
        /// <summary>Activates all members on a stored object to the specified depth.</summary>
		/// <remarks>
        /// Activates all members on a stored object to the specified depth.
		/// <br /><br />
        /// See <see cref="ICommonConfiguration.ActivationDepth"> "Why activation"</see>
        /// for an explanation why activation is necessary.<br/><br/>
        /// Calling this method activates a graph of persistent objects in memory.
        /// Only deactivated objects in the graph will be touched: Their
        /// fields will be loaded from the database. 
        /// When called it starts from the given
        /// object, traverses all member objects and activates them up to the given depth.
        /// The depth parameter is the distance in "field hops"
        /// (object.field.field) away from the root object. The nodes at 'depth' level
        /// away from the root (for a depth of 3: object.member.member) will be instantiated
        /// but not populated with data. Its fields will be null.
    	/// The activation depth of individual classes can be overruled
    	/// with the methods
        /// <see cref="IObjectClass.MaximumActivationDepth"/> and
        /// <see cref="IObjectClass.MinimumActivationDepth(int)"/> in the
        /// <see cref="IObjectClass"/>-interface.<br/><br/>
        /// </remarks>
        /// <seealso cref="ICommonConfiguration.ActivationDepth">Why activation?</seealso>
		/// <seealso cref="Db4objects.Db4o.Ext.IObjectCallbacks">Using callbacks</seealso>
		/// <param name="obj">the object to be activated.</param>
		/// <param name="depth">
		/// the member
		/// <see cref="Db4objects.Db4o.Config.IConfiguration.ActivationDepth">depth</see>
		/// to which activate is to cascade.
		/// </param>
		void Activate(object obj, int depth);

		/// <summary>Closes this object container.</summary>
		/// <remarks>
        /// Closes this object container.
		/// <br /><br />Calling Close() automatically performs a
        /// <see cref="Db4objects.Db4o.IObjectContainer.Commit">Commit()</see>.
        /// </remarks>
		/// <returns>
        /// success - true denotes that the object container was closed, false if it was already closed
		/// </returns>
		bool Close();

        /// <summary>Commits the running transaction.</summary>
		/// <remarks>
        /// Commits the running transaction.
		/// <br /><br />Transactions are back-to-back. A call to commit will start
        /// a new transaction immediately.
		/// </remarks>
		void Commit();

        /// <summary>Deactivates a stored object by setting all members to null.
		/// 	</summary>
        /// <remarks>
        /// Deactivates a stored object by setting all members to null.
        /// <br/><br/>Primitive types will be set to their default values.
        /// The method has no effect, if the passed object is not stored in the
        /// object container.
        /// <br/><br/>
        /// Be aware that calling may have side effects, which assume that a object is filled with data.
        /// <br/><br/>
        /// In general you should not deactivate objects, since it makes you application more
        /// complex and confusing.
        /// To control the scope of objects you should use session containers
        /// for your unit of work. Use <see cref="Ext"/>.<see cref="IExtObjectContainer.OpenSession"/>
        /// to create a new session.
		/// </remarks>
		/// <seealso cref="Db4objects.Db4o.Ext.IObjectCallbacks">Using callbacks</seealso>
		/// <seealso cref="Db4objects.Db4o.Config.IConfiguration.ActivationDepth">Why activation?</seealso>
		/// <param name="obj">the object to be deactivated.</param>
		/// <param name="depth">
		/// the member
		/// <see cref="Db4objects.Db4o.Config.IConfiguration.ActivationDepth">depth</see> 
        /// the object-graph depth up to which object are deactivated
		/// </param>
		void Deactivate(object obj, int depth);

        /// <summary>Deletes a stored object permanently from the database.</summary>
		/// <remarks>
        /// Deletes a stored object permanently from the database.
        /// Note that this method has to be called <b>for every single object
        /// individually</b>. Delete does not recurs to object members. Primitives, strings
        /// and array member types are deleted.
        /// <br/><br/>Referenced objects of the passed object remain untouched, unless
        /// cascaded deletes are  
        /// <see cref="IObjectClass.CascadeOnDelete">configured for the class</see> 
        /// or <see cref="IObjectField.CascadeOnDelete"> for member fields</see>.
        /// <br/><br/>The method has no effect, if
        /// the passed object is not stored in the object container.
        /// <br/><br/>A subsequent call to
        /// <see cref="Store"/> with the same object stores the object again in the database.<br/><br/>
		/// </remarks>
		/// <seealso cref="Db4objects.Db4o.Config.IObjectClass.CascadeOnDelete">Db4objects.Db4o.Config.IObjectClass.CascadeOnDelete
		/// 	</seealso>
		/// <seealso cref="Db4objects.Db4o.Config.IObjectField.CascadeOnDelete">Db4objects.Db4o.Config.IObjectField.CascadeOnDelete
		/// 	</seealso>
		/// <seealso cref="Db4objects.Db4o.Ext.IObjectCallbacks">Using callbacks</seealso>
		/// <param name="obj">
		/// the object to be deleted from the object container.<br />
		/// </param>
		void Delete(object obj);

        /// <summary>Returns an ObjectContainer with extended functionality.</summary>
		/// <remarks>
        /// Returns an ObjectContainer with extended functionality.
        /// <br/><br/>Every ObjectContainer that db4o provides can be casted to
        /// an ExtObjectContainer. This method is supplied for your convenience
        /// to work without a cast.
        /// <br/><br/>The object container functionality is split to two interfaces
        /// to allow newcomers to focus on the essential methods.<br/><br/>
		/// </remarks>
		/// <returns>this, casted to IExtObjectContainer</returns>
		Db4objects.Db4o.Ext.IExtObjectContainer Ext();

        /// <summary>Query-By-Example interface to retrieve objects.</summary>
        /// <remarks>
        /// Query-By-Example interface to retrieve objects.
        /// <br/><br/>
        /// QueryByExample() creates an object set containing
        /// all objects in the database that match the passed
        /// template object.<br/><br/>
        /// Calling QueryByExample(null) returns all objects stored in the database.
        /// <br/><br/>
        /// <b>Query Evaluation:</b>
        /// <ul><li>All non-null members of the template object are compared against
        /// all stored objects of the same class.</li>
        /// <li>Primitive type members are ignored if they are 0 or false respectively.</li>
        /// <li>Arrays and  collections  are
        /// evaluated for containment. Differences in length/size() are
        /// ignored.</li>
        /// </ul>
        /// </remarks>
        /// <param name="template">object to be used as an example to find all matching objects.
        /// 	</param>
        /// <returns>
        /// 
        /// <see cref="Db4objects.Db4o.IObjectSet">IObjectSet</see>
        /// containing all found objects.<br /><br />
        /// </returns>
        /// <seealso cref="Db4objects.Db4o.Config.IConfiguration.ActivationDepth">Why activation?</seealso>
        /// <seealso cref="Db4objects.Db4o.Ext.IObjectCallbacks">Using callbacks</seealso>
        Db4objects.Db4o.IObjectSet QueryByExample(object template);


		/// <summary>
		/// Creates a new SODA
		/// <see cref="Db4objects.Db4o.Query.IQuery">Query</see>
		/// , the low level db4o query api.
		/// <br /><br />
		/// <b>Prefer LINQ over SODA, unless you need a specific SODA featuere</b>
		/// <br /><br />
		/// </summary>
		/// <returns>a new IQuery object</returns>
		Db4objects.Db4o.Query.IQuery Query();

        /// <summary>Queries for all instances of a class.</summary>
        /// <remarks>Queries for all instances of a class.</remarks>
		/// <param name="clazz">the class to query for.</param>
        /// <returns>all instances of the given class
		/// </returns>
		Db4objects.Db4o.IObjectSet Query(System.Type clazz);

		/// <summary>Native Query Interface.</summary>
		/// <remarks>
        /// Native Query Interface.<br/><br/>
        /// <b>Prefer LINQ over Native Queries.</b>
        /// <br/><br/>
        /// Make sure that you reference Db4objects.Db4o.NativeQueries.dll, Mono.Cecil.dll and Cecil.FlowAnalysis.dll in your application
        /// when using native queries.<br/><br/>
        /// db4o will attempt to optimize native query expressions and execute them
        /// against indexes and without instantiating actual objects.
        /// Otherwise db4o falls back and instantiates objects to run them against the given predicate.
        /// That is an order of magnitude slower than a optimized native query.<br/><br/>
        /// 
        /// <code>
        /// IList&lt;Cat&gt; cats = db.query(delegate(Cat) { return cat.getName().equals("Occam")}); 
        /// </code>
        /// 
        /// Summing up the above:<br/>
        /// In order to execute a Native Query, you provide a predicate delegate<br/><br/>
		/// </remarks>
		/// <param name="predicate">
		/// the predicate containing the native query expression.
		/// </param>
		/// <returns>
		/// the query result
		/// </returns>
		Db4objects.Db4o.IObjectSet Query(Db4objects.Db4o.Query.Predicate predicate);

		/// <summary>Native Query Interface.</summary>
        /// <remarks>
        /// <b>Prefer LINQ over Native Queries.</b>
        /// <br/><br/>
        /// Native Query Interface. Queries as with <see cref="Query(Predicate)"/>,
        /// but will sort the resulting result according to the given comperator.
        /// 
		/// </remarks>
		/// <param name="predicate">
		/// the
		/// <see cref="Db4objects.Db4o.Query.Predicate">predicate</see>
		/// containing the native query expression.
		/// </param>
		/// <param name="comparator">
		/// the
		/// <see cref="Db4objects.Db4o.Query.IQueryComparator">comperator</see>
		/// specifiying the sort order of the result
		/// </param>
        /// <returns>
        /// the query result
		/// </returns>
		Db4objects.Db4o.IObjectSet Query(Db4objects.Db4o.Query.Predicate predicate, Db4objects.Db4o.Query.IQueryComparator
			 comparator);

        /// <summary>Native Query Interface.</summary>
        /// <remarks>
        /// <b>Prefer LINQ over Native Queries.</b>
        /// <br/><br/>
        /// Native Query Interface. Queries as with <see cref="Query(Predicate)"/>,
        /// but will sort the resulting result according to the given comperator.
        /// 
        /// </remarks>
        /// <param name="predicate">
        /// the
        /// <see cref="Db4objects.Db4o.Query.Predicate">predicate</see>
        /// containing the native query expression.
        /// </param>
        /// <param name="comparator">
        /// the
        /// <see cref="Db4objects.Db4o.Query.IQueryComparator">comperator</see>
        /// specifiying the sort order of the result
        /// </param>
        /// <returns>
        /// the query result
        /// </returns>
		Db4objects.Db4o.IObjectSet Query(Db4objects.Db4o.Query.Predicate predicate, System.Collections.IComparer comparer);

        /// <summary>Rolls back the running transaction.</summary>
		/// <remarks>
        /// Rolls back the running transaction.<br/><br/>
        /// <b>This only rolls back the changes in the database, but not the state of in memory objects</b>.
        /// <br/><br/>
        /// 
        /// Dealing with stale state of in memory objects after a rollback:<br/>
        /// <ul><li>Since in memory objects are not rolled back you probably want start with a clean state.
        /// The easiest way to do this is by creating a new object container:
        /// <see cref="Ext"/>.<see cref="IExtObjectContainer.OpenSession"/>.
        /// </li><li>Alternatively you can deactivate objects or <see cref="Ext"/>.<see cref="IExtObjectContainer.Refresh"/> them to get back to the state in the database.
        /// </li><li>In case you are using transparent persistence you can use a <see cref="IRollbackStrategy"/> to rollback
        /// the in memory objects as well. </li></ul>
		/// </remarks>
		void Rollback();

        /// <summary>Stores objects or updates stored objects..</summary>
        /// <remarks>
        /// Stores objects or updates stored objects.
        /// <br/><br/>An object not yet stored in the database will be
        /// stored. An object already stored in database will be updated.
        /// <br/><br/>
        /// <b>Updates:</b>
        /// <ul>
        /// <li>Will update all primitive types, strings and arrays of a object</li>
        /// <li>References to other object that are already stored will be updated.</li>
        /// <li>New object members will be stored.</li>
        /// <li>Referenced object members that are already stored are <b>not</b> updated
        /// themselves. Every object member needs to be updated individually with a
        /// call to store(). Unless a deeper update depth has been configured with on of these options:
        /// <see cref="ICommonConfiguration.UpdateDepth"/>- or
        /// <see cref="IObjectClass.UpdateDepth">class-specific update depth</see>,
        /// <see cref="IObjectClass.CascadeOnUpdate"> cascde on update for type</see> or
        /// <see cref="IObjectField.CascadeOnUpdate">field</see>.</li>
        /// </ul>
        /// </remarks>
        /// <param name="obj">the object to be stored or updated.</param>
        /// <seealso cref="Db4objects.Db4o.Ext.IExtObjectContainer.Store">IExtObjectContainer#Store(object, depth)
        /// 	</seealso>
        /// <seealso cref="Db4objects.Db4o.Config.IConfiguration.UpdateDepth">Db4objects.Db4o.Config.ICommonConfiguration.UpdateDepth
        /// 	</seealso>
        /// <seealso cref="Db4objects.Db4o.Config.IObjectClass.UpdateDepth">Db4objects.Db4o.Config.IObjectClass.UpdateDepth
        /// 	</seealso>
        /// <seealso cref="Db4objects.Db4o.Config.IObjectClass.CascadeOnUpdate">Db4objects.Db4o.Config.IObjectClass.CascadeOnUpdate
        /// 	</seealso>
        /// <seealso cref="Db4objects.Db4o.Config.IObjectField.CascadeOnUpdate">Db4objects.Db4o.Config.IObjectField.CascadeOnUpdate
        /// 	</seealso>
        /// <seealso cref="Db4objects.Db4o.Ext.IObjectCallbacks">Using callbacks</seealso>
        void Store(object obj);

        /// <summary>Native Query Interface.</summary>
        /// <remarks>
        /// Native Query Interface.<br/><br/>
        /// <b>Prefer LINQ over Native Queries.</b>
        /// <br/><br/>
        /// Make sure that you reference Db4objects.Db4o.NativeQueries.dll, Mono.Cecil.dll and Cecil.FlowAnalysis.dll in your application
        /// when using native queries.<br/><br/>
        /// db4o will attempt to optimize native query expressions and execute them
        /// against indexes and without instantiating actual objects.
        /// Otherwise db4o falls back and instantiates objects to run them against the given predicate.
        /// That is an order of magnitude slower than a optimized native query.<br/><br/>
        /// 
        /// <code>
        /// IList&lt;Cat&gt; cats = db.query(delegate(Cat) { return cat.getName().equals("Occam")}); 
        /// </code>
        /// 
        /// Summing up the above:<br/>
        /// In order to execute a Native Query, you provide a predicate delegate<br/><br/>
        /// </remarks>
        /// <param name="predicate">
        /// the predicate containing the native query expression.
        /// </param>
        /// <returns>
        /// the query result
        /// </returns>
        System.Collections.Generic.IList<Extent> Query<Extent>(System.Predicate<Extent> match);

        /// <summary>Native Query Interface.</summary>
        /// <remarks>
        /// <b>Prefer LINQ over Native Queries.</b>
        /// <br/><br/>
        /// Native Query Interface. Queries as with <see cref="Query(Predicate)"/>,
        /// but will sort the resulting result according to the given comperator.
        /// 
        /// </remarks>
        /// <param name="predicate">
        /// the
        /// <see cref="Db4objects.Db4o.Query.Predicate">predicate</see>
        /// containing the native query expression.
        /// </param>
        /// <param name="comparator">
        /// the
        /// <see cref="Db4objects.Db4o.Query.IQueryComparator">comperator</see>
        /// specifiying the sort order of the result
        /// </param>
        /// <returns>
        /// the query result
        /// </returns>
		System.Collections.Generic.IList<Extent> Query<Extent>(System.Predicate<Extent> match, System.Collections.Generic.IComparer<Extent> comparer);

        /// <summary>Native Query Interface.</summary>
        /// <remarks>
        /// <b>Prefer LINQ over Native Queries.</b>
        /// <br/><br/>
        /// Native Query Interface. Queries as with <see cref="Query(Predicate)"/>,
        /// but will sort the resulting result according to the given comperator.
        /// 
        /// </remarks>
        /// <param name="predicate">
        /// the
        /// <see cref="Db4objects.Db4o.Query.Predicate">predicate</see>
        /// containing the native query expression.
        /// </param>
        /// <param name="comparator">
        /// the
        /// <see cref="Db4objects.Db4o.Query.IQueryComparator">comperator</see>
        /// specifiying the sort order of the result
        /// </param>
        /// <returns>
        /// the query result
        /// </returns>
		System.Collections.Generic.IList<Extent> Query<Extent>(System.Predicate<Extent> match, System.Comparison<Extent> comparison);

		/// <summary>
		/// Queries for all instances of the given type
		/// </summary>
		System.Collections.Generic.IList<ElementType> Query<ElementType>(System.Type extent);

        /// <summary>
        /// Queries for all instances of the given generic argument
		/// </summary>
		System.Collections.Generic.IList<Extent> Query<Extent>();

		/// <summary>
		/// Queries for all instances of the given type sorting with the specified comparer.
		/// </summary>
		System.Collections.Generic.IList<Extent> Query<Extent>(System.Collections.Generic.IComparer<Extent> comparer);
	}
}
