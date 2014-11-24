		/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import com.db4o.internal.*;
import com.db4o.internal.btree.*;
import com.db4o.internal.encoding.*;

/**
 * @exclude
 */
public class FieldMarshaller1 extends FieldMarshaller0 {
    
    private boolean hasBTreeIndex(FieldMetadata field){
        return ! field.isVirtual();
    }

    public void write(Transaction trans, ClassMetadata clazz, ClassAspect aspect, ByteArrayBuffer writer) {
        super.write(trans, clazz, aspect, writer);
        if(! (aspect instanceof FieldMetadata)){
            return;
        }
        FieldMetadata field = (FieldMetadata) aspect;
        if(! hasBTreeIndex(field)){
            return;
        }
        writer.writeIDOf(trans, field.getIndex(trans));
    }

    protected RawFieldSpec readSpec(AspectType aspectType, ObjectContainerBase stream, ByteArrayBuffer reader) {
    	RawFieldSpec spec=super.readSpec(aspectType, stream, reader);
    	if(spec==null) {
    		return null;
    	}
		if (spec.isVirtual()) {
			return spec;
		}
        int indexID = reader.readInt();
        spec.indexID(indexID);
    	return spec;
    }
    
    protected FieldMetadata fromSpec(RawFieldSpec spec, ObjectContainerBase stream, ClassMetadata containingClass) {
		FieldMetadata actualField = super.fromSpec(spec, stream, containingClass);
		if(spec==null) {
			return null;
		}
		if (spec.indexID() != 0) {
			actualField.initIndex(stream.systemTransaction(), spec.indexID());
		}
		return actualField;
	}
    
    public int marshalledLength(ObjectContainerBase stream, ClassAspect aspect) {
        int len = super.marshalledLength(stream, aspect);
        if(! (aspect instanceof FieldMetadata)){
            return len;
        }
        FieldMetadata field = (FieldMetadata) aspect;
        if(! hasBTreeIndex(field)){
            return len;
        }
        return  len + Const4.ID_LENGTH;
    }

    public void defrag(ClassMetadata classMetadata, ClassAspect aspect, LatinStringIO sio,
    		final DefragmentContextImpl context){
    	super.defrag(classMetadata, aspect, sio, context);
    	if(! (aspect instanceof FieldMetadata)){
    	    return;
    	}
    	FieldMetadata field = (FieldMetadata) aspect;
    	if(field.isVirtual()) {
    		return;
    	}
    	if(field.hasIndex()) {
        	BTree index = field.getIndex(context.systemTrans());
    		int targetIndexID=context.copyID();
    		if(targetIndexID!=0) {
    			index.defragBTree(context.services());
    		}
    	}
    	else {
        	context.writeInt(0);
    	}
    }
}
