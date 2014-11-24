/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// interface to the internal reference that an ObjectContainer
	/// holds for a stored object.
	/// </summary>
	/// <remarks>
	/// interface to the internal reference that an ObjectContainer
	/// holds for a stored object.
	/// </remarks>
	public interface IObjectInfo
	{
		/// <summary>returns the internal db4o ID.</summary>
		/// <remarks>returns the internal db4o ID.</remarks>
		long GetInternalID();

		/// <summary>returns the object that is referenced.</summary>
		/// <remarks>
		/// returns the object that is referenced.
		/// <br /><br />This method may return null, if the object has
		/// been garbage collected.
		/// </remarks>
		/// <returns>
		/// the referenced object or null, if the object has
		/// been garbage collected.
		/// </returns>
		object GetObject();

		/// <summary>returns a UUID representation of the referenced object.</summary>
		/// <remarks>
		/// returns a UUID representation of the referenced object.
		/// UUID generation has to be turned on, in order to be able
		/// to use this feature:
		/// <see cref="Db4objects.Db4o.Config.IFileConfiguration.GenerateUUIDs(Db4objects.Db4o.Config.ConfigScope)
		/// 	">Db4objects.Db4o.Config.IFileConfiguration.GenerateUUIDs(Db4objects.Db4o.Config.ConfigScope)
		/// 	</see>
		/// </remarks>
		/// <returns>the UUID of the referenced object.</returns>
		Db4oUUID GetUUID();

		/// <summary>
		/// returns the transaction serial number ("version") the referenced object
		/// was stored with last.
		/// </summary>
		/// <remarks>
		/// returns the transaction serial number ("version") the referenced object
		/// was stored with last. Version number generation has to be turned on, in
		/// order to be able to use this feature:
		/// <see cref="Db4objects.Db4o.Config.IFileConfiguration.GenerateVersionNumbers(Db4objects.Db4o.Config.ConfigScope)
		/// 	">Db4objects.Db4o.Config.IFileConfiguration.GenerateVersionNumbers(Db4objects.Db4o.Config.ConfigScope)
		/// 	</see>
		/// <br />
		/// This feature was replaced by
		/// <see cref="GetCommitTimestamp()">GetCommitTimestamp()</see>
		/// . The main
		/// difference is that the old version mechanism used to assign a serial
		/// timestamp to the object upon storing time, and the new commitTimestamp
		/// approach, assigns it upon commit time.<br />
		/// </remarks>
		/// <returns>the version number.</returns>
		[System.ObsoleteAttribute(@"As of version 8.0 please use GetCommitTimestamp() instead."
			)]
		long GetVersion();

		/// <summary>
		/// The serial timestamp the object is assigned to when it is committed.<br />
		/// <br />
		/// You need to enable this feature before using it in
		/// <see cref="Db4objects.Db4o.Config.IFileConfiguration.GenerateCommitTimestamps(bool)
		/// 	">Db4objects.Db4o.Config.IFileConfiguration.GenerateCommitTimestamps(bool)</see>
		/// .<br />
		/// <br />
		/// All the objects commited within the same transaction will receive the same commitTimestamp.<br />
		/// <br />
		/// db4o replication system (dRS) relies on this feature.<br />
		/// </summary>
		/// <returns>the serial timestamp that was given to the object upon commit.</returns>
		/// <seealso cref="Db4objects.Db4o.Config.IFileConfiguration.GenerateCommitTimestamps(bool)
		/// 	">Db4objects.Db4o.Config.IFileConfiguration.GenerateCommitTimestamps(bool)</seealso>
		/// <since>8.0</since>
		long GetCommitTimestamp();
	}
}
