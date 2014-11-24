/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class IntIdGenerator
	{
		private int _current = 1;

		public virtual int Next()
		{
			_current++;
			if (_current < 0)
			{
				_current = 1;
			}
			return _current;
		}
	}
}
