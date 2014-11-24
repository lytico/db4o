package com.db4odoc.disconnectedobj.idexamples;

import com.db4o.ObjectContainer;
import com.db4o.config.ConfigScope;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.ext.Db4oUUID;


public class Db4oUuidExample implements IdExample<Db4oUUID> {

    public static Db4oUuidExample create(){
        return new Db4oUuidExample();
    }
    public Db4oUUID idForObject(Object obj, ObjectContainer container) {
        // #example: get the db4o-uuid
        Db4oUUID uuid = container.ext().getObjectInfo(obj).getUUID();
        // #end example
        return uuid;
    }

    public Object objectForID(Db4oUUID idForObject, ObjectContainer container) {
        // #example: get an object by a db4o-uuid
        Object objectForId = container.ext().getByUUID(idForObject);
        // getting by uuid doesn't activate the object
        // so you need to do it manually
        container.ext().activate(objectForId);
         // #end example
        return objectForId;
    }


    public void configure(EmbeddedConfiguration configuration) {
        // #example: db4o-uuids need to be activated
        configuration.file().generateUUIDs(ConfigScope.GLOBALLY);
        // #end example
    }

    public void registerEventOnContainer(ObjectContainer container) {
        // no events required for internal ids
    }
}
