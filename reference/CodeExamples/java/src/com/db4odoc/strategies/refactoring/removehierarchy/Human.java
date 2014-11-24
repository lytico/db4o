package com.db4odoc.strategies.refactoring.removehierarchy;


class Human extends Primate{
    private String name;


    public Human(String name) {
        this.name = name;
        setBodyTemperature(36);
        setIq(120);
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    @Override
    public String toString() {
        return "Human{" +
                "name='" + name + '\'' +
                "} is a "+super.toString();
    }
}