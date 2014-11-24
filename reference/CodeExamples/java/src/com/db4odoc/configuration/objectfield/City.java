package com.db4odoc.configuration.objectfield;

import com.db4o.config.annotations.Indexed;


public class City {
    // #example: Index a field
    @Indexed
    private String zipCode;
    // #end example
    private String name;

    public City(String zipCode) {
        this.zipCode = zipCode;
    }

    public String getZipCode() {
        return zipCode;
    }
}
