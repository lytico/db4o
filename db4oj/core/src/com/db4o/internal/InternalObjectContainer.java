/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.internal.activation.*;
import com.db4o.internal.callbacks.*;
import com.db4o.internal.events.*;
import com.db4o.internal.query.*;
import com.db4o.reflect.*;

/**
 * @exclude
 * @sharpen.partial
 */
public interface InternalObjectContainer extends ExtObjectContainer {
    
    public void callbacks(Callbacks cb);
    
    public Callbacks callbacks();
    
    /**
     * @sharpen.property
     */
    public ObjectContainerBase container();
    
    /**
     * @sharpen.property
     */
    public Transaction transaction();
    
    public NativeQueryHandler getNativeQueryHandler();

    public ClassMetadata classMetadataForReflectClass(ReflectClass reflectClass);

    public ClassMetadata classMetadataForName(String name);
    
    public ClassMetadata classMetadataForID(int id);

    /**
     * @sharpen.property
     */
    public HandlerRegistry handlers();
    
    /**
     * @sharpen.property
     */
    public Config4Impl configImpl();
    
    public <R> R syncExec(Closure4<R> block);
    
    public int instanceCount(ClassMetadata clazz, Transaction trans);
    
    /**
     * @sharpen.property
     */
    public boolean isClient();

	public void storeAll(Transaction trans, Iterator4 objects);

	public UpdateDepthProvider updateDepthProvider();

	public EventRegistryImpl newEventRegistry();
	
	boolean inCallback();
}
