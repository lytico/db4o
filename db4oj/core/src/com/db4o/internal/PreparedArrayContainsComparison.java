/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.foundation.*;
import com.db4o.internal.handlers.array.*;
import com.db4o.marshall.*;
import com.db4o.typehandlers.*;

/**
 * @exclude
 */
public class PreparedArrayContainsComparison implements PreparedComparison {
	
	private final ArrayHandler _arrayHandler;
	
	private final PreparedComparison _preparedComparison; 
	
	private ObjectContainerBase _container;
	
	public PreparedArrayContainsComparison(Context context, ArrayHandler arrayHandler, TypeHandler4 typeHandler, Object obj){
		_arrayHandler = arrayHandler;
		_preparedComparison = Handlers4.prepareComparisonFor(typeHandler, context, obj);
		_container = context.transaction().container();
	}

	public int compareTo(Object obj) {
		// We never expect this call
		// TODO: The callers of this class should be refactored to pass a matcher and
		//       to expect a PreparedArrayComparison.
		throw new IllegalStateException();
	}
	
    public boolean isEqual(Object array) {
    	return isMatch(array, IntMatcher.ZERO);
    }

    public boolean isGreaterThan(Object array) {
    	return isMatch(array, IntMatcher.POSITIVE);
    }

    public boolean isSmallerThan(Object array) {
    	return isMatch(array, IntMatcher.NEGATIVE);
    }
    
    private boolean isMatch(Object array, IntMatcher matcher){
        if(array == null){
            return false;
        }
        Iterator4 i = _arrayHandler.allElements(_container, array);
        while (i.moveNext()) {
        	if(matcher.match(_preparedComparison.compareTo(i.current()))){
        		return true;
        	}
        }
        return false;
    }

}
