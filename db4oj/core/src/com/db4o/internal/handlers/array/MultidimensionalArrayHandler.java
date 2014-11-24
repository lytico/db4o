/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.handlers.array;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.marshall.*;
import com.db4o.reflect.*;
import com.db4o.typehandlers.*;


/**
 * n-dimensional array
 * @exclude
 */
public class MultidimensionalArrayHandler extends ArrayHandler {
	
    public MultidimensionalArrayHandler(TypeHandler4 a_handler, boolean a_isPrimitive) {
        super(a_handler, a_isPrimitive);
    }
    
    public MultidimensionalArrayHandler(){
        // required for reflection cloning
    }
    
    @Override
    public final Iterator4 allElements(ObjectContainerBase container, Object array) {
		return allElementsMultidimensional(arrayReflector(container), array);
    }

	public static Iterator4 allElementsMultidimensional(final ReflectArray reflectArray, Object array) {
	    return new MultidimensionalArrayIterator(reflectArray, (Object[])array);
	}

    protected static final int elementCount(int[] a_dim) {
        int elements = a_dim[0];
        for (int i = 1; i < a_dim.length; i++) {
            elements = elements * a_dim[i];
        }
        return elements;
    }

    public final byte identifier() {
        return Const4.YAPARRAYN;
    }
    
    protected ArrayInfo newArrayInfo() {
        return new MultidimensionalArrayInfo();
    }

    protected void readDimensions(ArrayInfo info, ReadBuffer buffer) {
        readDimensions(info, buffer, buffer.readInt());
    }

    private void readDimensions(ArrayInfo info, ReadBuffer buffer, int dimensionCount) {
        int[] dim = new int[dimensionCount];
        for (int i = 0; i < dim.length; i++) {
            dim[i] = buffer.readInt();
        }
        ((MultidimensionalArrayInfo)info).dimensions(dim);
        info.elementCount(elementCount(dim));
    }

    protected void readElements(ReadContext context, ArrayInfo info, Object array) {
        if(array == null){
            return;
        }
        Object[] objects = new Object[info.elementCount()];
        readInto(context, info, objects);
        arrayReflector(container(context)).shape(objects, 0, array, ((MultidimensionalArrayInfo)info).dimensions() , 0);
    }
    
    protected void writeDimensions(WriteContext context, ArrayInfo info) {
        int[] dim = ((MultidimensionalArrayInfo)info).dimensions();
        context.writeInt(dim.length);
        for (int i = 0; i < dim.length; i++) {
            context.writeInt(dim[i]);
        }
    }

    protected void writeElements(WriteContext context, Object obj, ArrayInfo info) {
        Iterator4 objects = allElements(container(context), obj);
        
        if (hasNullBitmap(info)) {
            BitMap4 nullBitMap = new BitMap4(info.elementCount());
            ReservedBuffer nullBitMapBuffer = context.reserve(nullBitMap.marshalledLength());
            int currentElement = 0;
            while (objects.moveNext()) {
                Object current = objects.current();
                if(current == null){
                    nullBitMap.setTrue(currentElement);
                }else{
                    context.writeObject(delegateTypeHandler(), current);
                }
                currentElement++;
            }
            nullBitMapBuffer.writeBytes(nullBitMap.bytes());
        } else {
            while (objects.moveNext()) {
                context.writeObject(delegateTypeHandler(), objects.current());
            }
        }
        
    }
    
    protected void analyzeDimensions(ObjectContainerBase container, Object obj, ArrayInfo info){
        int[] dim = arrayReflector(container).dimensions(obj);
        ((MultidimensionalArrayInfo)info).dimensions(dim);
        info.elementCount(elementCount(dim));
    }

    public TypeHandler4 unversionedTemplate() {
        return new MultidimensionalArrayHandler();
    }

}
