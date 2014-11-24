package com.db4odoc.xml;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.thoughtworks.xstream.XStream;

import java.io.*;

public class XMLSerialisationExamples {

    public static void main(String[] args) throws Exception {
        new File("database.db4o").delete();
        fillData();
        writeToXML();
        readFromXML();
    }

    private static void writeToXML() throws Exception {
        // #example: Serialize to XML
        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            Pilot[] pilots = container.query(Pilot.class).toArray(new Pilot[0]);
            XStream xstream = new XStream();
            OutputStream stream = new FileOutputStream("pilots.xml");
            try {
                xstream.toXML(pilots, stream);
            } finally {
                stream.close();
            }
        } finally {
            container.close();
        }
        // #end example
    }

    private static void readFromXML() throws Exception {
        // #example: Read objects from XML
        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            XStream xstream = new XStream();
            InputStream stream = new FileInputStream("pilots.xml");
            try {
                Pilot[] pilots = (Pilot[]) xstream.fromXML(stream);
                for (Pilot pilot : pilots) {
                    container.store(pilot);
                }
            } finally {
                stream.close();
            }
        } finally {
            container.close();
        }
        // #end example
    }

    private static void fillData() {
        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            container.store(new Pilot("Joe", 42));
            container.store(new Pilot("Joanna", 24));
        } finally {
            container.close();
        }
    }

    public static class Pilot {
        private String name;
        private int point;

        public Pilot() {
        }

        public Pilot(String name, int point) {
            this.name = name;
            this.point = point;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public int getPoint() {
            return point;
        }

        public void setPoint(int point) {
            this.point = point;
        }
    }
}
