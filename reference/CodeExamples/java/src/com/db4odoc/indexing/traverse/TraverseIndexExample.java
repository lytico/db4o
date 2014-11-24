package com.db4odoc.indexing.traverse;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ext.StoredField;
import com.db4o.foundation.Visitor4;

public class TraverseIndexExample {
    public static void main(String[] args) {
        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            storeExampleObjects(container);

            traverseIndex(container);
        }finally {
            container.close();
        }
    }

    private static void traverseIndex(ObjectContainer container) {
        // #example: Traverse index
        final StoredField storedField = container.ext()
                .storedClass(Item.class).storedField("data", int.class);
        storedField.traverseValues(new Visitor4<Integer>() {
            @Override
            public void visit(Integer fieldValue) {
                System.out.println("Value "+fieldValue);
            }
        });
        // #end example
    }

    private static void storeExampleObjects(ObjectContainer container) {
        for(int i=0;i<100;i++){
            container.store(new Item(i));
        }
    }
}
