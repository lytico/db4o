package com.db4odoc.strategies.refactoring.arraychange;

class PersonOld {
    private String name = "Roman";

    public PersonOld(String name) {
        this.name = name;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    @Override
    public String toString() {
        return "PersonOld{" +
                "name='" + name + '\'' +
                '}';
    }
}
