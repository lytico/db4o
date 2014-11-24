/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import com.db4o.internal.*;
import com.db4o.marshall.*;


/**
 * @exclude
 */
public class CollectIdContext extends ObjectHeaderContext {
    
    private final IdObjectCollector _collector;
    
    public CollectIdContext(Transaction transaction, IdObjectCollector collector, ObjectHeader oh, ReadBuffer buffer) {
    	super(transaction, buffer, oh);
    	_collector = collector;
    }

    public CollectIdContext(Transaction transaction, ObjectHeader oh, ReadBuffer buffer) {
        this(transaction, new IdObjectCollector(), oh, buffer);
    }
    
    public static CollectIdContext forID(Transaction transaction, int id){
    	return forID(transaction, new IdObjectCollector(), id);
    }
    
    public static CollectIdContext forID(Transaction transaction, IdObjectCollector collector, int id){
        StatefulBuffer reader = transaction.container().readStatefulBufferById(transaction, id);
        if (reader == null) {
        	return null;
        }
        ObjectHeader oh = new ObjectHeader(transaction.container(), reader);
        return new CollectIdContext(transaction, collector, oh, reader);
    }

    public void addId() {
        int id = readInt();
        if(id <= 0){
            return;
        }
        addId(id);
    }

    private void addId(int id) {
        _collector.addId(id);
    }
    
    public ClassMetadata classMetadata() {
        return _objectHeader.classMetadata();
    }

    public TreeInt ids(){
        return _collector.ids();
    }

    public void readID(ReadsObjectIds objectIDHandler) {
        ObjectID objectID = objectIDHandler.readObjectID(this);
        if(objectID.isValid()){
            addId(objectID._id);
        }
    }
    
    public IdObjectCollector collector(){
        return _collector;
    }

}
