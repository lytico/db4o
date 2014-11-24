/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.cs.internal.objectexchange.*;
import com.db4o.foundation.*;
import com.db4o.internal.query.processor.*;
import com.db4o.internal.query.result.*;

public final class MGetAll extends MsgQuery implements MessageWithResponse {
	
	public final Msg replyFromServer() {
		QueryEvaluationMode evaluationMode = QueryEvaluationMode.fromInt(readInt());
		int prefetchDepth = readInt();
		int prefetchCount = readInt();
		synchronized(containerLock()) {
			return writeQueryResult(getAll(evaluationMode), evaluationMode, new ObjectExchangeConfiguration(prefetchDepth, prefetchCount));
		}
	}

	private AbstractQueryResult getAll(final QueryEvaluationMode mode) {
		return newQuery(mode).triggeringQueryEvents(new Closure4<AbstractQueryResult>() { public AbstractQueryResult run() {
			try {
				return localContainer().getAll(transaction(), mode);
			} catch (Exception e) {
				if(Debug4.atHome){
					e.printStackTrace();
				}
			}
			return newQueryResult(mode);
		}});
	}

	private QQuery newQuery(final QueryEvaluationMode mode) {
		QQuery query = (QQuery)localContainer().query();
		query.evaluationMode(mode);
		return query;
	}
	
}