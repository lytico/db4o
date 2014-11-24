package com.db4odoc.tutorial.demos.query;

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
        return asList(new ExampleSnippet("Query Example",
                "            List<Car> cars = container.query(new Predicate<Car>() {\n" +
                        "                @Override\n" +
                        "                public boolean match(Car car) {\n" +
                        "                    return car.getCarName().equals(\"VM Golf\");\n" +
                        "                }\n" +
                        "            });\n" +
                        "            System.out.println(\"Driver named Joe\");\n" +
                        "            for (Car car : cars) {\n" +
                        "                System.out.println(car);\n" +
                        "            }"));
    }


}
