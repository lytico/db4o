

package com.db4o.rms.log;
import java.io.*;
import javax.microedition.rms.*;

// Analyzes the contents of a record store.
// By default prints the analysis to System.out,
// but you can change this by implementing your
// own Logger.


public class RMSAnalyzer {
    
    
    private Logger logger;
    
    
    // Constructs an analyzer that logs to System.out.
    
    public RMSAnalyzer(){
        this( null );
    }
    
    // Constructs an analyzer that logs to the given logger.
    
    public RMSAnalyzer( Logger logger ){
        this.logger = ( logger != null ) ? logger :new SystemLogger();
    }
    
    // Open the record stores owned by this MIDlet suite
    // and analyze their contents.
    
    public void analyzeAll(){
        String[] names = RecordStore.listRecordStores();
        
        for( int i = 0;   names != null && i < names.length;   ++i ){
            analyze( names[i] );
        }
    }
    
    // Open a record store by name and analyze its contents.
    
    public void analyze( String rsName ){
        RecordStore rs = null;
        
        try {
            rs = RecordStore.openRecordStore( rsName, false );
            analyze( rs );
        } catch( RecordStoreException e ){
            logger.logException( rsName, e );
        } finally {
            try {
                rs.closeRecordStore();
            } catch( RecordStoreException e ){
                // Ignore this exception
            }
        }
    }
    
    // Analyze the contents of an open record store using
    // a simple brute force search through the record store.
    
    public synchronized void analyze( RecordStore rs ){
        try {
            logger.logStart( rs );
            
            int    lastID = rs.getNextRecordID();
            int    numRecords = rs.getNumRecords();
            int    count = 0;
            byte[] data = null;
            
            for( int id = 0;id < lastID && count < numRecords;++id ){
                try {
                    int size = rs.getRecordSize( id );
                    
                    // Make sure data array is big enough, plus add some for growth
                    
                    if( data == null || data.length < size ){
                        data = new byte[ size + 20 ];
                    }
                    
                    rs.getRecord( id, data, 0 );
                    logger.logRecord( rs, id, data, size );
                    
                    ++count; // only increase if record exists
                } catch( InvalidRecordIDException e ){
                    // just ignore and move to the next one
                } catch( RecordStoreException e ){
                    logger.logException( rs, e );
                }
            }
            
        } catch( RecordStoreException e ){
            logger.logException( rs, e );
        } finally {
            logger.logEnd( rs );
        }
    }
    
      
}