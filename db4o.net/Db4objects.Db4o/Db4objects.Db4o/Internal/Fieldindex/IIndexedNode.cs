/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Fieldindex;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Fieldindex
{
	public interface IIndexedNode : IEnumerable, IIntVisitable
	{
		bool IsResolved();

		IIndexedNode Resolve();

		BTree GetIndex();

		int ResultSize();

		void MarkAsBestIndex(QCandidates candidates);

		bool IsEmpty();
	}
}
