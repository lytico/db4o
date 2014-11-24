/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Constraints
{
	/// <summary>Base class for all constraint exceptions.</summary>
	/// <remarks>Base class for all constraint exceptions.</remarks>
	[System.Serializable]
	public class ConstraintViolationException : Db4oRecoverableException
	{
		/// <summary>
		/// ConstraintViolationException constructor with a specific
		/// message.
		/// </summary>
		/// <remarks>
		/// ConstraintViolationException constructor with a specific
		/// message.
		/// </remarks>
		/// <param name="msg">exception message</param>
		public ConstraintViolationException(string msg) : base(msg)
		{
		}
	}
}
