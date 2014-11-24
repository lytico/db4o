package com.db4odoc.tutorial.demos.firststeps;

import com.db4odoc.tutorial.demos.AbstractDemoPack;
import com.db4odoc.tutorial.runner.ExampleSnippet;

import java.util.List;

import static java.util.Arrays.asList;

/**
 * @author roman.stoffel@gamlor.info
 * @since 27.04.11
 */
public class DemoSetup extends AbstractDemoPack {
    @Override
    public List<ExampleSnippet> snippets() {
        return asList(
                new ExampleSnippet("Store Object",
                        "Driver driver = new Driver(\"Joe\");\n" +
                        "container.store(driver);\n" +
                        "System.out.println(\"Stored a driver: \"+driver);"),
                new ExampleSnippet("Query",
                        "List<Driver> drivers = container.query(new Predicate<Driver>() {\n" +
                        "    public boolean match(Driver d) {\n" +
                        "        return d.getName().equals(\"Joe\");\n" +
                        "    }\n" +
                        "});\n" +
                        "System.out.println(\"Stored Pilots:\");\n" +
                        "for (Driver driver : drivers) {\n" +
                        "    System.out.println(driver.getName());\n" +
                        "}")
        );
    }

}
