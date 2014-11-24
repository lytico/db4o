/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Drs.Foundation
{
	public class TimeStamps
	{
		private long _from;

		private long _commit;

		public TimeStamps(long from, long commit)
		{
			this._from = from;
			this._commit = commit;
		}

		public virtual long To()
		{
			return _commit - 1;
		}

		public virtual long From()
		{
			return _from;
		}

		public virtual long Commit()
		{
			return _commit;
		}
	}
}
