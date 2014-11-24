/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.CS.Internal.Objectexchange;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Result;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public abstract class MsgQuery : MsgObject
	{
		private static int nextID;

		protected MsgD WriteQueryResult(AbstractQueryResult queryResult, QueryEvaluationMode
			 evaluationMode, ObjectExchangeConfiguration config)
		{
			if (evaluationMode == QueryEvaluationMode.Immediate)
			{
				return WriteImmediateQueryResult(queryResult, config);
			}
			return WriteLazyQueryResult(queryResult, config);
		}

		private MsgD WriteLazyQueryResult(AbstractQueryResult queryResult, ObjectExchangeConfiguration
			 config)
		{
			int queryResultId = GenerateID();
			int maxCount = Config().PrefetchObjectCount();
			IIntIterator4 idIterator = queryResult.IterateIDs();
			MsgD message = BuildQueryResultMessage(queryResultId, idIterator, maxCount, config
				);
			IServerMessageDispatcher serverThread = ServerMessageDispatcher();
			serverThread.MapQueryResultToID(new LazyClientObjectSetStub(queryResult, idIterator
				), queryResultId);
			return message;
		}

		private MsgD WriteImmediateQueryResult(AbstractQueryResult queryResult, ObjectExchangeConfiguration
			 config)
		{
			IIntIterator4 idIterator = queryResult.IterateIDs();
			MsgD message = BuildQueryResultMessage(0, idIterator, queryResult.Size(), config);
			return message;
		}

		private MsgD BuildQueryResultMessage(int queryResultId, IIntIterator4 ids, int maxCount
			, ObjectExchangeConfiguration config)
		{
			ByteArrayBuffer payload = ObjectExchangeStrategyFactory.ForConfig(config).Marshall
				((LocalTransaction)Transaction(), ids, maxCount);
			MsgD message = QueryResult.GetWriterForLength(Transaction(), Const4.IntLength + payload
				.Length());
			StatefulBuffer writer = message.PayLoad();
			writer.WriteInt(queryResultId);
			writer.WriteBytes(payload._buffer);
			return message;
		}

		private static int GenerateID()
		{
			lock (typeof(MsgQuery))
			{
				nextID++;
				if (nextID < 0)
				{
					nextID = 1;
				}
				return nextID;
			}
		}

		protected virtual AbstractQueryResult NewQueryResult(QueryEvaluationMode mode)
		{
			return Container().NewQueryResult(Transaction(), mode);
		}
	}
}
