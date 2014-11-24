/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Bench.Logging.Replay.Commands;
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.Bench.Logging.Replay.Commands
{
	public class SyncCommand : IIoCommand
	{
		public virtual void Replay(IBin bin)
		{
			bin.Sync();
		}
	}
}
