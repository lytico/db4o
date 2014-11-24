/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Marshall
{
	/// <summary>a buffer interface with write methods.</summary>
	/// <remarks>a buffer interface with write methods.</remarks>
	public interface IWriteBuffer
	{
		/// <summary>writes a single byte to the buffer.</summary>
		/// <remarks>writes a single byte to the buffer.</remarks>
		/// <param name="b">the byte</param>
		void WriteByte(byte b);

		/// <summary>writes an array of bytes to the buffer</summary>
		/// <param name="bytes">the byte array</param>
		void WriteBytes(byte[] bytes);

		/// <summary>writes an int to the buffer.</summary>
		/// <remarks>writes an int to the buffer.</remarks>
		/// <param name="i">the int</param>
		void WriteInt(int i);

		/// <summary>writes a long to the buffer</summary>
		/// <param name="l">the long</param>
		void WriteLong(long l);
	}
}
