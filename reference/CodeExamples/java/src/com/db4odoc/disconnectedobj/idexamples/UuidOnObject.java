package com.db4odoc.disconnectedobj.idexamples;

import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.config.UuidSupport;
import com.db4o.diagnostic.Diagnostic;
import com.db4o.diagnostic.DiagnosticListener;
import com.db4o.diagnostic.DiagnosticToConsole;
import com.db4o.diagnostic.LoadedFromClassIndex;
import com.db4o.query.Predicate;
import com.db4o.query.Query;

import java.util.UUID;


public class UuidOnObject implements IdExample<UUID> {

    public static IdExample<UUID> create() {
        return new UuidOnObject();
    }

    public UUID idForObject(Object obj, ObjectContainer container) {
        // #example: get the uuid
        IDHolder uuidHolder = (IDHolder)obj;
        UUID uuid = uuidHolder.getObjectId();
        // #end example
        return uuid;
    }

    public IDHolder objectForID(final UUID idForObject, ObjectContainer container) {
        // #example: get an object its UUID
        Query query = container.query();
        query.constrain(IDHolder.class);
        query.descend("uuid").constrain(idForObject);
        IDHolder object= (IDHolder) query.execute().get(0);
        // #end example
        return object;
    }

    public void configure(EmbeddedConfiguration configuration) {
        // #example: index the uuid-field
        configuration.common().add(new UuidSupport());
        configuration.common().objectClass(IDHolder.class).objectField("uuid").indexed(true);
        // #end example
    }

    public void registerEventOnContainer(ObjectContainer container) {
        // no events required for internal ids
    }
}
