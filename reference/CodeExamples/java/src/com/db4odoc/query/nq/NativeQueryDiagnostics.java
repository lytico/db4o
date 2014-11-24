package com.db4odoc.query.nq;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.diagnostic.Diagnostic;
import com.db4o.diagnostic.DiagnosticListener;
import com.db4o.diagnostic.NativeQueryNotOptimized;
import com.db4o.diagnostic.NativeQueryOptimizerNotLoaded;
import com.db4o.query.Predicate;

import java.util.Locale;


public class NativeQueryDiagnostics {
    private static final String DATABASE_FILE = "database.db4o";
    
    public static void main(String[] args) {

        final EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // #example: Use diagnostics to find unoptimized queries
        configuration.common().diagnostic().addListener(new DiagnosticListener() {
            @Override
            public void onDiagnostic(Diagnostic diagnostic) {
                if(diagnostic instanceof NativeQueryNotOptimized){
                    System.out.println("Query not optimized"+diagnostic);
                } else if(diagnostic instanceof NativeQueryOptimizerNotLoaded){
                    System.out.println("Missing native query optimisation jars in classpath "+diagnostic);
                }
            }
        });
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration,DATABASE_FILE);
        try {
            storeData(container);

            optimizedNativeQuery(container);
            notOptimizedNativeQuery(container);
        } finally {
            container.close();
        }
    }

    private static void optimizedNativeQuery(ObjectContainer container) {
        final ObjectSet<Pilot> result = container.query(new Predicate<Pilot>() {
            @Override
            public boolean match(Pilot pilot) {
                return pilot.getName().startsWith("J");
            }
        });
        listResult(result);
    }
    private static void notOptimizedNativeQuery(ObjectContainer container) {
        final ObjectSet<Pilot> result = container.query(new Predicate<Pilot>() {
            @Override
            public boolean match(Pilot pilot) {
                return pilot.getName().toLowerCase(Locale.FRANCE).startsWith("J");
            }
        });
        listResult(result);
    }

    private static void listResult(ObjectSet result) {
        for (Object object : result) {
            System.out.println(object);
        }
    }


    private static void storeData(ObjectContainer container) {
        Pilot john = new Pilot("John",42);
        Pilot joanna = new Pilot("Joanna",45);
        Pilot jenny = new Pilot("Jenny",21);
        Pilot rick = new Pilot("Rick",33);
        Pilot juliette = new Pilot("Juliette",33);

        container.store(new Car(john,"Ferrari"));
        container.store(new Car(joanna,"Mercedes"));
        container.store(new Car(jenny,"Volvo"));
        container.store(new Car(rick,"Fiat"));
        container.store(new Car(juliette,"Suzuki"));

    }
}
