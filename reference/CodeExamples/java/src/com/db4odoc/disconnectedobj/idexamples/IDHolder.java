package com.db4odoc.disconnectedobj.idexamples;

import java.util.UUID;

/**
 * Id holder. For the example code it supports both, uuid and an int-id.
 * For a project you normally choose one or the other.
 */
public abstract class IDHolder {

    // #example: generate the id
    private final UUID uuid = UUID.randomUUID();

    public UUID getObjectId(){
        return uuid;
    }
    // #end example
    // #example: id holder
    private int id;
    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }
    // #end example
}
