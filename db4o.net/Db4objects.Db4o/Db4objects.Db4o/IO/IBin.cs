/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Sharpen.Lang;

namespace Db4objects.Db4o.IO
{
	/// <summary>
	/// Representation of a container for storage of db4o
	/// database data (to file, to memory).
	/// </summary>
	/// <remarks>
	/// Representation of a container for storage of db4o
	/// database data (to file, to memory).
	/// </remarks>
	public interface IBin
	{
		/// <summary>returns the length of the Bin (on disc, in memory).</summary>
		/// <remarks>returns the length of the Bin (on disc, in memory).</remarks>
		long Length();

		/// <summary>
		/// reads a given number of bytes into an array of bytes at an
		/// offset position.
		/// </summary>
		/// <remarks>
		/// reads a given number of bytes into an array of bytes at an
		/// offset position.
		/// </remarks>
		/// <param name="position">the offset position to read at</param>
		/// <param name="bytes">the byte array to read bytes into</param>
		/// <param name="bytesToRead">the number of bytes to be read</param>
		/// <returns></returns>
		int Read(long position, byte[] bytes, int bytesToRead);

		/// <summary>
		/// writes a given number of bytes from an array of bytes at
		/// an offset position
		/// </summary>
		/// <param name="position">the offset position to write at</param>
		/// <param name="bytes">the array of bytes to write</param>
		/// <param name="bytesToWrite">the number of bytes to write</param>
		void Write(long position, byte[] bytes, int bytesToWrite);

		/// <summary>
		/// flushes the buffer content to the physical storage
		/// media.
		/// </summary>
		/// <remarks>
		/// flushes the buffer content to the physical storage
		/// media.
		/// </remarks>
		void Sync();

		/// <summary>runs the Runnable between two calls to sync();</summary>
		void Sync(IRunnable runnable);

		/// <summary>
		/// reads a given number of bytes into an array of bytes at an
		/// offset position.
		/// </summary>
		/// <remarks>
		/// reads a given number of bytes into an array of bytes at an
		/// offset position. In contrast to the normal
		/// <see cref="Read(long, byte[], int)">Read(long, byte[], int)</see>
		/// method, the Bin should ensure direct access to the raw storage medium.
		/// No caching should take place.
		/// </remarks>
		/// <param name="position">the offset position to read at</param>
		/// <param name="bytes">the byte array to read bytes into</param>
		/// <param name="bytesToRead">the number of bytes to be read</param>
		/// <returns></returns>
		int SyncRead(long position, byte[] bytes, int bytesToRead);

		/// <summary>closes the Bin.</summary>
		/// <remarks>closes the Bin.</remarks>
		void Close();
	}
}
