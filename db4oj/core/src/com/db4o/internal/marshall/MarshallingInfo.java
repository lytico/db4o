/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import com.db4o.internal.*;
import com.db4o.marshall.*;


/**
 * @exclude
 */
public interface MarshallingInfo extends AspectVersionContext {

    public ClassMetadata classMetadata();

    public ReadBuffer buffer();
    
    public void beginSlot();
    
    public boolean isNull(int fieldIndex);
    
}
