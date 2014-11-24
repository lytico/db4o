/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.enhance;

import java.io.*;
import java.net.*;

import com.db4o.instrumentation.classfilter.*;
import com.db4o.instrumentation.core.*;
import com.db4o.instrumentation.main.*;
import com.db4o.nativequery.main.*;
import com.db4o.ta.instrumentation.*;



/**
 * Launcher to start applications with 
 */
public class Db4oEnhancedLauncher {
    
    /**
     * launches an application that is to be instrumented for db4o on-the-fly
     * at class loading time.
     * @param path path to the classes (typically the bin directory)
     * @param mainClassName the name of the main class that is to be started. 
     */
    public void launch(String path, String mainClassName) throws Exception{
        URL[] urls = { new File(path).toURL() };
        launch(urls, mainClassName);
    }
    
    /**
     * launches an application that is to be instrumented for db4o on-the-fly
     * at class loading time.
     * @param classPath Array of Classpath URLs.
     * @param mainClassName the name of the main class that is to be started.
     */
    public void launch(URL[] classPath, String mainClassName) throws Exception{
        BloatClassEdit[] edits = { 
            new TranslateNQToSODAEdit() , 
            new InjectTransparentActivationEdit(new AcceptAllClassesFilter()) };
        Db4oInstrumentationLauncher.launch(edits, classPath, mainClassName, new String[]{});
    }

}
