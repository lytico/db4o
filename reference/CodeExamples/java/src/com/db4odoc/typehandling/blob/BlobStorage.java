package com.db4odoc.typehandling.blob;

import com.db4o.ext.Status;
import com.db4o.types.Blob;

import java.io.File;
import java.io.IOException;


public class BlobStorage {
    private Blob blob;

    public BlobStorage() {
    }

    public void readFileIntoDB(File fileToStore) throws java.io.IOException {
        // #example: Store the file as a db4o-blob
        blob.readFrom(fileToStore);
        // #end example
        waitTillDBIsFinished();
    }

    public File readFromDBIntoFile() throws java.io.IOException {
        File file = createTemporaryFile();
        // #example: Load a blob from a db4o-blob
        blob.writeTo(file);
        // #end example
        waitTillDBIsFinished();
        return file;
    }

    /**
     * unfortunately there's no callback for blobs. So the only way it to poll for it
     */
    private void waitTillDBIsFinished() {
        // #example: wait until the operation is done
        while (blob.getStatus() > Status.COMPLETED){
            try {
                Thread.sleep(50);
            } catch (InterruptedException ex) {
                Thread.currentThread().interrupt();
            }
        }
        // #end example
    }

    /**
     * unfortunately the db4o-blob-type doesn't support streams. The only way is to use
     * files. Therefore we create temporary-files
     * @return
     * @throws java.io.IOException
     */
    private File createTemporaryFile() throws IOException {
        File file = File.createTempFile("temp","tmp");
        file.deleteOnExit();
        return file;
    }
}
