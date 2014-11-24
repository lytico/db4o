package com.db4odoc.tutorial;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ext.StoredClass;
import com.db4o.reflect.ReflectClass;

/**
 * @author roman.stoffel@gamlor.info
 * @since 29.03.11
 */
public class TestMe {
    public static void main(String[] args) {
        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            StoredClass[] storedClasses = container.ext().storedClasses();
            ReflectClass[] knownClasses = container.ext().knownClasses();

            System.out.println();
        } finally {
            container.close();
        }
    }

    static class Person{
        private String name;
        private int age;

        Person(String name) {
            this.name = name;
        }
    }
}
