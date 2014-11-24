/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Internal.Logging
{
	public class Level
	{
		private readonly string _name;

		private readonly int _level;

		internal Level(string name, int level)
		{
			_name = name;
			_level = level;
		}

		public virtual int Ordinal()
		{
			return _level;
		}

		public override string ToString()
		{
			return _name;
		}
	}
}
