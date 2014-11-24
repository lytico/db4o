/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Drs
{
	/// <summary>The state of the entity in a provider.</summary>
	/// <remarks>The state of the entity in a provider.</remarks>
	/// <author>Albert Kwan</author>
	/// <author>Klaus Wuestefeld</author>
	/// <version>1.2</version>
	/// <since>dRS 1.2</since>
	public interface IObjectState
	{
		/// <summary>The entity.</summary>
		/// <remarks>The entity.</remarks>
		/// <returns>null if the object has been deleted or if it was not replicated in previous replications.
		/// 	</returns>
		object GetObject();

		/// <summary>Is the object newly created since last replication?</summary>
		/// <returns>true when the object is newly created since last replication</returns>
		bool IsNew();

		/// <summary>Was the object modified since last replication?</summary>
		/// <returns>true when the object was modified since last replication</returns>
		bool WasModified();

		/// <summary>The time when the object is modified in a provider.</summary>
		/// <remarks>The time when the object is modified in a provider.</remarks>
		/// <returns>time when the object is modified in a provider.</returns>
		long ModificationDate();

		/// <summary>whether or not the object is known to the ReplicationProvider.</summary>
		/// <remarks>whether or not the object is known to the ReplicationProvider.</remarks>
		bool IsKnown();
	}
}
