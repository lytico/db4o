/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.internal.query.processor;

import com.db4o.foundation.*;
import com.db4o.internal.*;

/**
 * @exclude
 */
public class QESmaller extends QEAbstract {

    boolean evaluate(QConObject constraint, InternalCandidate candidate, Object obj) {
        if (obj == null) {
            return false;
        }
        PreparedComparison preparedComparison = constraint.prepareComparison(candidate);
        if(preparedComparison instanceof PreparedArrayContainsComparison){
        	return ((PreparedArrayContainsComparison)preparedComparison).isGreaterThan(obj);
        }
        return preparedComparison.compareTo(obj) > 0;
    }

    public void indexBitMap(boolean[] bits) {
        bits[QE.SMALLER] = true;
    }

}
