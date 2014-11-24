/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public interface ISequence4 : IEnumerable
	{
		bool Add(object element);

		void AddAll(IEnumerable iterable);

		bool IsEmpty();

		object Get(int index);

		int Size();

		void Clear();

		bool Remove(object obj);

		bool Contains(object obj);

		bool ContainsAll(IEnumerable iter);

		object[] ToArray();

		object[] ToArray(object[] array);
	}
}
