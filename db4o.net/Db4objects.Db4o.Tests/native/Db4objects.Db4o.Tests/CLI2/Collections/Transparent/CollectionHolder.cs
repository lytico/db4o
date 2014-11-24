/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

using System;

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent
{
	[Serializable]
	public class CollectionHolder<TColl>
	{
		public CollectionHolder(TColl collection)
		{
			_collection = collection;	
		}

		public TColl Collection
		{
			get { return _collection; }
		}

#if SILVERLIGHT
		public TColl _collection;
#else
		private readonly TColl _collection;
#endif
	}
}
