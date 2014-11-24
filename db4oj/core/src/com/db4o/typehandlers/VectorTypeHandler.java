/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.typehandlers;

import java.util.*;

import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.delete.*;
import com.db4o.internal.handlers.*;
import com.db4o.internal.marshall.*;
import com.db4o.marshall.*;

/**
 * TypeHandler for java.util.Vector for JDKs without the 
 * collection framework.
 * @sharpen.ignore
 */
public class VectorTypeHandler implements ReferenceTypeHandler, CascadingTypeHandler, VariableLengthTypeHandler {

    public PreparedComparison prepareComparison(Context context, Object obj) {
        // TODO Auto-generated method stub
        return null;
    }

	public void write(WriteContext context, Object obj) {
        Vector vector = (Vector)obj;
        TypeHandler4 elementHandler = detectElementTypeHandler(container(context), vector);
        writeElementClassMetadataId(context, elementHandler);
        writeElementCount(context, vector);
        writeElements(context, vector, elementHandler);
    }
    
	public void activate(ReferenceActivationContext context) {
        Vector vector = (Vector)((UnmarshallingContext) context).persistentObject();
        vector.removeAllElements();
        TypeHandler4 elementHandler = readElementTypeHandler(context, context);
        int elementCount = context.readInt();
        for (int i = 0; i < elementCount; i++) {
            vector.addElement(context.readObject(elementHandler));
        }
    }
    
	private void writeElementCount(WriteContext context, Vector vector) {
		context.writeInt(vector.size());
	}

	private void writeElements(WriteContext context, Vector vector, TypeHandler4 elementHandler) {
        final Enumeration elements = vector.elements();
        while (elements.hasMoreElements()) {
            context.writeObject(elementHandler, elements.nextElement());
        }
	}

    private ObjectContainerBase container(Context context) {
        return ((InternalObjectContainer)context.objectContainer()).container();
    }
    
    public void delete(final DeleteContext context) throws Db4oIOException {
		if (! context.cascadeDelete()) {
		    return;
		}
        TypeHandler4 elementHandler = readElementTypeHandler(context, context);
        int elementCount = context.readInt();
        for (int i = elementCount; i > 0; i--) {
			elementHandler.delete(context);
        }
    }

    public void defragment(DefragmentContext context) {
        TypeHandler4 elementHandler = readElementTypeHandler(context, context);
        int elementCount = context.readInt();
        for (int i = 0; i < elementCount; i++) {
            elementHandler.defragment(context);
        }
    }
    
    public final void cascadeActivation(ActivationContext context) {
        Enumeration all = ((Vector) context.targetObject()).elements();
        while (all.hasMoreElements()) {
            context.cascadeActivationToChild(all.nextElement());
        }
    }

    public TypeHandler4 readCandidateHandler(QueryingReadContext context) {
        return this;
    }
    
    public void collectIDs(final QueryingReadContext context) {
        TypeHandler4 elementHandler = readElementTypeHandler(context, context);
        int elementCount = context.readInt();
        for (int i = 0; i < elementCount; i++) {
            context.readId(elementHandler);
        }
    }

	private void writeElementClassMetadataId(WriteContext context, TypeHandler4 elementHandler) {
		context.writeInt(0);
	}

	private TypeHandler4 readElementTypeHandler(ReadBuffer buffer, Context context) {
		buffer.readInt();
		return (TypeHandler4) container(context).handlers().openTypeHandler();
	}

	private TypeHandler4 detectElementTypeHandler(InternalObjectContainer container, Vector vector) {
		return (TypeHandler4) container.handlers().openTypeHandler();
	}

}