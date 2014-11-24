/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import com.db4o.internal.*;
import com.db4o.internal.btree.*;
import com.db4o.internal.classindex.*;
import com.db4o.internal.convert.conversions.*;


/**
 * @exclude
 */
public class ClassMarshaller0 extends ClassMarshaller{
    
    protected void readIndex(ObjectContainerBase stream, ClassMetadata clazz, ByteArrayBuffer reader) {
        int indexID = reader.readInt();
        if(! stream.maintainsIndices() || ! (stream instanceof LocalObjectContainer)){
            return;
        }
        if(btree(clazz) != null){
            return;
        }
        clazz.index().read(stream, validIndexId(indexID));
        if(isOldClassIndex(indexID)){
            new ClassIndexesToBTrees_5_5().convert((LocalObjectContainer)stream, indexID, btree(clazz));
            stream.setDirtyInSystemTransaction(clazz);
        }
    }

    private BTree btree(ClassMetadata clazz) {
        return BTreeClassIndexStrategy.btree(clazz);
    }

    private int validIndexId(int indexID) {
        return isOldClassIndex(indexID) ? 0 : -indexID;
    }

    private boolean isOldClassIndex(int indexID) {
        return indexID > 0;
    }
    
    protected int indexIDForWriting(int indexID){
        return indexID;
    }
}
