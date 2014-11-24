/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public interface ISortable4
	{
		int Size();

		int Compare(int leftIndex, int rightIndex);

		void Swap(int leftIndex, int rightIndex);
	}
}
