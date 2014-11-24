/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Tests.Common.Backup
{
	public class BackupStressItem
	{
		public string _name;

		public int _iteration;

		public BackupStressItem()
		{
		}

		public BackupStressItem(string name, int iteration)
		{
			_name = name;
			_iteration = iteration;
		}
	}
}
