/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.j2me.bloat;

import EDU.purdue.cs.bloat.editor.*;
import EDU.purdue.cs.bloat.file.*;
import EDU.purdue.cs.bloat.reflect.*;

import com.db4o.j2me.bloat.testdata.*;
import com.db4o.reflect.self.*;

// TODO: Use plain classes for testing, not the SelfReflector test cases
// (which already implement SelfReflectable functionality)
public class Generation {

	public static void main(String[] args) {
		String outputDirName = "generated";
		ClassFileLoader loader = new ClassFileLoader();
		BloatJ2MEContext context = new BloatJ2MEContext(loader, outputDirName);
		
		ClassEditor ce = context.createClass(Modifiers.PUBLIC,
				"com.db4o.j2me.bloat.testdata.GeneratedDogSelfReflectionRegistry", Type.getType(Type.classDescriptor(SelfReflectionRegistry.class.getName())),
				new Type[0]);
		context.createLoadClassConstMethod(ce);

		RegistryEnhancer registryEnhancer = new RegistryEnhancer(context, ce,
				Dog.class);
		registryEnhancer.generate();
		ce.commit();

		enhanceClass(context,"../bin","com.db4o.j2me.bloat.testdata.Dog");
		enhanceClass(context,"../bin","com.db4o.j2me.bloat.testdata.Animal");
	}
	
	private static void enhanceClass(BloatJ2MEContext context,String path,String name) {
		ClassEditor cled = context.loadClass(path,name);
		ClassEnhancer classEnhancer = new ClassEnhancer(context, cled);
		classEnhancer.generate();
		cled.commit();
	}
}
