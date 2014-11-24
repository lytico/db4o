package com.db4odoc.configuration.idsystem;

import com.db4o.config.IdSystemFactory;
import com.db4o.internal.LocalObjectContainer;
import com.db4o.internal.ids.IdSystem;
import com.db4o.internal.ids.InMemoryIdSystem;


public class CustomIdSystemFactory implements IdSystemFactory {
    public IdSystem newInstance(LocalObjectContainer localObjectContainer) {
        return new InMemoryIdSystem(localObjectContainer);
    }
}
