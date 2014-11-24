/* Copyright (C) 2004, 2005   Versant Inc.   http://www.db4o.com */

package com.db4o.reflect;


/** 
 * representation for java.lang.reflect.Array.
 * <br><br>See the respective documentation in the JDK API.
 * @see Reflector
 */
public interface ReflectArray {
    
    public void analyze(Object obj, ArrayInfo info);
    
    public int[] dimensions(Object arr);
    
    public int flatten(
        Object a_shaped,
        int[] a_dimensions,
        int a_currentDimension,
        Object[] a_flat,
        int a_flatElement);
	
	public Object get(Object onArray, int index);
	
    public ReflectClass getComponentType(ReflectClass a_class);
	
	public int getLength(Object array);
	
	public boolean isNDimensional(ReflectClass a_class);
	
	public Object newInstance(ReflectClass componentType, ArrayInfo info);
	
	public Object newInstance(ReflectClass componentType, int length);
	
	public Object newInstance(ReflectClass componentType, int[] dimensions);
	
	public void set(Object onArray, int index, Object element);
    
    public int shape(
        Object[] a_flat,
        int a_flatElement,
        Object a_shaped,
        int[] a_dimensions,
        int a_currentDimension);

	
}

