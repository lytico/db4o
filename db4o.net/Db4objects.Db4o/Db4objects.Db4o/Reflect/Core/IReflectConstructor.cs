/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Reflect.Core
{
	/// <summary>Reflection Constructor representation.</summary>
	/// <remarks>
	/// Reflection Constructor representation
	/// <br/><br/>See documentation for System.Reflection API.
	/// </remarks>
	/// <seealso cref="IReflector">IReflector</seealso>
	public interface IReflectConstructor
	{
		IReflectClass[] GetParameterTypes();

		object NewInstance(object[] parameters);
	}
}
