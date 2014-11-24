/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Config;

namespace Db4objects.Db4o.CS.Config
{
	/// <summary>
	/// A configuration provider that provides access to the
	/// networking configuration methods.
	/// </summary>
	/// <remarks>
	/// A configuration provider that provides access to the
	/// networking configuration methods.
	/// </remarks>
	/// <since>7.5</since>
	public interface INetworkingConfigurationProvider
	{
		/// <summary>Access to the networking configuration methods.</summary>
		/// <remarks>Access to the networking configuration methods.</remarks>
		INetworkingConfiguration Networking
		{
			get;
		}
	}
}
