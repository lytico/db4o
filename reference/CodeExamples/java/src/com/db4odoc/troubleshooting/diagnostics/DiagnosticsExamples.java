package com.db4odoc.troubleshooting.diagnostics;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.config.encoding.StringEncodings;

public class DiagnosticsExamples {
    public static void main(String[] args) {
        final EmbeddedConfiguration cfg = Db4oEmbedded.newConfiguration();
        cfg.common().messageLevel(4);
        cfg.common().outStream(System.out);
        cfg.common().stringEncoding(StringEncodings.utf8());
        ObjectContainer container = Db4oEmbedded.openFile(cfg,"database.db4o");
        try {
            container.store(new FunMe());
            final ObjectSet<FunMe> query = container.query(FunMe.class);
            for (FunMe funMe : query) {
                System.out.println(funMe);
            }
        } finally {
            container.close();
        }
    }

    static class FunMe{
        public String toString(){
            return "Hi Roman, I'm a string";
        }
    }
}
