/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.binding.test;

import org.eclipse.ve.sweet.validator.IValidator;
import org.eclipse.ve.sweet.validators.reusable.RegularExpressionValidator;


public class Person {
    String name;
    int age;
    
    public IValidator getAgeVerifier() {
        return new RegularExpressionValidator("^[0-9]*$", "^[0-9]{1,3}$", 
                "Please enter an age between 0 and 999");
    }
}

