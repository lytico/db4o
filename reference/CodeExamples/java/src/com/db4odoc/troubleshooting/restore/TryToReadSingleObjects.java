package com.db4odoc.troubleshooting.restore;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;

public class TryToReadSingleObjects {

    private static final String DATABASE_FILE_NAME = "database.db4o";

    public static void main(String[] args) {
        storeExampleObjects();
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE_NAME);
        try {
            // #example: Try to read the intact objects
            final long[] idsOfPersons = container.ext().storedClass(Person.class).getIDs();
            for (long id : idsOfPersons) {
                try{
                    final Person person = (Person)container.ext().getByID(id);
                    container.ext().activate(person,1);
                    // store the person to another database
                    System.out.println("This object is ok "+person);
                } catch (Exception e){
                    System.out.println("We couldn't read the object with the id "+id +" anymore." +
                            " It is lost");
                    e.printStackTrace();
                }
            }
            // #end example

        } finally {
            container.close();
        }
    }

    private static void storeExampleObjects() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE_NAME);
        try {
            for(int i=0;i<100;i++){
                container.store(new Person("Fun"+i));
            }
        } finally {
            container.close();
        }
    }
}
