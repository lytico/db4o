/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.Bench.Logging.Replay.Commands
{
	public interface IIoCommand
	{
		void Replay(IBin bin);
	}
}
