/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class Collection4Iterator : Iterator4Impl
	{
		private readonly Collection4 _collection;

		private readonly int _initialVersion;

		public Collection4Iterator(Collection4 collection, List4 first) : base(first)
		{
			_collection = collection;
			_initialVersion = CurrentVersion();
		}

		public override bool MoveNext()
		{
			Validate();
			return base.MoveNext();
		}

		public override object Current
		{
			get
			{
				Validate();
				return base.Current;
			}
		}

		private void Validate()
		{
			if (_initialVersion != CurrentVersion())
			{
				// FIXME: change to ConcurrentModificationException
				throw new InvalidIteratorException();
			}
		}

		private int CurrentVersion()
		{
			return _collection.Version();
		}
	}
}
