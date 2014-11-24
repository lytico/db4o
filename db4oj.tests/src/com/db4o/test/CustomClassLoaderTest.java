/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.io.*;
import java.net.*;

/**
 * 
 */
/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class CustomClassLoaderTest {

    public static void main(String[] args) throws Exception{
        
            URL url = new File("C:/Zystem/D/db4o30j/src").toURL();
            
            ClassLoader loader = new CustomClassLoader(new URL[] {url}, null);

            Thread.currentThread().setContextClassLoader(loader);

            loader.loadClass("com.db4o.test.CustomClassLoaderHelper").newInstance();
        
    }
    
    
    
        
        

    
    
}
