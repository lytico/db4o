package com.db4odoc.validation;


import org.hibernate.validator.constraints.NotBlank;

import javax.validation.constraints.NotNull;


class Car {
    private String name;
    private Pilot drivenBy;

    Car(String name, Pilot drivenBy) {
        this.name = name;
        this.drivenBy = drivenBy;
    }

    @NotBlank
    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    @NotNull
    public Pilot getDrivenBy() {
        return drivenBy;
    }

    public void setDrivenBy(Pilot drivenBy) {
        this.drivenBy = drivenBy;
    }
}
