package com.db4odoc.collections.bigset;

import java.util.Set;


public class City {
    private final Set<Person> citizen;

    public City(Set<Person> citizen) {
        this.citizen = citizen;
    }

    public boolean isCitizen(Person person){
        return citizen.contains(person);
    }

    public Set<Person> citizen(){
        return citizen;
    }
    public int population(){
        return citizen.size();
    }
}

