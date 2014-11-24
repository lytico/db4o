/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;

namespace Db4objects.Db4o.Foundation
{
	public interface ISet4 : IEnumerable
	{
		bool Add(object obj);

		void Clear();

		bool Contains(object obj);

		bool IsEmpty();

		IEnumerator GetEnumerator();

		bool Remove(object obj);

		int Size();
	}
}
