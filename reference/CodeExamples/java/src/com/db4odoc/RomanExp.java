package com.db4odoc;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;

/**
 * @author roman.stoffel@gamlor.info
 * @since 17.02.11
 */
public class RomanExp {
    public static void main(String[] args) {
        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            Boho obj = new Boho();
            long id = container.ext().getID(obj);
            System.out.println(id);
        } finally {
            container.close();
        }
    }
    static class  Boho{

    }
}
