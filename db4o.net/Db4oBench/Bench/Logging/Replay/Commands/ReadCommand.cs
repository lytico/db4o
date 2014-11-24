/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Bench.Logging.Replay.Commands;
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.Bench.Logging.Replay.Commands
{
	public class ReadCommand : ReadWriteCommand, IIoCommand
	{
		public ReadCommand(long pos, int length) : base(pos, length)
		{
		}

		public virtual void Replay(IBin bin)
		{
			bin.Read(_pos, PrepareBuffer(), _length);
		}
	}
}
