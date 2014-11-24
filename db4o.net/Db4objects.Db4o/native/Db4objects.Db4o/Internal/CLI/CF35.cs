/* Copyright (C) 2011 Versant Inc.   http://www.db4o.com */
#if CF
using System.IO;

namespace Db4objects.Db4o.Internal.CLI
{
	internal class CF35 : CLIBase
	{
		public override void Flush(FileStream stream)
		{
			stream.Flush();
		}
	}
}
#endif