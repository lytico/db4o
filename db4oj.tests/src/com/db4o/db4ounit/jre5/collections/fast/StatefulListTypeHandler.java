/* Copyright (C) 2004 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.jre5.collections.fast;

import java.util.*;

import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.delete.*;
import com.db4o.internal.handlers.*;
import com.db4o.internal.marshall.*;
import com.db4o.marshall.*;
import com.db4o.typehandlers.*;

/**
 * TypeHandler for classes that implement java.util.List.<br><br>
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class StatefulListTypeHandler implements ReferenceTypeHandler, CascadingTypeHandler,
		VariableLengthTypeHandler, ModificationAware{

	public PreparedComparison prepareComparison(Context context, Object obj) {
		// TODO Auto-generated method stub
		return null;
	}

	public void write(WriteContext context, Object obj) {
		List list = (List) obj;
		writeElementCount(context, list);
		writeElements(context, list);
		setClean(obj);
	}

	private void setClean(Object obj) {
		((StatefulList)obj).setDirty(false);
	}

	public void activate(ReferenceActivationContext context) {
		List list = (List) ((UnmarshallingContext) context).persistentObject();
		list.clear();
		int elementCount = context.readInt();
		TypeHandler4 elementHandler = elementTypeHandler(context, list);
		for (int i = 0; i < elementCount; i++) {
			list.add(context.readObject(elementHandler));
		}
		setClean(list);
	}

	private void writeElementCount(WriteContext context, List list) {
		context.writeInt(list.size());
	}

	private void writeElements(WriteContext context, List list) {
		TypeHandler4 elementHandler = elementTypeHandler(context, list);
		for(Object element: list){
			context.writeObject(elementHandler, element);
		}
	}

	private InternalObjectContainer container(Context context) {
		return ((InternalObjectContainer) context.objectContainer()).container();
	}

	private TypeHandler4 elementTypeHandler(Context context, List list) {

		// TODO: If all elements in the list are of one type,
		// it is possible to use a more specific handler

		return (TypeHandler4) container(context).handlers().openTypeHandler();
	}

	public void delete(final DeleteContext context) throws Db4oIOException {
		if (!context.cascadeDelete()) {
			return;
		}
		TypeHandler4 handler = elementTypeHandler(context, null);
		int elementCount = context.readInt();
		for (int i = elementCount; i > 0; i--) {
			handler.delete(context);
		}
	}

	public void defragment(DefragmentContext context) {
		TypeHandler4 handler = elementTypeHandler(context, null);
		int elementCount = context.readInt();
		for (int i = 0; i < elementCount; i++) {
			handler.defragment(context);
		}
	}

	public final void cascadeActivation(ActivationContext context) {
		List list = (List) context.targetObject();
		for(Object obj: list){
			context.cascadeActivationToChild(obj);
		}
	}

	public TypeHandler4 readCandidateHandler(QueryingReadContext context) {
		return this;
	}


	private TypeHandler4 untypedObjectHandlerFrom(Context context) {
	    return (TypeHandler4) context.transaction().container().handlers().openTypeHandler();
	}

    public void collectIDs(final QueryingReadContext context) {
        int elementCount = context.readInt();
        TypeHandler4 elementHandler = untypedObjectHandlerFrom(context);
        for (int i = 0; i < elementCount; i++) {
            context.readId(elementHandler);
        }
    }

	public boolean isModified(Object obj) {
		return ((StatefulList)obj).isDirty();
	}

}
