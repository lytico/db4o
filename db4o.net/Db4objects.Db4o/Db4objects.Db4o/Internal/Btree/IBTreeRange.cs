/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	public interface IBTreeRange
	{
		/// <summary>
		/// Iterates through all the valid pointers in
		/// this range.
		/// </summary>
		/// <remarks>
		/// Iterates through all the valid pointers in
		/// this range.
		/// </remarks>
		/// <returns>an Iterator4 over BTreePointer value</returns>
		IEnumerator Pointers();

		IEnumerator Keys();

		int Size();

		IBTreeRange Greater();

		IBTreeRange Union(IBTreeRange other);

		IBTreeRange ExtendToLast();

		IBTreeRange Smaller();

		IBTreeRange ExtendToFirst();

		IBTreeRange Intersect(IBTreeRange range);

		IBTreeRange ExtendToLastOf(IBTreeRange upperRange);

		bool IsEmpty();

		void Accept(IBTreeRangeVisitor visitor);

		BTreePointer LastPointer();
	}
}
