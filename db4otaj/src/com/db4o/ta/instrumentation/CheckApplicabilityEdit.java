/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.ta.instrumentation;

import java.util.*;

import EDU.purdue.cs.bloat.editor.*;

import com.db4o.instrumentation.core.*;
import com.db4o.instrumentation.util.*;

/**
 * @exclude
 */
class CheckApplicabilityEdit implements BloatClassEdit {

	public InstrumentationStatus enhance(ClassEditor ce, ClassLoader origLoader, BloatLoaderContext loaderContext) {
		try {
			if (ce.isInterface()) {
				return InstrumentationStatus.FAILED;
			}
			if (BloatUtil.extendsInHierarchy(ce, Enum.class, loaderContext)) {
				return InstrumentationStatus.FAILED;
			}
			if (!isApplicableClass(ce, loaderContext)) {
				return InstrumentationStatus.FAILED;
			}
		} catch (ClassNotFoundException e) {
			return InstrumentationStatus.FAILED;
		}
		return InstrumentationStatus.NOT_INSTRUMENTED;
	}

	private boolean isApplicableClass(ClassEditor ce, BloatLoaderContext loaderContext) {
		ClassEditor curEditor = ce;
		try {
			while (curEditor != null && !isApplicablePlatformClass(curEditor)) {
				if (BloatUtil.isPlatformClassName(BloatUtil.normalizeClassName(curEditor.type()))) {
					return false;
				}
				curEditor = loaderContext.classEditor(curEditor.superclass());
			}
		} catch (ClassNotFoundException exc) {
			return false;
		}
		return true;
	}

	private boolean isApplicablePlatformClass(ClassEditor ce) {
		String className = BloatUtil.normalizeClassName(ce.name());
		return Enum.class.getName().equals(className) || isSupportedCollection(className) || Object.class.getName().equals(className);
	}

	private boolean isSupportedCollection(String className) {
		return ArrayList.class.getName().equals(className);
	}

	private boolean isEnum(Class curClazz) {
		return curClazz == Enum.class;
	}
	
}
