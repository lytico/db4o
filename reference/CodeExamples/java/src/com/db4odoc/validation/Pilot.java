package com.db4odoc.validation;


import org.hibernate.validator.constraints.NotBlank;

// #example: Validation attributes
class Pilot {
    private String name;

    Pilot(String name) {
        this.name = name;
    }

    @NotBlank
    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
}
// #end example
