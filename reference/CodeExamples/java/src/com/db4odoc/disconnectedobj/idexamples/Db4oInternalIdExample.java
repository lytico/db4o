package com.db4odoc.disconnectedobj.idexamples;

import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;


public class Db4oInternalIdExample implements IdExample<Long> {

    public static Db4oInternalIdExample create(){
        return new Db4oInternalIdExample();
    } 

    public Long idForObject(Object obj, ObjectContainer container) {
        // #example: get the db4o internal ids
        long interalId = container.ext().getID(obj);
        // #end example
        return interalId;
    }

    public Object objectForID(Long idForObject, ObjectContainer container) {
        // #example: get an object by db4o internal id
        long internalId =idForObject;
        Object objectForID = container.ext().getByID(internalId);
        // getting by id doesn't activate the object
        // so you need to do it manually
        container.ext().activate(objectForID);
        // #end example
        return objectForID;
    }

    public void configure(EmbeddedConfiguration configuration) {
        // no configuration required for internal ids  
    }

    public void registerEventOnContainer(ObjectContainer container) {
        // no events required for internal ids  
    }
}
