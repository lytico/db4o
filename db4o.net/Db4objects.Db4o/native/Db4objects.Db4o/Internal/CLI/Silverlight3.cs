/* Copyright (C) 2011 Versant Inc.   http://www.db4o.com */
#if SILVERLIGHT && NET_3_5 && !NET_4_0
using System.IO;

namespace Db4objects.Db4o.Internal.CLI
{
	internal class Silverlight3 : SilverlightCLIBase
	{
		public override void Flush(FileStream stream)
		{
			stream.Flush();
		}
	}
}
#endif