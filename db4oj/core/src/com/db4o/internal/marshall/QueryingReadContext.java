/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.activation.*;
import com.db4o.internal.query.processor.*;
import com.db4o.marshall.*;
import com.db4o.typehandlers.*;

/**
 * @exclude
 */
public class QueryingReadContext extends AbstractReadContext implements HandlerVersionContext, AspectVersionContext, ObjectIdContext {
    
    private final QCandidates _candidates;
    
    private final int _collectionID;
    
    private final int _handlerVersion;
    
    private IdObjectCollector _collector;
    
	private int _declaredAspectCount;

	private int _id;
    
    private QueryingReadContext(Transaction transaction, QCandidates candidates, int handlerVersion, ReadBuffer buffer, int collectionID, IdObjectCollector collector) {
        super(transaction, buffer);
        _candidates = candidates;
        _activationDepth = new LegacyActivationDepth(0);
        _collectionID = collectionID;
        _handlerVersion = handlerVersion;
        _collector = collector;
    }
    
    public QueryingReadContext(Transaction transaction, QCandidates candidates, int handlerVersion, ReadBuffer buffer, int collectionID) {
        this(transaction, candidates, handlerVersion, buffer, collectionID, new IdObjectCollector());
    }
    
    public QueryingReadContext(Transaction transaction, int handlerVersion, ReadBuffer buffer, int id) {
        this(transaction, null, handlerVersion, buffer, 0);
		_id = id;
    }
    
    public QueryingReadContext(Transaction transaction, int handlerVersion, ReadBuffer buffer,
    		int collectionID, IdObjectCollector collector) {
        this(transaction, null, handlerVersion, buffer, collectionID, collector);
    }

    public int collectionID() {
        return _collectionID;
    }
    
    public QCandidates candidates(){
        return _candidates;
    }
    
    public int handlerVersion() {
        return _handlerVersion;
    }
    
    public void addId(int id) {
        _collector.addId(id);
    }
    
    public TreeInt ids() {
        return _collector.ids();
    }
    
    public void add(Object obj) {
        int id = getID(obj);
        if(id > 0){
            addId(id);
            return;
        }
        addObjectWithoutId(obj);
    }

	private int getID(Object obj) {
		return container().getID(transaction(), obj);
	}
    
    public void readId(TypeHandler4 handler) {
        ObjectID objectID = ObjectID.NOT_POSSIBLE;
        try {
            int offset = offset();
            if(handler instanceof ReadsObjectIds){
                objectID = ((ReadsObjectIds)handler).readObjectID(this);
            }
            if(objectID.isValid()){
                addId(objectID._id);
                return;
            }
            if(objectID == ObjectID.NOT_POSSIBLE){
                seek(offset);
                // FIXME: there's no point in activating the object
                // just find its id
                // type handlers know how to do it
                Object obj = read(handler);
                if(obj != null){
                	int id = (int) getID(obj);
                	if (id > 0) {
                		addId(id);
                	} else {
                		addObjectWithoutId(obj);
                	}
                }
            }
            
        } catch (Exception e) {
            // FIXME: Catchall
        }
    }

    private void addObjectWithoutId(Object obj) {
        _collector.add(obj);
    }

    public void skipId(TypeHandler4 handler) {
        if(handler instanceof ReadsObjectIds){
            ((ReadsObjectIds)handler).readObjectID(this);
            return;
        }
        // TODO: Optimize for just doing a seek here.
        read(handler);
    }
    
    public Iterator4 objectsWithoutId(){
        return _collector.objects();
    }
    
	public int declaredAspectCount() {
		return _declaredAspectCount;
	}

	public void declaredAspectCount(int count) {
		_declaredAspectCount = count;
	}
	
	public IdObjectCollector collector(){
		return _collector;
	}

	public int objectId() {
		return _id;
	}

}
