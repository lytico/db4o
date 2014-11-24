/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Bench.Logging.Replay.Commands
{
	public class ReadWriteCommand
	{
		protected readonly long _pos;

		protected readonly int _length;

		public ReadWriteCommand(long pos, int length)
		{
			_pos = pos;
			_length = length;
		}

		protected virtual byte[] PrepareBuffer()
		{
			return new byte[_length];
		}
	}
}
