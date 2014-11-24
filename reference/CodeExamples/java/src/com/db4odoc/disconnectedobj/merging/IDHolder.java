package com.db4odoc.disconnectedobj.merging;

import java.util.UUID;

public abstract class IDHolder {
    private final UUID uuid = UUID.randomUUID();

    public UUID getObjectId(){
        return uuid;
    }
}