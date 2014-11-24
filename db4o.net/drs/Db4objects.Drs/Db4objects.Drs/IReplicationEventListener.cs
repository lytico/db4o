/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs;

namespace Db4objects.Drs
{
	/// <summary>Defines the contract for handling of replication events generated from a replication session.
	/// 	</summary>
	/// <remarks>
	/// Defines the contract for handling of replication events generated from a replication session.
	/// Users can implement this interface to resolve replication conflicts according to their own business rules.
	/// </remarks>
	/// <version>1.2</version>
	/// <since>dRS 1.2</since>
	public interface IReplicationEventListener
	{
		/// <summary>invoked when a replication of an object occurs.</summary>
		/// <remarks>invoked when a replication of an object occurs.</remarks>
		/// <param name="e">the event</param>
		void OnReplicate(IReplicationEvent e);
	}
}
