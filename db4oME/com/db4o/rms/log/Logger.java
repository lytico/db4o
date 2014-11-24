/*
 * Logger.java
 *
 * Created on 6. Mai 2006, 15:33
 *
 * To change this template, choose Tools | Template Manager
 * and open the template in the editor.
 */

package com.db4o.rms.log;

import javax.microedition.rms.RecordStore;
// The logging interface.

public interface Logger {
    void logEnd( RecordStore rs );
    void logException( String name, Throwable e );
    void logException( RecordStore rs, Throwable e );
    void logRecord( RecordStore rs, int id,byte[] data, int size );
    void logStart( RecordStore rs );
}
