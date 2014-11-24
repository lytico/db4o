package com.db4odoc.typehandling.blob;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;

import java.io.File;
import java.io.IOException;


public class BlobExamples {
    public static void main(String[] args) throws IOException {
        storeBlob();
        readBlob();
    }

    private static void readBlob() throws IOException {
        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            BlobStorage blob = container.query(BlobStorage.class).get(0);
            File file = blob.readFromDBIntoFile();
            
        } finally {
            container.close();
        }
    }

    private static void storeBlob() throws IOException {
        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            BlobStorage blob = new BlobStorage();
            container.store(blob);
            blob.readFileIntoDB(new File("C:\\Pictures\\IMG_1.jpg"));
        } finally {
            container.close();
        }
    }
}
