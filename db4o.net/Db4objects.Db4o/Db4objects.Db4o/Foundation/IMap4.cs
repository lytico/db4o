/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public interface IMap4
	{
		int Size();

		object Get(object key);

		void Put(object key, object value);

		bool ContainsKey(object key);

		object Remove(object key);

		IEnumerable Values();

		void Clear();
	}
}
