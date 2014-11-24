/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.enhance;

import java.io.*;

import com.db4o.instrumentation.classfilter.*;
import com.db4o.instrumentation.core.*;
import com.db4o.instrumentation.main.*;
import com.db4o.nativequery.main.*;
import com.db4o.ta.instrumentation.*;


/**
 * Programmatic user interface to the db4o enhancer.
 */
public class Db4oFileEnhancer {
    
    /**
     * enhances a set of class files for db4o. The enhancer applies optimizations for
     * Native Queries and for Transparent Activation to all files.  
     * @param sourceDir the source directory of the class files that are to be enhanced.
     * @param targetDir the target directory where the enhanced files are to be places
     */
    public void enhance(String sourceDir, String targetDir) throws Exception{
    	enhance(new File(sourceDir), new File(targetDir));
    }

    /**
     * enhances a set of class files for db4o. The enhancer applies optimizations for
     * Native Queries and for Transparent Activation to all files.  
     * @param sourceDir the source directory of the class files that are to be enhanced.
     * @param targetDir the target directory where the enhanced files are to be places
     */
    public void enhance(File sourceDir, File targetDir) throws Exception{
        Db4oFileInstrumentor instrument = new Db4oFileInstrumentor(new BloatClassEdit[]{
            new TranslateNQToSODAEdit(),
            new InjectTransparentActivationEdit(new AcceptAllClassesFilter()),
        });
        instrument.enhance(sourceDir, targetDir, new String[]{});
    }

}
