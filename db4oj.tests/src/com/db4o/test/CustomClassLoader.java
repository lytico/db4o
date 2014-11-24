/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.net.*;

/**
 * 
 */
/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class CustomClassLoader extends URLClassLoader{
    
    CustomClassLoader(URL[] urls, ClassLoader parent) {
        super(urls, parent);
    }
    
    public Class loadClass(String name) throws ClassNotFoundException {
        System.out.println(name);
        return super.loadClass(name);
    }
    
    protected synchronized Class loadClass(String name, boolean resolve)
        throws ClassNotFoundException {
        // System.out.println(name);
        return super.loadClass(name, resolve);
    }
    
    protected Class findClass(String name) throws ClassNotFoundException {
        // System.out.println(name);
        return super.findClass(name);
    }
}
