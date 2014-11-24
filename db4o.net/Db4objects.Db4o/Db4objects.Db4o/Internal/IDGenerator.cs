/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Internal
{
	public class IDGenerator
	{
		private int id = 0;

		public virtual int Next()
		{
			id++;
			if (id > 0)
			{
				return id;
			}
			id = 1;
			return 1;
		}
	}
}
