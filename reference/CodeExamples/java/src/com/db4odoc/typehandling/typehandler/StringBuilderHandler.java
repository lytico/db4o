package com.db4odoc.typehandling.typehandler;

import com.db4o.ext.Db4oIOException;
import com.db4o.internal.DefragmentContext;
import com.db4o.internal.delete.DeleteContext;
import com.db4o.marshall.ReadBuffer;
import com.db4o.marshall.ReadContext;
import com.db4o.marshall.WriteContext;
import com.db4o.typehandlers.ValueTypeHandler;

import java.nio.charset.Charset;

/**
 * This is a very simple example for a type handler.
 * Take a look at the existing db4o type handlers.
 */
class StringBuilderHandler implements ValueTypeHandler {
    static final Charset CHAR_SET = Charset.forName("UTF-8");


    // #example: Delete the content
    @Override
    public void delete(DeleteContext deleteContext) throws Db4oIOException {
        skipData(deleteContext);
    }

    private void skipData(ReadBuffer deleteContext) {
        int numBytes = deleteContext.readInt();
        deleteContext.seek(deleteContext.offset()+ numBytes);
    }
    // #end example

    // #example: Defragment the content
    @Override
    public void defragment(DefragmentContext defragmentContext) {
        skipData(defragmentContext);
    }
    // #end example

    // #example: Write the StringBuilder
    @Override
    public void write(WriteContext writeContext, Object o) {
        StringBuilder builder = (StringBuilder) o;
        String str = builder.toString();
        final byte[] bytes = str.getBytes(CHAR_SET);
        writeContext.writeInt(bytes.length);
        writeContext.writeBytes(bytes);
    }
    // #end example

    // #example: Read the StringBuilder
    @Override
    public Object read(ReadContext readContext) {
        final int length = readContext.readInt();
        byte[] data = new byte[length];
        readContext.readBytes(data);
        return new StringBuilder(new String(data,CHAR_SET));
    }
    // #end example
}
// #end example
