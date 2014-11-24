/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.sample;

import com.db4o.activation.*;
import com.db4o.db4ounit.common.ta.*;


public class Country extends ActivatableImpl {
    
    public State[] _states;
    
    public State getState(String zipCode){
        activate(ActivationPurpose.READ);
        return _states[0];
    }
}
