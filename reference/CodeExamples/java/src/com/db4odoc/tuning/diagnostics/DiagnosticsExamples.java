package com.db4odoc.tuning.diagnostics;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.diagnostic.Diagnostic;
import com.db4o.diagnostic.DiagnosticListener;
import com.db4o.diagnostic.DiagnosticToConsole;
import com.db4o.diagnostic.LoadedFromClassIndex;
import com.db4o.query.Predicate;

import java.io.File;
import java.util.Arrays;
import java.util.HashSet;
import java.util.Set;


public class DiagnosticsExamples {
    private static final String DATABASE_FILE_NAME = "database.db4o";

    public static void main(String[] args) {
        filterDiagnosticMessages();
    }

    private static void filterDiagnosticMessages() {
        cleanUp();
        ObjectContainer container = openDatabase();
        container.store(new SimpleClass());
        ObjectSet<SimpleClass> data = runQuery(container);
        printData(data);
        container.close();
        cleanUp();
    }

    private static void printData(ObjectSet<SimpleClass> data) {
        for (SimpleClass item : data) {
            System.out.println(item);
        }
    }

    private static ObjectSet<SimpleClass> runQuery(ObjectContainer container) {
        return container.query(new Predicate<SimpleClass>() {
            @Override
            public boolean match(SimpleClass o) {
                return o.getNumber() < 100;
            }
        });
    }

    private static void cleanUp() {
        new File(DATABASE_FILE_NAME).delete();
    }

    private static ObjectContainer openDatabase() {
        // #example: Filter for unindexed fields
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().diagnostic()
                .addListener(new DiagnosticFilter(new DiagnosticToConsole(), LoadedFromClassIndex.class));
        // #end example
        return Db4oEmbedded.openFile(configuration, DATABASE_FILE_NAME);
    }

    // #example: A simple message filter
    private static class DiagnosticFilter implements DiagnosticListener{
        private final Set<Class> filterFor;
        private final DiagnosticListener delegate;

        private DiagnosticFilter(DiagnosticListener delegate,Class<? extends Diagnostic>...filterFor) {
            this.delegate = delegate;
            this.filterFor = new HashSet<Class>(Arrays.asList(filterFor));
        }

        public void onDiagnostic(Diagnostic diagnostic) {
            Class<?> type = diagnostic.getClass();
            if(filterFor.contains(type)){
                delegate.onDiagnostic(diagnostic);
            }
        }
    }
    // #end example

    private static class SimpleClass {
        private int number = 0;

        public int getNumber() {
            return number;
        }

        public void setNumber(int number) {
            this.number = number;
        }
    }
}
