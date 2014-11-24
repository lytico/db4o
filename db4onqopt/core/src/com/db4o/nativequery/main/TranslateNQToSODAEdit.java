/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.main;

import EDU.purdue.cs.bloat.editor.*;
import EDU.purdue.cs.bloat.file.*;

import com.db4o.instrumentation.core.*;
import com.db4o.instrumentation.util.*;
import com.db4o.internal.query.*;
import com.db4o.nativequery.optimization.*;
import com.db4o.query.*;

/**
 * @exclude
 */
public class TranslateNQToSODAEdit implements BloatClassEdit {

	private final NativeQueryEnhancer _enhancer = new NativeQueryEnhancer();
	
	public InstrumentationStatus enhance(ClassEditor ce, ClassLoader origLoader, BloatLoaderContext loaderContext) {
		try {
		    Class enhancedPredicateClass = origLoader.loadClass(Db4oEnhancedFilter.class.getName());
            if(BloatUtil.implementsInHierarchy(ce, enhancedPredicateClass, loaderContext)) {
                return InstrumentationStatus.FAILED; // why failed?!?
            }

			Type type=ce.superclass();
			while(type!=null) {
				if(BloatUtil.normalizeClassName(type.className()).equals(Predicate.class.getName())) {
					boolean success = _enhancer.enhance(loaderContext,ce,PredicatePlatform.PREDICATEMETHOD_NAME,null,new DefaultClassSource());
					return (success ? InstrumentationStatus.INSTRUMENTED : InstrumentationStatus.NOT_INSTRUMENTED);
				}
				type = loaderContext.superType(type);
			}
			//System.err.println("Bypassing "+ce.name());
		} catch (Exception exc) {
//			throw new RuntimeException(exc);
			return InstrumentationStatus.FAILED;
		}
		return InstrumentationStatus.NOT_INSTRUMENTED;
	}

}
