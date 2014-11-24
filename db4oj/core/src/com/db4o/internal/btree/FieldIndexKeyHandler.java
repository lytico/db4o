/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.btree;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.marshall.*;


/**
 * @exclude
 */
public class FieldIndexKeyHandler implements Indexable4, CanExcludeNullInQueries{
	
    private final Indexable4 _valueHandler;
    
    private final IDHandler _parentIdHandler;
    
    public FieldIndexKeyHandler(Indexable4 delegate_) {
        _parentIdHandler = new IDHandler();
        _valueHandler = delegate_;
    }

    public int linkLength() {
        return _valueHandler.linkLength() + Const4.INT_LENGTH;
    }

    public Object readIndexEntry(Context context, ByteArrayBuffer buffer) {
        int parentID = buffer.readInt();
        Object objPart = _valueHandler.readIndexEntry(context, buffer);
        if (parentID < 0){
            objPart = null;
            parentID = - parentID;
        }
        return new FieldIndexKeyImpl(parentID, objPart);
    }

    public void writeIndexEntry(Context context, ByteArrayBuffer writer, Object obj) {
        FieldIndexKey composite = (FieldIndexKey)obj;
        int parentID = composite.parentID();
        Object value = composite.value();
        if (value == null){
            parentID = - parentID;
        }
        _parentIdHandler.write(parentID, writer);
        _valueHandler.writeIndexEntry(context, writer, composite.value());
    }
    
    public Indexable4 valueHandler() {
    	return _valueHandler;
    }
    
	public void defragIndexEntry(DefragmentContextImpl context) {
		_parentIdHandler.defragIndexEntry(context);
        _valueHandler.defragIndexEntry(context);
	}

	public PreparedComparison prepareComparison(Context context, Object fieldIndexKey) {
		if(fieldIndexKey == null){
			fieldIndexKey = new FieldIndexKeyImpl(Integer.MAX_VALUE, null);
		}
		final FieldIndexKey source = (FieldIndexKey)fieldIndexKey;
        final PreparedComparison preparedValueComparison = _valueHandler.prepareComparison(context, source.value());
        final PreparedComparison preparedParentIdComparison = _parentIdHandler.newPrepareCompare(source.parentID());
		return new PreparedComparison() {
			public int compareTo(Object obj) {
				FieldIndexKey target = (FieldIndexKey) obj;
		        try{
		        	int delegateResult = preparedValueComparison.compareTo(target.value());  
		            if(delegateResult != 0 ){
		                return delegateResult;
		            }
		        }catch (IllegalComparisonException ex){
		            // can happen, is expected
		        }
		        return preparedParentIdComparison.compareTo(new Integer(target.parentID()));
			}
		};
	}

	public boolean excludeNull() {
		if(_valueHandler instanceof CanExcludeNullInQueries){
			return ((CanExcludeNullInQueries)_valueHandler).excludeNull();
		}
		return false;
	}

}

