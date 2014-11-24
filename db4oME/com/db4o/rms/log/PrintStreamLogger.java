/*
 * PrintStreamLogger.java
 *
 * Created on 6. Mai 2006, 15:31
 *
 * To change this template, choose Tools | Template Manager
 * and open the template in the editor.
 */

package com.db4o.rms.log;

import java.io.PrintStream;
import javax.microedition.rms.RecordStore;
import javax.microedition.rms.RecordStoreException;
// A logger that outputs to a PrintStream.
    
    public  class PrintStreamLogger implements Logger {
        public static final int COLS_MIN = 10;
        public static final int COLS_DEFAULT = 20;
        
        private int          cols;
        private int          numBytes;
        private StringBuffer hBuf;
        private StringBuffer cBuf;
        private StringBuffer pBuf;
        private PrintStream  out;
        
        public PrintStreamLogger( PrintStream out ){
            this( out, COLS_DEFAULT );
        }
        
        public PrintStreamLogger( PrintStream out, int cols ){
            this.out = out;
            this.cols = ( cols > COLS_MIN ? cols : COLS_MIN );
        }
        
        private char convertChar( char ch ){
            if( ch < 0x20 ) return '.';
            return ch;
        }
        
        public void logEnd( RecordStore rs ){
            out.println( "\nActual size of records = "+ numBytes );
            printChar( '-', cols * 4 + 1 );
            
            hBuf = null;
            cBuf = null;
            pBuf = null;
        }
        
        public void logException( String name, Throwable e ){
            out.println( "Exception while analyzing " +
                    name + ": " + e );
        }
        
        public void logException( RecordStore rs, Throwable e ){
            String name;
            
            try {
                name = rs.getName();
            } catch( RecordStoreException rse ){
                name = "";
            }
            
            logException( name, e );
        }
        
        public void logRecord( RecordStore rs, int id, byte[] data, int len ){
            if( len < 0 && data != null ){
                len = data.length;
            }
            
            hBuf.setLength( 0 );
            cBuf.setLength( 0 );
            
            numBytes += len;
            
            out.println( "Record #" + id + " of length "+ len + " bytes" );
            
            for( int i = 0; i < len; ++i ){
                int    b = Math.abs( data[i] );
                String hStr = Integer.toHexString( b );
                
                if( b < 0x10 ){
                    hBuf.append( '0');
                }
                
                hBuf.append( hStr );
                hBuf.append( ' ' );
                
                cBuf.append( convertChar( (char) b ) );
                
                if( cBuf.length() == cols ){
                    out.println( hBuf + " " + cBuf );
                    
                    hBuf.setLength( 0 );
                    cBuf.setLength( 0 );
                }
            }
            
            len = cBuf.length();
            
            if( len > 0 ){
                while( len++ < cols ){
                    hBuf.append( "   " );
                    cBuf.append( ' ' );
                }
                
                out.println( hBuf + " " + cBuf );
            }
        }
        
        public void logStart( RecordStore rs ){
            hBuf = new StringBuffer( cols * 3 );
            cBuf = new StringBuffer( cols );
            pBuf = new StringBuffer();
            
            printChar( '=', cols * 4 + 1 );
            
            numBytes = 0;
            
            try {
                out.println( "Record store: "+ rs.getName() );
                out.println( "    Number of records = "+ rs.getNumRecords() );
                out.println( "    Total size = "+ rs.getSize() );
                out.println( "    Version = "+ rs.getVersion() );
                out.println( "    Last modified = "+ rs.getLastModified() );
                out.println( "    Size available = "+ rs.getSizeAvailable() );
                out.println( "" );
            } catch( RecordStoreException e ){
                logException( rs, e );
            }
        }
        
        private void printChar( char ch, int num ){
            pBuf.setLength( 0 );
            while( num-- > 0 ){
                pBuf.append( ch );
            }
            out.println( pBuf.toString() );
        }
    }
