/* Copyright (C) 2011 Versant Inc.   http://www.db4o.com */
#if SILVERLIGHT && NET_4_0
namespace Db4objects.Db4o.Internal.CLI
{
	internal class CLIFacadeFactory
	{
		internal static ICLIFacade NewInstance()
		{
			return new Silverlight4();
		}
	}
}
#endif