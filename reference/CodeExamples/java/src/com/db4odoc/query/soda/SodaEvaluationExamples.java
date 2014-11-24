package com.db4odoc.query.soda;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.query.Candidate;
import com.db4o.query.Evaluation;
import com.db4o.query.Query;

import java.io.File;
import java.util.regex.Pattern;


public class SodaEvaluationExamples {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        cleanUp();
        final EmbeddedConfiguration cfg = Db4oEmbedded.newConfiguration();
        ObjectContainer container = Db4oEmbedded.openFile(cfg,DATABASE_FILE);
        try {
            storeData(container);

            simpleEvaluation(container);
            evaluationOnField(container);
            regexEvaluator(container);
        } finally {
            container.close();
        }
    }

    private static void simpleEvaluation(ObjectContainer container) {
        System.out.println("Simple evaluation");
        // #example: Simple evaluation
        final Query query = container.query();
        query.constrain(Pilot.class);
        query.constrain(new OnlyOddAge());

        ObjectSet result = query.execute();
        // #end example
        listResult(result);
    }
    private static void evaluationOnField(ObjectContainer container) {
        System.out.println("Evaluation on field");
        // #example: Evaluation on field
        final Query query = container.query();
        query.constrain(Car.class);
        query.descend("pilot").constrain(new OnlyOddAge());

        ObjectSet result = query.execute();
        // #end example
        listResult(result);
    }

    private static void regexEvaluator(ObjectContainer container) {
        System.out.println("Regex-Evaluation");
        // #example: Regex-evaluation on a field
        final Query query = container.query();
        query.constrain(Pilot.class);
        query.descend("name").constrain(new RegexConstrain("J.*nn.*"));
        ObjectSet result = query.execute();
        // #end example
        listResult(result);
    }



    private static void cleanUp() {
        new File(DATABASE_FILE).delete();
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

        container.store(new Car(john,"Ferrari"));
        container.store(new Car(joanna,"Mercedes"));
        container.store(new Car(jenny,"Volvo"));
        container.store(new Car(rick,"Fiat"));

    }
}

// #example: Simple evaluation which includes only odd aged pilots
class OnlyOddAge implements Evaluation {
    public void evaluate(Candidate candidate) {
        Pilot pilot = (Pilot) candidate.getObject();
        candidate.include(pilot.getAge()%2!=0);
    }
}
// #end example
// #example: Regex Evaluator
class RegexConstrain implements Evaluation {
    private final Pattern pattern;

    public RegexConstrain(String pattern) {
        this.pattern = Pattern.compile(pattern);
    }

    public void evaluate(Candidate candidate) {
        String stringValue = (String) candidate.getObject();
        candidate.include(pattern.matcher(stringValue).matches());
    }
}
// #end example
