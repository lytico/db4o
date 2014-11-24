/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Reflect
{
	/// <summary>Reflection Method representation.</summary>
	/// <remarks>
	/// Reflection Method representation
	/// <br/><br/>See documentation for System.Reflection API.
	/// </remarks>
	/// <seealso cref="IReflector">IReflector</seealso>
	public interface IReflectMethod
	{
		/// <exception cref="Db4objects.Db4o.Internal.ReflectException"></exception>
		object Invoke(object onObject, object[] parameters);

		IReflectClass GetReturnType();
	}
}
