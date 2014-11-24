package com.db4odoc.concurrency;

import com.db4o.ObjectContainer;


public interface DatabaseOperation {
    void invoke(ObjectContainer container);
}
