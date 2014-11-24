package com.db4odoc.reflection;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.reflect.ReflectArray;
import com.db4o.reflect.ReflectClass;
import com.db4o.reflect.Reflector;
import com.db4o.reflect.ReflectorConfiguration;
import com.db4o.reflect.jdk.JdkReflector;


public class ReflectorExamples {
    public static void main(String[] args) {
        useLoggerReflector();
    }

    private static void useLoggerReflector() {
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().reflectWith(new LoggerReflector());
        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {

        } finally {
            container.close();
        }
    }
}

// #example: Logging reflector
class LoggerReflector implements Reflector{
    private final Reflector readReflector;

    public LoggerReflector() {
        this(new JdkReflector(Thread.currentThread().getContextClassLoader()));
    }

    public LoggerReflector(Reflector readReflector) {
        this.readReflector = readReflector;
    }

    @Override
    public void configuration(ReflectorConfiguration reflectorConfiguration) {
        readReflector.configuration(reflectorConfiguration);
    }

    @Override
    public ReflectArray array() {
        return readReflector.array();
    }

    @Override
    public ReflectClass forClass(Class aClass) {
        System.out.println("Reflector.forClass("+aClass+")");
        return readReflector.forClass(aClass);
    }

    @Override
    public ReflectClass forName(String className) {
        System.out.println("Reflector.forName("+className+")");
        return readReflector.forName(className);
    }

    @Override
    public ReflectClass forObject(Object o) {
        System.out.println("Reflector.forObject("+o+")");
        return readReflector.forObject(o);
    }

    @Override
    public boolean isCollection(ReflectClass reflectClass) {
        return readReflector.isCollection(reflectClass);
    }

    @Override
    public void setParent(Reflector reflector) {
        readReflector.setParent(reflector);
    }

    @Override
    public Object deepClone(Object o) {
        return new LoggerReflector((Reflector) readReflector.deepClone(o));
    }
}
// #end example
