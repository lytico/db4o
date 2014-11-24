/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.internal.activation.*;
import com.db4o.io.*;


/**
 * @exclude
 */
public class Serializer {
	
    public static StatefulBuffer marshall(Transaction ta, Object obj) {
        SerializedGraph serialized = marshall(ta.container(), obj);
        StatefulBuffer buffer = new StatefulBuffer(ta, serialized.length());
        buffer.append(serialized._bytes);
        buffer.useSlot(serialized._id, 0, serialized.length());
        return buffer;
    }

    public static SerializedGraph marshall(ObjectContainerBase serviceProvider, Object obj) {
        MemoryBin memoryBin = new MemoryBin(223, growthStrategy());
    	TransportObjectContainer carrier = newTransportObjectContainer(serviceProvider, memoryBin);
    	carrier.produceClassMetadata(carrier.reflector().forObject(obj));
		carrier.store(obj);
		int id = (int)carrier.getID(obj);
		carrier.close();
		return new SerializedGraph(id, memoryBin.data());
    }

	private static ConstantGrowthStrategy growthStrategy() {
		return new ConstantGrowthStrategy(300);
	}

	private static TransportObjectContainer newTransportObjectContainer(ObjectContainerBase serviceProvider,
            MemoryBin memoryBin) {
	    final TransportObjectContainer container = new TransportObjectContainer(serviceProvider, memoryBin);
	    container.deferredOpen();
		return container;
    }
    
    public static Object unmarshall(ObjectContainerBase serviceProvider, StatefulBuffer buffer) {
        return unmarshall(serviceProvider, buffer._buffer, buffer.getID());
    }
    
    public static Object unmarshall(ObjectContainerBase serviceProvider, SerializedGraph serialized) {
    	return unmarshall(serviceProvider, serialized._bytes, serialized._id);
    }

    public static Object unmarshall(ObjectContainerBase serviceProvider, byte[] bytes, int id) {
		if(id <= 0){
			return null;
		}
        MemoryBin memoryBin = new MemoryBin(bytes, growthStrategy());
		TransportObjectContainer carrier = newTransportObjectContainer(serviceProvider, memoryBin);
		Object obj = carrier.getByID(id);
		carrier.activate(carrier.transaction(), obj, new FullActivationDepth());
		carrier.close();
		return obj;
    }

}
