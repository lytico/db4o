/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Config
{
	/// <summary>Configures the environment (set of services) used by db4o.</summary>
	/// <remarks>Configures the environment (set of services) used by db4o.</remarks>
	/// <seealso cref="Db4objects.Db4o.Foundation.IEnvironment">Db4objects.Db4o.Foundation.IEnvironment
	/// 	</seealso>
	/// <seealso cref="Db4objects.Db4o.Foundation.Environments.My(System.Type{T})">Db4objects.Db4o.Foundation.Environments.My(System.Type&lt;T&gt;)
	/// 	</seealso>
	public interface IEnvironmentConfiguration
	{
		/// <summary>Contributes a service to the db4o environment.</summary>
		/// <remarks>Contributes a service to the db4o environment.</remarks>
		/// <param name="service"></param>
		void Add(object service);
	}
}
