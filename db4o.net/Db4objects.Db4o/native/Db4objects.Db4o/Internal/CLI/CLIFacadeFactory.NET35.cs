/* Copyright (C) 2011 Versant Inc.   http://www.db4o.com */
#if !NET_4_0 && !CF && !SILVERLIGHT
namespace Db4objects.Db4o.Internal.CLI
{
	internal class CLIFacadeFactory
	{
		internal static ICLIFacade NewInstance()
		{
			return Platform4.IsMono() ? (ICLIFacade) new Mono35() : new CLR35();
		}
	}
}
#endif