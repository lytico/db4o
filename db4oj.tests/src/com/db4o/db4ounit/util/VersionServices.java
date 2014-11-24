/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.util;

import java.io.*;

import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.internal.activation.*;
import com.db4o.internal.marshall.*;


public class VersionServices {
    
    public static final byte HEADER_30_40 = 123;
    
    public static final byte HEADER_46_57 = 4;
    
    public static final byte HEADER_60 = 100;
    

    public static byte fileHeaderVersion(String testFile) throws IOException{
        RandomAccessFile raf = new RandomAccessFile(testFile, "r");
        byte[] bytes = new byte[1];
        raf.read(bytes);  // readByte() doesn't convert to .NET.
        byte db4oHeaderVersion = bytes[0]; 
        raf.close();
        return db4oHeaderVersion;
    }
    
    public static int slotHandlerVersion(ExtObjectContainer objectContainer, Object obj){
        int id = (int) objectContainer.getID(obj);
        ObjectInfo objectInfo = objectContainer.getObjectInfo(obj);
        ObjectContainerBase container = (ObjectContainerBase) objectContainer;
        Transaction trans = container.transaction();
        ByteArrayBuffer buffer = container.readBufferById(trans, id);
        UnmarshallingContext context = new UnmarshallingContext(trans, (ObjectReference)objectInfo, Const4.TRANSIENT, false);
        context.buffer(buffer);
        context.persistentObject(obj);
        context.activationDepth(new LegacyActivationDepth(0));
        context.read();
        return context.handlerVersion();
    }


}
