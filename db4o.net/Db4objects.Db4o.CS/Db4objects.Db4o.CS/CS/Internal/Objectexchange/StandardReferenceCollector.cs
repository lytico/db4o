/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.CS.Internal.Objectexchange;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.CS.Internal.Objectexchange
{
	public class StandardReferenceCollector : IReferenceCollector
	{
		private Transaction _transaction;

		public StandardReferenceCollector(Transaction transaction)
		{
			_transaction = transaction;
		}

		public virtual IEnumerator ReferencesFrom(int id)
		{
			CollectIdContext context = CollectIdContext.ForID(_transaction, id);
			ClassMetadata classMetadata = context.ClassMetadata();
			if (null == classMetadata)
			{
				// most probably ClassMetadata reading
				return Iterators.EmptyIterator;
			}
			if (!classMetadata.HasIdentity())
			{
				throw new InvalidOperationException(classMetadata.ToString());
			}
			if (!Handlers4.IsCascading(classMetadata.TypeHandler()))
			{
				return Iterators.EmptyIterator;
			}
			classMetadata.CollectIDs(context);
			return new TreeKeyIterator(context.Ids());
		}
	}
}
