package com.db4odoc.clientserver.pooling;

import com.db4o.ObjectContainer;


public interface ClientConnectionFactory {
    ObjectContainer connect(); 
}
