/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal;

import com.db4o.cs.internal.messages.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.query.result.*;


/**
 * @exclude
 */
public class LazyClientQueryResult extends AbstractQueryResult{
	
	private static final int SIZE_NOT_SET = -1;
	
	private final ClientObjectContainer _client;
	
	private final int _queryResultID;
	
	private int _size = SIZE_NOT_SET;
	
	private final LazyClientIdIterator _iterator;

	public LazyClientQueryResult(Transaction trans, ClientObjectContainer client, int queryResultID) {
		super(trans);
		_client = client;
		_queryResultID = queryResultID;
		_iterator = new LazyClientIdIterator(this);
	}

	public Object get(int index) {
        synchronized (lock()) {
            return activatedObject(getId(index));
        }
	}
	
	public int getId(int index) {
		return askServer(Msg.OBJECTSET_GET_ID, index);
	}

	public int indexOf(int id) {
		return askServer(Msg.OBJECTSET_INDEXOF, id);
	}
	
	private int askServer(MsgD message, int param){
		_client.write(message.getWriterForInts(_transaction, new int[]{_queryResultID, param}));
		return ((MsgD)_client.expectedResponse(message)).readInt();
	}

	public IntIterator4 iterateIDs() {
		return _iterator;
	}
	
	public Iterator4 iterator() {
		return skip(ClientServerPlatform.createClientQueryResultIterator(this));
	}

	public int size() {
		if(_size == SIZE_NOT_SET){
			_client.write(Msg.OBJECTSET_SIZE.getWriterForInt(_transaction, _queryResultID));
			_size = ((MsgD)_client.expectedResponse(Msg.OBJECTSET_SIZE)).readInt();
		}
		return _size;
	}

	protected void finalize() {
		_client.write(Msg.OBJECTSET_FINALIZED.getWriterForInt(_transaction, _queryResultID));
	}
	
	@Override
	public void loadFromIdReader(Iterator4 reader) {
		_iterator.loadFromIdReader(reader);
	}

	public void reset() {
		_client.write(Msg.OBJECTSET_RESET.getWriterForInt(_transaction, _queryResultID));
	}

	public void fetchIDs(int batchSize) {
		_client.write(Msg.OBJECTSET_FETCH.getWriterForInts(_transaction, _queryResultID, batchSize, _client.prefetchDepth()));
		ByteArrayBuffer reader = _client.expectedBufferResponse(Msg.ID_LIST);
		loadFromIdReader(_client.idIteratorFor(_transaction, reader));
	}
	

}
