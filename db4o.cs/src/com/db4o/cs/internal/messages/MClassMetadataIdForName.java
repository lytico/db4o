/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.ext.*;

/**
 * @exclude
 */
public class MClassMetadataIdForName extends MsgD implements MessageWithResponse {
    
    public final Msg replyFromServer() {
        String name = readString();
        try {
            synchronized (containerLock()) {
                int id = container().classMetadataIdForName(name);
                return Msg.CLASS_ID.getWriterForInt(systemTransaction(), id);
            }
        } 
        catch (Db4oException e) {
            // TODO: send the exception to the client
        } 
        return Msg.FAILED;
    }



}
