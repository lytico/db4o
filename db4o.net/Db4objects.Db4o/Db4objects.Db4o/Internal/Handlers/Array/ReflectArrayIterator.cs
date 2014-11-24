/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Handlers.Array
{
	/// <exclude></exclude>
	internal sealed class ReflectArrayIterator : IndexedIterator
	{
		private readonly object _array;

		private readonly IReflectArray _reflectArray;

		public ReflectArrayIterator(IReflectArray reflectArray, object array) : base(reflectArray
			.GetLength(array))
		{
			_reflectArray = reflectArray;
			_array = array;
		}

		protected override object Get(int index)
		{
			return _reflectArray.Get(_array, index);
		}
	}
}
