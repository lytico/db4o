/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

using System;
using System.Reflection;
using Db4objects.Db4o.Config;

namespace Db4objects.Db4o.Internal
{
	public partial class Config4Impl
	{
		private static ILegacyClientServerFactory DefaultClientServerFactory()
		{
			Assembly csAssembly = Assembly.Load(ClientServerAssemblyName());
			return (ILegacyClientServerFactory) Activator.CreateInstance(csAssembly.GetType("Db4objects.Db4o.CS.Internal.Config.LegacyClientServerFactoryImpl"));
		}

		private static string ClientServerAssemblyName()
		{
			Assembly db4oAssembly = typeof(IObjectContainer).Assembly;
			string db4oAssemblySimpleName = db4oAssembly.GetName().Name;
			return db4oAssembly.FullName.Replace(db4oAssemblySimpleName, "Db4objects.Db4o.CS");
		}
	}
}
