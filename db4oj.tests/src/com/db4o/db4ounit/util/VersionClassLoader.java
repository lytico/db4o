/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.util;

import java.io.*;
import java.net.*;
import java.util.*;

/**
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class VersionClassLoader extends URLClassLoader {

    private Map cache = new HashMap();

    private String[] prefixes;

    public VersionClassLoader(URL[] urls, String[] prefixes) {
        super(urls);
        this.prefixes = prefixes;
    }

    protected synchronized Class loadClass(String name, boolean resolve)
            throws ClassNotFoundException {
        
        if (cache.containsKey(name)) {
            return (Class) cache.get(name);
        }

        if (!knownPrefix(name)) {
            return super.loadClass(name, resolve);
        }

        String resourceName = name.replace('.', '/') + ".class";
        URL resourceURL = findResource(resourceName);
        if (resourceURL == null) {
            System.out.println("Warning: Cannot find resource " + resourceName);
            return super.loadClass(name, resolve);
        }

        byte[] byteCode = null;
        try {
            byteCode = readBytes(resourceURL);
        } catch (IOException e) {
            e.printStackTrace();
            super.loadClass(name, resolve);
        }
        
        Class clazz = defineClass(name, byteCode, 0, byteCode.length);
        if (resolve) {
            resolveClass(clazz);
        }
        cache.put(name, clazz);
        return clazz;
    }
    
    private byte[] readBytes(URL resURL) throws IOException {
        InputStream in = resURL.openStream();
        ByteArrayOutputStream out = new ByteArrayOutputStream();
        byte[] buf = new byte[1024];
        int bytesRead = 0;
        while ((bytesRead = in.read(buf)) >= 0) {
            out.write(buf, 0, bytesRead);
        }
        in.close();
        out.close();
        byte[] full = out.toByteArray();
        return full;
    }

    private boolean knownPrefix(String className) {
        for (int idx = 0; idx < prefixes.length; idx++) {
            if (className.startsWith(prefixes[idx])) {
                return true;
            }
        }
        return false;
    }

}
