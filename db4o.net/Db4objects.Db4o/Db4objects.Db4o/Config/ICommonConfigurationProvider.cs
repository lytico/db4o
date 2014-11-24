/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Config;

namespace Db4objects.Db4o.Config
{
	/// <summary>
	/// A configuration provider that provides access to
	/// the common configuration methods that can be called
	/// for embedded, server and client use of db4o.
	/// </summary>
	/// <remarks>
	/// A configuration provider that provides access to
	/// the common configuration methods that can be called
	/// for embedded, server and client use of db4o.
	/// </remarks>
	/// <since>7.5</since>
	public interface ICommonConfigurationProvider
	{
		/// <summary>Access to the common configuration methods.</summary>
		/// <remarks>Access to the common configuration methods.</remarks>
		ICommonConfiguration Common
		{
			get;
		}
	}
}
