package com.db4odoc.strategies.refactoring;

import java.util.Random;


class Person {
    // #example: change type of field
    public Identity id = Identity.newId();
//  was an int previously:
//    public int id = new Random().nextInt();
    // #end example

    private String name;

    Person(String name) {
        this.name = name;
    }

    Person(){
        this("Joe");
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    @Override
    public String toString() {
        return "Person{" +
                "id=" + id +
                ", name='" + name + '\'' +
                '}';
    }
}

class Identity{
    private final int id;

    Identity(int id) {
        this.id = id;
    }

    static Identity newId() {
        return new Identity(new Random().nextInt());
    }

    @Override
    public String toString() {
        return String.valueOf(id);
    }
}
