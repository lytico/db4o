/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Drs;

namespace Db4objects.Drs
{
	/// <summary>Execute a round of replication between two ReplicationProviders.</summary>
	/// <remarks>
	/// Execute a round of replication between two ReplicationProviders.
	/// <p/>
	/// Sample code of using ReplicationSession:
	/// <pre>
	/// ReplicationSession session = Replication.begin(objectContainer1, objectContainer2);
	/// session.replicate(object);
	/// session.commit();
	/// session.close();
	/// </pre>
	/// </remarks>
	/// <version>1.2</version>
	/// <seealso cref="Replication">Replication</seealso>
	/// <since>dRS 1.0</since>
	public interface IReplicationSession
	{
		/// <summary>Closes this session and frees used resources.</summary>
		/// <remarks>
		/// Closes this session and frees used resources. Sessions that were opened
		/// during the creation of ReplicationProviders will be closed as well.
		/// <p/>
		/// Hibernate Sessions created from Hibernate Configurations will be closed.
		/// db4o ObjectContainers will remain open.
		/// </remarks>
		/// <exception cref="System.InvalidOperationException">if the session is still active (neither commitReplicationTransaction() nor rollback is called).
		/// 	</exception>
		void Close();

		/// <summary>
		/// Commits replication changes to both ReplicationProviders and marks the
		/// involved ReplicationProviders to be "clean" against each other - to contain
		/// no modified objects.
		/// </summary>
		/// <remarks>
		/// Commits replication changes to both ReplicationProviders and marks the
		/// involved ReplicationProviders to be "clean" against each other - to contain
		/// no modified objects.
		/// </remarks>
		void Commit();

		/// <summary>
		/// Returns the ReplicationProvider representing the <b>first</b> persistence
		/// system passed as a parameter when the replication session was instantiated.
		/// </summary>
		/// <remarks>
		/// Returns the ReplicationProvider representing the <b>first</b> persistence
		/// system passed as a parameter when the replication session was instantiated.
		/// </remarks>
		/// <returns>the first persistence system</returns>
		IReplicationProvider ProviderA();

		/// <summary>
		/// Returns the ReplicationProvider representing the <b>second</b> persistence
		/// system passed as a parameter when the replication session was instantiated.
		/// </summary>
		/// <remarks>
		/// Returns the ReplicationProvider representing the <b>second</b> persistence
		/// system passed as a parameter when the replication session was instantiated.
		/// </remarks>
		/// <returns>the second persistence system</returns>
		IReplicationProvider ProviderB();

		/// <summary>
		/// Replicates an individual object and traverses to member objects as long as
		/// members are new or modified since the last time the two ReplicationProviders
		/// were replicated.
		/// </summary>
		/// <remarks>
		/// Replicates an individual object and traverses to member objects as long as
		/// members are new or modified since the last time the two ReplicationProviders
		/// were replicated.
		/// </remarks>
		/// <param name="obj">the object to be replicated.</param>
		/// <seealso cref="IReplicationEventListener">IReplicationEventListener</seealso>
		void Replicate(object obj);

		/// <summary>Replicates all deletions between the two providers.</summary>
		/// <remarks>
		/// Replicates all deletions between the two providers.
		/// Cascade delete is disabled, regardless the user's settings.
		/// <p/>
		/// If the deletion violates referential integrity, exception will be thrown.
		/// You can use the dRS replication callback to check whether the object to
		/// be deleted violates referential integrity. If so, you can stop traversal.
		/// </remarks>
		/// <param name="extent">the Class that you want to delete</param>
		void ReplicateDeletions(Type extent);

		/// <summary>Rollbacks all changes done during the replication session.</summary>
		/// <remarks>
		/// Rollbacks all changes done during the replication session. This call
		/// guarantees the changes will not be applied to the underlying databases. The
		/// state of the objects involved in the replication is undefined.
		/// Both ReplicationProviders may still contain cached references of touched objects.
		/// <p/>
		/// To restart replication, it is recommended to reopen both involved
		/// ReplicationProviders and to start a new ReplicationSession.
		/// </remarks>
		void Rollback();

		/// <summary>Sets the direction of replication.</summary>
		/// <remarks>
		/// Sets the direction of replication. By default, if this method is not called, replication is bidirectional,
		/// which means the newer copy of the object is copied to the other provider..
		/// <p/> If you want to force unidirectional replication, call this method before calling
		/// <see cref="Replicate(object)">Replicate(object)</see>
		/// .
		/// </remarks>
		/// <param name="from">objects in this provider will not be changed.</param>
		/// <param name="to">objects will be copied to this provider if copies in "from" is newer
		/// 	</param>
		void SetDirection(IReplicationProvider from, IReplicationProvider to);
	}
}
