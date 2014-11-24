package com.db4odoc.practises.relations;


import java.util.Collection;
import java.util.Collections;
import java.util.HashSet;
import java.util.Set;

class Person {
    private String sirname;
    private String firstname;


    public Person(String sirname, String firstname, Country bornIn) {
        this.sirname = sirname;
        this.firstname = firstname;
        this.bornIn = bornIn;
    }


    // #example: Simple 1-n relation. Navigating from the person to the countries
    // Optionally we can index this field, when we want to find all people for a certain country
    private Country bornIn;

    public Country getBornIn() {
        return bornIn;
    }
    // #end example

    // #example: One directional N-N relation
    private Set<Country> citizenIn = new HashSet<Country>();

    public void removeCitizenship(Country o) {
        citizenIn.remove(o);
    }

    public void addCitizenship(Country country) {
        citizenIn.add(country);
    }
    // #end example

    // #example: Bidirectional 1-N relations. The person has dogs
    private Set<Dog> owns = new HashSet<Dog>();

    // The add and remove method ensure that the relations is always consistent
    public void addOwnerShipOf(Dog dog) {
        owns.add(dog);
        dog.setOwner(this);
    }

    public void removeOwnerShipOf(Dog dog) {
        owns.remove(dog);
        dog.setOwner(null);
    }

    public Collection<Dog> ownedDogs() {
        return Collections.unmodifiableCollection(owns);
    }
    // #end example

    // #example: Bidirectional N-N relation
    private Set<Club> memberIn = new HashSet<Club>();

    public void join(Club club) {
        if (!memberIn.contains(club)) {
            memberIn.add(club);
            club.addMember(this);
        }
    }

    public void leave(Club club) {
        if (memberIn.contains(club)) {
            memberIn.remove(club);
            club.removeMember(this);
        }
    }

    public Collection<Club> memberOf() {
        return Collections.unmodifiableCollection(memberIn);
    }
    // #end example


}
