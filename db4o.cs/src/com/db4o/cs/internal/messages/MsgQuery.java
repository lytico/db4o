/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.config.*;
import com.db4o.cs.internal.*;
import com.db4o.cs.internal.objectexchange.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.query.result.*;

public abstract class MsgQuery extends MsgObject {
	
	private static int nextID;
	
	protected final MsgD writeQueryResult(AbstractQueryResult queryResult, QueryEvaluationMode evaluationMode, ObjectExchangeConfiguration config) {
		
		if(evaluationMode == QueryEvaluationMode.IMMEDIATE){
			return writeImmediateQueryResult(queryResult, config);
		} 
		return writeLazyQueryResult(queryResult, config);
	}

	private MsgD writeLazyQueryResult(AbstractQueryResult queryResult, ObjectExchangeConfiguration config) {
	    int queryResultId = generateID();
	    int maxCount = config().prefetchObjectCount();
	    IntIterator4 idIterator = queryResult.iterateIDs();
	    MsgD message = buildQueryResultMessage(queryResultId, idIterator, maxCount, config);
	    ServerMessageDispatcher serverThread = serverMessageDispatcher();
	    serverThread.mapQueryResultToID(new LazyClientObjectSetStub(queryResult, idIterator), queryResultId);
	    return message;
    }

	private MsgD writeImmediateQueryResult(AbstractQueryResult queryResult, ObjectExchangeConfiguration config) {
	    IntIterator4 idIterator = queryResult.iterateIDs();
	    MsgD message = buildQueryResultMessage(0, idIterator, queryResult.size(), config);
	    return message;
    }

	private MsgD buildQueryResultMessage(int queryResultId, IntIterator4 ids, int maxCount, ObjectExchangeConfiguration config) {
		final ByteArrayBuffer payload = ObjectExchangeStrategyFactory.forConfig(config).marshall((LocalTransaction) transaction(), ids, maxCount);
	    MsgD message = QUERY_RESULT.getWriterForLength(transaction(), Const4.INT_LENGTH + payload.length());
		StatefulBuffer writer = message.payLoad();
		writer.writeInt(queryResultId);
		writer.writeBytes(payload._buffer);
	    return message;
    }
	
	private static synchronized int generateID(){
		nextID ++;
		if(nextID < 0){
			nextID = 1;
		}
		return nextID;
	}
	
	protected AbstractQueryResult newQueryResult(QueryEvaluationMode mode){
		return container().newQueryResult(transaction(), mode);
	}

}
