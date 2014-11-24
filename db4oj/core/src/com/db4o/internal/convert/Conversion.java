/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.convert;

import com.db4o.internal.convert.ConversionStage.*;

/**
 * @exclude
 */
public abstract class Conversion {
    
    /** @param stage */
    public void convert(ClassCollectionAvailableStage stage){
    }
    
    /** @param stage */
	public void convert(SystemUpStage stage){
    }
}
