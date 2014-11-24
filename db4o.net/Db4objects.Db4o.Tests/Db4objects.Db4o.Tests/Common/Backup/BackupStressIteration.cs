/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Tests.Common.Backup
{
	public class BackupStressIteration
	{
		public int _count;

		public BackupStressIteration()
		{
		}

		public virtual void SetCount(int count)
		{
			_count = count;
		}

		public virtual int GetCount()
		{
			return _count;
		}
	}
}
