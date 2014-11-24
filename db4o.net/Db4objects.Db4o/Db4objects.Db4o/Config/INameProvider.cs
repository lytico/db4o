/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;

namespace Db4objects.Db4o.Config
{
	/// <summary>A provider for custom database names.</summary>
	/// <remarks>A provider for custom database names.</remarks>
	public interface INameProvider
	{
		/// <summary>
		/// Derives a name for the given
		/// <see cref="Db4objects.Db4o.IObjectContainer">Db4objects.Db4o.IObjectContainer</see>
		/// . This method will be called when
		/// database startup has completed, i.e. the method will see a completely initialized
		/// <see cref="Db4objects.Db4o.IObjectContainer">Db4objects.Db4o.IObjectContainer</see>
		/// .
		/// Any code invoked during the startup process (for example
		/// <see cref="IConfigurationItem">IConfigurationItem</see>
		/// instances) will still
		/// see the default naming.
		/// </summary>
		string Name(IObjectContainer db);
	}
}
