/* Copyright (C) 2006  db4objects Inc.  http://www.db4o.com */

package org.polepos.drs;

import org.polepos.framework.Car;

import com.db4o.drs.test.*;


/**
 * a combination of two replication providers !
 */
public abstract class DrsCar extends Car {
    
    private DrsFixture _a;
    
    private DrsFixture _b;

    public DrsCar(DrsFixture a, DrsFixture b) {
        _a = a;
        _b = b;
    }
    
    DrsFixture fixtureA(){
        return _a;
    }
    
    DrsFixture fixtureB(){
        return _b;
    }

    public void clean() {
        _a.clean();
        _b.clean();
    }

}
