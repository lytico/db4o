/* Copyright (C) 2011 Versant Inc.   http://www.db4o.com */
using System.IO;
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.Internal.CLI
{
	internal interface ICLIFacade
	{
		void Flush(FileStream stream);
		IStorage NewStorage();
	}
}
