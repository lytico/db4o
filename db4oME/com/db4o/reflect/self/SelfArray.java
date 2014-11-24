package com.db4o.reflect.self;

import com.db4o.reflect.*;

public class SelfArray implements ReflectArray {
	private final Reflector _reflector;
	private final SelfReflectionRegistry _registry;

	SelfArray(Reflector reflector,SelfReflectionRegistry registry) {
		_reflector = reflector;
		_registry=registry;
	}

	public int[] dimensions(Object arr) {
		return new int[]{getLength(arr)};
	}

	public int flatten(Object a_shaped, int[] a_dimensions,
			int a_currentDimension, Object[] a_flat, int a_flatElement) {
		if(a_shaped instanceof Object[]) {
			Object[] shaped=(Object[])a_shaped;
			System.arraycopy(shaped, 0, a_flat, 0, shaped.length);
			return shaped.length;
		}
		return _registry.flattenArray(a_shaped,a_flat);
	}

	public Object get(Object onArray, int index) {
		if(onArray instanceof Object[]) {
			return ((Object[])onArray)[index];
		}
		return _registry.getArray(onArray,index);
	}

	public ReflectClass getComponentType(ReflectClass a_class) {
		return ((SelfClass)a_class).getComponentType();
	}

	public int getLength(Object array) {
		if(array instanceof Object[]) {
			return ((Object[])array).length;
		}
		return _registry.arrayLength(array);
	}

	public boolean isNDimensional(ReflectClass a_class) {
		return false;
	}

	public Object newInstance(ReflectClass componentType, int length) {
		return _registry.arrayFor(((SelfClass)componentType).getJavaClass(),length);
	}

	public Object newInstance(ReflectClass componentType, int[] dimensions) {
		return newInstance(componentType,dimensions[0]);
	}

	public void set(Object onArray, int index, Object element) {
		if(onArray instanceof Object[]) {
			((Object[])onArray)[index]=element;
			return;
		}
		_registry.setArray(onArray,index,element);
	}

	public int shape(Object[] a_flat, int a_flatElement, Object a_shaped,
			int[] a_dimensions, int a_currentDimension) {
		if(a_shaped instanceof Object[]) {
			Object[] shaped=(Object[])a_shaped;
			System.arraycopy(a_flat, 0, shaped, 0, a_flat.length);
			return a_flat.length;
		}
		return _registry.shapeArray(a_flat,a_shaped);
	}

}
