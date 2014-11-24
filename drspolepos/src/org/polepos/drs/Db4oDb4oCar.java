/* Copyright (C) 2006  db4objects Inc.  http://www.db4o.com */

package org.polepos.drs;

import com.db4o.drs.test.*;

/**
 * @exclude
 */
public class Db4oDb4oCar extends DrsCar{
    
    public Db4oDb4oCar () {
        super(new Db4oDrsFixture("poleposA"), new Db4oDrsFixture("poleposB"));
    }
    
    @Override
    public String name() {
        return "db4o-db4o";
    }

}
