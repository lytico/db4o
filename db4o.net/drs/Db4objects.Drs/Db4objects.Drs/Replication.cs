/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;
using Db4objects.Drs;
using Db4objects.Drs.Inside;

namespace Db4objects.Drs
{
	/// <summary>Factory to create ReplicationSessions.</summary>
	/// <remarks>Factory to create ReplicationSessions.</remarks>
	/// <version>1.3</version>
	/// <seealso cref="com.db4o.drs.hibernate.HibernateReplication">com.db4o.drs.hibernate.HibernateReplication
	/// 	</seealso>
	/// <seealso cref="IReplicationProvider">IReplicationProvider</seealso>
	/// <seealso cref="IReplicationEventListener">IReplicationEventListener</seealso>
	/// <since>dRS 1.0</since>
	public class Replication
	{
		/// <summary>
		/// Begins a replication session between two ReplicationProviders without a
		/// ReplicationEventListener and with no Reflector provided.
		/// </summary>
		/// <remarks>
		/// Begins a replication session between two ReplicationProviders without a
		/// ReplicationEventListener and with no Reflector provided.
		/// </remarks>
		/// <exception cref="ReplicationConflictException">when conflicts occur</exception>
		/// <seealso cref="IReplicationEventListener">IReplicationEventListener</seealso>
		public static IReplicationSession Begin(IReplicationProvider providerA, IReplicationProvider
			 providerB)
		{
			return Begin(providerA, providerB, null, null);
		}

		/// <summary>
		/// Begins a replication session between two ReplicationProviders using a
		/// ReplicationEventListener and with no Reflector provided.
		/// </summary>
		/// <remarks>
		/// Begins a replication session between two ReplicationProviders using a
		/// ReplicationEventListener and with no Reflector provided.
		/// </remarks>
		/// <exception cref="ReplicationConflictException">when conflicts occur</exception>
		/// <seealso cref="IReplicationEventListener">IReplicationEventListener</seealso>
		public static IReplicationSession Begin(IReplicationProvider providerA, IReplicationProvider
			 providerB, IReplicationEventListener listener)
		{
			return Begin(providerA, providerB, listener, null);
		}

		/// <summary>
		/// Begins a replication session between two ReplicationProviders without a
		/// ReplicationEventListener and with a Reflector provided.
		/// </summary>
		/// <remarks>
		/// Begins a replication session between two ReplicationProviders without a
		/// ReplicationEventListener and with a Reflector provided.
		/// </remarks>
		/// <exception cref="ReplicationConflictException">when conflicts occur</exception>
		/// <seealso cref="IReplicationEventListener">IReplicationEventListener</seealso>
		public static IReplicationSession Begin(IReplicationProvider providerFrom, IReplicationProvider
			 providerTo, IReflector reflector)
		{
			return Begin(providerFrom, providerTo, null, reflector);
		}

		/// <summary>
		/// Begins a replication session between two ReplicationProviders using a
		/// ReplicationEventListener and with a Reflector provided.
		/// </summary>
		/// <remarks>
		/// Begins a replication session between two ReplicationProviders using a
		/// ReplicationEventListener and with a Reflector provided.
		/// </remarks>
		/// <exception cref="ReplicationConflictException">when conflicts occur</exception>
		/// <seealso cref="IReplicationEventListener">IReplicationEventListener</seealso>
		public static IReplicationSession Begin(IReplicationProvider providerFrom, IReplicationProvider
			 providerTo, IReplicationEventListener listener, IReflector reflector)
		{
			if (listener == null)
			{
				listener = new DefaultReplicationEventListener();
			}
			ReplicationReflector rr = new ReplicationReflector(providerFrom, providerTo, reflector
				);
			providerFrom.ReplicationReflector(rr);
			providerTo.ReplicationReflector(rr);
			return new GenericReplicationSession(providerFrom, providerTo, listener, reflector
				);
		}
	}
}
