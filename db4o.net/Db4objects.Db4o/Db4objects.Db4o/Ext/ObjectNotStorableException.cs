/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// this Exception is thrown, if objects can not be stored and if
	/// db4o is configured to throw Exceptions on storage failures.
	/// </summary>
	/// <remarks>
	/// this Exception is thrown, if objects can not be stored and if
	/// db4o is configured to throw Exceptions on storage failures.
	/// </remarks>
	/// <seealso cref="Db4objects.Db4o.Config.IConfiguration.ExceptionsOnNotStorable(bool)
	/// 	">Db4objects.Db4o.Config.IConfiguration.ExceptionsOnNotStorable(bool)</seealso>
	[System.Serializable]
	public class ObjectNotStorableException : Db4oRecoverableException
	{
		public ObjectNotStorableException(IReflectClass clazz) : base(Db4objects.Db4o.Internal.Messages
			.Get(clazz.IsSimple() ? 59 : 45, clazz.GetName()))
		{
		}

		public ObjectNotStorableException(string message) : base(message)
		{
		}

		public ObjectNotStorableException(IReflectClass clazz, string message) : base(clazz
			.GetName() + ": " + message)
		{
		}
	}
}
