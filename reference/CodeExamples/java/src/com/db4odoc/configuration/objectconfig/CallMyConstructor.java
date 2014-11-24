package com.db4odoc.configuration.objectconfig;

import com.db4o.config.annotations.CalledConstructor;
import com.db4o.config.annotations.Indexed;

@CalledConstructor
public class CallMyConstructor {
    @Indexed
    private String fieldFun;
    public CallMyConstructor() {
        System.out.println("Constructor called 2");
    }
}
