package com.db4odoc.strategies.refactoring;


class PersonOld {
    private String name = "Joe";

    public PersonOld() {
    }

    public PersonOld(String name) {
        this.name = name;
    }


    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
}
