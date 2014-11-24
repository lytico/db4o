package com.db4odoc.identiy;


class Pilot {
    private String name;

    public Pilot(String name) {
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
        return "Pilot{" +
                "name='" + name + '\''+
                '}';
    }
}
