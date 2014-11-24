/*
 * SystemLogger.java
 *
 * Created on 6. Mai 2006, 15:30
 *
 * To change this template, choose Tools | Template Manager
 * and open the template in the editor.
 */

package com.db4o.rms.log;

import com.db4o.test.*;
 public class SystemLogger  extends PrintStreamLogger {
        public SystemLogger(){
            super( System.out );
        }
        
        public SystemLogger( int cols ){
            super( System.out, cols );
        }
    }