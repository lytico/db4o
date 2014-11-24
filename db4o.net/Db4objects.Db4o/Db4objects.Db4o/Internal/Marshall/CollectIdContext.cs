/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class CollectIdContext : ObjectHeaderContext
	{
		private readonly IdObjectCollector _collector;

		public CollectIdContext(Transaction transaction, IdObjectCollector collector, ObjectHeader
			 oh, IReadBuffer buffer) : base(transaction, buffer, oh)
		{
			_collector = collector;
		}

		public CollectIdContext(Transaction transaction, ObjectHeader oh, IReadBuffer buffer
			) : this(transaction, new IdObjectCollector(), oh, buffer)
		{
		}

		public static Db4objects.Db4o.Internal.Marshall.CollectIdContext ForID(Transaction
			 transaction, int id)
		{
			return ForID(transaction, new IdObjectCollector(), id);
		}

		public static Db4objects.Db4o.Internal.Marshall.CollectIdContext ForID(Transaction
			 transaction, IdObjectCollector collector, int id)
		{
			StatefulBuffer reader = transaction.Container().ReadStatefulBufferById(transaction
				, id);
			if (reader == null)
			{
				return null;
			}
			ObjectHeader oh = new ObjectHeader(transaction.Container(), reader);
			return new Db4objects.Db4o.Internal.Marshall.CollectIdContext(transaction, collector
				, oh, reader);
		}

		public virtual void AddId()
		{
			int id = ReadInt();
			if (id <= 0)
			{
				return;
			}
			AddId(id);
		}

		private void AddId(int id)
		{
			_collector.AddId(id);
		}

		public override Db4objects.Db4o.Internal.ClassMetadata ClassMetadata()
		{
			return _objectHeader.ClassMetadata();
		}

		public virtual TreeInt Ids()
		{
			return _collector.Ids();
		}

		public virtual void ReadID(IReadsObjectIds objectIDHandler)
		{
			ObjectID objectID = objectIDHandler.ReadObjectID(this);
			if (objectID.IsValid())
			{
				AddId(objectID._id);
			}
		}

		public virtual IdObjectCollector Collector()
		{
			return _collector;
		}
	}
}
