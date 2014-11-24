/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// Ordering by non primitive fields works only for classes that implement the
	/// <see cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable</see>
	/// interface
	/// and
	/// <see cref="Db4objects.Db4o.TA.TransparentActivationSupport">Db4objects.Db4o.TA.TransparentActivationSupport
	/// 	</see>
	/// is enabled.
	/// </summary>
	[System.Serializable]
	public class UnsupportedOrderingException : Db4oRecoverableException
	{
		public UnsupportedOrderingException(string msg) : base(msg)
		{
		}
	}
}
