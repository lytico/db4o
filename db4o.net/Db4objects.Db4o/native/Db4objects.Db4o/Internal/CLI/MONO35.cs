/* Copyright (C) 2011 Versant Inc.   http://www.db4o.com */
#if !NET_4_0 && NET_3_5 && !CF && !SILVERLIGHT
using System.IO;

namespace Db4objects.Db4o.Internal.CLI
{
	internal class Mono35 : CLIBase
	{
		public override void Flush(FileStream stream)
		{
			stream.Flush();
		}
	}
}
#endif