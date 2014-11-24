/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Ext
{
	/// <summary>provides information about system state and system settings.</summary>
	/// <remarks>provides information about system state and system settings.</remarks>
	public interface ISystemInfo
	{
		/// <summary>returns the number of entries in the Freespace Manager.</summary>
		/// <remarks>
		/// returns the number of entries in the Freespace Manager.
		/// <br /><br />A high value for the number of freespace entries
		/// is an indication that the database is fragmented and
		/// that defragment should be run.
		/// </remarks>
		/// <returns>the number of entries in the Freespace Manager.</returns>
		int FreespaceEntryCount();

		/// <summary>returns the freespace size in the database in bytes.</summary>
		/// <remarks>
		/// returns the freespace size in the database in bytes.
		/// <br /><br />When db4o stores modified objects, it allocates
		/// a new slot for it. During commit the old slot is freed.
		/// Free slots are collected in the freespace manager, so
		/// they can be reused for other objects.
		/// <br /><br />This method returns a sum of the size of all
		/// free slots in the database file.
		/// <br /><br />To reclaim freespace run defragment.
		/// </remarks>
		/// <returns>the freespace size in the database in bytes.</returns>
		long FreespaceSize();

		/// <summary>Returns the total size of the database on disk.</summary>
		/// <remarks>Returns the total size of the database on disk.</remarks>
		/// <returns>total size of database on disk</returns>
		long TotalSize();
	}
}
