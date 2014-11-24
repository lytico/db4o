/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Foundation;

namespace Db4objects.Drs.Inside
{
	public interface IReplicationReference
	{
		IDrsUUID Uuid();

		/// <summary>
		/// Must return the latests version of the object AND OF
		/// ALL COLLECTIONS IT REFERENCES IN ITS FIELDS because
		/// collections are treated as 2nd class objects
		/// (just like arrays) for Hibernate replication
		/// compatibility purposes.
		/// </summary>
		/// <remarks>
		/// Must return the latests version of the object AND OF
		/// ALL COLLECTIONS IT REFERENCES IN ITS FIELDS because
		/// collections are treated as 2nd class objects
		/// (just like arrays) for Hibernate replication
		/// compatibility purposes.
		/// </remarks>
		long Version();

		object Object();

		object Counterpart();

		void SetCounterpart(object obj);

		void MarkForReplicating(bool flag);

		bool IsMarkedForReplicating();

		void MarkForDeleting();

		bool IsMarkedForDeleting();

		void MarkCounterpartAsNew();

		bool IsCounterpartNew();
	}
}
