/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.instrumentation.core;

import com.db4o.instrumentation.ant.*;
import com.db4o.instrumentation.main.*;

import EDU.purdue.cs.bloat.editor.*;

/**
 * Abstract instrumentation step for use with {@link Db4oInstrumentationLauncher} or {@link Db4oFileEnhancerAntTask}.
 */
public interface BloatClassEdit {
	InstrumentationStatus enhance(ClassEditor ce, ClassLoader origLoader, BloatLoaderContext loaderContext);
}
