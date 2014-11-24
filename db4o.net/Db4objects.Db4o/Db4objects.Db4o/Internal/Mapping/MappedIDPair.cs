/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Internal.Mapping
{
	/// <exclude></exclude>
	public class MappedIDPair
	{
		private int _orig;

		private int _mapped;

		public MappedIDPair(int orig, int mapped)
		{
			_orig = orig;
			_mapped = mapped;
		}

		public virtual int Orig()
		{
			return _orig;
		}

		public virtual int Mapped()
		{
			return _mapped;
		}

		public override string ToString()
		{
			return _orig + "->" + _mapped;
		}
	}
}
