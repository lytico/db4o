/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Config;

namespace Db4objects.Db4o.Config
{
	/// <summary>interface to configure the freespace system to be used.</summary>
	/// <remarks>
	/// interface to configure the freespace system to be used.
	/// <br /><br />All methods should be called before opening database files.
	/// If db4o is instructed to exchange the system
	/// (
	/// <see cref="UseBTreeSystem()">UseBTreeSystem()</see>
	/// ,
	/// <see cref="UseRamSystem()">UseRamSystem()</see>
	/// )
	/// this will happen on opening the database file.<br /><br />
	/// By default the ram based system will be used.
	/// </remarks>
	public interface IFreespaceConfiguration
	{
		/// <summary>
		/// tuning feature: configures the minimum size of free space slots in the database file
		/// that are to be reused.
		/// </summary>
		/// <remarks>
		/// tuning feature: configures the minimum size of free space slots in the database file
		/// that are to be reused.
		/// <br /><br />When objects are updated or deleted, the space previously occupied in the
		/// database file is marked as "free", so it can be reused. db4o maintains two lists
		/// in RAM, sorted by address and by size. Adjacent entries are merged. After a large
		/// number of updates or deletes have been executed, the lists can become large, causing
		/// RAM consumption and performance loss for maintenance. With this method you can
		/// specify an upper bound for the byte slot size to discard.
		/// <br /><br />Pass Integer.MAX_VALUE to this method to discard all free slots for
		/// the best possible startup time.<br /><br />
		/// The downside of setting this value: Database files will necessarily grow faster.
		/// <br /><br />Default value:<br />
		/// 0 all space is reused
		/// </remarks>
		/// <param name="byteCount">Slots with this size or smaller will be lost.</param>
		void DiscardSmallerThan(int byteCount);

		/// <summary>
		/// Configure a way to overwrite freed space in the database file with custom
		/// (for example: random) bytes.
		/// </summary>
		/// <remarks>
		/// Configure a way to overwrite freed space in the database file with custom
		/// (for example: random) bytes. Will slow down I/O operation.
		/// The value of this setting may be cached internally and can thus not be
		/// reliably set after an object container has been opened.
		/// </remarks>
		/// <param name="freespaceFiller">The freespace overwriting callback to use</param>
		void FreespaceFiller(IFreespaceFiller freespaceFiller);

		/// <summary>configures db4o to use a BTree-based freespace system.</summary>
		/// <remarks>
		/// configures db4o to use a BTree-based freespace system.
		/// <br /><br /><b>Advantages</b><br />
		/// - ACID, no freespace is lost on abnormal system termination<br />
		/// - low memory consumption<br />
		/// <br /><b>Disadvantages</b><br />
		/// - slower than the RAM-based system, since freespace information
		/// is written during every commit<br />
		/// </remarks>
		void UseBTreeSystem();

		/// <summary>discontinued freespace system, only available before db4o 7.0.</summary>
		/// <remarks>discontinued freespace system, only available before db4o 7.0.</remarks>
		[System.ObsoleteAttribute(@"Please use the BTree freespace system instead by calling UseBTreeSystem() ."
			)]
		void UseIndexSystem();

		/// <summary>configures db4o to use a RAM-based freespace system.</summary>
		/// <remarks>
		/// configures db4o to use a RAM-based freespace system.
		/// <br /><br /><b>Advantages</b><br />
		/// - best performance<br />
		/// <br /><b>Disadvantages</b><br />
		/// - upon abnormal system termination all freespace is lost<br />
		/// - memory consumption<br />
		/// </remarks>
		void UseRamSystem();
	}
}
