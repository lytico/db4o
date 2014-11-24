/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.handlers.array;

import com.db4o.*;
import com.db4o.internal.*;
import com.db4o.marshall.*;
import com.db4o.reflect.*;


/**
 * @exclude
 */
public class ArrayVersionHelper3 extends ArrayVersionHelper5 {
    
    public int classIDFromInfo(ObjectContainerBase container, ArrayInfo info){
        ClassMetadata classMetadata = container.produceClassMetadata(info.reflectClass());
        if (classMetadata == null) {
            // TODO: This one is a terrible low-frequency blunder !!!
            // If YapClass-ID == 99999 then we will get IGNORE back.
            // Discovered on adding the primitives
            return Const4.IGNORE_ID;
        }
        return classMetadata.getID();
    }
    
    public int classIdToMarshalledClassId(int classID, boolean primitive){
        if(primitive){
            classID -= Const4.PRIMITIVE;
        }
        return - classID;
    }
    
    public ReflectClass classReflector(Reflector reflector, ClassMetadata classMetadata, boolean isPrimitive){
        if(Deploy.csharp){
            ReflectClass primitiveClaxx = Handlers4.primitiveClassReflector(classMetadata, reflector);
            if(primitiveClaxx != null){
                return primitiveClaxx;
            }
        }
        return super.classReflector(reflector, classMetadata, isPrimitive);
    }
    
    public boolean hasNullBitmap(ArrayInfo info) {
        return false;
    }
    
    public boolean isPrimitive(Reflector reflector, ReflectClass claxx, ClassMetadata classMetadata) {
        if(Deploy.csharp){
            return Handlers4.primitiveClassReflector(classMetadata, reflector) != null;
        }
        return claxx.isPrimitive();
    }

	public ReflectClass reflectClassFromElementsEntry(ObjectContainerBase container, ArrayInfo info, int classID) {

        
        if(classID == Const4.IGNORE_ID){
            // TODO: Here is a low-frequency mistake, extremely unlikely.
            // If classID == 99999 by accident then we will get ignore.
            
            return null;
        }
            
        info.primitive(false);
        
        if(useJavaHandling()){
            if(classID < Const4.PRIMITIVE){
                info.primitive(true);
                classID -= Const4.PRIMITIVE;
            }
        }
        classID = - classID;
        
        ClassMetadata classMetadata = container.classMetadataForID(classID);
        if (classMetadata != null) {
            return classReflector(container.reflector(), classMetadata, info.primitive());
        }
            
        return null;
    }
    
    public final boolean useJavaHandling() {
        return ! Deploy.csharp;
    }
    
    public void writeTypeInfo(WriteContext context, ArrayInfo info) {
        // do nothing, the byte for additional type information was added after format 3
    }
    
    public void readTypeInfo(Transaction trans, ReadBuffer buffer, ArrayInfo info, int classID) {
        // do nothing, the byte for additional type information was added after format 3
    }


}
