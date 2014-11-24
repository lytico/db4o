/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.Internal.Query.Result
{
	/// <exclude></exclude>
	public class IdTreeQueryResult : AbstractQueryResult
	{
		private Tree _ids;

		public IdTreeQueryResult(Transaction transaction, IIntIterator4 ids) : base(transaction
			)
		{
			_ids = TreeInt.AddAll(null, ids);
		}

		public override IIntIterator4 IterateIDs()
		{
			return new IntIterator4Adaptor(new TreeKeyIterator(_ids));
		}

		public override int Size()
		{
			if (_ids == null)
			{
				return 0;
			}
			return _ids.Size();
		}

		public override AbstractQueryResult SupportSort()
		{
			return ToIdList();
		}

		public override AbstractQueryResult SupportElementAccess()
		{
			return ToIdList();
		}
	}
}
