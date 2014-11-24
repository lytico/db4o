package com.db4odoc.tutorial.activation;


// #example: Domain model for people
public class Person {
    private String name;
    private Person mother;

    public Person(String name, Person mother) {
        this.name = name;
        this.mother = mother;
    }

    public String getName() {
        return name;
    }

    public Person getMother() {
        return mother;
    }

    @Override
    public String toString() {
        return "Person{" +
                "name='" + name +
                "'}";
    }
}
// #end example
