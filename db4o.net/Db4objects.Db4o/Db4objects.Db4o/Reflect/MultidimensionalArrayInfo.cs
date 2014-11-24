/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Reflect
{
	/// <exclude></exclude>
	public class MultidimensionalArrayInfo : ArrayInfo
	{
		private int[] _dimensions;

		public virtual void Dimensions(int[] dim)
		{
			_dimensions = dim;
		}

		public virtual int[] Dimensions()
		{
			return _dimensions;
		}
	}
}
