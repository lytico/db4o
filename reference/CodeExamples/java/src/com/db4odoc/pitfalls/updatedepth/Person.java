package com.db4odoc.pitfalls.updatedepth;

import java.util.ArrayList;
import java.util.List;


class Person {
    private List<Person> friends = new ArrayList<Person>();

    private String name;

    Person(String name) {
        this.name = name;
    }

    public String getName() {
        return name;
    }

    public List<Person> getFriends() {
        return friends;
    }

    public boolean add(Person person) {
        return friends.add(person);
    }

    public void setName(String name) {
        this.name = name;
    }
}
