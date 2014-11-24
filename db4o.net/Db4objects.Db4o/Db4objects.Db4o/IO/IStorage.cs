/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.IO
{
	/// <summary>
	/// Base interface for Storage adapters that open a
	/// <see cref="IBin">IBin</see>
	/// to store db4o database data to.
	/// </summary>
	/// <seealso cref="Db4objects.Db4o.Config.IFileConfiguration.Storage(IStorage)"></seealso>
	public interface IStorage
	{
		/// <summary>
		/// opens a
		/// <see cref="IBin">IBin</see>
		/// to store db4o database data.
		/// </summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		IBin Open(BinConfiguration config);

		/// <summary>returns true if a Bin (file or memory) exists with the passed name.</summary>
		/// <remarks>returns true if a Bin (file or memory) exists with the passed name.</remarks>
		bool Exists(string uri);

		/// <summary>Deletes the bin for the given URI from the storage.</summary>
		/// <remarks>Deletes the bin for the given URI from the storage.</remarks>
		/// <since>7.9</since>
		/// <param name="uri">bin URI</param>
		/// <exception cref="System.IO.IOException">if the bin could not be deleted</exception>
		void Delete(string uri);

		/// <summary>Renames the bin for the given old URI to the new URI.</summary>
		/// <remarks>
		/// Renames the bin for the given old URI to the new URI. If a bin for the new URI
		/// exists, it will be overwritten.
		/// </remarks>
		/// <since>7.9</since>
		/// <param name="oldUri">URI of the existing bin</param>
		/// <param name="newUri">future URI of the bin</param>
		/// <exception cref="System.IO.IOException">if the bin could not be deleted</exception>
		void Rename(string oldUri, string newUri);
	}
}
