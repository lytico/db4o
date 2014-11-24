/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.model.query;

import com.db4o.objectmanager.model.IDatabase;
import com.db4o.query.Query;
import com.db4o.reflect.ReflectClass;

public class QueryBuilderModel {

    private IDatabase database;

    private QueryPrototypeInstance rootInstance;

    public QueryBuilderModel(ReflectClass input, IDatabase database) {
        this.database = database;
        rootInstance = new QueryPrototypeInstance(input, this);
    }

    public Query getQuery() {
        Query result = database.query();
        rootInstance.addUserConstraints(result);
        return result;
    }

    public QueryPrototypeInstance getRootInstance() {
        return rootInstance;
    }
    
    public IDatabase getDatabase() {
        return database;
    }
    
}
