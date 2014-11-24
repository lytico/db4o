/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Config.Encoding
{
	/// <summary>
	/// encodes a String to a byte array and decodes a String
	/// from a part of a byte array
	/// </summary>
	public interface IStringEncoding
	{
		/// <summary>called when a string is to be encoded to a byte array.</summary>
		/// <remarks>called when a string is to be encoded to a byte array.</remarks>
		/// <param name="str">the string to encode</param>
		/// <returns>the encoded byte array</returns>
		byte[] Encode(string str);

		/// <summary>called when a byte array is to be decoded to a string.</summary>
		/// <remarks>called when a byte array is to be decoded to a string.</remarks>
		/// <param name="bytes">the byte array</param>
		/// <param name="start">the start offset in the byte array</param>
		/// <param name="length">the length of the encoded string in the byte array</param>
		/// <returns>the string</returns>
		string Decode(byte[] bytes, int start, int length);
	}
}
