package com.db4odoc.typehandling.typehandler;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.typehandlers.SingleClassTypeHandlerPredicate;


public class TypeHandlerExample {
    public static void main(String[] args) {

        ObjectContainer container = openContainer();
        try {
            MyType testType = new MyType();
            container.store(testType);
        } finally {
            container.close();
        }

        container = openContainer();
        try {
            MyType builder = container.query(MyType.class).get(0);
            System.out.println(builder);
        } finally {
            container.close();
        }
    }

    private static ObjectContainer openContainer() {
        // #example: Register type handler
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().registerTypeHandler(
                new SingleClassTypeHandlerPredicate(StringBuilder.class), new StringBuilderHandler());
        // #end example
        return Db4oEmbedded.openFile(configuration, "database.db4o");
    }

    static class MyType{
        StringBuilder builder = new StringBuilder("TestData");

        public String toString(){
            return builder.toString();
        }
    }
}
