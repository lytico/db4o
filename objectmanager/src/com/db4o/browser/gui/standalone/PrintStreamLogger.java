/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.gui.standalone;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.OutputStream;

import com.swtworkbench.community.xswt.metalogger.Logger;


public class PrintStreamLogger extends OutputStream {

    private ByteArrayOutputStream delegate = new ByteArrayOutputStream();
    
    /* (non-Javadoc)
     * @see java.io.OutputStream#close()
     */
    public void close() throws IOException {
        Logger.log().data(delegate.toString());
        delegate.close();
        delegate = null;
    }

    /* (non-Javadoc)
     * @see java.io.OutputStream#flush()
     */
    public void flush() throws IOException {
        Logger.log().data(delegate.toString());
        reset();
    }
    
    public void reset() throws IOException {
        delegate.close();
        delegate = new ByteArrayOutputStream();
    }

    /* (non-Javadoc)
     * @see java.io.OutputStream#write(byte[], int, int)
     */
    public void write(byte[] b, int off, int len) throws IOException {
        delegate.write(b, off, len);
        flush();
    }

    /* (non-Javadoc)
     * @see java.io.OutputStream#write(byte[])
     */
    public void write(byte[] b) throws IOException {
        delegate.write(b);
        flush();
    }

    public void write(int b) throws IOException {
        delegate.write(b);
    }


}
