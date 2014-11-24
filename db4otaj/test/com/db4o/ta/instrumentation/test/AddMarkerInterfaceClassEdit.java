/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.ta.instrumentation.test;

import EDU.purdue.cs.bloat.editor.*;

import com.db4o.instrumentation.core.*;

public class AddMarkerInterfaceClassEdit implements BloatClassEdit {

	private final Class _markerInterface;
	
	public AddMarkerInterfaceClassEdit(Class markerInterface) {
		_markerInterface = markerInterface;
	}

	public InstrumentationStatus enhance(ClassEditor ce,
			ClassLoader origLoader, BloatLoaderContext loaderContext) {
		ce.addInterface(_markerInterface);
		ce.commit();
		return InstrumentationStatus.INSTRUMENTED;
	}

}
