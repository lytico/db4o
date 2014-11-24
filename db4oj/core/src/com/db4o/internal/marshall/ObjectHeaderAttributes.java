/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import com.db4o.foundation.*;
import com.db4o.internal.*;


/**
 * @exclude
 */
public class ObjectHeaderAttributes {

    private final int _fieldCount;
    
    private final BitMap4 _nullBitMap;
    
    public ObjectHeaderAttributes(ByteArrayBuffer reader){
        _fieldCount = reader.readInt();
        _nullBitMap = reader.readBitMap(_fieldCount);
    }
    
    public boolean isNull(int fieldIndex){
        return _nullBitMap.isTrue(fieldIndex);
    }

}
