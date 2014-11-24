/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Marshall
{
	/// <summary>a reserved buffer within a write buffer.</summary>
	/// <remarks>
	/// a reserved buffer within a write buffer.
	/// The usecase this class was written for: A null bitmap should be at the
	/// beginning of a slot to allow lazy processing. During writing the content
	/// of the null bitmap is not yet fully known until all members are processed.
	/// With the Reservedbuffer the space in the slot can be occupied and writing
	/// can happen after all members are processed.
	/// </remarks>
	public interface IReservedBuffer
	{
		/// <summary>writes a byte array to the reserved buffer.</summary>
		/// <remarks>writes a byte array to the reserved buffer.</remarks>
		/// <param name="bytes">the byte array.</param>
		void WriteBytes(byte[] bytes);
	}
}
