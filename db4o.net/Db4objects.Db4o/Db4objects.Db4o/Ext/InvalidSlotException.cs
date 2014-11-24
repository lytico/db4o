/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when db4o reads slot
	/// information which is not valid (length or address).
	/// </summary>
	/// <remarks>
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when db4o reads slot
	/// information which is not valid (length or address).
	/// </remarks>
	[System.Serializable]
	public class InvalidSlotException : Db4oRecoverableException
	{
		/// <summary>Constructor allowing to specify a detailed message.</summary>
		/// <remarks>Constructor allowing to specify a detailed message.</remarks>
		/// <param name="msg">message</param>
		public InvalidSlotException(string msg) : base(msg)
		{
		}

		/// <summary>Constructor allowing to specify the address, length and id.</summary>
		/// <remarks>Constructor allowing to specify the address, length and id.</remarks>
		/// <param name="address">offending address</param>
		/// <param name="length">offending length</param>
		/// <param name="id">id where the address and length were read.</param>
		public InvalidSlotException(int address, int length, int id) : base("address: " +
			 address + ", length : " + length + ", id : " + id)
		{
		}
	}
}
