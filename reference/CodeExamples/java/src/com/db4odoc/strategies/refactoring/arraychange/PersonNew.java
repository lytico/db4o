package com.db4odoc.strategies.refactoring.arraychange;

import java.util.Arrays;


class PersonNew {
    private String[] name = new String[0];

    PersonNew(String...names) {
        this.name = names;
    }

    public String[] getName() {
        return name;
    }

    public void setName(String...name) {
        this.name = name;
    }

    @Override
    public String toString() {
        return "PersonNew{" +
                "name=" + (name == null ? null : Arrays.asList(name)) +
                '}';
    }
}
