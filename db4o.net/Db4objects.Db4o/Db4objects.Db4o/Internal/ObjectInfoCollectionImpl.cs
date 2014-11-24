/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public sealed class ObjectInfoCollectionImpl : IObjectInfoCollection
	{
		public static readonly IObjectInfoCollection Empty = new Db4objects.Db4o.Internal.ObjectInfoCollectionImpl
			(Iterators.EmptyIterable);

		public IEnumerable _collection;

		public ObjectInfoCollectionImpl(IEnumerable collection)
		{
			_collection = collection;
		}

		public IEnumerator GetEnumerator()
		{
			return _collection.GetEnumerator();
		}
	}
}
