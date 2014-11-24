/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.convert.conversions;

import com.db4o.internal.convert.*;
import com.db4o.internal.convert.ConversionStage.*;

/**
 * @exclude
 */
public class ClassAspects_7_4 extends Conversion {
	
    public static final int VERSION = 7;
    
	public void convert(SystemUpStage stage) {
        stage.file().classCollection().writeAllClasses();
	}

}
