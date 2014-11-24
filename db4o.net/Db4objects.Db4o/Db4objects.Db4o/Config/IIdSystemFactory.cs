/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Ids;

namespace Db4objects.Db4o.Config
{
	/// <summary>Factory interface to create a custom IdSystem.</summary>
	/// <remarks>Factory interface to create a custom IdSystem.</remarks>
	/// <seealso cref="IIdSystemConfiguration.UseCustomSystem(IIdSystemFactory)"></seealso>
	public interface IIdSystemFactory
	{
		/// <summary>creates</summary>
		/// <param name="container"></param>
		/// <returns></returns>
		IIdSystem NewInstance(LocalObjectContainer container);
	}
}
