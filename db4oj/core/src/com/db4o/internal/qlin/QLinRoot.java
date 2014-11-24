/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.qlin;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.internal.query.*;
import com.db4o.internal.query.processor.*;
import com.db4o.internal.query.result.*;
import com.db4o.qlin.*;
import com.db4o.query.*;

import static com.db4o.qlin.QLinSupport.*;

/**
 * @exclude
 */
public class QLinRoot<T> extends QLinSodaNode<T>{
	
	private final QQuery _query;
	
	private int _limit = -1;

	public QLinRoot(Query query, Class<T> clazz) {
		_query = (QQuery) query;
		query.constrain(clazz);
		context(clazz);
	}
	
	public Query query(){
		return _query;
	}

	public ObjectSet<T> select() {
		if(_limit == -1){
			return _query.execute();
		}
		QueryResult queryResult = _query.getQueryResult();
		IdListQueryResult limitedResult = new IdListQueryResult(_query.transaction(), _limit);
		int counter = 0;
		IntIterator4 i = queryResult.iterateIDs();
		while(i.moveNext()){
			if(counter++ >= _limit){
				break;
			}
			limitedResult.add(i.currentInt());
		}
		return new ObjectSetFacade(limitedResult);
	}
	
	public QLin<T> limit(int size){
		if(size < 1){
			throw new QLinException("Limit must be greater that 0");
		}
		_limit = size;
		return this;
	}


	@Override
	protected QLinRoot<T> root() {
		return this;
	}

	Query descend(Object expression) {
		// TODO: Implement deep descend
		return query().descend(field(expression).getName());
	}

}
