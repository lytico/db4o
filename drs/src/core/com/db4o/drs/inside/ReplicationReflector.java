/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com

This file is part of the db4o open source object database.

db4o is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License as published
by the Free Software Foundation and as clarified by db4objects' GPL 
interpretation policy, available at
http://www.db4o.com/about/company/legalpolicies/gplinterpretation/
Alternatively you can write to db4objects, Inc., 1900 S Norfolk Street,
Suite 350, San Mateo, CA 94403, USA.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. */
package com.db4o.drs.inside;

import com.db4o.drs.*;
import com.db4o.drs.db4o.*;
import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;


public class ReplicationReflector {
	
	private InternalObjectContainer _container;
	
	private Reflector _reflector;

	public ReplicationReflector(ReplicationProvider providerA, ReplicationProvider providerB, Reflector reflector) {
		if (reflector == null) {
			if ((_container = containerFrom(providerA)) != null) {
				return;
			}
			if ((_container = containerFrom(providerB)) != null) {
				return;
			}
		}
		GenericReflector genericReflector = new GenericReflector(null, reflector == null ? Platform4.reflectorForType(ReplicationReflector.class) : reflector);
		Platform4.registerCollections(genericReflector);
		_reflector = genericReflector;
	}

	public Object[] arrayContents(Object array) {
		int[] dim = arrayReflector().dimensions(array);
		Object[] result = new Object[volume(dim)];
		arrayReflector().flatten(array, dim, 0, result, 0); //TODO Optimize add a visit(Visitor) method to ReflectArray or navigate the array to avoid copying all this stuff all the time.
		return result;
	}

	private int volume(int[] dim) {
		int result = dim[0];
		for (int i = 1; i < dim.length; i++) {
			result = result * dim[i];
		}
		return result;
	}

	public ReflectClass forObject(Object obj) {
		return reflector().forObject(obj);
	}

	public ReflectClass forClass(Class clazz) {
		return reflector().forClass(clazz);
	}

	ReflectClass getComponentType(ReflectClass claxx) {
		return arrayReflector().getComponentType(claxx);
	}

	int[] arrayDimensions(Object obj) {
		return arrayReflector().dimensions(obj);
	}

	public Object newArrayInstance(ReflectClass componentType, int[] dimensions) {
		return arrayReflector().newInstance(componentType, dimensions);
	}

	public int arrayShape(
			Object[] a_flat,
			int a_flatElement,
			Object a_shaped,
			int[] a_dimensions,
			int a_currentDimension) {
		return arrayReflector().shape(a_flat, a_flatElement, a_shaped, a_dimensions, a_currentDimension);
	}

	public boolean isValueType(ReflectClass clazz) {
		if(_container == null){
			return clazz.isSimple();
		}
		ClassMetadata classMetadata = _container.classMetadataForReflectClass(clazz);
		if(classMetadata == null) {
			return false;
		}
		return classMetadata.isValueType();
	}
	
	private InternalObjectContainer containerFrom(ReplicationProvider provider) {
		if(!(provider instanceof Db4oReplicationProvider)) {
			return null;
		}
		Db4oReplicationProvider db4oProvider = (Db4oReplicationProvider) provider;
		ExtObjectContainer container = db4oProvider.getObjectContainer();
		if(!(container instanceof InternalObjectContainer)) {
			return null;
		}
		return (InternalObjectContainer) container;
	}
	
	private ReflectArray arrayReflector() {
		return reflector().array();
//		return _container.reflector().array();
	}
	
	private Reflector reflector(){
		return _container == null ? _reflector : _container.reflector();
	}

	public void copyState(Object to, Object from) {
		
		
		ReflectClass fromClass = reflector().forObject(from);
		
		// FIXME: We need to get the classes parents fields copied too.

		for (ReflectField f : fromClass.getDeclaredFields()) {
			if (f.isStatic()) {
				continue;
			}
			f.set(to, f.get(from));
		}
		
	}
}
