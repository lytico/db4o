/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.CS.Config;

namespace Db4objects.Db4o.CS.Config
{
	/// <summary>
	/// Implement this interface for configuration items that encapsulate
	/// a batch of configuration settings or that need to be applied
	/// to ObjectServers after they are opened.
	/// </summary>
	/// <remarks>
	/// Implement this interface for configuration items that encapsulate
	/// a batch of configuration settings or that need to be applied
	/// to ObjectServers after they are opened.
	/// </remarks>
	/// <since>7.12</since>
	public interface IServerConfigurationItem
	{
		/// <summary>Gives a chance for the item to augment the configuration.</summary>
		/// <remarks>Gives a chance for the item to augment the configuration.</remarks>
		/// <param name="configuration">the configuration that the item was added to</param>
		void Prepare(IServerConfiguration configuration);

		/// <summary>Gives a chance for the item to configure the just opened ObjectContainer.
		/// 	</summary>
		/// <remarks>Gives a chance for the item to configure the just opened ObjectContainer.
		/// 	</remarks>
		/// <param name="server">the ObjectContainer to configure</param>
		void Apply(IObjectServer server);
	}
}
