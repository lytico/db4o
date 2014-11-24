package com.db4odoc.reporting;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.query.Predicate;
import net.sf.jasperreports.engine.*;
import net.sf.jasperreports.engine.data.JRBeanCollectionDataSource;

import java.util.HashMap;


public class JasperReportsExample {

    private static final String DATABASE_FILE_NAME = "database.db4o";

    public static void main(String[] args) throws Exception {
        storeExampleObjects();

        final JasperReport report = JasperCompileManager.compileReport(
                JasperReportsExample.class.getResourceAsStream("report.jrxml"));

        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE_NAME);
        try {
            // #example: Run a query an export the result as report
            final ObjectSet<Person> queryResult = container.query(new Predicate<Person>() {
                @Override
                public boolean match(Person p) {
                    return p.getSirname().contains("a");
                }
            });
            final JRBeanCollectionDataSource dataSource = new JRBeanCollectionDataSource(queryResult);
            final JasperPrint jasperPrint = JasperFillManager.fillReport(report, new HashMap(), dataSource);
            JasperExportManager.exportReportToPdfFile(jasperPrint, "the-report.pdf");
            // #end example
        } finally {
            container.close();
        }
    }

    private static void storeExampleObjects() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE_NAME);
        try {
            container.store(new Person("Johanson", "John", new Address("Cool Street", "Zuerich")));
            container.store(new Person("Park", "In-jeong", new Address("Cool Street", "Somewhere Cool")));
            container.store(new Person("Miller", "Frank", new Address("A Place", "Town")));
        } finally {
            container.close();
        }
    }
}
