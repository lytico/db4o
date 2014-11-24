package com.db4odoc.configuration.common;

import com.db4o.reflect.jdk.JdkLoader;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.List;

// #example: Complex class loader scenario
public class ClassLoaderLookup implements JdkLoader {
    private final List<ClassLoader> classLoaders;


    ClassLoaderLookup(ClassLoader...classLoaders) {
        this.classLoaders = Arrays.asList(classLoaders);
    }

    ClassLoaderLookup(Collection<ClassLoader> classLoaders) {
        this.classLoaders = new ArrayList<ClassLoader>(classLoaders);
    }

    @Override
    public Class loadClass(String className) {
        for (ClassLoader loader : classLoaders) {
            Class<?> theClass = null;
            try {
                theClass = loader.loadClass(className);
                return theClass;
            } catch (ClassNotFoundException e) {
                // first check the other loaders
            }
        }
        throw new RuntimeException(new ClassNotFoundException(className));
    }

    @Override
    public Object deepClone(Object o) {
        return new ClassLoaderLookup(classLoaders);
    }
}
// #end example
