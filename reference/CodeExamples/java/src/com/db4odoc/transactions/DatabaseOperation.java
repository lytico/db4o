package com.db4odoc.transactions;

import com.db4o.ObjectContainer;


interface DatabaseOperation {
    void invoke(ObjectContainer container);
}
