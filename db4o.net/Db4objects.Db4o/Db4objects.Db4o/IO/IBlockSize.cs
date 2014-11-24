/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.IO
{
	/// <summary>Block size registry.</summary>
	/// <remarks>
	/// Block size registry.
	/// Accessible through the environment.
	/// </remarks>
	/// <seealso cref="Db4objects.Db4o.Foundation.Environments.My(System.Type{T})">Db4objects.Db4o.Foundation.Environments.My(System.Type&lt;T&gt;)
	/// 	</seealso>
	/// <since>7.7</since>
	public interface IBlockSize
	{
		void Register(IListener4 listener);

		void Set(int newValue);

		int Value();
	}
}
