/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.CS.Internal.Objectexchange;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MGetInternalIDs : MsgD, IMessageWithResponse
	{
		public Msg ReplyFromServer()
		{
			ByteArrayBuffer bytes = GetByteLoad();
			int classMetadataID = bytes.ReadInt();
			int prefetchDepth = bytes.ReadInt();
			int prefetchCount = bytes.ReadInt();
			bool triggerQueryEvents = bytes.ReadInt() == 1;
			ByteArrayBuffer payload = MarshallIDsFor(classMetadataID, prefetchDepth, prefetchCount
				, triggerQueryEvents);
			MsgD message = Msg.IdList.GetWriterForLength(Transaction(), payload.Length());
			message.PayLoad().WriteBytes(payload._buffer);
			return message;
		}

		private ByteArrayBuffer MarshallIDsFor(int classMetadataID, int prefetchDepth, int
			 prefetchCount, bool triggerQueryEvents)
		{
			lock (ContainerLock())
			{
				long[] ids = GetIDs(classMetadataID, triggerQueryEvents);
				return ObjectExchangeStrategyFactory.ForConfig(new ObjectExchangeConfiguration(prefetchDepth
					, prefetchCount)).Marshall((LocalTransaction)Transaction(), IntIterators.ForLongs
					(ids), ids.Length);
			}
		}

		private long[] GetIDs(int classMetadataID, bool triggerQueryEvents)
		{
			lock (ContainerLock())
			{
				ClassMetadata classMetadata = Container().ClassMetadataForID(classMetadataID);
				if (!triggerQueryEvents)
				{
					return classMetadata.GetIDs(Transaction());
				}
				return ((long[])NewQuery(classMetadata).TriggeringQueryEvents(new _IClosure4_45(this
					, classMetadata)));
			}
		}

		private sealed class _IClosure4_45 : IClosure4
		{
			public _IClosure4_45(MGetInternalIDs _enclosing, ClassMetadata classMetadata)
			{
				this._enclosing = _enclosing;
				this.classMetadata = classMetadata;
			}

			public object Run()
			{
				return classMetadata.GetIDs(this._enclosing.Transaction());
			}

			private readonly MGetInternalIDs _enclosing;

			private readonly ClassMetadata classMetadata;
		}

		private QQuery NewQuery(ClassMetadata classMetadata)
		{
			QQuery query = (QQuery)LocalContainer().Query();
			query.Constrain(classMetadata);
			return query;
		}
	}
}
