/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections.transparent.map;

import java.util.*;

import com.db4o.db4ounit.jre12.collections.transparent.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;

import db4ounit.fixtures.*;

/**
 * @sharpen.remove
 */
@decaf.Remove(decaf.Platform.JDK11)
public class MapSpec <T extends Map<CollectionElement, CollectionElement>> implements Labeled{

	private final Closure4<T> _activatableMapFactory;
	
	public MapSpec(Closure4<T> activatableMapFactory){
		_activatableMapFactory = activatableMapFactory;
	}
	
	public Map<CollectionElement, CollectionElement> newActivatableMap() {
		return _activatableMapFactory.run();
	}
	
	public String label() {
		return ReflectPlatform.simpleName(newActivatableMap().getClass());
	}

}
