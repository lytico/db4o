package com.db4odoc.reporting;


public class Person {
    private String sirname;
    private String firstname;
    private Address address;

    public Person(String sirname, String firstname, Address address) {
        this.sirname = sirname;
        this.firstname = firstname;
        this.address = address;
    }

    public String getSirname() {
        return sirname;
    }

    public String getFirstname() {
        return firstname;
    }

    public Address getAddress() {
        return address;
    }
}
